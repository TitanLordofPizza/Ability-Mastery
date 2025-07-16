using System.Linq;

using UnityEngine;
using Verse;

using Mastery.Core.Utility.UI;
using Mastery.Core.UI.Tabs;

using Mastery.Ability.Data;
using Mastery.Ability.Settings;

namespace Mastery.Ability.UI
{
    public class Ability_Tab : IMastery_Tab
    {
        public string Title { get => "Ability_Mastery_Tab"; }

        public void FillTab(Rect inRect, Pawn pawn)
        {
            if (Abilities_Settings.Instance.ActiveOnThing(pawn, out Ability_Mastery_Comp comp) == true)
            {
                var standard = new Listing_Standard();

                standard.Begin(inRect);

                standard.Gap(UIUtility.tinyUISpacing);

                var labelCapWidth = Text.CalcSize(comp.Entries.Keys.OrderByDescending(key => Abilities_Settings.Instance.GetLabelCap(key).Length).FirstOrDefault()).x;

                foreach (var entryKey in comp.Entries.Keys)
                {
                    UIUtility.LevelInfo(standard, entryKey, comp, labelCapWidth);
                    standard.Gap(UIUtility.tinyUISpacing);
                }

                standard.End();
            }
        }
    }
}