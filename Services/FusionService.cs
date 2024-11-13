using CassetteBeastsAPI.Models;
using Humanizer;
using LiteDB;
using Color = Godot.Color;

namespace CassetteBeastsAPI.Services
{
    public class FusionService
    {
        private ILiteCollection<Species> _species;
        private ILiteCollection<FusionPartSprite> _fusionParts;
        private ILiteCollection<FusionComponent> _fusionComponents;
        private ILiteCollection<Move> _moves;
        private SpeciesService _speciesService;

        List<string> SameFusionNameModifiers = ["Meta", "Super", "Ultra", "Double", "Jumbo", "Mutant", "Hyper", "Giga", "2.0", "Mega"];

        Color blackObj = new()
        {
            R = 0,
            G = 0,
            B = 0,
            A = 1,
        };

        int CoordinatesConversion(int val)
        {
            if (val == 0) return 0;
            else if (val < 0) return val * -1;

            return val;
 }

        public FusionService(LiteDbContext liteDbContext, SpeciesService speciesService)
        {
            _species = liteDbContext.SpeciesContext;
            _moves = liteDbContext.MoveContext;
            _fusionParts = liteDbContext.FusionPartsContext;
            _fusionComponents = liteDbContext.FusionComponentContext;
            _speciesService = speciesService;
        }

        private List<FusionNode> GetParts(FusionComponent monster, string slotName)
        {
            List<FusionNode> res = monster.FusionNodes.FindAll(x => x.Name == slotName);
            return res;
        }


        private List<FusionNode> GetPartsForComparison(FusionComponent monster1, FusionComponent monster2, int monsterIndex, string slotName )
        {
            FusionComponent gen = monsterIndex == 0 ? monster1 : monster2;
            return GetParts(gen, slotName);
        }

        private double GetOffset (double num, double dim)
        {
            double abValue = Math.Abs(num);
            double res = abValue + (dim / 2);
            if (num < 0) return 0 - res;
            return res;
        }

        private List<Color> FusePalettes(string monster1, string monster2, CBRandom rand, string mon1bootleg, string mon2bootleg, Species mon1species, Species mon2species)
        {
            List<Species> monsters = [];

            

            Species mon1 = mon1species;
            Species mon2 = mon2species;

            monsters.Add(mon1);

            if (mon2bootleg.ToLower() != "none")
            {
                mon2.SwapColorsRgba = _speciesService.GenerateRecolorPalette(mon2.SwapColorsRgba, mon2bootleg);
            }

            monsters.Add(mon2);

            List<Color> result = [];

            var colorIndex = (int)rand.RandInt(2);
            var form = new Species();

            List<Color> formColors = monsters[colorIndex].SwapColorsRgba;

            for (int i = 0; i < 5; i++)
            {
                result.Add(i < formColors.Count ? formColors[i] : blackObj);
            }

            formColors = monsters[(colorIndex + 1) % monsters.Count].SwapColorsRgba;

            int startIndex = 5;

            if (formColors.Count > 5 && result.Contains(formColors[5]))
            {
                startIndex = 0;
            }

            int endIndex = startIndex + 5;

            for (int i = startIndex; i < endIndex; i++)
            {
                result.Add(i < formColors.Count ? formColors[i] : blackObj);
            }

            colorIndex = (int)rand.RandInt(2);
            formColors = monsters[colorIndex].SwapColorsRgba;

            if (formColors.Count > 10 && result.Contains(formColors[10]))
            {
                colorIndex = (colorIndex + 1) % monsters.Count;
                formColors = monsters[colorIndex].SwapColorsRgba;
            }

            for (int i = 10; i < 15; i++)
            {
                result.Add(i < 15 ? formColors[i] : blackObj);
            }

            return result;
        }

