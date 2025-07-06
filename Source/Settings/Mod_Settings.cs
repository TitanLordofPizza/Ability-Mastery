using UnityEngine;
using Verse;

namespace Mastery.Ability.Settings
{
    public class Mod_Settings : ModSettings
    {
        public Abilities_Settings Data = new Abilities_Settings();

        public void Window(Rect inRect) //Showing The Settings.
        {
            Data.Window(inRect);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref Data, "data");
        }
    }
}