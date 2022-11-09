using System;
using UnityEngine;
using RoR2;

namespace HenryMod.Modules.Achievements {

    internal abstract class BaseMasteryUnlockable : GenericModdedUnlockable
    {
        public abstract string RequiredCharacterBody { get; }
        public abstract float RequiredDifficultyCoefficient { get;}

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += OnClientGameOverGlobal;
        }
        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }
        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if ((bool)runReport.gameEnding && runReport.gameEnding.isWin) {

                DifficultyIndex difficultyIndex = runReport.ruleBook.FindDifficulty();
                DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if ((runDifficulty.countsAsHardMode && runDifficulty.scalingValue >= RequiredDifficultyCoefficient) ||
                    (difficultyIndex >= DifficultyIndex.Eclipse1 && difficultyIndex <= DifficultyIndex.Eclipse8) ||
                    (runDifficulty.nameToken == "INFERNO_NAME")) {
                    Grant();
                }
            }
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
    }
}