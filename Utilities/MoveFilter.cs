using CassetteBeastsAPI.Models;

namespace CassetteBeastsAPI.Utilities
{
    public static class MoveFilter
    {
        public static List<string> GetStickers(MoveList moveTags, List<Move> allMoves)
        {
            List<string> result = [];

            allMoves.FindAll(x => moveTags.Tags.Contains(x.Name) && !x.Tags.Contains("unsellable"))
            .ForEach(y => result.Add(y.Name));

            return result;
        }
    }
}
