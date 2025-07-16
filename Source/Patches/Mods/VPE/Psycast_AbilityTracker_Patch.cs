using RimWorld.Planet;

using Mastery.Ability.Settings;

namespace Mastery.Ability.Patches.Mods.VPE
{
    public static class VPE_Ability_PostCast
    {
        public static void Postfix(object __instance, params GlobalTargetInfo[] targets)
        {
            if (Abilities_Settings.PsycastExp == true) //Is Psycast Exp enabled?
            {
#if v1_5
                var instance = __instance as VFECore.Abilities.Ability;
#else
                var instance = __instance as VEF.Abilities.Ability;
#endif

                var action = Abilities_Settings.GetPsycastExpGain(instance.def);

                var hediff = instance.pawn.health.hediffSet.GetFirstHediff<VanillaPsycastsExpanded.Hediff_PsycastAbilities>(); //Get Psycast Hediff.

                var level = hediff.level + 1;
                var evaluation = action.expGainCurve.Evaluate(level);

                hediff.GainExperience(evaluation); //Add EXP.
            }
        }
    }
}