using Godot;
using Newtonsoft.Json;

namespace CassetteBeastsAPI.Models
{
    public class FusionMonster
    {
        public string MonsterName { get; set; } = "";
        public List<Color> SwapColorsRgba { get; set; } = [];
        public List<NodeFrame> NodeFrames { get; set; } = [];
        public int SpriteWidth { get; set; } = 0;
        public int SpriteHeight { get; set; } = 0;
        public MoveListView Moves { get; set; } = new MoveListView();
        public List<string> ElementalTypes { get; set; } = [];
        public StatSpread FusionStats { get; set; }
    }

    public class NodeFrame
    {
       public int StackOrder { get; set; } = 0;
       public List<DrawParams> Params { get; set; } = [];
    }

public class NodePriorityClass : IndexedClass
{
    public int Arm_Back { get; set; }
    public int Tail { get; set; }
    public int BackLeg_Back { get; set; }
    public int FrontLeg_Back { get; set; }
    public int Body { get; set; }
    public int BackLeg_Front { get; set; }
    public int FrontLeg_Front { get; set; }
    public int HelmetBack { get; set; }
    public int Head { get; set; }
    public int HelmetFront { get; set; }
    public int Arm_Front { get; set; }
}

public class DrawParams
    {
        public int? sx { get; set; } = 0;
        public int? sy { get; set; } = 0;
        public int? swidth { get; set; } = 0;
        public int? sheight { get; set; } = 0;
        public float? dx { get; set; } = 0;
        public float? dy { get; set; } = 0;
        public int? dwidth { get; set; } = 0;
        public int? dheight { get; set; } = 0;
        public string path { get; set; } = "";
        public string name { get; set; } = "";
    }

    public class FusionNode
    {
        public string Name { get; set; } = "";
        public float XOffset { get; set; } = 0;
        public float YOffset { get; set; } = 0;
        public bool Visible { get; set; } = true;
        public bool ForceUsage { get; set; } = false;
        public string MatchPart { get; set; } = "";
        public bool InverseMatch { get; set; } = false;
        public List<string> SpriteNames { get; set; } = [];
        public List<string> SpritePaths { get; set; } = [];
    }

    public class FusionResultNode : FusionNode {
        public string MonsterKey { get; set; } = "";
        public List<FusionPartSprite> Sprites { get; set; }
        }

    public class FusionPart
    {
        public string Name { get; set; } = "";
        public string Parent { get; set; } = "";
    }


    public class Frame
    {
        public int Height { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
    }

    

    public class Sprite
    {
        public string FileName { get; set; } = "";
        public List<Frame> Frames { get; set; } = [];
        public string Name { get; set; } = "";
        public string FilePath { get; set; } = "";
    }

    public class FusionPartSprite : Sprite
    {
        public string Category { get; set; } = "";
    }

    public class SpeciesSprite : Sprite
    {
        public string MonsterKey { get; set; } = "";
    }

    public class FusionComponent
    {
        public string MonsterKey { get; set; } = "";
        public string NamePrefix { get; set; } = "";
        public string NameSuffix { get; set; } = "";
        public List<FusionNode> FusionNodes { get; set; } = [];
        public ulong Hash { get; set; } = 0;
    }

    public class FusionSlots
    {
        List<FusionNode> FrontLeg_Back { get; set; } = [];
        List<FusionNode> BackLeg_Back { get; set; } = [];
        List<FusionNode> Body { get; set; } = [];
        List<FusionNode> FrontLeg_Front { get; set; } = [];
        List<FusionNode> BackLeg_Front { get; set; } = [];
        List<FusionNode> HelmetBack { get; set; } = [];
        List<FusionNode> Head { get; set; } = [];
        List<FusionNode> HelmetFront { get; set; } = [];
        List<FusionNode> Arm_Front { get; set; } = [];
        List<FusionNode> Arm_Back { get; set; } = [];
        List<FusionNode> Tail { get; set; } = [];
    }

    public class FusionNodes : IndexedClass
    {
        public FusionResultNode? FrontLeg_Back { get; set; }
        public FusionResultNode? BackLeg_Back { get; set; }
        public FusionResultNode? Body { get; set; }
        public FusionResultNode? FrontLeg_Front { get; set; }
        public FusionResultNode? BackLeg_Front { get; set; }
        public FusionResultNode? HelmetBack { get; set; }
        public FusionResultNode? Head { get; set; }
        public FusionResultNode? HelmetFront { get; set; }
        public FusionResultNode? Arm_Front { get; set; }
        public FusionResultNode? Arm_Back { get; set; }
        public FusionResultNode? Tail { get; set; }
    }

    public class FusionChoice : IndexedClass
    {
        public ChoiceDetail? FrontLeg_Back { get; set; }
        public ChoiceDetail? BackLeg_Back { get; set; }
        public ChoiceDetail? Body { get; set; }
        public ChoiceDetail? FrontLeg_Front { get; set; }
        public ChoiceDetail? BackLeg_Front { get; set; }
        public ChoiceDetail? HelmetBack { get; set; }
        public ChoiceDetail? Head { get; set; }
        public ChoiceDetail? HelmetFront { get; set; }
        public ChoiceDetail? Arm_Front { get; set; }
        public ChoiceDetail? Arm_Back { get; set; }
        public ChoiceDetail? Tail { get; set; }

    }

    public class ChoiceDetail
    {
        public int Monster;
        public ulong Variant;
    }

    public class FusionSeed
    {
        public static ulong GenFusionSeed(ulong hash1, ulong hash2)
        {
            ulong fusion_seed = 0;
             List<ulong> hashList = [hash1, hash2];
            foreach (ulong hash in hashList)
            {
                fusion_seed = (fusion_seed << 16) | (fusion_seed >> 16);
                fusion_seed ^= hash;
            }
                return fusion_seed;
            }
        }

   
}
