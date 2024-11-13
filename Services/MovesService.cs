using CassetteBeastsAPI.Models;
using LiteDB;
using System.ComponentModel;

namespace CassetteBeastsAPI.Services
{
    public class MovesService
    {
        private ILiteCollection<Move> _moves;
        private ILiteCollection<Status> _statuses;

        public MovesService(LiteDbContext liteDbContext)
        {
            _moves = liteDbContext.MoveContext;
            _statuses = liteDbContext.StatusContext;
        }

        public IList<Move> GetAllMoves()
        {
            IList<Move> results = _moves
                .FindAll()
                .OrderBy(x => x.Name)
                .ToList();

            return results;
        }

        public IList<MoveListDetailView> GetAllMoveListViews()
        {
            IList<MoveListDetailView> results = _moves
                .FindAll()
                .OrderBy(x => x.Name)
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

            return results;
        }

        public List<MoveListDetailView> GetMoveListViewByElement(string elType)
        {
            List<MoveListDetailView> results = _moves
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

            return results;
        }

        public Move GetMoveByName(string name)
        {
            string convertQuery = name.Replace("+", " ");

            Move result = _moves
                .Query()
                .Where(x => x.Name == convertQuery)
                .FirstOrDefault();

            return result;
        }

        public MoveView GetMoveByKey(string key)
        {
            MoveView result = _moves
                .Query()
                .Where(x => x.Key == key)
                .Select(x => x as MoveView)
                .FirstOrDefault();

            result.Statuses = _statuses
                .Query()
                .Where(x => result.StatusEffects.Contains(x.Name))
                .ToList();

            return result;
        }

        public IList<Move> GetMovesByElementalType(string elementalType)
        {
            string convertQuery = elementalType.Replace("+", " ");

            IList<Move> results = _moves
                .Query()
                .Where(x => x.ElementalType == elementalType)
                .ToList();

            return results;
        }

        public IList<Move> GetMovesByCategory(string category)
        {
           string convertQuery = category.Replace("+", " ");

            IList<Move> results = _moves
                .Query()
                .Where(x => x.Category == category)
                .ToList();

            return results;
        }

        public IList<Move> GetMovesByTag(string tag)
        {
            string convertQuery = tag.Replace("+", " ");

            IList<Move> results = _moves
                .Query()
                .Where(x => x.Tags.Any(y => y == tag))
                .ToList();

            return results;
        }

        public IList<Move> GetMovesByStatusEffect(string status)
        {
            string convertQuery = status.Replace("+", " ");

            IList<Move> results = _moves
                .Query()
                .Where(x => x.StatusEffects.Any(y => y == status))
                .ToList();

            return results;
        }

        public IList<Move> GetMovesByPhysicality(string phys)
        {
            string convertQuery = phys.Replace("+", " ");

            IList<Move> results = _moves
                .Query()
                .Where(x => x.Physicality == phys)
                .ToList();

            return results;
        }

        public List<string> GetCompatibleMoves(List<string> tags)
        {
            List<string> results = _moves
                .Query()
                .Where("$.Tags[*] ANY IN @0", BsonMapper.Global.Serialize(tags))
                .Select(x => x.Name)
                .ToList();

            results.Sort();

            return results;
        }

    }
}
