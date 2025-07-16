using Verse;

using Mastery.Core.Utility;
using Mastery.Core.Data.Level_Framework.Extensions;

namespace Mastery.Ability.Data
{
    public class Ability_Mastery_Extension : Level_Effect_Extension, IDuplicable<Ability_Mastery_Extension>
    {
        public UtilityCurve rangeCurve; //This is how much Range is Increased Per Level.
        public OperationType rangeType;
        public UtilityCurve radiusCurve; //This is how much Radius is Increased Per Level.
        public OperationType radiusType;

        public UtilityCurve castTimeCurve; //This is how much CastTime is Decreased Per Level.
        public OperationType castTimeType;
        public UtilityCurve cooldownCurve; //This is how much Cooldown is Decreased Per Level.
        public OperationType cooldownType;
        public UtilityCurve durationCurve; //This is how much Duration is Increased Per Level.
        public OperationType durationType;

        public UtilityCurve psyfocusCurve; //This is how much Psyfocus is Decreased Per Level.
        public OperationType psyfocusType;
        public UtilityCurve entropyCurve; //This is how much Entropy is Decreased Per Level.
        public OperationType entropyType;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref rangeCurve, "rangeCurve");
            Scribe_Values.Look(ref rangeType, "radiusType");
            Scribe_Deep.Look(ref radiusCurve, "radiusCurve");
            Scribe_Values.Look(ref radiusType, "radiusType");

            Scribe_Deep.Look(ref castTimeCurve, "castTimeCurve");
            Scribe_Values.Look(ref castTimeType, "castTimeType");
            Scribe_Deep.Look(ref cooldownCurve, "cooldownCurve");
            Scribe_Values.Look(ref cooldownType, "cooldownType");
            Scribe_Deep.Look(ref durationCurve, "durationCurve");
            Scribe_Values.Look(ref durationType, "durationType");

            Scribe_Deep.Look(ref psyfocusCurve, "psyfocusCurve");
            Scribe_Values.Look(ref psyfocusType, "psyfocusType");
            Scribe_Deep.Look(ref entropyCurve, "entropyCurve");
            Scribe_Values.Look(ref entropyType, "entropyType");
        }

        public void CopyTo(Ability_Mastery_Extension target)
        {
            base.CopyTo(target);

            target.rangeCurve = rangeCurve.Duplicate();
            target.rangeType = rangeType;
            target.radiusCurve = radiusCurve.Duplicate();
            target.radiusType = radiusType;

            target.castTimeCurve = castTimeCurve.Duplicate();
            target.castTimeType = castTimeType;
            target.cooldownCurve = cooldownCurve.Duplicate();
            target.cooldownType = cooldownType;
            target.durationCurve = durationCurve.Duplicate();
            target.durationType = durationType;

            target.psyfocusCurve = psyfocusCurve.Duplicate();
            target.psyfocusType = psyfocusType;
            target.entropyCurve = entropyCurve.Duplicate();
            target.entropyType = entropyType;
        }

        public Ability_Mastery_Extension Duplicate()
        {
            var duplicate = new Ability_Mastery_Extension();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}