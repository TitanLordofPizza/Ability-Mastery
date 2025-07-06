using Verse;

using Mastery.Ability.Data;

namespace Mastery.Ability.Patches.Vanilla
{
    public static class PawnGenerator_Patch
    {
        public static void Postfix(ref Pawn __result, ref PawnGenerationRequest request)
        {
            try
            {
                if (__result != null && __result.HasComp<Ability_Mastery_Comp>())
                {
                    __result.GetComp<Ability_Mastery_Comp>().GenerateEntries();
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Failed to Generate a Pawns Ability Mastery. " + ex);
            }
        }
    }
}