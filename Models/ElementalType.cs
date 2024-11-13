using CassetteBeastsAPI.Models;
using Godot;

public class Interaction
{
    public string Attacker { get; set; }
    public string Defender { get; set; }
    public string Hint { get; set; }
    public string Message { get; set; }
    public List<TypeStatus> Statuses { get; set; }
    
}

public class InteractionView  {
    public string Attacker { get; set; } = "";
    public string Defender { get; set; } = "";
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";
    public int Duration { get; set; } = 0;
    public string Category { get; set; } = "";
    public string StatusKey { get; set; } = "";
}

public class ElementalType
{
    public List<Interaction> Inflicts { get; set; }
    public string Name { get; set; }
    public List<string> Palette { get; set; }
    public List<Interaction> Receives { get; set; }
    public int SortOrder { get; set; }
    public List<Color> VfxPaletteRgba { get; set; }
    public List<Color> PaletteRgba { get; set; }
    public bool Sparkle { get; set; } = false;
}

public class ElementalTypeView
{
    public List<InteractionView> Inflicts { get; set; }
    public string Name { get; set; }
    public List<string> Palette { get; set; }
    public List<InteractionView> Receives { get; set; }
    public int SortOrder { get; set; }
    public bool Sparkle { get; set; } = false;
}

public class ElementalTypePageView
{
    public List<InteractionView> Inflicts { get; set; }
    public string Name { get; set; }
    public List<InteractionView> Receives { get; set; }

    public List<MoveListDetailView> Moves { get; set; } = [];
    public List<MoveSpeciesListItem> Monsters { get; set; } = [];
}

public class TypeStatus
{
    public string Name { get; set; }
    public int Duration { get; set; }
}

