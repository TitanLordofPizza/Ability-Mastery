using System.Collections.Generic;

using RimWorld.Planet;
using RimWorld;
using Verse;

using HarmonyLib;

using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Extensions;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

using Mastery.Ability.Patches.Vanilla;
using Mastery.Ability.Patches.Mods.VFE;
using Mastery.Ability.Patches.Mods.VPE;

namespace Mastery.Ability.Patches
{
    public static class Ability_Patches
    {
        public static void Patch()
        {
            var harmony = Mod_Ability_Mastery.harmony;

            var defs = DefDatabase<AbilityDef>.AllDefsListForReading; //Get all Abilities.

            Mod_Ability_Mastery.settings.Data.Initilize();

            foreach (var def in defs) //Adds Ability Configs to Mastery.
            {
                AbilityCacheManager.Add(def);

                Abilities_Settings.Instance.AddConfig(def); //Add Ability.
            }

            try
            {
                harmony.Patch(typeof(PawnGenerator).Method("GenerateNewPawnInternal"), postfix: new HarmonyMethod(typeof(PawnGenerator_Patch), nameof(PawnGenerator_Patch.Postfix)));

                harmony.Patch(typeof(Level_Comp_Manager).Method(nameof(Level_Comp_Manager.ActionEvent)), postfix: new HarmonyMethod(typeof(Comp_Manager_ActionEvent_Patch), nameof(Comp_Manager_ActionEvent_Patch.Postfix)));

                harmony.Patch(typeof(Pawn_AbilityTracker).Method(nameof(Pawn_AbilityTracker.GainAbility)), prefix: new HarmonyMethod(typeof(AbilityTracker_GainAbility), nameof(AbilityTracker_GainAbility.Prefix)));
                harmony.Patch(typeof(Pawn_AbilityTracker).Method(nameof(Pawn_AbilityTracker.RemoveAbility)), prefix: new HarmonyMethod(typeof(AbilityTracker_RemoveAbility), nameof(AbilityTracker_RemoveAbility.Prefix)));

                harmony.Patch(typeof(Pawn_AbilityTracker).Method(nameof(Pawn_AbilityTracker.GetAbility)), postfix: new HarmonyMethod(typeof(AbilityTracker_GetAbility), nameof(AbilityTracker_GetAbility.Postfix)));

                harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.Initialize)), postfix: new HarmonyMethod(typeof(Ability_Initialize), nameof(Ability_Initialize.Postfix)));

                harmony.Patch(typeof(Ability_Mastery_Comp).Method(nameof(Ability_Mastery_Comp.GainExperience)), postfix: new HarmonyMethod(typeof(Ability_ExpGain), nameof(Ability_ExpGain.Postfix)));

                harmony.Patch(typeof(AbilityDef).Method(nameof(AbilityDef.GetTooltip)), postfix: new HarmonyMethod(typeof(Ability_Description), nameof(Ability_Description.Postfix)));