        public FusionMonster GetFusion(string monster1, string monster2, ulong initSeed, string mon1Bootleg, string mon2Bootleg)
        {
            string convertQuery1 = monster1.Replace(" ", "_").ToUpper();
            string convertQuery2 = monster2.Replace(" ", "_").ToUpper();

            List<FusionComponent> monsterArr = [];

            FusionComponent monster1Component = _fusionComponents
                .Query()
                .Where(x => x.MonsterKey == convertQuery1)
                .FirstOrDefault();

            monsterArr.Add(monster1Component);

            FusionComponent monster2Component = _fusionComponents
                .Query()
                .Where(x => x.MonsterKey == convertQuery2)
                .FirstOrDefault();

            Species mon1Species = new Species();

            if (mon1Bootleg.ToLower() != "none")
            {
                mon1Species = _speciesService.GetBootlegSpecies(monster1, mon1Bootleg);
            }
            else
            {
                mon1Species = _speciesService.GetSpeciesByName(monster1);
            }

            Species mon2Species = new Species();

            if (mon2Bootleg.ToLower() != "none")
            {
                mon2Species = _speciesService.GetBootlegSpecies(monster2, mon2Bootleg);
            }
            else
            {
                mon2Species = _speciesService.GetSpeciesByName(monster2);
            }

            monsterArr.Add(monster2Component);

            ulong seed = initSeed;
            seed ^= FusionSeed.GenFusionSeed((ulong)monster1Component.Hash, (ulong)monster2Component.Hash);

            CBRandom rand = new CBRandom(seed);

            FusionNodes slots = new FusionNodes();
            List<FusionResultNode> selectedNodes = new List<FusionResultNode>();

            FusionChoice choices = new FusionChoice();

            monster1Component.FusionNodes.ForEach( x =>
            {
                if (x.Visible) {
                int monster = x.ForceUsage ? 0 : (int)rand.RandInt(2);
                List<FusionNode> nodes = GetPartsForComparison(monster1Component, monster2Component, monster, x.Name);
                if (nodes.Count == 0)
                {
                    monster = (monster + 1) % 2;
                }
                choices[x.Name] = new ChoiceDetail
                {
                    Monster = monster,
                    Variant = rand.RandInt()
                };
            }
            });

            monster1Component.FusionNodes.ForEach(x =>
            {
                if (x.MatchPart != null && x.MatchPart.Length > 0)
                {
                    ChoiceDetail tempChoice = (ChoiceDetail)choices[x.Name];
                    ChoiceDetail matchedChoice = (ChoiceDetail)choices[x.MatchPart];

                    if (x.InverseMatch)
                    {
                        tempChoice.Monster = (matchedChoice.Monster + 1) % 2;
                    }
                    else
                    {
                        choices[x.Name] = matchedChoice;
                    }
                    if (GetParts(monster2Component, x.Name).Count == 0) {
                        tempChoice.Monster = 0;
                    }

                }
            });

            List<NodeFrame> nodeArr = [];

            int nodeStackOrder = 0;
            int spriteWidth = 0; 
            int spriteHeight = 0;
            int maxXPoint = 0;
            int minXOffset = 0;
            int maxYPoint = 0;
            int minYOffset = 0;

            monsterArr[0].FusionNodes.ForEach(x =>
            {
                if (x.XOffset < minXOffset) minXOffset = (int)x.XOffset;
                if (x.YOffset < minYOffset) minYOffset = (int)x.YOffset;
            });

            monsterArr[0].FusionNodes.ForEach(x =>
            {
                 ChoiceDetail obj = new ChoiceDetail();

                List<DrawParams> paramArr = [];

                obj = (ChoiceDetail)choices[x.Name];
                if (obj != null)
                {
                    FusionNode partNode = monsterArr[obj.Monster].FusionNodes.FirstOrDefault(y => y.Name == x.Name);

                    if (x != null && partNode != null && x.Visible)
                    {
                        List<FusionPartSprite> sprites = _fusionParts
                        .Query()
                        .Where(x => partNode.SpriteNames.Contains(x.Name))
                        .ToList();

                        sprites.ForEach(y =>
                        {
                            if ((x.YOffset + y.Frames[0].Height) > maxYPoint) maxYPoint = (int)(x.YOffset + y.Frames[0].Height);
                            if ((x.XOffset + y.Frames[0].Width) > maxXPoint) maxXPoint = (int)(x.XOffset + y.Frames[0].Width);
                        });

                        sprites.ForEach(y =>
                        {

                            y.Frames.ForEach(z =>
                            {
                                DrawParams paramObj = new DrawParams
                                {
                                    sx = z.X,
                                    sy = z.Y,
                                    swidth = z.Width,
                                    sheight = z.Height,
                                    dx = x.XOffset + CoordinatesConversion((int)minXOffset),
                                    dy = x.YOffset + CoordinatesConversion((int)minYOffset),
                                    dheight = z.Height,
                                    dwidth = z.Width,
                                    name = y.Name,
                                    path = y.FilePath
                                };
                                paramArr.Add(paramObj);
                            });
                            
                        });
                        
                        NodeFrame frame = new NodeFrame
                        {
                            StackOrder = nodeStackOrder,
                            Params = paramArr
                        };
                        nodeArr.Add(frame);
                        nodeStackOrder++;
                    }
                }
            });
            spriteHeight = maxYPoint + CoordinatesConversion(minYOffset);
            spriteWidth = maxXPoint + CoordinatesConversion(minXOffset);

            string monsterName = "";

            if (convertQuery1 != convertQuery2)
            {
                monsterName = monster1Component.NamePrefix + monster2Component.NameSuffix;
            }
            else
            {
                CBRandom sameNameRand = new CBRandom(FusionSeed.GenFusionSeed((ulong)monster1Component.Hash, (ulong)monster2Component.Hash));
                string choice = SameFusionNameModifiers[sameNameRand.Choice(SameFusionNameModifiers)];

                if (choice == "2.0")
                {
                    monsterName = _species.Query().Where(x => x.Key == convertQuery1).FirstOrDefault().Name.ToString() + " 2.0";
                }
                else
                {
                    monsterName = choice + " " + _species.Query().Where(x => x.Key == convertQuery1).FirstOrDefault().Name.ToString();
                } 
            }

            List<Color> palette = FusePalettes(convertQuery1, convertQuery2, rand, mon1Bootleg, mon2Bootleg, mon1Species, mon2Species);

            List<string> initMovesStrs = mon1Species.Moves.Initial.Union(mon2Species.Moves.Initial).ToList();
            List<string> upgradeMovesStrs = mon1Species.Moves.Upgrades.Union(mon2Species.Moves.Upgrades).ToList();
            List<string> compatMovesStrs = mon1Species.Moves.CompatibleMoves.Union(mon2Species.Moves.CompatibleMoves).ToList();

            List<Move> initMovesList = _moves
                .Query()
                .Where(x => initMovesStrs.Contains(x.Name))
            .ToList();

            List<Move> upgradeMovesList = _moves
                .Query()
                .Where(x => upgradeMovesStrs.Contains(x.Name))
            .ToList();

            List<Move> compatMovesList = _moves
                .Query()
                .Where(x => compatMovesStrs.Contains(x.Name))
            .ToList();

            List<MoveListDetailView> learnedMoves = [];
            List<MoveListDetailView> stickerMoves = [];

            initMovesList.ForEach(x =>
            {
                MoveListDetailView item = new()
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    SortCategory = 0,
                    MoveCategory = x.Category,
                    Power = x.Power,
                    Accuracy = x.Accuracy,
                    ApCost = x.ApCost,
                };
                learnedMoves.Add(item);
            });

