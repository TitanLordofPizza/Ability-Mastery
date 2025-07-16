using RimWorld.Planet;
using Verse;

using Mastery.Core.Data.Level_Framework.Comps;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Mods.VFE
{
    public static class VFE_Defs_Patch
    {
        public static void LoadDefs()
        {
#if v1_5
            var defs = DefDatabase<VFECore.Abilities.AbilityDef>.AllDefsListForReading; //Get all Abilities.

            foreach (var def in defs)
            {
                Abilities_Settings.Instance.AddConfig(def); //Add Ability.
            }
#else
            var defs = DefDatabase<VEF.Abilities.AbilityDef>.AllDefsListForReading; //Get all Abilities.

            foreach (var def in defs)
            {
                Abilities_Settings.Instance.AddConfig(def); //Add Ability.
            }
#endif
        }
    }

    public static class VFE_Ability_Gain
    {
        public static void Postfix(object __instance, object abilityDef) //Adding Ability.
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.CompAbilities;
#else
            var instance = __instance as VEF.Abilities.CompAbilities;
#endif

            if (instance.Pawn.HasComp<Ability_Mastery_Comp>())
            {
                instance.Pawn.GetComp<Ability_Mastery_Comp>().GetOrAdd(((Def)abilityDef).defName); //Adding Ability.
            }
        }
    }

    public static class VFE_Ability_Description
    {
        public static void Postfix(object __instance, ref string __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
            {
                var ability = comp.GetOrAdd(instance.def.defName);

                var title = instance.def.LabelCap.Colorize(ColoredText.TipSectionTitleColor);

                __result = __result.Replace(title, title + " - " + Abilities_Settings.Instance.GetConfig(instance.def.defName).MasteryCalculated(ability.level, ability.exp));
            }
        }
    }

    public static class VFE_Ability_Range
    {
        public static void Postfix(object __instance, ref float __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(instance.def.defName).CalculateField("range", comp.GetOrAdd(instance.def.defName).level, __result);
            }
        }
    }

    public static class VFE_Ability_Radius
    {
        public static void Postfix(object __instance, ref float __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Proficiency enabled?
            {
                __result = Abilities_Settings.Instance.GetConfig(instance.def.defName).CalculateField("radius", comp.GetOrAdd(instance.def.defName).level, __result);
            }
        }
    }

    public static class VFE_Ability_CastTime
    {
        public static void Postfix(object __instance, ref int __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Proficiency enabled?
            {
                __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).CalculateField("castTime", comp.GetOrAdd(instance.def.defName).level, __result);
            }
        }
    }

    public static class VFE_Ability_Cooldown
    {
        public static void Postfix(object __instance, ref int __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Proficiency enabled?
            {
                __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).CalculateField("cooldown", comp.GetOrAdd(instance.def.defName).level, __result);
            }
        }
    }

    public static class VFE_Ability_Duration
    {
        public static void Postfix(object __instance, ref int __result)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Proficiency enabled?
            {
                __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).CalculateField("duration", comp.GetOrAdd(instance.def.defName).level, __result);
            }
        }
    }

    public static class VFE_Ability_PostCast
    {
        public static void Postfix(object __instance, params GlobalTargetInfo[] targets)
        {
#if v1_5
            var instance = __instance as VFECore.Abilities.Ability;
#else
            var instance = __instance as VEF.Abilities.Ability;
#endif

            if (Abilities_Settings.Instance.ActiveConfig(instance.def.defName) == true) //Is This Not Ignored?
            {
                if (instance.pawn.HasComp<Level_Comp_Manager>())
                {
                    instance.pawn.GetComp<Level_Comp_Manager>().ActionEvent("Ability", instance.def);
                }
            }
        }
    }
}