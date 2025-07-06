using RimWorld.Planet;
using Verse;

using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Data.Extensions;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Mods.VCE
{
    public static class VCE_Defs_Patch
    {
        public static void LoadDefs()
        {
            var vce_defs = DefDatabase<VFECore.Abilities.AbilityDef>.AllDefsListForReading; //Get all Abilities.

            foreach (var def in vce_defs)
            {
                Abilities_Settings.Instance.AddConfig(def); //Add Ability.
            }
        }
    }

    public static class VCE_Ability_Gain
    {
        public static void Postfix(object __instance, object abilityDef) //Adding Ability.
        {
            var instance = __instance as VFECore.Abilities.CompAbilities;

            if (instance.Pawn.HasComp<Ability_Mastery_Comp>())
            {
                var def = abilityDef as VFECore.Abilities.AbilityDef;
                instance.Pawn.GetComp<Ability_Mastery_Comp>().GetOrAdd(def.defName); //Adding Ability.
            }
        }
    }

    public static class VCE_Ability_Description
    {
        public static void Postfix(object __instance, ref string __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Mastery enabled?
                {
                    var ability = comp.GetOrAdd(instance.def.defName);

                    var title = instance.def.LabelCap.Colorize(ColoredText.TipSectionTitleColor);

                    __result = __result.Replace(title, title + " - " + Abilities_Settings.Instance.GetConfig(instance.def.defName).MasteryCalculated(ability.Level, ability.Exp));
                }
            }
        }
    }

    public static class VCE_Ability_Range
    {
        public static void Postfix(object __instance, ref float __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Mastery enabled?
                {
                    __result = Abilities_Settings.Instance.GetConfig(instance.def.defName).RangeCalculated
                        (comp.GetOrAdd(instance.def.defName).Level, __result);
                }
            }
        }
    }

    public static class VCE_Ability_Radius
    {
        public static void Postfix(object __instance, ref float __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Proficiency enabled?
                {
                    __result = Abilities_Settings.Instance.GetConfig(instance.def.defName).RadiusCalculated
                        (comp.GetOrAdd(instance.def.defName).Level, __result);
                }
            }
        }
    }

    public static class VCE_Ability_CastTime
    {
        public static void Postfix(object __instance, ref int __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Proficiency enabled?
                {
                    __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).CastTimeCalculated
                        (comp.GetOrAdd(instance.def.defName).Level, __result);
                }
            }
        }
    }

    public static class VCE_Ability_Cooldown
    {
        public static void Postfix(object __instance, ref int __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Proficiency enabled?
                {
                    __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).CooldownCalculated
                        (comp.GetOrAdd(instance.def.defName).Level, __result);
                }
            }
        }
    }

    public static class VCE_Ability_Duration
    {
        public static void Postfix(object __instance, ref int __result)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                Ability_Mastery_Comp comp = null;
                if (Abilities_Settings.Instance.ActiveOnThing(instance.pawn, out comp) == true) //Is Proficiency enabled?
                {
                    __result = (int)Abilities_Settings.Instance.GetConfig(instance.def.defName).DurationCalculated
                        (comp.GetOrAdd(instance.def.defName).Level, __result);
                }
            }
        }
    }

    public static class VCE_Ability_PostCast
    {
        public static void Postfix(object __instance, params GlobalTargetInfo[] targets)
        {
            var instance = __instance as VFECore.Abilities.Ability;

            if (Mastery_Mod_Extension.IsIgnored(instance.def) == false) //Is This Ignored?
            {
                if (instance.pawn.HasComp<Level_Comp_Manager>())
                {
                    instance.pawn.GetComp<Level_Comp_Manager>().ActionEvent("Ability", instance.def);
                }
            }
        }
    }
}