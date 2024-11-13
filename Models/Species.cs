using Godot;

namespace CassetteBeastsAPI.Models
{
    
    public class StatSpread
    {
        public int MaxAp { get; set; } = 0;
        public int MaxHp { get; set; } = 0;
        public int MeleeAttack { get; set; } = 0;
        public int MeleeDefense { get; set; } = 0;
        public int RangedAttack { get; set; } = 0;
        public int RangedDefense { get; set; } = 0;
        public int Speed { get; set; } = 0;
        public int RecordRate { get; set; } = 0;
        public int MoveSlots { get; set; } = 0;
    }

    public class BootlegMoves
    {
        public List<string> AlwaysCompatible { get; set; } = [];
        public List<string> TypeSpecific { get; set; } = [];
    }

    public class MoveList
    {
        public List<string> Initial { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public List<string> Upgrades { get; set; } = [];
        public List<string> CompatibleMoves { get; set; } = [];
    }

    public class MoveListView
    {
        public List<MoveListDetailView> LearnedMoves { get; set; } = [];
        public List<MoveListDetailView> StickerMoves { get; set; } = [];
    }

    public class EvolutionCondition
    {
        public required string EvolvedForm { get; set; }
        public int MaxHour { get; set; } = 24;
        public int MinHour { get; set; } = 0;
        public int RequiredGrade { get; set; } = 5;
        public string RequiredLocation { get; set; } = "";
        public string RequiredMove { get; set; } = "";
        public string RequiredTypeOverride { get; set; } = "";
        public string Specialization { get; set; } = "";
    }
    
    public class Species
    {
        public int Id { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public int BestiaryCategory { get; set; } = 0; 
        public string? Description { get; set; }
        public List<EvolutionCondition> EvolvesTo { get; set; } = [];
        public List<string> EvolvesFrom { get; set; } = [];
        public List<string> BestiaryBios { get; set; } = [];
        public List<string> Habitats { get; set; } = [];
        public MoveList Moves { get; set; } = new MoveList();
        public StatSpread Stats { get; set; } = new StatSpread();
        public string ElementalType { get; set; } = "Typeless";
        public int BestiaryIndex { get; set; } = 0;
        public List<string> SwapColors { get; set; } = [];
        public List<string> EmissionPalette { get; set; } = [];

        public List<Color> SwapColorsRgba { get; set; } = [];
    }

    public class SpeciesView
    {
        public int Id { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public List<EvolutionCondition> EvolvesTo { get; set; } = [];
        public List<string> EvolvesFrom { get; set; } = [];
        public List<string> BestiaryBios { get; set; } = [];
        public List<string> Habitats { get; set; } = [];
        public MoveListView Moves { get; set; } = new MoveListView();
        public StatSpread Stats { get; set; } = new StatSpread();
        public string ElementalType { get; set; } = "Typeless";
        public int BestiaryIndex { get; set; } = 0;
    }

    public class BootlegSpeciesView: SpeciesView
    {
        public List<Color> RecolorsRgba { get; set; } = [];
        public List<Color> SwapColorsRgba { get; set; } = [];
        public int SpriteWidth { get; set; } = 0;
        public int SpriteHeight { get; set; } = 0;
        public List<DrawParams> SpriteParams { get; set; } = [];
    }

    public class DropdownMenuItem
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
    }

    public class SpeciesListItem : DropdownMenuItem
    {
        public string ElementalType { get; set; } = "Typeless";
        public int BestiaryIndex { get; set; } = 0;
        public string? Description { get; set; }
    }

    public class MoveSpeciesListItem : SpeciesListItem
    {
        public int SortCategory { get; set; } = 0;
    }
}
