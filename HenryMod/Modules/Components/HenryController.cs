using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace HenryMod.Modules.Components
{
    // just a class to run some custom code for things like weapon models
    public class HenryController : MonoBehaviour
    {
        public bool hasBazookaReady;

        private CharacterBody characterBody;
        private CharacterModel model;
        private ChildLocator childLocator;
        private HenryTracker tracker;
        private Animator modelAnimator;

        private bool inFrenzy;
        private ParticleSystem[] frenzyEffects;
        private ParticleSystem superSaiyanEffect;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.gameObject.GetComponentInChildren<CharacterModel>();
            this.tracker = this.gameObject.GetComponent<HenryTracker>();
            this.modelAnimator = this.gameObject.GetComponentInChildren<Animator>();
            this.hasBazookaReady = false;
            this.inFrenzy = false;

            this.frenzyEffects = new ParticleSystem[]
            {
                this.childLocator.FindChild("FrenzyEffect").gameObject.GetComponent<ParticleSystem>(),
                this.childLocator.FindChild("FrenzyFistEffectL").gameObject.GetComponent<ParticleSystem>(),
                this.childLocator.FindChild("FrenzyFistEffectR").gameObject.GetComponent<ParticleSystem>()
            };

            this.superSaiyanEffect = this.childLocator.FindChild("SuperSaiyanEffect").gameObject.GetComponent<ParticleSystem>();

            Invoke("CheckWeapon", 0.2f);
        }

        private void FixedUpdate()
        {
            if (this.inFrenzy)
            {
                if (!this.characterBody.HasBuff(Modules.Buffs.frenzyBuff) && !this.characterBody.HasBuff(Modules.Buffs.frenzyScepterBuff))
                {
                    this.ExitFrenzy();
                }
            }
        }

        private void CheckWeapon()
        {
            switch (this.characterBody.skillLocator.primary.skillDef.skillNameToken)
            {
                default:
                    this.childLocator.FindChild("SwordModel").gameObject.SetActive(true);
                    this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(false);
                    this.childLocator.FindChild("AltGun").gameObject.SetActive(false);
                    this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 0f);
                    break;
                case HenryPlugin.developerPrefix + "_HENRY_BODY_PRIMARY_PUNCH_NAME":
                    this.childLocator.FindChild("SwordModel").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(true);
                    this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(true);
                    this.childLocator.FindChild("AltGun").gameObject.SetActive(false);
                    this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 1f);
                    break;
                case HenryPlugin.developerPrefix + "_HENRY_BODY_PRIMARY_GUN_NAME":
                    this.childLocator.FindChild("SwordModel").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(false);
                    this.childLocator.FindChild("AltGun").gameObject.SetActive(true);
                    this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 0f);
                    break;
            }

            bool hasTrackingSkill = false;

            if (this.characterBody.skillLocator.secondary.skillDef.skillNameToken == HenryPlugin.developerPrefix + "_HENRY_BODY_SECONDARY_STINGER_NAME")
            {
                this.childLocator.FindChild("GunModel").gameObject.SetActive(false);
                this.childLocator.FindChild("Gun").gameObject.SetActive(false);

                this.characterBody.crosshairPrefab = Modules.Assets.LoadCrosshair("SimpleDot");
                hasTrackingSkill = true;
            }
            else if (this.characterBody.skillLocator.secondary.skillDef.skillNameToken == HenryPlugin.developerPrefix + "_HENRY_BODY_SECONDARY_UZI_NAME")
            {
                this.childLocator.FindChild("GunModel").GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshUzi");
            }

            bool hasFury = (this.characterBody.skillLocator.special.skillDef.skillNameToken == HenryPlugin.developerPrefix + "_HENRY_BODY_SPECIAL_FRENZY_NAME");
            if (this.characterBody.skillLocator.special.skillDef.skillNameToken == HenryPlugin.developerPrefix + "_HENRY_BODY_SPECIAL_SCEPFRENZY_NAME") hasFury = true;

            if (!hasFury)
            {
                HenryFuryComponent furyComponent = this.GetComponent<HenryFuryComponent>();
                if (furyComponent) Destroy(furyComponent);
            }

            if (!hasTrackingSkill && this.tracker) Destroy(this.tracker);
        }

        public void UpdateCrosshair()
        {
            GameObject desiredCrosshair = Modules.Assets.LoadCrosshair("Standard");

            if (this.characterBody.skillLocator.secondary.skillDef.skillNameToken == HenryPlugin.developerPrefix + "_HENRY_BODY_SECONDARY_STINGER_NAME")
            {
                desiredCrosshair = Modules.Assets.LoadCrosshair("SimpleDot");
            }

            if (this.hasBazookaReady)
            {
                desiredCrosshair = Modules.Assets.bazookaCrosshair;
            }

            this.characterBody.crosshairPrefab = desiredCrosshair;
        }

        public void EnterFrenzy()
        {
            this.inFrenzy = true;

            if (Modules.Config.rampageEffects.Value)
            {
                for (int i = 0; i < this.frenzyEffects.Length; i++)
                {
                    if (this.frenzyEffects[i]) this.frenzyEffects[i].Play();
                }
            }
        }

        public void EnterScepterFrenzy()
        {
            this.inFrenzy = true;

            if (Modules.Config.rampageEffects.Value)
            {
                for (int i = 0; i < this.frenzyEffects.Length; i++)
                {
                    if (this.frenzyEffects[i]) this.frenzyEffects[i].Play();
                }
            }

            this.childLocator.FindChild("SaiyanHair").gameObject.SetActive(true);
        }

        private void ExitFrenzy()
        {
            this.inFrenzy = false;

            if (Modules.Config.rampageEffects.Value)
            {
                for (int i = 0; i < this.frenzyEffects.Length; i++)
                {
                    if (this.frenzyEffects[i]) this.frenzyEffects[i].Stop();
                }
            }

            //if (this.superSaiyanEffect) this.superSaiyanEffect.Stop();
            this.childLocator.FindChild("SaiyanHair").gameObject.SetActive(false);
        }
    }
}