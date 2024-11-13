using CassetteBeastsAPI.Models;
using CassetteBeastsAPI.Utilities;
using Godot;
using LiteDB;
using System.Xml.Linq;

namespace CassetteBeastsAPI.Services
{
    public class DataManagementService
    {

        private ILiteCollection<Move> _moves;
        private ILiteCollection<Species> _species;
        private ILiteCollection<ElementalType> _elementalTypes;
        private ILiteCollection<Status> _statuses;
        private ILiteCollection<FusionPartSprite> _fusionParts;
        private ILiteCollection<FusionComponent> _fusionComponents;
        private ILiteCollection<SpeciesSprite> _speciesSprites;
        private ILiteCollection<Interaction> _typeInteractions;

        public DataManagementService(LiteDbContext liteDbContext)
        {
            _moves = liteDbContext.MoveContext;
            _elementalTypes = liteDbContext.ElementalTypeContext;
            _statuses = liteDbContext.StatusContext;
            _species = liteDbContext.SpeciesContext;
            _fusionParts = liteDbContext.FusionPartsContext;
            _fusionComponents = liteDbContext.FusionComponentContext;
            _speciesSprites = liteDbContext.SpeciesSpriteContext;
            _typeInteractions = liteDbContext.TypeInteractionContext;
        }

        public async Task<IList<Species>> InjestSpecies() {
            IList<SpeciesJson> res = await JsonFileReader.ReadAsync<IList<SpeciesJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\species.json");

            IList<PalettesJson> paletteRes = await JsonFileReader.ReadAsync<IList<PalettesJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\monster_palettes_hex.json");
            IList<PalettesJson> paletteResRgba = await JsonFileReader.ReadAsync<IList<PalettesJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\monster_palettes_rgba.json");

            IList<Species> List = [];

        SpeciesInjest speciesInjest = new SpeciesInjest();

            foreach (SpeciesJson item in res)
            {
                Species convertedSpecies = speciesInjest.SpeciesInjestFunc(item);
                PalettesJson palette = new PalettesJson();
                PalettesJson paletteRgba = new PalettesJson();
                palette = paletteRes.FirstOrDefault(x => x.name == item.name);
                paletteRgba = paletteResRgba.FirstOrDefault(x => x.name == item.name);

                List<Color> rgbaArr = new List<Color>();

                paletteRgba.swap_colors.ForEach(x => {
                    string[] vals = x.Split(",");
                    Color col = new Color
                    {
                        R = float.Parse(vals[0]),
                        G = float.Parse(vals[1]),
                        B = float.Parse(vals[2]),
                        A = float.Parse(vals[3])
                    };
                    rgbaArr.Add(col);
                });

                List<string> swapArr = palette.swap_colors;
                List<string> emArr = palette.emission_palette;

                convertedSpecies.SwapColors = swapArr;
                convertedSpecies.EmissionPalette = emArr;
                convertedSpecies.SwapColorsRgba = rgbaArr;

                _species.Insert(convertedSpecies);
                List.Add(convertedSpecies);
            }

            return List;
            }

        public async Task<IList<ElementalType>> InjestElementalTypes()
        {
            IList<ElementalTypeJson> res = await JsonFileReader.ReadAsync<IList<ElementalTypeJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\types2.json");
            IList<ElementTypesRgbaJson> elementTypesRgba = await JsonFileReader.ReadAsync<IList<ElementTypesRgbaJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\typePalettes.json");


            IList<ElementalType> List = [];

            ElementalTypeInjest typeInjest = new ElementalTypeInjest();

            foreach (ElementalTypeJson item in res)
            {
                ElementTypesRgbaJson rgbaItem = elementTypesRgba.First(x => x.name == item.name);
                ElementalType convertedType = typeInjest.ElementalTypeInjestFunc(item, rgbaItem);
                _elementalTypes.Insert(convertedType);
                List.Add(convertedType);
            }

            return List;
        }

