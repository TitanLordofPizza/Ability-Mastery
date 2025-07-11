using Verse;

using Mastery.Core.Utility;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Mods.VPE
{
    public static class VPE_Ability_Psyfocus
    {
        public static void Postfix(object __instance, ref float __result, Pawn pawn)
        {
            var def = ClassUtility.GetField<Def>(__instance, "abilityDef");

            if (Abilities_Settings.Instance.ActiveOnThing(pawn, def.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(def.defName).PsyfocusCalculated
                    (comp.GetOrAdd(def.defName).Level, __result);
            }
        }
    }

    public static class VPE_Ability_Entropy
    {
        public static void Postfix(object __instance, ref float __result, Pawn pawn)
        {
            var def = ClassUtility.GetField<Def>(__instance, "abilityDef");

            if (Abilities_Settings.Instance.ActiveOnThing(pawn, def.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(def.defName).EntropyCalculated
                    (comp.GetOrAdd(def.defName).Level, __result);
            }
        }
    }
}