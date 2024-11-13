using System.Reflection.Metadata;

namespace CassetteBeastsAPI.Models
{
    public class Move
    {
        public required string Accuracy { get; set; }
        public required int ApCost { get; set; }
        public required bool CanBeCopied { get; set; }
        public required string Category { get; set; }
        public int CritDamagePercent { get; set; } = 150;
        public double CritRate { get; set; } = .0625;
        public required string DefaultTarget { get; set; }
        public required string Description { get; set; }
        public string? ElementalType { get; set; }
        public required string TargetType { get; set; }
        public required bool PassiveOnly { get; set; }
        public int MaxHits { get; set; } = 1;
        public int MinHits { get; set;} = 1;
        public required string Name { get; set; }
        public required string Key { get; set; }
        public required string Physicality { get; set; }
        public int Power { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public List<string> StatusEffects { get; set; } = [];
        public List<string> Tags { get; set; } = [];
    }

    public class MoveView : Move
    {
        public List<Status> Statuses { get; set; } = [];
    }

    public class MoveListDetail
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public string ElementalType { get; set; } = "";
        public List<string> Tags { get; set; } = [];
        public string Category { get; set; } = "";
        public int Power { get; set; } = 0;
        public string Accuracy { get; set; } = "";
        public int ApCost { get; set; } = 0;

    }

    public class MoveListDetailView : MoveListDetail
    {
        public int SortCategory { get; set; } = 0;
        public string MoveCategory { get; set; } = "";
        public string Description { get; set; } = "";
    }

}