                if (Action_Manager.AddAction("Ability", "Activate") == false)  // Does This ActionPoint Already Exist?
                {
                    harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.Activate), new System.Type[] { typeof(LocalTargetInfo), typeof(LocalTargetInfo) }), postfix: new HarmonyMethod(typeof(Ability_ActivateLocal), nameof(Ability_ActivateLocal.Postfix)));

                    harmony.Patch(typeof(RimWorld.Ability).Method(nameof(RimWorld.Ability.Activate), new System.Type[] { typeof(GlobalTargetInfo) }), postfix: new HarmonyMethod(typeof(Ability_ActivateGlobal), nameof(Ability_ActivateGlobal.Postfix)));
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Rimworld Abilities for Mastery. " + ex);
            }

            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Expanded Framework"))
                {
                    VFE_Defs_Patch.LoadDefs(); //Adds Ability Configs to Mastery.

#if v1_5
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.Ability");

                    harmony.Patch(GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.CompAbilities").GetMethod("GiveAbility"), postfix: new HarmonyMethod(typeof(VFE_Ability_Gain), nameof(VFE_Ability_Gain.Postfix))); //Adds Abilities to Mastery Comp.
#else
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VEF.Abilities.Ability");

                    harmony.Patch(GenTypes.GetTypeInAnyAssembly("VEF.Abilities.CompAbilities").GetMethod("GiveAbility"), postfix: new HarmonyMethod(typeof(VFE_Ability_Gain), nameof(VFE_Ability_Gain.Postfix))); //Adds Abilities to Mastery Comp.
#endif

                    harmony.Patch(AbilityType.GetMethod("GetDescriptionForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_Description), nameof(VFE_Ability_Description.Postfix))); //Gives Mastery Level and Exp of Ability.

                    harmony.Patch(AbilityType.Method("GetRangeForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_Range), nameof(VFE_Ability_Range.Postfix))); //Sends the Updated Range of a Ability using Mastery.

                    harmony.Patch(AbilityType.Method("GetRadiusForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_Radius), nameof(VFE_Ability_Radius.Postfix))); //Sends the Updated Radius of a Ability using Mastery.

                    harmony.Patch(AbilityType.Method("GetCastTimeForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_CastTime), nameof(VFE_Ability_CastTime.Postfix))); //Sends the Updated CastTime of a Ability using Mastery.

                    harmony.Patch(AbilityType.Method("GetCooldownForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_Cooldown), nameof(VFE_Ability_Cooldown.Postfix))); //Sends the Updated Cooldown of a Ability using Mastery.

                    harmony.Patch(AbilityType.Method("GetDurationForPawn"), postfix: new HarmonyMethod(typeof(VFE_Ability_Duration), nameof(VFE_Ability_Duration.Postfix))); //Sends the Updated Duration of a Ability using Mastery.

                    if (Action_Manager.AddAction("Ability", "PostCast") == false)  // Does This ActionPoint Already Exist?
                    {
                        harmony.Patch(AbilityType.Method("PostCast"), postfix: new HarmonyMethod(typeof(VFE_Ability_PostCast), nameof(VFE_Ability_PostCast.Postfix))); //Sends Level Action Extensions to Level Comp Manager each time the Ability is Used.
                    }
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
#if v1_5
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VFECore.Abilities.Ability");
#else
                    var AbilityType = GenTypes.GetTypeInAnyAssembly("VEF.Abilities.Ability");
#endif

                    var PsycastType = GenTypes.GetTypeInAnyAssembly("VanillaPsycastsExpanded.AbilityExtension_Psycast");

                    harmony.Patch(PsycastType.Method("GetPsyfocusUsedByPawn"), postfix: new HarmonyMethod(typeof(VPE_Ability_Psyfocus), nameof(VPE_Ability_Psyfocus.Postfix))); //Sends the Updated Psyfocus of a Ability using Mastery.

                    harmony.Patch(PsycastType.Method("GetEntropyUsedByPawn"), postfix: new HarmonyMethod(typeof(VPE_Ability_Entropy), nameof(VPE_Ability_Entropy.Postfix))); //Sends the Updated Entropy of a Ability using Mastery.

                    harmony.Patch(AbilityType.Method("PostCast"), postfix: new HarmonyMethod(typeof(VPE_Ability_PostCast), nameof(VPE_Ability_PostCast.Postfix))); //Gives Psycast Exp each time the Ability is Used.
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Vanilla Psycasts Expanded Abilities for Mastery. " + ex);
            }
        }

        public static class Comp_Manager_ActionEvent_Patch
        {
            public static void Postfix(Level_Comp_Manager __instance, List<string> __result, string actionType, Def def, Dictionary<string, object> states = null)
            {
                if (actionType == "Ability" && __result.Contains(Abilities_Settings.Instance.LevelKey) == false) //Does it already have a ActionExtension?
                {
                    if (Abilities_Settings.Instance.ActiveOnThing(__instance.parent, out Ability_Mastery_Comp comp) == true) //Is Mastery enabled?
                    {
                        comp.ActionEvent(def, Abilities_Settings.Instance.ActionBase, states);
                    }
                }
            }
        }
    }
}