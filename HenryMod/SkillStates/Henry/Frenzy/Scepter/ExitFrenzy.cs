using EntityStates;
using HenryMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates.Henry.Frenzy.Scepter
{
    public class ExitFrenzy : BaseHenrySkillState
    {
        public static float baseDuration = 0.6f;

        public static float shockwaveRadius = 32f;
        public static float shockwaveForce = 8000f;
        public static float shockwaveBonusForce = 1500f;

        public static float hopForce = 12f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ExitFrenzy.baseDuration / this.attackSpeedStat;

            if (NetworkServer.active) base.characterBody.AddTimedBuff(Modules.Buffs.frenzyScepterBuff, 15f);

            //todo cum2 fix camera
            //base.cameraTargetParams.cameraParams = HenryPlugin.defaultCameraParams;
            //base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Aura;

            this.FireShockwave();
            base.PlayAnimation("FullBody, Override", "FrenzyExit", "Frenzy.playbackRate", this.duration);
            base.SmallHop(base.characterMotor, ExitFrenzy.hopForce);
        }

        private void FireShockwave()
        {
            Util.PlaySound("HenryFrenzyShockwave", base.gameObject);

            EffectData effectData = new EffectData();
            effectData.origin = base.characterBody.corePosition;
            effectData.scale = 1;

            EffectManager.SpawnEffect(Modules.Assets.frenzyShockwaveEffect, effectData, false);

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = base.characterBody.corePosition;
                blastAttack.procCoefficient = 0f;
                blastAttack.radius = ExitFrenzy.shockwaveRadius;
                blastAttack.baseForce = ExitFrenzy.shockwaveForce;
                blastAttack.bonusForce = Vector3.up * ExitFrenzy.shockwaveBonusForce;
                blastAttack.baseDamage = 0f;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }

            if (this.henryController) this.henryController.EnterScepterFrenzy();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            // todo cum2 camera base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;

            Util.PlaySound("HenrySuperSand", base.gameObject);

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}