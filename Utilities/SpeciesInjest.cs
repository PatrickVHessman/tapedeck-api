using CassetteBeastsAPI.Models;

namespace CassetteBeastsAPI.Utilities
{
    public class SpeciesInjest
    {
        public Species SpeciesInjestFunc(SpeciesJson item) {
            
            List<EvolutionCondition> conditions = new List<EvolutionCondition>();

            foreach (EvolvesToJson x in item.evolves_to)
            {
                EvolutionCondition tempEvo = new EvolutionCondition {
                EvolvedForm = x.evolved_form,
                MaxHour = x.max_hour,
                MinHour = x.min_hour,
                RequiredGrade = x.required_grade,
                RequiredLocation = x.required_location,
                RequiredTypeOverride = x.required_type_override,
                RequiredMove = x.required_move,
                Specialization = x.specialization,
                };

                conditions.Add(tempEvo);
            }

            Species tempSpecies = new Species {
            Name = item.name,
            Key = item.name.Replace(" ", "_").ToUpper(),
            BestiaryCategory = item.bestiary_category,
            Description = item.description,
            Moves = new MoveList
            {
                Initial = item.moves.initial,
                Tags = item.moves.tags,
                Upgrades = item.moves.upgrades
            },
            Stats = new StatSpread
            {
                MaxAp = item.stats.max_ap,
                MaxHp = item.stats.max_hp,
                MeleeAttack = item.stats.melee_attack,
                MeleeDefense = item.stats.melee_defense,
                RangedAttack = item.stats.ranged_attack,
                RangedDefense = item.stats.ranged_defense,
                Speed = item.stats.speed,
                RecordRate = item.stats.record_rate,
                MoveSlots = item.stats.move_slots,
            },
            ElementalType = item.elemental_type,
            EvolvesFrom = item.evolves_from,
            BestiaryBios = item.bestiary_bios,
            Habitats = item.habitats,
            EvolvesTo = conditions,
            BestiaryIndex = item.bestiary_index,
            };

            return tempSpecies; 
        }
    }
}
