namespace HenryMod.Modules.Achievements
{
    internal class GrandMasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => HenryPlugin.developerPrefix + "_HENRY_BODY_TYPHOON";
        public override string PrerequisiteUnlockableIdentifier => HenryPlugin.developerPrefix + "_HENRY_BODY_UNLOCKABLE_REWARD_ID";
        public override string AchievementSpriteName => "texGrandMasteryAchievement";
        
        public override string RequiredCharacterBody => "HenryBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }

    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => HenryPlugin.developerPrefix + "_HENRY_BODY_MASTERY";
        public override string PrerequisiteUnlockableIdentifier => "_HENRY_BODY_UNLOCKABLE_REWARD_ID";
        public override string AchievementSpriteName => "texMasteryAchievement";

        public override string RequiredCharacterBody => "HenryBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }
}