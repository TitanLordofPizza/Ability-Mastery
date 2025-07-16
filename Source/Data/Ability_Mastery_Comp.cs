using Verse;

using Mastery.Core.Data.Level_Framework.Extensions;
using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.UI.Tabs;

using Mastery.Ability.UI;

namespace Mastery.Ability.Data
{
    public class Ability_Mastery_Comp : Level_Comp
    {
        public override string LevelKey => "AbilityMastery";

        private readonly Ability_Tab _tab = new Ability_Tab();

        public override IMastery_Tab Tab => _tab;

        public override bool GainExperience(Def def, Level_Action_Extension action, float multiplier = 1) //This is here beacuse I can't patch Ability_Mastery_Comp GainExperience without it
                                                                                                          //After all patching Level_Comp would be way more ineffective.
        {
            return base.GainExperience(def, action, multiplier);
        }
    }
}