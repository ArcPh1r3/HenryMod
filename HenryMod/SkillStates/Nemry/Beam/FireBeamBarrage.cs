using EntityStates;
using HenryMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace HenryMod.SkillStates.Nemry.Beam
{
    public class FireBeamBarrage : BaseNemrySkillState
    {
        public float charge;

        public static float laserDamageCoefficient = 4.2f;
        public static float laserBlastRadius = 8f;
        public static float laserBlastForce = 1000f;

        public static float baseDuration = 0.8f;
        public static float timeBetweenShots = 0.1f;
        public static float recoil = 5f;

        private int totalBulletsFired;
        private int bulletCount;
        public float stopwatchBetweenShots;
        private Animator modelAnimator;
        private Transform modelTransform;
        private float duration;
        private float durationBetweenShots;
        private GameObject muzzleFlashEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/FusionCellExplosion");

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetSpreadBloom(0.2f, false);
            base.characterBody.isSprinting = false;
            this.duration = FireBeamBarrage.baseDuration;
            this.modelAnimator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            this.durationBetweenShots = FireBeamBarrage.timeBetweenShots;
            this.bulletCount = 10;
            base.characterBody.SetAimTimer(2f);
            base.characterBody.outOfCombatStopwatch = 0f;

            this.FireBullet();
        }

        private void FireBullet()
        {
            Ray aimRay = base.GetAimRay();
            string muzzleName = "Muzzle";

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, muzzleName, false);
            base.PlayCrossfade("UpperBody, Override", "Special", "Special.rate", this.durationBetweenShots, 0.05f);
            Util.PlaySound("HenryShootHandCannon", base.gameObject);
            Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);

            float recoil = FireBeamBarrage.recoil / this.attackSpeedStat;
            base.AddRecoil(-0.8f * recoil, -1f * recoil, -0.1f * recoil, 0.15f * recoil);

            EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, "Muzzle", false);

            this.SpendEnergy(10f, SkillSlot.Special);

            if (base.isAuthority)
            {
                this.FireLaser();
            }

            base.characterBody.AddSpreadBloom(2f * EntityStates.Commando.CommandoWeapon.FireBarrage.spreadBloomValue);
            this.totalBulletsFired++;
        }

        private void FireLaser()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 blastPosition = aimRay.origin + aimRay.direction * 1000f;

            RaycastHit raycastHit;
            if (Physics.Raycast(aimRay, out raycastHit, 1000f, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.entityPrecise.mask))
            {
                blastPosition = raycastHit.point;
            }

            BlastAttack blast = new BlastAttack
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
                baseDamage = this.damageStat * FireBeamBarrage.laserDamageCoefficient,
                baseForce = FireBeamBarrage.laserBlastForce * 0.2f,
                position = blastPosition,
                radius = FireBeamBarrage.laserBlastRadius,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                bonusForce = FireBeamBarrage.laserBlastForce * aimRay.direction
            };

            blast.Fire();

            if (this.modelTransform)
            {
                ChildLocator childLocator = this.modelTransform.GetComponent<ChildLocator>();
                if (childLocator)
                {
                    int childIndex = childLocator.FindChildIndex("Muzzle");
                    if (EntityStates.GolemMonster.FireLaser.tracerEffectPrefab)
                    {
                        EffectData effectData = new EffectData
                        {
                            origin = blastPosition,
                            start = aimRay.origin
                        };

                        effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);

                        EffectManager.SpawnEffect(EntityStates.GolemMonster.FireLaser.tracerEffectPrefab, effectData, true);
                        EffectManager.SpawnEffect(EntityStates.GolemMonster.FireLaser.hitEffectPrefab, effectData, true);
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (base.cameraTargetParams) base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatchBetweenShots += Time.fixedDeltaTime;

            if (this.stopwatchBetweenShots >= this.durationBetweenShots && this.totalBulletsFired < this.bulletCount)
            {
                this.stopwatchBetweenShots -= this.durationBetweenShots;
                this.FireBullet();
            }

            if (base.fixedAge >= this.duration && this.totalBulletsFired == this.bulletCount && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}