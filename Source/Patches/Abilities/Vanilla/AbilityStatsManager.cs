using System.Collections.Generic;

using RimWorld;
using Verse;

namespace Mastery.Ability.Patches.Vanilla
{
    public static class AbilityStatsManager
    {
        public static Dictionary<string, AbilityStats> Abilities = new Dictionary<string, AbilityStats>();

        public static void Add(AbilityDef def)
        {
            Abilities[def.defName] = new AbilityStats
            {
                verbs = new Dictionary<string, float>(),
                comps = new Dictionary<string, AbilityStats.StatIndex>(),
                statBases = new Dictionary<StatDef, AbilityStats.StatIndex>(),

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
                        Abilities[def.defName].comps["hemogenCost"] = new AbilityStats.StatIndex()
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
                        Abilities[def.defName].statBases.Add(stat.stat, new AbilityStats.StatIndex()
                        {
                            Index = index,
                            Value = stat.value
                        });
                    }

                    index++;
                }
            }
        }
    }

    public class AbilityStats
    {
        public Dictionary<string, float> verbs;
        public Dictionary<string, StatIndex> comps;
        public Dictionary<StatDef, StatIndex> statBases;

        public IntRange Cooldown;

        public class StatIndex
        {
            public int Index;
            public float Value;
        }
    }
}