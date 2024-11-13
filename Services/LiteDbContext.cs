using CassetteBeastsAPI.Models;
using LiteDB;
using Microsoft.Extensions.Options;

namespace CassetteBeastsAPI.Services
{
    public class LiteDbContext
    {
        public LiteDatabase Context { get; }
        public ILiteCollection<Status> StatusContext { get; set; }
        public ILiteCollection<Move> MoveContext { get; set; }
        public ILiteCollection<Species> SpeciesContext { get; set; }
        public ILiteCollection<ElementalType> ElementalTypeContext { get; set; }
        public ILiteCollection<FusionPartSprite> FusionPartsContext { get; set; }
        public ILiteCollection<FusionComponent> FusionComponentContext { get; set; }
        public ILiteCollection<SpeciesSprite> SpeciesSpriteContext { get; set; }
        public ILiteCollection<Interaction> TypeInteractionContext { get; set; }

        public LiteDbContext() {
            Context = new LiteDatabase(Global.connectionStr);
            StatusContext = Context.GetCollection<Status>("statuses");
            MoveContext = Context.GetCollection<Move>("moves");
            SpeciesContext = Context.GetCollection<Species>("species");
            ElementalTypeContext = Context.GetCollection<ElementalType>("elementalTypes");
            FusionPartsContext = Context.GetCollection<FusionPartSprite>("fusionParts");
            FusionComponentContext = Context.GetCollection<FusionComponent>("fusionComponents");
            SpeciesSpriteContext = Context.GetCollection<SpeciesSprite>("speciesSprites");
            TypeInteractionContext = Context.GetCollection<Interaction>("typeInteractions");
        }

       
    }
}
