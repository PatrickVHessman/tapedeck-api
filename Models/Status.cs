using NuGet.Protocol.Core.Types;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace CassetteBeastsAPI.Models
{
    public class Status
    {
        public int Id { get; set; } = 0;
        public required string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool HasDuration { get; set; } = false;
        public bool IsBuff { get; set; } = false;
        public bool IsDebuff { get; set; } = false;
        public bool IsRemovable { get; set; } = false;
        public string Category { get; set; } = "Misc";
        public string Key { get; set; } = "";

    }

    public class StatusView: Status
    {
        public List<InteractionView> TypeInteractions { get; set; } = [];
        public List<MoveListDetailView> AssociatedMoves { get; set; } = [];
    }
}