        public async Task<List<Interaction>> InjestTypeInteractions()
        {
            IList<ElementalTypeJson> res = await JsonFileReader.ReadAsync<IList<ElementalTypeJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\types2.json");


            List<Interaction> tempInflictsArr = [];
            List<Interaction> tempReceivesArr = [];

            foreach (ElementalTypeJson item in res)
            {
                foreach (InteractionJson x in item.inflicts)
                {
                    List<TypeStatus> tempStatusesInflictsArr = new List<TypeStatus>();

                    foreach (TypeStatusJson inflictsStatus in x.statuses)
                    {
                        tempStatusesInflictsArr.Add(new TypeStatus { Duration = inflictsStatus.duration, Name = inflictsStatus.name });
                    }

                    Interaction temp = new Interaction
                    {
                        Attacker = x.attacker,
                        Defender = x.defender,
                        Hint = x.hint,
                        Message = x.message,
                        Statuses = tempStatusesInflictsArr,
                    };

                    tempInflictsArr.Add(temp);
                }

                foreach (InteractionJson x in item.receives)
                {
                    List<TypeStatus> tempStatusesReceivesArr = new List<TypeStatus>();

                    foreach (TypeStatusJson receivesStatus in x.statuses)
                    {
                        tempStatusesReceivesArr.Add(new TypeStatus { Duration = receivesStatus.duration, Name = receivesStatus.name });
                    }

                    Interaction temp = new Interaction
                    {
                        Attacker = x.attacker,
                        Defender = x.defender,
                        Hint = x.hint,
                        Message = x.message,
                        Statuses = tempStatusesReceivesArr,
                    };

                    tempReceivesArr.Add(temp);
                }

            }

            List<Interaction> List = tempInflictsArr.Union(tempReceivesArr).ToList();
            foreach (var item in List)
            {
                _typeInteractions.Insert(item);
            }
            return List;
        }

        public async Task<IList<Move>> InjestMoves()
        {
            IList<MoveJson> res = await JsonFileReader.ReadAsync<IList<MoveJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\moves.json");

            IList<Move> List = [];

            foreach (MoveJson item in res)
            {
                Move convertedItem = new Move
                {
                    Name = item.name,
                    Description = item.description,
                    Accuracy = item.accuracy.ToString(),
                    ApCost = item.ap_cost,
                    CanBeCopied = item.can_be_copied,
                    Category = item.category,
                    CritDamagePercent = item.crit_damage_percent,
                    CritRate = item.crit_rate,
                    DefaultTarget = item.default_target,
                    ElementalType = item.elemental_type,
                    PassiveOnly = item.is_passive_only,
                    MaxHits = item.max_hits,
                    MinHits = item.min_hits,
                    Physicality = item.physicality,
                    Power = item.power,
                    Priority = item.priority,
                    StatusEffects = item.status_effects,
                    Tags = item.tags,
                    TargetType = item.target_type,
                    Key = Global.RemoveSpecialCharacters(item.name).ToLower()
                };
                _moves.Insert(convertedItem);
                List.Add(convertedItem);
            }

            return List;
        }

        public async Task<IList<Status>> InjestStatus()
        {
            IList<StatusJson> res = await JsonFileReader.ReadAsync<IList<StatusJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\status.json");

            IList<Status> List = [];

            foreach (StatusJson item in res)
            {
                string key = item.name.Replace(" ", "_").ToLower();
                string cat = "";

                if (item.name.Contains("Coating")) cat = "Transmutation";
                else if (item.is_buff && !item.is_debuff) cat = "Buff";
                else if (!item.is_buff && item.is_debuff) cat = "Debuff";
                else cat = "Misc";

                Status convertedItem = new Status
                {
                    Name = item.name,
                    Description = item.description,
                    HasDuration = item.has_duration,
                    IsBuff = item.is_buff,
                    IsDebuff = item.is_debuff,
                    IsRemovable = item.is_removable,
                    Key = key,
                    Category = cat
                };
                _statuses.Insert(convertedItem);
                List.Add(convertedItem);
            }

            return List;
        }

        public async Task<IList<FusionPartSprite>> InjestFusionParts()
        {

            IList<SpriteJson> res = await JsonFileReader.ReadAsync<IList<SpriteJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\fusionPartSprites.json");

            IList<FusionPartSprite> List = [];
            

            foreach (SpriteJson item in res)
            {
                List<Frame> frameArr = [];

                FrameTagJson frameTag = item.frame_tags.FirstOrDefault(x => x.name == "idle") != null ? item.frame_tags.FirstOrDefault(x => x.name == "idle") : new FrameTagJson { end_frame = 6, start_frame = 0, name = "idle"  };

                for (int i = frameTag.start_frame > 0 ? frameTag.start_frame - 1 : 0; i <= frameTag.end_frame - 1; i++) {
                    Console.WriteLine(i);
                    Frame obj = new Frame
                    {
                        Height = item.frames[i].box.height,
                        Width = item.frames[i].box.width,
                        X = item.frames[i].box.x,
                        Y = item.frames[i].box.y,
                    };
                    frameArr.Add(obj);
                }

                List<string> categories = ["_arm", "_leg", "_body", "_helmet", "_tail", "_head"];

                string cat = "";
                string? matchingCat = categories.FirstOrDefault(x => item.image.Contains(x));
                if (matchingCat != null)
                 {
                        cat = matchingCat[1..];
                  }

                string filePath = "";

                string partialPath = "sprites/fusions/";
                if (cat == "arm" || cat == "leg")
                {
                    filePath = partialPath + cat + "s/" + item.image;
                }
                else { 
                    filePath = partialPath + cat + "/" + item.image;
                } 

                FusionPartSprite fusionPart = new FusionPartSprite
                {
                    FileName = item.image,
                    Frames = frameArr,
                    Name = item.image.Split(".png")[0],
                    Category = cat,
                    FilePath = filePath,
            };

                _fusionParts.Insert(fusionPart);
                List.Add(fusionPart);
                Console.WriteLine("Inserted " + fusionPart + " into database");
            };

            return List;
        }

