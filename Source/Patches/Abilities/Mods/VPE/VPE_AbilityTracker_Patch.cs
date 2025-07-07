using Verse;

using Mastery.Core.Data.Level_Framework.Data.Extensions;

using Mastery.Ability.Settings;
using Mastery.Ability.Data;

namespace Mastery_Core.Patches.Mods.VPE
{
    public static class VPE_Ability_Psyfocus
    {
        public static void Postfix(object __instance, ref float __result, Pawn pawn)
        {
            var instance = __instance as VanillaPsycastsExpanded.AbilityExtension_Psycast;

            Ability_Mastery_Comp comp = null;
            if (Abilities_Settings.Instance.ActiveOnThing(pawn, instance.abilityDef.defName, out comp) == true) //Is Mastery enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(instance.abilityDef.defName).PsyfocusCalculated
                    (comp.GetOrAdd(instance.abilityDef.defName).Level, __result);
            }
        }
    }

    public static class VPE_Ability_Entropy
    {
        public static void Postfix(object __instance, ref float __result, Pawn pawn)
        {
            var instance = __instance as VanillaPsycastsExpanded.AbilityExtension_Psycast;

            Ability_Mastery_Comp comp = null;
            if (Abilities_Settings.Instance.ActiveOnThing(pawn, instance.abilityDef.defName, out comp) == true) //Is Mastery enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(instance.abilityDef.defName).EntropyCalculated
                    (comp.GetOrAdd(instance.abilityDef.defName).Level, __result);
            }
        }
    }
}