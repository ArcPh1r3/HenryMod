using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace HenryMod.Modules.Components
{
    public class HenryFuryComponent : MonoBehaviour
    {
        public float maxFury = 100f;
        public float currentFury;

        private float lastFury;
        private CharacterBody characterBody;
        private CharacterMaster characterMaster;
        private CharacterModel model;
        private ChildLocator childLocator;
        private Animator modelAnimator;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.gameObject.GetComponentInChildren<CharacterModel>();
            this.modelAnimator = this.gameObject.GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            this.characterMaster = this.characterBody.master;
        }

        public bool AddFury()
        {
            return this.AddFury(2f);
        }

        public bool AddFury(float amount)
        {
            if (this.characterBody.HasBuff(Modules.Buffs.frenzyBuff)) return false;
            if (this.currentFury >= this.maxFury) return false;

            if (this.characterBody.healthComponent.combinedHealth <= (0.5f * this.characterBody.healthComponent.fullCombinedHealth)) amount *= 2f;
            if (this.characterBody.healthComponent.combinedHealth <= (0.25f * this.characterBody.healthComponent.fullCombinedHealth)) amount *= 3f;

            this.currentFury = Mathf.Clamp(this.currentFury + amount, 0f, this.maxFury);

            if (this.currentFury >= this.maxFury && this.lastFury < this.maxFury) Util.PlaySound("HenryFrenzyReady", this.gameObject);

            this.lastFury = this.currentFury;
            return true;
        }

        public bool SpendFury(float amount)
        {
            this.currentFury = Mathf.Clamp(this.currentFury - amount, 0f, this.maxFury);
            this.lastFury = this.currentFury;
            return true;
        }
    }
}