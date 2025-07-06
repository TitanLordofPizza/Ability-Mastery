using System.Collections.Generic;

using UnityEngine;
using Verse;

using Mastery.Core.Utility;
using Mastery.Core.Utility.UI;

using Mastery.Core.Data.Level_Framework.Extensions;
using Mastery.Core.Settings.Level_Framework.Extensions;

using Mastery.Ability.Data;

namespace Mastery.Ability.Settings
{
    public class Abilities_Settings : Extension_Level_Settings<Ability_Mastery_Comp, Ability_Mastery_Extension>, IExposable
    {
        #region Data

        public static bool PsycastExp; //Does VPE Abilties Give Exp?
        public static Level_Action_Extension PsycastExpGainBase = new Level_Action_Extension() //How much Exp does VPE Abilities Give by default if there is no Extension?
        {
            LevelKey = "PsycastExp",
            ExpGainCurve = new UtilityCurve()
            {
                Curve = new SimpleCurve(new List<CurvePoint>()
                {
                    new CurvePoint(0, 0.01f),
                    new CurvePoint(300, 0.003f) //1940201600
                }),

                Percentage = true
            }
        };

        public Abilities_Settings()
        {
            Active = true;

            ExtensionBase = new Ability_Mastery_Extension()
            {
                TitleCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(20, 20)
                    })
                },
                ExpCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 1000),
                        new CurvePoint(9, 10000),
                        new CurvePoint(20, 32000)
                    })
                },

                rangeCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = false
                },
                radiusCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = false
                },

                castTimeCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },
                cooldownCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },
                durationCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },

                psyfocusCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },
                entropyCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                }
            };

            ActionBase = new Level_Action_Extension()
            {
                LevelKey = "AbilityMastery",
                ExpGainCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 250),
                        new CurvePoint(20, 250)
                    })
                }
            };
        }

        public static Level_Action_Extension GetPsycastExpGain(Def def)
        {
            Level_Action_Extension extension = null;

            foreach (var _extension in def.modExtensions)
            {
                if (_extension is Level_Action_Extension)
                {
                    var ex = _extension as Level_Action_Extension;
                    if (ex.LevelKey == "PsycastExp")
                    {
                        extension = ex;
                    }
                }
            }

            if (extension == null)
                extension = PsycastExpGainBase;

            return extension;
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref PsycastExp, "psycastExp");
            Scribe_Deep.Look(ref PsycastExpGainBase, "psycastExpGainBase");
        }

        #endregion

        #region Extension Settings

        public override string LevelKey => "AbilityMastery";

        private const string baseExtensionName = "masteryBase";

        public static Abilities_Settings Instance;

        public override void AddConfig(Def def)
        {
            base.AddConfig(def);

            isExpanded.Add(def.defName, false);
        }

        #region UI

        private Vector2 scrollPos;

        public void Window(Rect inRect)
        {
            Listing_Standard options = new Listing_Standard();

            options.Begin(inRect);

            if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Psycasts Expanded"))
            {
                options.Label("Vanilla Expanded Psycasts");

                options.CheckboxLabeled("Ability_Mastery_PsycastExp_Settings".Translate(), ref PsycastExp, "Ability_Mastery_Description_Settings".Translate());
            }

            options.CheckboxLabeled("Ability_Mastery_Settings".Translate(), ref Active, "Ability_Mastery_Description_Settings".Translate());

            options.End();

            //ListView Start

            //ListHeight Start
            float Height = 0;

            foreach (var expanded in isExpanded.Keys)
            {
                Height += Text.CalcHeight(expanded, options.ColumnWidth);

                if (isExpanded[expanded] == true)
                {
                    Height += GUIExpanded.smallUISpacing;

                    Height += (UtilityCurve.UIHeight + GUIExpanded.smallUISpacing) * 9;

                    Height += Text.CalcHeight("Title", options.ColumnWidth);
                    Height += Text.CalcHeight("Exp", options.ColumnWidth);

                    Height += Text.CalcHeight("Range", options.ColumnWidth);
                    Height += Text.CalcHeight("Radius", options.ColumnWidth);

                    Height += Text.CalcHeight("CastTime", options.ColumnWidth);
                    Height += Text.CalcHeight("Cooldown", options.ColumnWidth);
                    Height += Text.CalcHeight("Duration", options.ColumnWidth);

                    Height += Text.CalcHeight("Psyfocus", options.ColumnWidth);
                    Height += Text.CalcHeight("Entropy", options.ColumnWidth);
                }

                if (expanded != baseExtensionName)
                {
                    Height += GUIExpanded.mediumUISpacing + Text.CalcHeight("Override", options.ColumnWidth) + GUIExpanded.smallUISpacing;
                }
            }

            //ListHeight End

            //ListArea Start
            var outRect = new Rect(inRect.x, inRect.y + options.CurHeight, inRect.width, inRect.height - options.CurHeight); //outRect is where the entire ScrollView is.
            var viewRect = new Rect(inRect.x, inRect.y, inRect.width - GUIExpanded.mediumUISpacing, Height); //inRect is where the contents of the ScrollView is.

            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect, true);

            //ListArea End

            options.Begin(viewRect);

            MasteryItem(viewRect, options, baseExtensionName);

            foreach (var key in Configs.Keys)
            {
                MasteryItem(viewRect, options, key);
            }

            options.End();

            Widgets.EndScrollView();

            //ListView End
        }

        public Dictionary<string, bool> isExpanded = new Dictionary<string, bool>();

        public void MasteryItem(Rect viewRect, Listing_Standard options, string key)
        {
            if (key != baseExtensionName)
                options.GapLine(GUIExpanded.mediumUISpacing);

            bool foldoutIsExpanded = isExpanded[key];
            GUIExpanded.Foldout(options, key, ref foldoutIsExpanded);
            isExpanded[key] = foldoutIsExpanded;

            options.verticalSpacing = GUIExpanded.smallUISpacing;

            if (key != baseExtensionName)
            {
                options.CheckboxLabeled("Override", ref Configs[key].Override);

                if (Configs[key].Override == true)
                {
                    if (Configs[key].Value == null)
                    {
                        Configs[key].Value = ClassCopy.CopyClass(GetConfig(key));
                    }
                }
            }


            if (isExpanded[key] == true)
            {
                var masteryConfig = (key == baseExtensionName ? ExtensionBase : GetConfig(key));

                var active = (key == baseExtensionName ? false : Configs[key].Override);

                masteryConfig.TitleCurve.Editor(options, "Title", active: active);
                masteryConfig.ExpCurve.Editor(options, "Exp", active: active);

                masteryConfig.rangeCurve.Editor(options, "Range", active: active);
                masteryConfig.radiusCurve.Editor(options, "Radius", active: active);

                masteryConfig.castTimeCurve.Editor(options, "CastTime", active: active);
                masteryConfig.cooldownCurve.Editor(options, "Cooldown", active: active);
                masteryConfig.durationCurve.Editor(options, "Duration", active: active);

                masteryConfig.psyfocusCurve.Editor(options, "Psyfocus", active: active);
                masteryConfig.entropyCurve.Editor(options, "Entropy", active: active);
            }
        }

        #endregion

        public override void Initilize()
        {
            base.Initilize();

            Instance = this;

            if (isExpanded == null)
                isExpanded = new Dictionary<string, bool>();

            isExpanded.Add(baseExtensionName, false);
        }

        #endregion
    }
}