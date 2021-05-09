using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using HenryMod.Modules.Components;

namespace HenryMod.Modules.Misc
{
    public class HenryFurySkillDef : SkillDef
    {
        public float cost;

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new HenryFurySkillDef.InstanceData
            {
                furyComponent = skillSlot.GetComponent<HenryFuryComponent>()
            };
        }

        private static bool HasSufficientfury([NotNull] GenericSkill skillSlot)
        {
            HenryFuryComponent furyComponent = ((HenryFurySkillDef.InstanceData)skillSlot.skillInstanceData).furyComponent;
            return (furyComponent != null) ? (furyComponent.currentFury >= skillSlot.rechargeStock) : false;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HenryFurySkillDef.HasSufficientfury(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HenryFurySkillDef.HasSufficientfury(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public HenryFuryComponent furyComponent;
        }
    }
}