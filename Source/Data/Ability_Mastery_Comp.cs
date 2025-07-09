using RimWorld;
using Verse;

using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Extensions;

using Mastery.Ability.Settings;

using Mastery.Ability.Patches.Vanilla;

namespace Mastery.Ability.Data
{
    public class Ability_Mastery_Comp : Level_Comp
    {
        public override string LevelKey => "AbilityMastery";

        public override bool GainExperience(Def def, Level_Action_Extension action, float multiplier = 1) //This is here beacuse I can't patch Proficiency_Comp GainExperience without it
                                                                                                          //After all patching Level_Comp would be way more ineffective.
        {
            Log.Message("I am here");
            return base.GainExperience(def, action, multiplier);
        }

        #region Vanilla

        public void AbilityStatsAllocate(AbilityDef def)
        {
            var config = Abilities_Settings.Instance.GetConfig(def.defName);

            var ability = AbilityStatsManager.Abilities[def.defName];

            var level = GetOrAdd(def.defName).Level;

            def.cooldownTicksRange.min = (int)config.CooldownCalculated(level, ability.Cooldown.min);
            def.cooldownTicksRange.max = (int)config.CooldownCalculated(level, ability.Cooldown.max);

            foreach (var verbKey in ability.verbs.Keys)
            {
                if (verbKey == "minRange")
                {
                    def.verbProperties.minRange = config.RangeCalculated(level, ability.verbs[verbKey]);
                }
                else if (verbKey == "range")
                {
                    def.verbProperties.range = config.RangeCalculated(level, ability.verbs[verbKey]);
                }
                else if (verbKey == "warmupTime")
                {
                    def.verbProperties.warmupTime = config.CastTimeCalculated(level, ability.verbs[verbKey]);
                }
                else
                {
                    break;
                }
            }

            foreach (var compKey in ability.comps.Keys)
            {
                if (compKey == "hemogenCost")
                {
                    var comp = def.comps[ability.comps[compKey].Index] as CompProperties_AbilityHemogenCost;
                    comp.hemogenCost = config.EntropyCalculated(level, ability.comps[compKey].Value);

                    break;
                }
            }

            foreach (var statKey in ability.statBases.Keys)
            {
                var stat = def.statBases[ability.statBases[statKey].Index];

                if (statKey == StatDefOf.Ability_Range)
                {
                    stat.value = config.RangeCalculated(level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_EffectRadius)
                {
                    stat.value = config.RadiusCalculated(level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_CastingTime)
                {
                    stat.value = config.CastTimeCalculated(level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_Duration)
                {
                    stat.value = config.DurationCalculated(level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_PsyfocusCost)
                {
                    stat.value = config.PsyfocusCalculated(level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_EntropyGain)
                {
                    stat.value = config.EntropyCalculated(level, ability.statBases[statKey].Value);
                }
                else
                {
                    break;
                }
            }
        }
        
        #endregion

    }
}