using System.Collections.Generic;

using RimWorld.Planet;
using RimWorld;
using Verse;

using HarmonyLib;

using Mastery.Core.Data.Level_Framework.Comps;

using Mastery.Ability.Data;
using Mastery.Ability.Patches.Mods.VCE;
using Mastery.Ability.Patches.Mods.VPE;
using Mastery.Ability.Patches.Vanilla;
using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches
{
    public static class Ability_Patches
    {
        public static void Patch()
        {
            var harmony = Mod_Mastery_Ability.harmony;

            var defs = DefDatabase<AbilityDef>.AllDefsListForReading; //Get all Abilities.

            Mod_Mastery_Ability.settings.Data.Initilize();

            foreach (var def in defs) //Adds Ability Configs to Proficiency.
            {
                AbilityStatsManager.Add(def);

                Abilities_Settings.Instance.AddConfig(def); //Add Ability.
            }

            try
            {
                harmony.Patch(typeof(PawnGenerator).Method("GenerateNewPawnInternal"), postfix: new HarmonyMethod(typeof(PawnGenerator_Patch), nameof(PawnGenerator_Patch.Postfix)));

                harmony.Patch(typeof(Level_Comp_Manager).Method(nameof(Level_Comp_Manager.ActionEvent)), postfix: new HarmonyMethod(typeof(Comp_Manager_ActionEvent_Patch), nameof(Comp_Manager_ActionEvent_Patch.Postfix)));

                harmony.Patch(typeof(Pawn_AbilityTracker).Method(nameof(Pawn_AbilityTracker.GainAbility)), postfix: new HarmonyMethod(typeof(Ability_Gain), nameof(Ability_Gain.Postfix)));

                harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.ExposeData)), postfix: new HarmonyMethod(typeof(Ability_ExposeData), nameof(Ability_ExposeData.Postfix)));

                harmony.Patch(typeof(Ability_Mastery_Comp).Method(nameof(Ability_Mastery_Comp.GainExperience)), postfix: new HarmonyMethod(typeof(Ability_ExpGain), nameof(Ability_ExpGain.Postfix)));

                harmony.Patch(typeof(AbilityDef).Method(nameof(AbilityDef.GetTooltip)), postfix: new HarmonyMethod(typeof(Ability_Description), nameof(Ability_Description.Postfix)));
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Rimworld Abilities for Mastery. " + ex);
            }

            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Expanded Framework"))
                {
                    VCE_Defs_Patch.LoadDefs(); //Adds Ability Configs to Proficiency.

                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.Ability");

                    harmony.Patch(GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.CompAbilities").GetMethod("GiveAbility"), postfix: new HarmonyMethod(typeof(VCE_Ability_Gain), nameof(VCE_Ability_Gain.Postfix))); //Adds Abilities to Proficiency Comp.

                    harmony.Patch(AbilityType.GetMethod("GetDescriptionForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_Description), nameof(VCE_Ability_Description.Postfix))); //Gives Proficiency Level and Exp of Ability.

                    harmony.Patch(AbilityType.Method("GetRangeForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_Range), nameof(VCE_Ability_Range.Postfix))); //Sends the Updated Range of a Ability using Proficiency.

                    harmony.Patch(AbilityType.Method("GetRadiusForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_Radius), nameof(VCE_Ability_Radius.Postfix))); //Sends the Updated Radius of a Ability using Proficiency.

                    harmony.Patch(AbilityType.Method("GetCastTimeForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_CastTime), nameof(VCE_Ability_CastTime.Postfix))); //Sends the Updated CastTime of a Ability using Proficiency.

                    harmony.Patch(AbilityType.Method("GetCooldownForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_Cooldown), nameof(VCE_Ability_Cooldown.Postfix))); //Sends the Updated Cooldown of a Ability using Proficiency.

                    harmony.Patch(AbilityType.Method("GetDurationForPawn"), postfix: new HarmonyMethod(typeof(VCE_Ability_Duration), nameof(VCE_Ability_Duration.Postfix))); //Sends the Updated Duration of a Ability using Proficiency.
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Vanilla Expanded Framework Abilities for Mastery. " + ex);
            }

            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Psycasts Expanded"))
                {
                    var PsycastType = GenTypes.GetTypeInAnyAssembly("VanillaPsycastsExpanded.AbilityExtension_Psycast");

                    harmony.Patch(PsycastType.Method("GetPsyfocusUsedByPawn"), postfix: new HarmonyMethod(typeof(VPE_Ability_Psyfocus), nameof(VPE_Ability_Psyfocus.Postfix))); //Sends the Updated Psyfocus of a Ability using Proficiency.

                    harmony.Patch(PsycastType.Method("GetEntropyUsedByPawn"), postfix: new HarmonyMethod(typeof(VPE_Ability_Entropy), nameof(VPE_Ability_Entropy.Postfix))); //Sends the Updated Entropy of a Ability using Proficiency.
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Vanilla Psycasts Expanded Abilities for Mastery. " + ex);
            }

            try
            {
                harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.Activate), new System.Type[] { typeof(LocalTargetInfo), typeof(LocalTargetInfo) }), postfix: new HarmonyMethod(typeof(Ability_ActivateLocal), nameof(Ability_ActivateLocal.Postfix)));

                harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.Activate), new System.Type[] { typeof(GlobalTargetInfo) }), postfix: new HarmonyMethod(typeof(Ability_ActivateGlobal), nameof(Ability_ActivateGlobal.Postfix)));
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Rimworld for Vanilla Ability Activation. " + ex);
            }

            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Expanded Framework"))
                {
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.Ability");

                    harmony.Patch(AbilityType.Method("PostCast"), postfix: new HarmonyMethod(typeof(VCE_Ability_PostCast), nameof(VCE_Ability_PostCast.Postfix))); //Sends Level Action Extensions to Level Comp Manager each time the Ability is Used.
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Vanilla Expanded Framework for Abilities. " + ex);
            }

            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Psycasts Expanded"))
                {
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.Ability");

                    harmony.Patch(AbilityType.Method("PostCast"), postfix: new HarmonyMethod(typeof(VPE_Ability_PostCast), nameof(VPE_Ability_PostCast.Postfix))); //Gives Psycast Exp each time the Ability is Used.
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Vanilla Psycasts Expanded for Abilities. " + ex);
            }
        }

        public static class Comp_Manager_ActionEvent_Patch
        {
            public static void Postfix(Level_Comp_Manager __instance, List<string> __result, string actionType, Def def, Dictionary<string, object> states = null)
            {
                if (actionType == "Ability" && __result.Contains(Abilities_Settings.Instance.LevelKey) == false)
                {
                    Ability_Mastery_Comp comp = null;
                    if (Abilities_Settings.Instance.ActiveOnThing(__instance.parent, out comp) == true) //Is Proficiency enabled?
                    {
                        comp.ActionEvent(def, Abilities_Settings.Instance.ActionBase, states);
                    }
                }
            }
        }
    }
}