using CassetteBeastsAPI.Models;
using LiteDB;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CassetteBeastsAPI.Services
{
    public class StatusService
    {
        private ILiteCollection<Status> _statuses;
        private ILiteCollection<Move> _moves;
        private ILiteCollection<ElementalType> _elementalTypes;

        public StatusService(LiteDbContext liteDbContext)
        {
            _statuses = liteDbContext.StatusContext;
            _moves = liteDbContext.MoveContext;
            _elementalTypes = liteDbContext.ElementalTypeContext;
        }

        public List<Status> GetAllStatuses()
        {
            List<Status> results = _statuses
                .Query()
                .Where(x => x.Name != "Apple Tree")
                .OrderBy(x => x.Name)
                .ToList();

            return results;
        }

        public Status GetStatusByName(string status)
        {
            string convertQuery = status.Replace("_", " ");

            Status result = _statuses
                .Query()
                .Where(x => x.Name == convertQuery)
                .FirstOrDefault();

            return result;
        }

        public StatusView GetStatusViewByName(string status)
        {
            string convertQuery = status.Replace("_", " ");

            StatusView result = _statuses
                .Query()
                .Where(x => x.Name == convertQuery)
                .Select(x => new StatusView { 
                    Name = x.Name,
                    Id = x.Id,
                    Description = x.Description,
                    IsBuff = x.IsBuff,
                    IsDebuff = x.IsDebuff,
                    HasDuration = x.HasDuration,
                    IsRemovable = x.IsRemovable,
                    Key = x.Key,
                    Category = x.Category,
                })
                .FirstOrDefault();

            
            List<List<Interaction>> interactionsList = 
                _elementalTypes
                .FindAll()
                .Select(x => x.Inflicts.Union(x.Receives).ToList())
                .ToList();

            List<InteractionView> interactions = [];

            List<Interaction> flattenedInteractionsList = interactionsList.SelectMany(x => x).ToList().FindAll(x => x.Statuses.Any(y => y.Name == result.Name)).Distinct().ToList();

            flattenedInteractionsList.ForEach(y =>
            {
               
                if (y != null && y.Statuses.Any(z => z.Name == result.Name) && !interactions.Any(x => x.Attacker == y.Attacker && x.Defender == y.Defender ))
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

                            interactions.Add(new InteractionView
                            {
                                Name = y.Message,
                                Attacker = y.Attacker,
                                Defender = y.Defender,
                                Category = cat,
                                Duration = z.Duration,
                                Status = z.Name,
                                StatusKey = result.Key
                            });
                        }
                    });
                }

            });

            List<MoveListDetailView> moves = _moves
                .Query()
                .Where(x => x.StatusEffects.Contains(result.Name))
                .Select(x => new MoveListDetailView
                {
                    Name = x.Name,
                    Accuracy = x.Accuracy,
                    Key = x.Key,
                    ElementalType = x.ElementalType,
                    Tags = x.Tags,
                    Description = x.Description,
                    ApCost = x.ApCost,
                    Category = x.Category,
                    Power = x.Power,
                })
                .ToList();

            result.TypeInteractions = interactions;
            result.AssociatedMoves = moves;

            return result;
        }

        public IList<Status> GetBuffStatuses() {
            IList<Status> results = _statuses
                .Query()
                .Where(x => x.IsBuff == true)
                .OrderBy(x => x.Name)
                .ToList();

            return results;
        }

        public IList<Status> GetDebuffStatuses()
        {
            IList<Status> results = _statuses
                .Query()
                .Where(x => x.IsDebuff == true)
                .OrderBy(x => x.Name)
                .ToList();

            return results;
        }
    }
}
