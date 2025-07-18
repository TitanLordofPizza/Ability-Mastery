﻿using UnityEngine;
using Verse;

using HarmonyLib;

using Mastery.Ability.Patches;
using Mastery.Ability.Settings;

namespace Mastery.Ability
{
    [StaticConstructorOnStartup]
    public class Setup
    {
        static Setup()
        {
            #region Patches

            try
            {
                Ability_Patches.Patch();
            }
            catch (System.Exception ex)
            {
                Log.Error("Ability Mastery Failed to Patch Abilities. " + ex);
            }

            Mod_Ability_Mastery.harmony.PatchAll();

            Log.Message("Ability Mastery Loaded.");

            #endregion
        }
    }

    public class Mod_Ability_Mastery : Mod
    {
        public static Harmony harmony;

        public static Mod_Settings settings;

        public Mod_Ability_Mastery(ModContentPack content) : base(content)
        {
            settings = GetSettings<Mod_Settings>();

            harmony = new Harmony(content.PackageIdPlayerFacing);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.Window(inRect);
        }

        public override string SettingsCategory()
        {
            return "Ability Mastery";
        }
    }
}