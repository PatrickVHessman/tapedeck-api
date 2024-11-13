namespace CassetteBeastsAPI.Models
{
    public class TranslationDocJson
    {
        public string id { get; set; } = "";
        public string en { get; set; } = "";
    }

    public class BoxJson
    {
        public int height { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class FrameJson
    {
        public BoxJson box { get; set; }
    }

    public class FrameTagJson
    {
        public int end_frame { get; set; }
        public string name { get; set; }
        public int start_frame { get; set; }
    }

    public class SpriteJson
    {
        public List<FrameTagJson> frame_tags { get; set; }
        public List<FrameJson> frames { get; set; }
        public string image { get; set; }
    }

    public class FusionNodeJson
    {
        public string name { get; set; }
        public string monster { get; set; }
        public float xOffset { get; set; }
        public float yOffset { get; set; }
        public bool visible { get; set; }
        public bool forceUsage { get; set; }
        public string matchPart { get; set; }
        public bool inverseMatch { get; set; }
        public List<string> spriteNames { get; set; }
        public List<string> spritePaths { get; set; }
    }

    public class FusionPartJson
    {
        public string name { get; set; }
        public string monster { get; set; }
        public string parent { get; set; }
        public string path { get; set; }
    }

    public class EvolvesToJson
    {
        public string evolved_form { get; set; }
        public int max_hour { get; set; }
        public int min_hour { get; set; }
        public int required_grade { get; set; }
        public string required_location { get; set; }
        public string required_move { get; set; }
        public string required_type_override { get; set; }
        public string specialization { get; set; }
    }

    public class SpeciesJson
    {
        public List<string> bestiary_bios { get; set; }
        public int bestiary_category { get; set; }
        public int bestiary_index { get; set; }
        public string description { get; set; }
        public string elemental_type { get; set; }
        public List<string> evolves_from { get; set; }
        public List<EvolvesToJson> evolves_to { get; set; }
        public List<string> habitats { get; set; }
        public MovesJson moves { get; set; }
        public string name { get; set; }
        public StatsJson stats { get; set; }
    }

    public class StatsJson
    {
        public int max_ap { get; set; }
        public int max_hp { get; set; }
        public int melee_attack { get; set; }
        public int melee_defense { get; set; }
        public int move_slots { get; set; }
        public int ranged_attack { get; set; }
        public int ranged_defense { get; set; }
        public int record_rate { get; set; }
        public int speed { get; set; }
    }
    public class PalettesJson
    {
        public List<string> default_palette { get; set; }
        public List<string> emission_palette { get; set; }
        public string name { get; set; }
        public List<string> swap_colors { get; set; }
    }

    public class MovesJson
    {
        public List<string> initial { get; set; }
        public List<string> tags { get; set; }
        public List<string> upgrades { get; set; }
    }

    public class InteractionJson
    {
        public string attacker { get; set; }
        public string defender { get; set; }
        public string hint { get; set; }
        public string message { get; set; }
        public List<TypeStatusJson> statuses { get; set; }
    }

    public class ElementalTypeJson
    {
        public List<InteractionJson> inflicts { get; set; }
        public string name { get; set; }
        public List<string> palette { get; set; }
        public List<InteractionJson> receives { get; set; }
        public int sort_order { get; set; }
    }

    public class TypeStatusJson
    {
        public string name { get; set; }
        public int duration { get; set; }
    }

    public class MoveJson
    {
        public object accuracy { get; set; }
        public int ap_cost { get; set; }
        public bool can_be_copied { get; set; }
        public string category { get; set; }
        public int crit_damage_percent { get; set; }
        public double crit_rate { get; set; }
        public string default_target { get; set; }
        public string description { get; set; }
        public string elemental_type { get; set; }
        public bool is_passive_only { get; set; }
        public int max_hits { get; set; }
        public int min_hits { get; set; }
        public string name { get; set; }
        public string physicality { get; set; }
        public int power { get; set; }
        public int priority { get; set; }
        public List<string> status_effects { get; set; }
        public List<string> tags { get; set; }
        public string target_type { get; set; }
    }

    public class StatusJson
    {
        public string description { get; set; }
        public bool has_duration { get; set; }
        public bool is_buff { get; set; }
        public bool is_debuff { get; set; }
        public bool is_removable { get; set; }
        public string name { get; set; }
    }

    public class HashJson
    {
        public string monster { get; set; } = "";
        public ulong hash { get; set; } = 0;
    }

     public class ElementTypesRgbaJson
    {
        public string name { get; set; }
        public List<string> rgbaPalettte { get; set; }
        public List<string> vfxPalette { get; set; }
        public bool sparkle { get; set; } = false;
    }

}
