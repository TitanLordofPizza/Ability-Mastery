using System.Collections.Generic;
using System;

using UnityEngine;
using RimWorld;
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

            expGainCurve = new UtilityCurve()
            {
                Curve = new SimpleCurve(new List<CurvePoint>()
                {
                    new CurvePoint(0, 0.01f),
                    new CurvePoint(300, 0.003f) //1940201600
                }),

                Percentage = true
            },
            expGainType = OperationType.Additive
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

                    Percentage = true
                },
                rangeType = OperationType.Additive,
                radiusCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },
                radiusType = OperationType.Additive,

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
                castTimeType = OperationType.Subtractive,
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
                cooldownType = OperationType.Subtractive,
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
                durationType = OperationType.Additive,

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
                psyfocusType = OperationType.Subtractive,
                entropyCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 0),
                        new CurvePoint(9, 0.2f),
                        new CurvePoint(20, 0.5f)
                    }),

                    Percentage = true
                },
                entropyType = OperationType.Subtractive
            };

            ActionBase = new Level_Action_Extension()
            {
                LevelKey = "AbilityMastery",

                expGainCurve = new UtilityCurve()
                {
                    Curve = new SimpleCurve(new List<CurvePoint>()
                    {
                        new CurvePoint(0, 250),
                        new CurvePoint(20, 250)
                    })
                },
                expGainType = OperationType.Additive
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

            isCollapsed.Add(def.defName, true);
            cachedNames.Add(def.defName, $"{def.LabelCap.RawText} ({def.defName})");
        }

        #region UI

        private string search;

        private Vector2 scrollPos;

        private Dictionary<string, bool> isCollapsed = new Dictionary<string, bool>();
        private Dictionary<string, string> cachedNames = new Dictionary<string, string>();

        public void Window(Rect inRect)
        {
            Listing_Standard options = new Listing_Standard();

            options.Begin(inRect);

            if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Psycasts Expanded"))
            {
                options.Label("Vanilla Expanded Psycasts");

                options.CheckboxLabeled("Ability_Mastery_PsycastExp_Settings".Translate(), ref PsycastExp, "Ability_Mastery_PsycastExp_Description_Settings".Translate());
            }

            options.CheckboxLabeled("Ability_Mastery_Settings".Translate(), ref Active, "Ability_Mastery_Description_Settings".Translate());

            options.CheckboxLabeled("Ability_Mastery_Tab_Settings".Translate(), ref TabActive, "Ability_Mastery_Tab_Description_Settings".Translate());

            search = options.TextEntryLabeled("Ability_Mastery_Searchbar_Settings".Translate(), search);

            options.End();

            #region List View

            var outRect = new Rect(inRect.x, inRect.y + options.CurHeight, inRect.width, inRect.height - options.CurHeight); //outRect is where the entire ScrollView is.
            var viewRect = new Rect(inRect.x, inRect.y, inRect.width - UIUtility.mediumUISpacing, 0); //inRect is where the contents of the ScrollView is.

            foreach (var isCollapsedKey in isCollapsed.Keys) //Calculate List Height.
            {
                if (cachedNames[isCollapsedKey].IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    viewRect.height += Text.CalcHeight(cachedNames[isCollapsedKey], options.ColumnWidth);

                    if (isCollapsed[isCollapsedKey] == false)
                    {
                        viewRect.height += UIUtility.smallUISpacing;

                        if (isCollapsedKey != baseExtensionName)
                        {
                            viewRect.height += Text.CalcHeight("Ability_Mastery_IsIgnored_Settings".Translate(), options.ColumnWidth);
                        }

                        viewRect.height += (UtilityCurve.UIHeight + UIUtility.smallUISpacing) * 9;

                        viewRect.height += Text.CalcHeight("Ability_Mastery_TitleCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_ExpCurve_Settings".Translate(), options.ColumnWidth);

                        viewRect.height += Text.CalcHeight("Ability_Mastery_RangeCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_RangeType_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_RadiusCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_RadiusType_Settings".Translate(), options.ColumnWidth);

                        viewRect.height += Text.CalcHeight("Ability_Mastery_CastTimeCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_CastTimeType_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_CooldownCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_CooldownType_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_DurationCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_DurationType_Settings".Translate(), options.ColumnWidth);

                        viewRect.height += Text.CalcHeight("Ability_Mastery_PsyfocusCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_PsyfocusType_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_EntropyCurve_Settings".Translate(), options.ColumnWidth);
                        viewRect.height += Text.CalcHeight("Ability_Mastery_EntropyType_Settings".Translate(), options.ColumnWidth);
                    }

                    if (isCollapsedKey != baseExtensionName)
                    {
                        viewRect.height += UIUtility.mediumUISpacing + Text.CalcHeight("Ability_Mastery_Override_Settings".Translate(), options.ColumnWidth) + UIUtility.smallUISpacing;
                    }
                }
            }

            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect, true);

            options.Begin(viewRect);

            if (baseExtensionName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MasteryItem(viewRect, options, baseExtensionName);
            }

            foreach (var key in Configs.Keys) //Create List.
            {
                if (isCollapsed.ContainsKey(key) == true && cachedNames[key].IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MasteryItem(viewRect, options, key);
                }
            }

            options.End();

            Widgets.EndScrollView();

            #endregion
        }

        public void MasteryItem(Rect viewRect, Listing_Standard standard, string key)
        {
            if (key != baseExtensionName)
                standard.GapLine(UIUtility.mediumUISpacing);

            bool foldoutIsCollapsed = isCollapsed[key];
            UIUtility.Foldout(standard, cachedNames[key], ref foldoutIsCollapsed);
            isCollapsed[key] = foldoutIsCollapsed;

            standard.verticalSpacing = UIUtility.smallUISpacing;

            if (key != baseExtensionName)
            {
                bool previousOverride = Configs[key].Override;

                standard.CheckboxLabeled("Ability_Mastery_Override_Settings".Translate(), ref Configs[key].Override);

                if (Configs[key].Override == true && previousOverride != true)
                {
                    if (Configs[key].Value == null)
                    {
                        Configs[key].Value = GetConfig(key).Duplicate();
                    }
                }
            }

            if (isCollapsed[key] == false)
            {
                var masteryConfig = (key == baseExtensionName ? ExtensionBase : GetConfig(key));

                var active = (key == baseExtensionName ? false : Configs[key].Override);

                if (key != baseExtensionName)
                {
                    standard.CheckboxLabeled("Ability_Mastery_IsIgnored_Settings".Translate(), ref masteryConfig.isIgnored);
                }

                var options = new List<string>
                {
                    "Mastery_Core_Additive".Translate(),
                    "Mastery_Core_Subtractive".Translate(),
                    "Mastery_Core_Multiplicative".Translate(),
                    "Mastery_Core_Divisive".Translate()
                };

                masteryConfig.TitleCurve.Editor(standard, "Ability_Mastery_TitleCurve_Settings".Translate(), active: active);
                masteryConfig.ExpCurve.Editor(standard, "Ability_Mastery_ExpCurve_Settings".Translate(), active: active);

                masteryConfig.rangeCurve.Editor(standard, "Ability_Mastery_RangeCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_RangeType_Settings".Translate(), (int)masteryConfig.rangeType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.rangeType = (OperationType)selected;
                    }
                });

                masteryConfig.radiusCurve.Editor(standard, "Ability_Mastery_RadiusCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_RadiusType_Settings".Translate(), (int)masteryConfig.radiusType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.radiusType = (OperationType)selected;
                    }
                });

                masteryConfig.castTimeCurve.Editor(standard, "Ability_Mastery_CastTimeCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_CastTimeType_Settings".Translate(), (int)masteryConfig.castTimeType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.castTimeType = (OperationType)selected;
                    }
                });

                masteryConfig.cooldownCurve.Editor(standard, "Ability_Mastery_CooldownCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_CooldownType_Settings".Translate(), (int)masteryConfig.cooldownType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.cooldownType = (OperationType)selected;
                    }
                });

                masteryConfig.durationCurve.Editor(standard, "Ability_Mastery_DurationCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_DurationType_Settings".Translate(), (int)masteryConfig.durationType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.durationType = (OperationType)selected;
                    }
                });

                masteryConfig.psyfocusCurve.Editor(standard, "Ability_Mastery_PsyfocusCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_PsyfocusType_Settings".Translate(), (int)masteryConfig.psyfocusType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.psyfocusType = (OperationType)selected;
                    }
                });

                masteryConfig.entropyCurve.Editor(standard, "Ability_Mastery_EntropyCurve_Settings".Translate(), active: active);
                UIUtility.Dropdown(standard, "Ability_Mastery_EntropyType_Settings".Translate(), (int)masteryConfig.entropyType, options, (int selected) =>
                {
                    if (active == true)
                    {
                        masteryConfig.entropyType = (OperationType)selected;
                    }
                });
            }
        }

        #endregion

        public override void Initilize()
        {
            base.Initilize();

            Instance = this;

            if (isCollapsed == null)
                isCollapsed = new Dictionary<string, bool>();

            if (cachedNames == null)
                cachedNames = new Dictionary<string, string>();

            isCollapsed.Add(baseExtensionName, true);
            cachedNames.Add(baseExtensionName, baseExtensionName);
        }

        #endregion
    }
}