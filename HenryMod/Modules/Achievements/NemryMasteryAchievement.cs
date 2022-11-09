using RoR2;
using System;
using UnityEngine;
using R2API;

namespace HenryMod.Modules.Achievements
{
    internal class NemryMasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => HenryPlugin.developerPrefix + "_NEMRY_BODY_MASTERY";
        public override string PrerequisiteUnlockableIdentifier => "_NEMRY_BODY_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texMasteryAchievementNemry";

        public override string RequiredCharacterBody => Enemies.Nemry.bodyName;

        public override float RequiredDifficultyCoefficient => 3f;
    }
}