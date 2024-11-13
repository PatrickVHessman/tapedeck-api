using CassetteBeastsAPI.Models;
using Godot;
using System.Text.Json;

namespace CassetteBeastsAPI.Utilities
{
    public class ElementalTypeInjest
    {
        public ElementalType ElementalTypeInjestFunc(ElementalTypeJson item, ElementTypesRgbaJson rgbaItem) {
            List<Interaction> tempInflictsArr = [];
            List<Interaction> tempReceivesArr = [];
            foreach (InteractionJson x in item.inflicts)
            {
                List<TypeStatus> tempStatusesInflictsArr = new List<TypeStatus>();

                foreach (TypeStatusJson inflictsStatus in x.statuses) {
                    tempStatusesInflictsArr.Add(new TypeStatus { Duration = inflictsStatus.duration, Name = inflictsStatus.name});
                }

                Interaction temp = new Interaction {
                Attacker = x.attacker,
                Defender = x.defender,
                Hint = x.hint,
                Message = x.message,
                Statuses = tempStatusesInflictsArr,
                };

                tempInflictsArr.Add(temp);
            }

            foreach (InteractionJson x in item.receives)
            {
                List<TypeStatus> tempStatusesReceivesArr = new List<TypeStatus>();

                foreach (TypeStatusJson receivesStatus in x.statuses)
                {
                    tempStatusesReceivesArr.Add(new TypeStatus { Duration = receivesStatus.duration, Name = receivesStatus.name });
                }

                Interaction temp = new Interaction
                {
                    Attacker = x.attacker,
                    Defender = x.defender,
                    Hint = x.hint,
                    Message = x.message,
                    Statuses = tempStatusesReceivesArr,
                };

                tempReceivesArr.Add(temp);
            }

            List<Color> rgbaArr = [];
            List<Color> vfxArr = [];

            rgbaItem.rgbaPalettte.ForEach(x => {
                 string[] vals = x.Split(",");
                 Color col = new Color
                 {
                     R = float.Parse(vals[0]),
                     G = float.Parse(vals[1]),
                     B = float.Parse(vals[2]),
                     A = float.Parse(vals[3])
                 };
                 rgbaArr.Add(col);
             });

            rgbaItem.vfxPalette.ForEach(x => {
                string[] vals = x.Split(",");
                Color col = new Color
                {
                    R = float.Parse(vals[0]),
                    G = float.Parse(vals[1]),
                    B = float.Parse(vals[2]),
                    A = float.Parse(vals[3])
                };
                vfxArr.Add(col);
            });

            ElementalType tempType = new ElementalType
            {
                Name = item.name,
                SortOrder = item.sort_order,
                Receives = tempReceivesArr,
                Inflicts = tempInflictsArr,
                Palette = item.palette,
                PaletteRgba = rgbaArr,
                VfxPaletteRgba = vfxArr,
                Sparkle = rgbaItem.sparkle,
            };

            return tempType;
        }
    }
}