            upgradeMovesList.ForEach(x =>
            {
                MoveListDetailView item = new()
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    SortCategory = 1,
                    MoveCategory = x.Category,
                    Power = x.Power,
                    Accuracy = x.Accuracy,
                    ApCost = x.ApCost,
                };
                learnedMoves.Add(item);
            });

            compatMovesList.ForEach(x =>
            {
                if (!initMovesStrs.Contains(x.Name) && !upgradeMovesStrs.Contains(x.Name))
                {
                    MoveListDetailView item = new()
                    {
                        Name = x.Name,
                        Key = x.Key,
                        ElementalType = x.ElementalType,
                        SortCategory = 2,
                        MoveCategory = x.Category,
                        Power = x.Power,
                        Accuracy = x.Accuracy,
                        ApCost = x.ApCost,
                    };
                    stickerMoves.Add(item);
                }
            });

            MoveListView fusionMoves = new MoveListView
            {
                LearnedMoves = learnedMoves,
                StickerMoves = stickerMoves
            };

            List<string> elTypes = [];
            elTypes.Add(mon1Species.ElementalType);
            if ((mon1Species.Key != mon2Species.Key) || (mon1Species.Key == mon2Species.Key && mon1Bootleg != mon2Bootleg))
            {
                    elTypes.Add(mon2Species.ElementalType); 
            }

            StatSpread fuseStats = new StatSpread
            {
                MeleeAttack = (int)((mon1Species.Stats.MeleeAttack + mon2Species.Stats.MeleeAttack) * .75),
                MeleeDefense = (int)((mon1Species.Stats.MeleeDefense + mon2Species.Stats.MeleeDefense) * .75),
                RangedAttack = (int)((mon1Species.Stats.RangedAttack + mon2Species.Stats.RangedAttack) * .75),
                RangedDefense = (int)((mon1Species.Stats.RangedDefense + mon2Species.Stats.RangedDefense) * .75),
                Speed = (int)((mon1Species.Stats.Speed + mon2Species.Stats.Speed) * .75),
                MaxAp = 10,
                MaxHp = mon1Species.Stats.MaxHp + mon2Species.Stats.MaxHp,
                MoveSlots = mon1Species.Stats.MoveSlots + mon2Species.Stats.MoveSlots,
            };

            FusionMonster result = new()
            {
                MonsterName = monsterName,
                NodeFrames = nodeArr,
                SwapColorsRgba = palette,
                SpriteHeight = spriteHeight,
                SpriteWidth = spriteWidth,
                Moves = fusionMoves,
                ElementalTypes = elTypes,
                FusionStats = fuseStats,
            };

            return result;
        }

    }
}
