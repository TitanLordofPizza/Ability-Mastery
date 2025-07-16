using System.Reflection;

using RimWorld.Planet;
using RimWorld;
using Verse;

using Mastery.Core.Utility;
using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Extensions;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Vanilla
{
    public static class Ability_Gain
    {
        public static void Postfix(Pawn_AbilityTracker __instance, AbilityDef def) //Adding Ability.
        {
            if (__instance.pawn.HasComp<Ability_Mastery_Comp>())
            {
                var comp = __instance.pawn.GetComp<Ability_Mastery_Comp>();

                comp.GetOrAdd(def.defName); //Adding Ability.

                if (Abilities_Settings.Instance.Active) //Is Mastery enabled?
                {
                    if (Abilities_Settings.Instance.ActiveConfig(def.defName) == true) //Is This Not Ignored?
                    {
                        var ability = __instance.GetAbility(def);

                        ability.def = ClassUtility.CopyClass(ability.def);

                        AbilityCacheManager.AllocateStats(comp, ability.def);
                    }
                }
            }
        }
    }

    public static class Ability_ExposeData
    {
        public static void Postfix(RimWorld.Ability __instance)
        {
            if (__instance.def != null)
            {
                if (Abilities_Settings.Instance.ActiveOnThing(__instance.pawn, __instance.def.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
                {
                    __instance.def = ClassUtility.CopyClass(__instance.def);

                    AbilityCacheManager.AllocateStats(comp, __instance.def);
                }
            }
        }
    }

    public static class Ability_ExpGain
    {
        public static void Postfix(Ability_Mastery_Comp __instance, ref bool __result, Def def, Level_Action_Extension action)
        {
            if (__result == true) //Has it Leveled Up?
            {
                if (def.GetType() == typeof(AbilityDef) || def.GetType().BaseType == typeof(AbilityDef))
                {
                    var abilityDef = (AbilityDef)def;

                    typeof(AbilityDef).GetField("cachedTooltip", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(abilityDef, null); //Clears away the cachedTooltip so that the new stats can be shown.

                    AbilityCacheManager.AllocateStats(__instance, abilityDef);
                }
            }
        }
    }

    public static class Ability_Description
    {
        public static void Postfix(AbilityDef __instance, ref string __result, Pawn pawn)
        {
            if (Abilities_Settings.Instance.ActiveOnThing(pawn, __instance.defName, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
            {
                var ability = comp.GetOrAdd(__instance.defName);

                var title = __instance.LabelCap.Colorize(ColoredText.TipSectionTitleColor);

                __result = __result.Replace(title, title + " - " + Abilities_Settings.Instance.GetConfig(__instance.defName).MasteryCalculated(ability.level, ability.exp));
            }
        }
    }

    public static class Ability_ActivateLocal
    {
        public static void Postfix(RimWorld.Ability __instance, LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (Abilities_Settings.Instance.ActiveConfig(__instance.def.defName) == true) //Is This Not Ignored?
            {
                if (__instance.pawn.HasComp<Level_Comp_Manager>())
                {
                    __instance.pawn.GetComp<Level_Comp_Manager>().ActionEvent("Ability", __instance.def);
                }
            }
        }
    }

    public static class Ability_ActivateGlobal
    {
        public static void Postfix(RimWorld.Ability __instance, GlobalTargetInfo target)
        {
            if (Abilities_Settings.Instance.ActiveConfig(__instance.def.defName) == true) //Is This Not Ignored?
            {
                if (__instance.pawn.HasComp<Level_Comp_Manager>())
                {
                    __instance.pawn.GetComp<Level_Comp_Manager>().ActionEvent("Ability", __instance.def);
                }
            }
        }
    }
}