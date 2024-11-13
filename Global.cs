using CassetteBeastsAPI.Models;
using System.Text.RegularExpressions;

namespace CassetteBeastsAPI
{
    static partial class Global
    {
        public static string connectionStr = System.Environment.CurrentDirectory.ToString() + @"\db\CassetteBeasts.db";

        public static List<string> ElementalTypeList = ["beast", "air", "astral", "earth", "fire", "ice", "lightning", "metal", "plant", "plastic", "poison", "water", "glass", "glitter"];

        public static NodePriorityClass NodePriority = new NodePriorityClass
        {
            Arm_Back = 1,
            Tail = 2,
            BackLeg_Back = 3,
            FrontLeg_Back = 4,
            Body = 5,
            BackLeg_Front = 6,
            FrontLeg_Front = 7,
            HelmetBack = 8,
            Head = 9,
            HelmetFront = 10,
            Arm_Front = 11
        };

        public static bool HasIntersection(List<string> arr1, List<string> arr2)
        {
            return arr1.Intersect(arr2).Any();
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return RemoveSpecialCharsRegex().Replace(str, "_");
        }

        [GeneratedRegex("[^a-zA-Z0-9_.]+", RegexOptions.Compiled)]
        private static partial Regex RemoveSpecialCharsRegex();
    }
}
