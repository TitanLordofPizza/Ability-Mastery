using System.Collections.Generic;

using RimWorld;
using Verse;

using Mastery.Core.Utility.Data;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Vanilla
{
    public static class AbilityCacheManager // This is used to detect Variables that are modified by this Mod for ease of use and Performance.
    {
        public static Dictionary<string, AbilityCache> Abilities = new Dictionary<string, AbilityCache>();

        public static void Add(AbilityDef def)
        {
            Abilities[def.defName] = new AbilityCache
            {
                verbs = new Dictionary<string, float>(),
                comps = new Dictionary<string, IndexedCache>(),
                statBases = new Dictionary<StatDef, IndexedCache>(),

                Cooldown = new IntRange()
                {
                    min = def.cooldownTicksRange.min,
                    max = def.cooldownTicksRange.max
                }
            };

            if (def.verbProperties.minRange != 0)
                Abilities[def.defName].verbs["minRange"] = def.verbProperties.minRange;
            if (def.verbProperties.range != 0)
                Abilities[def.defName].verbs["range"] = def.verbProperties.range;

            if (def.verbProperties.warmupTime != 0)
                Abilities[def.defName].verbs["warmupTime"] = def.verbProperties.warmupTime;

            if (def.comps != null)
            {
                int index = 0;

                foreach (var comp in def.comps)
                {
                    if (comp is CompProperties_AbilityHemogenCost == true)
                    {
                        Abilities[def.defName].comps["hemogenCost"] = new IndexedCache()
                        {
                            Index = index,
                            Value = (comp as CompProperties_AbilityHemogenCost).hemogenCost
                        };

                        break;
                    }

                    index++;
                }
            }

            if (def.statBases != null)
            {
                int index = 0;

                foreach (var stat in def.statBases)
                {
                    if (stat.stat == StatDefOf.Ability_Range ||
                        stat.stat == StatDefOf.Ability_EffectRadius ||
                        stat.stat == StatDefOf.Ability_CastingTime ||
                        stat.stat == StatDefOf.Ability_Duration ||
                        stat.stat == StatDefOf.Ability_PsyfocusCost ||
                        stat.stat == StatDefOf.Ability_EntropyGain)
                    {
                        Abilities[def.defName].statBases.Add(stat.stat, new IndexedCache()
                        {
                            Index = index,
                            Value = stat.value
                        });
                    }

                    index++;
                }
            }
        }

        public static void AllocateStats(Ability_Mastery_Comp pawnComp, AbilityDef def)
        {
            var config = Abilities_Settings.Instance.GetConfig(def.defName);

            var ability = Abilities[def.defName];

            var level = pawnComp.GetOrAdd(def.defName).level;

            def.cooldownTicksRange.min = (int)config.CalculateField("cooldown", level, ability.Cooldown.min);
            def.cooldownTicksRange.max = (int)config.CalculateField("cooldown", level, ability.Cooldown.max);

            foreach (var verbKey in ability.verbs.Keys)
            {
                if (verbKey == "minRange")
                {
                    def.verbProperties.minRange = config.CalculateField("range", level, ability.verbs[verbKey]);
                }
                else if (verbKey == "range")
                {
                    def.verbProperties.range = config.CalculateField("range", level, ability.verbs[verbKey]);
                }
                else if (verbKey == "warmupTime")
                {
                    def.verbProperties.warmupTime = config.CalculateField("castTime", level, ability.verbs[verbKey]);
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
                    comp.hemogenCost = config.CalculateField("entropy", level, ability.comps[compKey].Value);

                    break;
                }
            }

            foreach (var statKey in ability.statBases.Keys) //I had wanted to use A StatPart but if you check Ability/AbilityDef it directly grabs the Value and doesn't run through the StatParts.
            {
                var stat = def.statBases[ability.statBases[statKey].Index];

                if (statKey == StatDefOf.Ability_Range)
                {
                    stat.value = config.CalculateField("range", level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_EffectRadius)
                {
                    stat.value = config.CalculateField("radius", level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_CastingTime)
                {
                    stat.value = config.CalculateField("castTime", level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_Duration)
                {
                    stat.value = config.CalculateField("duration", level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_PsyfocusCost)
                {
                    stat.value = config.CalculateField("psyfocus", level, ability.statBases[statKey].Value);
                }
                else if (statKey == StatDefOf.Ability_EntropyGain)
                {
                    stat.value = config.CalculateField("entropy", level, ability.statBases[statKey].Value);
                }
                else
                {
                    break;
                }
            }
        }

    }

    public class AbilityCache
    {
        public Dictionary<string, float> verbs;
        public Dictionary<string, IndexedCache> comps;
        public Dictionary<StatDef, IndexedCache> statBases;

        public IntRange Cooldown;
    }
}