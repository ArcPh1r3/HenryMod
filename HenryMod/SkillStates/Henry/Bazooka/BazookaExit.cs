using EntityStates;
using HenryMod.Modules.Components;
using HenryMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates.Bazooka
{
    public class BazookaExit : BaseHenrySkillState
    {
        public static float baseDuration = 0.6f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BazookaExit.baseDuration / this.attackSpeedStat;

            base.henryController.hasBazookaReady = false;
            base.characterBody.aimOriginTransform = henryController.origAimOrigin;
            base.henryController.ExitBazookaCamera();

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            base.PlayAnimation("Bazooka, Override", "BazookaExit", "Bazooka.playbackRate", this.duration);
            Util.PlaySound("HenryBazookaUnequip", base.gameObject);

            // todo cum2 camera if (base.cameraTargetParams) {
            //todo cum2 camera fix, requestaimtype literally only works with aura now
            //base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            //}

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, BazookaEnter.fireDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, BazookaEnter.cancelDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, BazookaEnter.cancelDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.special, BazookaEnter.cancelDef, GenericSkill.SkillOverridePriority.Contextual);

            this.henryController.UpdateCrosshair();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}