using CassetteBeastsAPI.Models;
using LiteDB;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace CassetteBeastsAPI.Services
{
    public class ElementalTypesService { 

        private ILiteCollection<ElementalType> _elementalTypes;
        private ILiteCollection<Species> _species;
        private ILiteCollection<Move> _moves;

        public ElementalTypesService(LiteDbContext liteDbContext)
    {
            _elementalTypes = liteDbContext.ElementalTypeContext;
            _species = liteDbContext.SpeciesContext;
            _moves = liteDbContext.MoveContext;
    }

        public IList<ElementalType> GetAllElementalTypes()
        {
            IList<ElementalType> results = _elementalTypes
                .FindAll()
                .OrderBy(x => x.Name)
                .ToList();

            return results;
        }

        public IList<string> GetElementalTypesDropdown()
        {
            IList<string> results = _elementalTypes
                .FindAll()
                .OrderBy(x => x.SortOrder)
                .Select(x => x.Name)
                .ToList();

            results.Insert(0, "None");

            return results;
        }

        public ElementalType GetElementalTypeByName(string name)
        {
            string convertQuery = name.Replace("+", " ");

            ElementalType result = _elementalTypes
                .Query()
                .Where(x => x.Name == convertQuery)
                .FirstOrDefault();

            return result;
        }

        public ElementalTypePageView GetElementalTypeViewByName(string elType)
        {

            ElementalType result = _elementalTypes
                .Query()
                .Where(x => x.Name == elType)
                .FirstOrDefault();

            List <MoveSpeciesListItem> monList = _species
                .Query()
                .Where(x => x.ElementalType == elType && x.BestiaryIndex >= 0)
                .OrderBy(x => x.BestiaryIndex)
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

            List<MoveSpeciesListItem> monListDlc = _species
                .Query()
                .Where(x => x.ElementalType == elType && x.BestiaryIndex < 0)
                .OrderBy(x => x.BestiaryIndex)
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

            monListDlc.ForEach(x => monList.Add(x));

            //monList.OrderBy(x => x.BestiaryIndex > 0).Reverse();

            List<MoveListDetailView> moveList = _moves
                .Query()
                .Where(x => x.ElementalType == elType)
                .Select(x => new MoveListDetailView
                {
                    Name = x.Name,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    MoveCategory = x.Category,
                    Power = x.Power,
                    Accuracy = x.Accuracy,
                    ApCost = x.ApCost,
                    Description = x.Description,
                })
                .ToList();

            List<InteractionView> inflicts = [];

            result.Inflicts.ForEach(y =>
            {

                if (y != null)
                {
                    y.Statuses.ForEach(z =>
                    {
                        if (z != null)
                        {
                            string cat = "";

                            if (y.Hint == "NEGATIVE") cat = "Debuff";
                            else if (y.Hint == "POSITIVE") cat = "Buff";
                            else if (y.Hint == "TRANSMUTATION") cat = "Transmutation";
                            else cat = "Misc";

                            inflicts.Add(new InteractionView
                            {
                                Name = y.Message,
                                Attacker = y.Attacker,
                                Defender = y.Defender,
                                Category = cat,
                                Duration = z.Duration,
                                Status = z.Name,
                                StatusKey = z.Name.ToLower().Replace(" ","_")
                            });
                        }
                    });
                }

            });

            List<InteractionView> receives = [];

            result.Receives.ForEach(y =>
            {

                if (y != null)
                {
                    y.Statuses.ForEach(z =>
                    {
                        if (z != null)
                        {
                            string cat = "";

                            if (y.Hint == "NEGATIVE") cat = "Debuff";
                            else if (y.Hint == "POSITIVE") cat = "Buff";
                            else if (y.Hint == "TRANSMUTATION") cat = "Transmutation";
                            else cat = "Misc";

                            receives.Add(new InteractionView
                            {
                                Name = y.Message,
                                Attacker = y.Attacker,
                                Defender = y.Defender,
                                Category = cat,
                                Duration = z.Duration,
                                Status = z.Name,
                                StatusKey = z.Name.ToLower().Replace(" ", "_")
                            });
                        }
                    });
                }

            });

            ElementalTypePageView res = new ElementalTypePageView
            {
                Name = result.Name,
                Inflicts = inflicts,
                Receives = receives,
                Monsters = monList,
                Moves = moveList,
            };

            return res;
        }

    }
}