        public async Task<IList<FusionComponent>> InjestFusionComponents()
        {
            IList<FusionNodeJson> fusionNodeRes = await JsonFileReader.ReadAsync<IList<FusionNodeJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\fusionNodes.json");

            IList<TranslationDocJson> translationRes = await JsonFileReader.ReadAsync<IList<TranslationDocJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\translations-en.json");

            IList<HashJson> hashRes = await JsonFileReader.ReadAsync<IList<HashJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\monster_hashes.json");

            IList<string> monsterNames = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => x.Name)
                .ToList();

            IList<FusionComponent> List = [];


            foreach (string monster in monsterNames)
            {
                

                List<FusionNode> fusionNodes = [];
                List<FusionPart> fusionParts = [];

                List<FusionNodeJson> fusionNodesJson = fusionNodeRes.ToList().FindAll(x => x.monster == monster.Replace(" ", "_"));
                fusionNodesJson.ForEach(x =>
                {
                    FusionNode node = new FusionNode
                    {
                        Name = x.name,
                        XOffset = x.xOffset,
                        YOffset = x.yOffset,
                        Visible = x.visible,
                        ForceUsage = x.forceUsage,
                        MatchPart = x.matchPart,
                        InverseMatch = x.inverseMatch,
                        SpriteNames = x.spriteNames,
                        SpritePaths = x.spritePaths,
                    };
                    fusionNodes.Add(node);
                });

                string monsterKey = monster.ToUpper().Replace(" ", "_");

                string prefixKey = monsterKey + "_NAME_PREFIX";
                string suffixKey = monsterKey + "_NAME_SUFFIX";

                string prefix = translationRes.ToList().Find(x => x.id == prefixKey).en;
                string suffix = translationRes.ToList().Find(x => x.id == suffixKey).en;

                ulong hash = hashRes.ToList().Find(x => x.monster == monsterKey).hash;

                FusionComponent obj = new FusionComponent
                {
                    MonsterKey = monster.ToUpper().Replace(" ", "_"),
                    FusionNodes = fusionNodes,
                    NamePrefix = prefix,
                    NameSuffix = suffix,
                    Hash = hash
                };

                _fusionComponents.Insert(obj);
                List.Add(obj);
                Console.WriteLine("Inserted " + obj + " into database");
            };

            return List;
        }

        public async Task<IList<SpeciesSprite>> InjestSpeciesAnimations()
        {

            IList<SpriteJson> res = await JsonFileReader.ReadAsync<IList<SpriteJson>>(System.Environment.CurrentDirectory.ToString() + @"\JSON\monster_animations.json");

            IList<string> monsterKeys = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => x.Key)
                .ToList();

            IList<SpeciesSprite> List = [];


            foreach (SpriteJson item in res)
            {
                List<Frame> frameArr = [];

                FrameTagJson frameTag = item.frame_tags.FirstOrDefault(x => x.name == "alt_idle");

                for (int i = frameTag.start_frame > 0 ? frameTag.start_frame - 1 : 0; i <= frameTag.end_frame - 1; i++)
                {
                    Console.WriteLine(i);
                    Frame obj = new Frame
                    {
                        Height = item.frames[i].box.height,
                        Width = item.frames[i].box.width,
                        X = item.frames[i].box.x,
                        Y = item.frames[i].box.y,
                    };
                    frameArr.Add(obj);
                }

                string filePath = "sprites/monsters/" + item.image;

                if (monsterKeys.Contains(item.image.Split(".png")[0].ToUpper()))
                {
                    SpeciesSprite speciesSprite = new SpeciesSprite
                    {
                        FileName = item.image,
                        Frames = frameArr,
                        Name = item.image.Split(".png")[0],
                        MonsterKey = item.image.Split(".png")[0].ToUpper(),
                        FilePath = filePath,
                    };



                    _speciesSprites.Insert(speciesSprite);
                    List.Add(speciesSprite);
                    Console.WriteLine("Inserted " + speciesSprite.Name + " into database");
                }

                    
            };

            return List;
        }


        public string TestDataMgmt()
        {
            return "it worked";
        }
        }
}
