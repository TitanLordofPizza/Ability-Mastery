using Verse;

using Mastery.Core.Utility;
using Mastery.Core.Data.Level_Framework.Extensions;

namespace Mastery.Ability.Data
{
    public class Ability_Mastery_Extension : Level_Effect_Extension, IDuplicable<Ability_Mastery_Extension>
    {
        public UtilityCurve rangeCurve; //This is how much Range is Increased Per Level.
        public UtilityCurve radiusCurve; //This is how much Radius is Increased Per Level.

        public UtilityCurve castTimeCurve; //This is how much CastTime is Decreased Per Level.
        public UtilityCurve cooldownCurve; //This is how much Cooldown is Decreased Per Level.
        public UtilityCurve durationCurve; //This is how much Duration is Increased Per Level.

        public UtilityCurve psyfocusCurve; //This is how much Psyfocus is Decreased Per Level.
        public UtilityCurve entropyCurve; //This is how much Entropy is Decreased Per Level.

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref rangeCurve, "rangeCurve");
            Scribe_Deep.Look(ref radiusCurve, "radiusCurve");

            Scribe_Deep.Look(ref castTimeCurve, "castTimeCurve");
            Scribe_Deep.Look(ref cooldownCurve, "cooldownCurve");
            Scribe_Deep.Look(ref durationCurve, "durationCurve");

            Scribe_Deep.Look(ref psyfocusCurve, "psyfocusCurve");
            Scribe_Deep.Look(ref entropyCurve, "entropyCurve");
        }

        public float RangeCalculated(int Level, float Base)
        {
            var evaluation = rangeCurve.Evaluate(Level);
            return Base + (rangeCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float RadiusCalculated(int Level, float Base)
        {
            var evaluation = radiusCurve.Evaluate(Level);
            return Base + (radiusCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float CastTimeCalculated(int Level, float Base)
        {
            var evaluation = castTimeCurve.Evaluate(Level);
            return Base - (castTimeCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float CooldownCalculated(int Level, float Base)
        {
            var evaluation = cooldownCurve.Evaluate(Level);
            return Base - (cooldownCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float DurationCalculated(int Level, float Base)
        {
            var evaluation = durationCurve.Evaluate(Level);
            return Base + (durationCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float PsyfocusCalculated(int Level, float Base)
        {
            var evaluation = psyfocusCurve.Evaluate(Level);
            return Base - (psyfocusCurve.Percentage ? Base * evaluation : evaluation);
        }

        public float EntropyCalculated(int Level, float Base)
        {
            var evaluation = entropyCurve.Evaluate(Level);
            return Base - (entropyCurve.Percentage ? Base * evaluation : evaluation);
        }

        public void CopyTo(Ability_Mastery_Extension target)
        {
            base.CopyTo(target);

            target.rangeCurve = rangeCurve.Duplicate();
            target.radiusCurve = radiusCurve.Duplicate();

            target.castTimeCurve = castTimeCurve.Duplicate();
            target.cooldownCurve = cooldownCurve.Duplicate();
            target.durationCurve = durationCurve.Duplicate();

            target.psyfocusCurve = psyfocusCurve.Duplicate();
            target.entropyCurve = entropyCurve.Duplicate();
        }

        public Ability_Mastery_Extension Duplicate()
        {
            var duplicate = new Ability_Mastery_Extension();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}