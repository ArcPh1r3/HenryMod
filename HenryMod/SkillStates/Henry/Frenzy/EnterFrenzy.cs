using EntityStates;
using HenryMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates.Henry.Frenzy
{
    public class EnterFrenzy : BaseSkillState
    {
        public static float baseDuration = 1.25f;

        private float duration;
        private Vector3 storedPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = EnterFrenzy.baseDuration;// / this.attackSpeedStat;

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            foreach (EntityStateMachine i in base.gameObject.GetComponents<EntityStateMachine>())
            {
                if (i)
                {
                    if (i.customName == "Weapon")
                    {
                        i.SetNextStateToMain();
                    }
                    if (i.customName == "Slide")
                    {
                        i.SetNextStateToMain();
                    }
                }
            }

            HenryFuryComponent furyComponent = base.gameObject.GetComponent<HenryFuryComponent>();
            if (furyComponent) furyComponent.SpendFury(100f);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.frenzyChargeEffect, base.gameObject, "Chest", false);
            base.PlayAnimation("FullBody, Override", "FrenzyEnter", "Frenzy.playbackRate", this.duration);
            Util.PlaySound("HenryFrenzyCharge", base.gameObject);

            base.cameraTargetParams.cameraParams = HenryPlugin.zoomInCameraParams;

            this.storedPosition = base.transform.position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.transform.position = this.storedPosition;
            if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new ExitFrenzy());
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}