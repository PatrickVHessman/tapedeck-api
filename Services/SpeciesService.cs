using CassetteBeastsAPI.Models;
using Godot;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CassetteBeastsAPI.Services
{
    public class SpeciesService()
    {
        private ILiteCollection<Species> _species;
        private ILiteCollection<SpeciesSprite> _speciesSprites;
        private MovesService _movesService;
        private ElementalTypesService _typesService;
        private ILiteCollection<Move> _moves;

        public SpeciesService(LiteDbContext liteDbContext, MovesService movesService, ElementalTypesService typesService) : this()
        {
            this._species = liteDbContext.SpeciesContext;
            this._moves = liteDbContext.MoveContext;
            this._movesService = movesService;
            this._typesService = typesService;
            this._speciesSprites = liteDbContext.SpeciesSpriteContext;
        }

        public List<Color> GenerateRecolorPalette(List<Color> sourcePalette, string elTypeName)
        {
            if (elTypeName == "none")
            {
                return sourcePalette;
            }
            List<Color> outputColors = [.. sourcePalette];
            ElementalType elType = _typesService.GetElementalTypeByName(elTypeName);
            int i = 0;
            while (i < outputColors.Count && outputColors[i] == elType.PaletteRgba[0]) {
                i += elType.PaletteRgba.Count;
            }
            for(int j = 0; j < elType.PaletteRgba.Count; j++)
            {
                outputColors[j] = outputColors[i + j];
            }

            for (int k = 0; k < elType.PaletteRgba.Count; k++)
            {
                outputColors[i + k] = elType.PaletteRgba[k];
            }

            return outputColors;
        }

        SpeciesView ConvertSpeciesToView(Species mon)
        {
            List<string> tagList = new List<string>(mon.Moves.Tags)
    {
        mon.ElementalType,
        "any"
    };
            mon.Moves.CompatibleMoves = _movesService.GetCompatibleMoves(tagList);

            List<Move> initMovesList = _moves
            .Query()
                .Where(x => mon.Moves.Initial.Contains(x.Name))
            .ToList();

            initMovesList = initMovesList.Distinct().ToList();

            List<Move> upgradeMovesList = _moves
            .Query()
                .Where(x => mon.Moves.Upgrades.Contains(x.Name))
            .ToList();

            upgradeMovesList = upgradeMovesList.Distinct().ToList();

            List<Move> compatMovesList = _moves
            .Query()
                .Where(x => mon.Moves.CompatibleMoves.Contains(x.Name))
            .ToList();

            compatMovesList = compatMovesList.Distinct().ToList();

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
                if (!mon.Moves.Initial.Contains(x.Name) && !mon.Moves.Upgrades.Contains(x.Name))
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

            MoveListView speciesMoves = new MoveListView
            {
                LearnedMoves = learnedMoves,
                StickerMoves = stickerMoves
            };

            SpeciesView monView = new SpeciesView
            {
                Name = mon.Name,
                Habitats = mon.Habitats,
                BestiaryBios = mon.BestiaryBios,
                BestiaryIndex = mon.BestiaryIndex,
                Id = mon.Id,
                Key = mon.Key,
                ElementalType = mon.ElementalType,
                Moves = speciesMoves,
                Stats = mon.Stats,
                Description = mon.Description,
                EvolvesFrom = mon.EvolvesFrom,
                EvolvesTo = mon.EvolvesTo,
            };

            return monView;
        }

        public IList<Species> GetAllSpecies()
        {
            IList<Species> results = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex)
                .ToList();

            return results;
        }

        public IList<string> GetAllSpeciesNameList()
        {
            IList<string> results = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => x.Name)
                .ToList();

            return results;
        }

        public IList<DropdownMenuItem> GetSpeciesNameDropdownList(bool inclSec = false, bool inclDlc = false)
        {
            IList<DropdownMenuItem> results = [];
            List<int> filterOut = [];

            if (inclSec && inclDlc)
            {
                results = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => new DropdownMenuItem { Name = x.Name, Key = x.Key })
                .ToList();

                return results;
            }
            else
            {
                if (!inclDlc) filterOut.Add(-1);
                if (!inclSec)
                {
                    List<int> secMonsters = [0, 116, 117, 118, 119, 120, 121, 122, 123];
                    secMonsters.ForEach(x =>
                    {
                        filterOut.Add(x);
                    });
                }

                results = _species
                    .Query()
                .Where(x => !filterOut.Contains(x.BestiaryIndex))
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => new DropdownMenuItem { Name = x.Name, Key = x.Key })
                .ToList();

                return results;
            }
                
        }

        public IList<SpeciesListItem> GetSpeciesListItems(bool inclSec = false, bool inclDlc = false)
        {
            IList<SpeciesListItem> results = [];
            List<int> filterOut = [];

            if (inclSec && inclDlc)
            {
                results = _species
                .FindAll()
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => new SpeciesListItem { Name = x.Name, Key = x.Key, BestiaryIndex = x.BestiaryIndex, Description = x.Description, ElementalType = x.ElementalType })
                .ToList();

                return results;
            }
            else
            {
                if (!inclDlc) filterOut.Add(-1);
                if (!inclSec)
                {
                    List<int> secMonsters = [0, 116, 117, 118, 119, 120, 121, 122, 123];
                    secMonsters.ForEach(x =>
                    {
                        filterOut.Add(x);
                    });
                }

                results = _species
                    .Query()
                .Where(x => !filterOut.Contains(x.BestiaryIndex))
                .OrderBy(x => x.BestiaryIndex <= 1)
                .Select(x => new SpeciesListItem { Name = x.Name, Key = x.Key, BestiaryIndex = x.BestiaryIndex, Description = x.Description, ElementalType = x.ElementalType })
                .ToList();

                return results;
            }

        }

        public Species GetSpeciesByName(string name)
        {
            string convertQuery = name.Replace(" ", "_").ToUpper();

            Species result = _species
                .Query()
                .Where(x => x.Key == convertQuery)
                .FirstOrDefault();
            List<string> tagList = new List<string>(result.Moves.Tags)
            {
                result.ElementalType,
                "any"
            };
            result.Moves.CompatibleMoves = _movesService.GetCompatibleMoves(tagList);

            return result;
        }

        public SpeciesView GetSpeciesViewByName(string name)
        {
            string convertQuery = name.Replace(" ", "_").ToUpper();

            Species result = _species
                .Query()
                .Where(x => x.Key == convertQuery)
                .FirstOrDefault();
            
            SpeciesView resultView = ConvertSpeciesToView(result);

            return resultView;
        }

        public BootlegSpeciesView GetSpeciesBootlegViewByName(string name, string elementalType)
        {
            string convertQuery = name.Replace(" ", "_").ToUpper();

            Species result = _species
                .Query()
                .Where(x => x.Key == convertQuery)
            .FirstOrDefault();

            if (elementalType.ToLower() != "none")
            {
                result.ElementalType = elementalType.ToLower();
            }
            
            SpeciesView resultView = ConvertSpeciesToView(result);

            SpeciesSprite sprite = _speciesSprites
                        .Query()
                        .Where(x => x.MonsterKey == result.Key)
                        .FirstOrDefault();

            List<DrawParams> drawParams = new List<DrawParams>();

            sprite.Frames.ForEach(z =>
            {
                DrawParams paramObj = new DrawParams
                {
                    sx = z.X,
                    sy = z.Y,
                    swidth = z.Width,
                    sheight = z.Height,
                    dx = 0,
                    dy = 0,
                    dheight = z.Height,
                    dwidth = z.Width,
                    name = sprite.Name,
                    path = sprite.FilePath
                };
                drawParams.Add(paramObj);
            });

            List<Color> bootlegPalette = GenerateRecolorPalette(result.SwapColorsRgba, result.ElementalType);

            BootlegSpeciesView bootlegResultView = new BootlegSpeciesView
            {
                Name = resultView.Name,
                Habitats = resultView.Habitats,
                BestiaryBios = resultView.BestiaryBios,
                BestiaryIndex = resultView.BestiaryIndex,
                Id = resultView.Id,
                Key = resultView.Key,
                ElementalType = resultView.ElementalType,
                Moves = resultView.Moves,
                Stats = resultView.Stats,
                Description = resultView.Description,
                EvolvesFrom = resultView.EvolvesFrom,
                EvolvesTo = resultView.EvolvesTo,
                SwapColorsRgba = result.SwapColorsRgba,
                SpriteParams = drawParams,
                RecolorsRgba = bootlegPalette,
                SpriteHeight = sprite.Frames[0].Height,
                SpriteWidth = sprite.Frames[0].Width,
            };

            return bootlegResultView;
        }


        public List<Species> GetSpeciesByBestiaryIndex(int index)
        {
            // Returns a list in the case of the the query being -1, which all of the DLC monsters are returned
            List<Species> results = _species
            .Query()
            .Where(x => x.BestiaryIndex == index)
            .ToList();

            return results;

        }

        public List<Species> GetSpeciesByElementalType([FromQuery] string type)
        {
            string convertQuery = type.Replace("+", " ");

            List<Species> results = _species
                .Query()
                .Where(x => x.ElementalType == convertQuery)
                .ToList();

            return results;
        }

        public List<Species> GetSpeciesByMove([FromQuery] string move)
        {
            string convertQuery = move.Replace("_", " ");

            List<string> moveTags = _moves
                .Query()
                .Where(x => x.Key == move)
                .Select(x => x.Tags)
                .FirstOrDefault();

            List<Species> results = _species
                .Query()
                .Where(x => x.Moves.Initial.Any(y => y == move) || x.Moves.Upgrades.Any(y => y == move))
                .ToList();

            List<Species> compatResults = _species
                .Query()
                .Where("$.Tags[*] ANY IN @0", BsonMapper.Global.Serialize(moveTags))
                .ToList();

            List<Species> unionList = results.Union(compatResults).ToList();

            return unionList;
        }

        public List<MoveSpeciesListItem> GetSpeciesViewByMove([FromQuery] string move)
        {
            string convertQuery = move.Replace("_", " ");

            List<string> moveTags = _moves
                .Query() 
                .Where(x => x.Key == move)
                .FirstOrDefault()
                .Tags;

            List<MoveSpeciesListItem> initResults = _species
                .Query()
                .Where(x => x.Moves.Initial.Any(y => y == convertQuery))
                .Select(x => new MoveSpeciesListItem
                { 
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    BestiaryIndex = x.BestiaryIndex,
                    Description = x.Description,
                    SortCategory = 0
                })
                .ToList();

            List<MoveSpeciesListItem> upgradeResults = _species
                .Query()
                .Where(x => x.Moves.Upgrades.Any(y => y == convertQuery))
                .Select(x => new MoveSpeciesListItem
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    BestiaryIndex = x.BestiaryIndex,
                    Description = x.Description,
                    SortCategory = 1
                })
                .ToList();

            List<MoveSpeciesListItem> compatResults = _species
                .Query()
                .Where("$.Moves.Tags[*] ANY IN @0", BsonMapper.Global.Serialize(moveTags))
                .Select(x => new MoveSpeciesListItem
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    BestiaryIndex = x.BestiaryIndex,
                    Description = x.Description,
                    SortCategory = 2
                })
                .ToList();

            List<MoveSpeciesListItem> unionList = initResults.Union(upgradeResults).ToList();

            compatResults = compatResults.FindAll(x => !unionList.Any(y => y.Name == x.Name));

            unionList = unionList.Union(compatResults).ToList();

            return unionList;
        }

        public List<MoveSpeciesListItem> GetSpeciesViewByElementalType([FromQuery] string elType)
        {

            List<MoveSpeciesListItem> results = _species
                .Query()
                .Where(x => x.ElementalType == elType )
                .Select(x => new MoveSpeciesListItem
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    BestiaryIndex = x.BestiaryIndex,
                    Description = x.Description,
                    SortCategory = 0
                })
                .ToList();


            return results;
        }

        public List<Species> GetSpeciesByHabitat([FromQuery] string habitat)
        {
            string convertQuery = habitat.Replace("_", " ");

            List<Species> results = _species
                .Query()
                .Where(x => x.Habitats.Any(y => y == habitat))
                .ToList();

            return results;
        }


        public Species GetBootlegSpecies(string name, string elementalType)
        {
            string convertQuery = name.Replace("_", " ").ToLower();

            Species result = _species
                .Query()
                .Where(x => x.Name == convertQuery)
                .FirstOrDefault();

            result.ElementalType = elementalType.ToLower();

            List<Color> bootlegPalette = GenerateRecolorPalette(result.SwapColorsRgba, result.ElementalType);

            result.SwapColorsRgba = bootlegPalette;

            List<string> tagList = new List<string>(result.Moves.Tags)
            {
                result.ElementalType,
                "any"
            };
            result.Moves.CompatibleMoves = _movesService.GetCompatibleMoves(tagList);

            return result;
        }
    }
}
