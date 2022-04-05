using EntityStates;
using HenryMod.Modules.Misc;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules {

    internal static class Skills {

        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();

        #region genericskills
        public static void CreateSkillFamilies(GameObject targetPrefab, int families = 15, bool destroyExisting = true) {

            if (destroyExisting) {
                foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>()) {
                    UnityEngine.Object.DestroyImmediate(obj);
                }
            }

            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            if ((families & (1 << 0)) != 0) {
                skillLocator.primary = CreateGenericSkillWithSkillFamily(targetPrefab, "Primary");
            }
            if ((families & (1 << 1)) != 0) {
                skillLocator.secondary = CreateGenericSkillWithSkillFamily(targetPrefab, "Secondary");
            }
            if ((families & (1 << 2)) != 0) {
                skillLocator.utility = CreateGenericSkillWithSkillFamily(targetPrefab, "Utility");
            }
            if ((families & (1 << 3)) != 0) {
                skillLocator.special = CreateGenericSkillWithSkillFamily(targetPrefab, "Special");
            }
        }

        public static GenericSkill CreateGenericSkillWithSkillFamily(GameObject targetPrefab, string familyName, bool hidden = false) {

            GenericSkill skill = targetPrefab.AddComponent<GenericSkill>();
            skill.skillName = familyName;
            skill.hideInCharacterSelect = hidden;

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = targetPrefab.name + familyName + "Family";
            newFamily.variants = new SkillFamily.Variant[0];

            skill._skillFamily = newFamily;

            skillFamilies.Add(newFamily);
            return skill;
        }
        #endregion

        #region skillfamilies

        //everything calls this
        public static void AddSkillToFamily(SkillFamily skillFamily, SkillDef skillDef, UnlockableDef unlockableDef = null) {

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);

            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = skillDef,
                unlockableDef = unlockableDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        public static void AddSkillsToFamily(SkillFamily skillFamily, params SkillDef[] skillDefs) {

            foreach (SkillDef skillDef in skillDefs) {
                AddSkillToFamily(skillFamily, skillDef);
            }
        }
        public static void AddPrimarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().primary.skillFamily, skillDefs);
        }
        public static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().secondary.skillFamily, skillDefs);
        }
        public static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().utility.skillFamily, skillDefs);
        }
        public static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().special.skillFamily, skillDefs);
        }

        /// <summary>
        /// pass in an amount of unlockables equal to or less than skill variants, null for skills that aren't locked
        /// <code>
        /// AddUnlockablesToFamily(skillLocator.primary, null, skill2UnlockableDef, null, skill4UnlockableDef);
        /// </code>
        /// </summary>
        public static void AddUnlockablesToFamily(SkillFamily skillFamily, params UnlockableDef[] unlockableDefs) {

            for (int i = 0; i < unlockableDefs.Length; i++) {
                SkillFamily.Variant variant = skillFamily.variants[i];
                variant.unlockableDef = unlockableDefs[i];
                skillFamily.variants[i] = variant;
            }
        }
        #endregion

        #region skilldefs
        public static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo) {

            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            popuplateSKillDef(skillDefInfo, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }

        public static T CreateSkillDef<T>(SkillDefInfo skillDefInfo) where T : SkillDef {

            T skillDef = ScriptableObject.CreateInstance<T>();

            popuplateSKillDef(skillDefInfo, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static NemryEnergySkillDef CreateEnergySkillDef(SkillDefInfo skillDefInfo) {

            NemryEnergySkillDef skillDef = CreateSkillDef<NemryEnergySkillDef>(skillDefInfo);

            return skillDef;
        }
        internal static HenryFurySkillDef CreateFurySkillDef(SkillDefInfo skillDefInfo) {

            HenryFurySkillDef skillDef = CreateSkillDef<HenryFurySkillDef>(skillDefInfo);

            return skillDef;
        }
        internal static TrackingEnergySkillDef CreateTrackingEnergySkillDef(SkillDefInfo skillDefInfo) {

            TrackingEnergySkillDef skillDef = CreateSkillDef<TrackingEnergySkillDef>(skillDefInfo);

            return skillDef;
        }
        internal static HenryTrackingSkillDef CreateTrackingSkillDef(SkillDefInfo skillDefInfo) {
            
            HenryTrackingSkillDef skillDef = CreateSkillDef<HenryTrackingSkillDef>(skillDefInfo);

            return skillDef;
        }

        private static void popuplateSKillDef(SkillDefInfo skillDefInfo, SkillDef skillDef) {
            skillDef.skillName = skillDefInfo.skillName;
            (skillDef as ScriptableObject).name = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;

            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;
        }

        internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile) {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            skillDef.skillName = skillNameToken;
            skillDef.skillNameToken = skillNameToken;
            skillDef.skillDescriptionToken = skillDescriptionToken;
            skillDef.icon = skillIcon;

            skillDef.activationState = state;
            skillDef.activationStateMachineName = stateMachine;
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.forceSprintDuringState = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;
            skillDef.resetCooldownTimerOnUse = false;
            skillDef.isCombatSkill = true;
            skillDef.mustKeyPress = false;
            skillDef.cancelSprintingOnActivation = !agile;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 0;
            skillDef.stockToConsume = 0;

            if (agile) skillDef.keywordTokens = new string[] { "KEYWORD_AGILE" };

            skillDefs.Add(skillDef);

            return skillDef;
        }

        #endregion skilldefs
    }
}

/// <summary>
/// class for easily creating skilldefs with default values, and with a field for UnlockableDef
/// </summary>
internal class SkillDefInfo {

    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public string[] keywordTokens = new string[0];
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public InterruptPriority interruptPriority;
    public string activationStateMachineName;

    public float baseRechargeInterval;

    public int baseMaxStock = 1;
    public int rechargeStock = 1;
    public int requiredStock = 1;
    public int stockToConsume = 1;

    public bool isCombatSkill = true;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool cancelSprintingOnActivation = true;

    public bool beginSkillCooldownOnSkillEnd;
    public bool fullRestockOnAssign = true;
    public bool resetCooldownTimerOnUse;
    public bool mustKeyPress;

    #region building
    public SkillDefInfo() { }

    public SkillDefInfo(string skillName,
                          string skillNameToken,
                          string skillDescriptionToken,
                          Sprite skillIcon,

                          SerializableEntityStateType activationState,
                          string activationStateMachineName,
                          InterruptPriority interruptPriority,
                          bool isCombatSkill,

                          float baseRechargeInterval) {
        this.skillName = skillName;
        this.skillNameToken = skillNameToken;
        this.skillDescriptionToken = skillDescriptionToken;
        this.skillIcon = skillIcon;
        this.activationState = activationState;
        this.activationStateMachineName = activationStateMachineName;
        this.interruptPriority = interruptPriority;
        this.isCombatSkill = isCombatSkill;
        this.baseRechargeInterval = baseRechargeInterval;
    }
    /// <summary>
    /// Creates a skilldef for a typical primary.
    /// <para>combat skill, cooldown: 0, required stock: 0, InterruptPriority: Any</para>
    /// </summary>
    public SkillDefInfo(string skillName,
                          string skillNameToken,
                          string skillDescriptionToken,
                          Sprite skillIcon,

                          SerializableEntityStateType activationState,
                          string activationStateMachineName = "Weapon",
                          bool agile = false) {

        this.skillName = skillName;
        this.skillNameToken = skillNameToken;
        this.skillDescriptionToken = skillDescriptionToken;
        this.skillIcon = skillIcon;

        this.activationState = activationState;
        this.activationStateMachineName = activationStateMachineName;

        this.interruptPriority = InterruptPriority.Any;
        this.isCombatSkill = true;
        this.baseRechargeInterval = 0;

        this.requiredStock = 0;
        this.stockToConsume = 0;

        this.cancelSprintingOnActivation = !agile;
    }
    #endregion construction complete
}

//using EntityStates;
//using HenryMod.Modules.Misc;
//using R2API;
//using RoR2;
//using RoR2.Skills;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HenryMod.Modules
//{
//    internal static class Skills
//    {
//        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
//        internal static List<SkillDef> skillDefs = new List<SkillDef>();

//        internal static void CreateSkillFamilies(GameObject targetPrefab)
//        {
//            foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>())
//            {
//                HenryPlugin.DestroyImmediate(obj);
//            }

//            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

//            skillLocator.primary = targetPrefab.AddComponent<GenericSkill>();
//            SkillFamily primaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
//            (primaryFamily as ScriptableObject).name = targetPrefab.name + "PrimaryFamily";
//            primaryFamily.variants = new SkillFamily.Variant[0];
//            skillLocator.primary._skillFamily = primaryFamily;

//            skillLocator.secondary = targetPrefab.AddComponent<GenericSkill>();
//            SkillFamily secondaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
//            (secondaryFamily as ScriptableObject).name = targetPrefab.name + "SecondaryFamily";
//            secondaryFamily.variants = new SkillFamily.Variant[0];
//            skillLocator.secondary._skillFamily = secondaryFamily;

//            skillLocator.utility = targetPrefab.AddComponent<GenericSkill>();
//            SkillFamily utilityFamily = ScriptableObject.CreateInstance<SkillFamily>();
//            (utilityFamily as ScriptableObject).name = targetPrefab.name + "UtilityFamily";
//            utilityFamily.variants = new SkillFamily.Variant[0];
//            skillLocator.utility._skillFamily = utilityFamily;

//            skillLocator.special = targetPrefab.AddComponent<GenericSkill>();
//            SkillFamily specialFamily = ScriptableObject.CreateInstance<SkillFamily>();
//            (specialFamily as ScriptableObject).name = targetPrefab.name + "SpecialFamily";
//            specialFamily.variants = new SkillFamily.Variant[0];
//            skillLocator.special._skillFamily = specialFamily;

//            skillFamilies.Add(primaryFamily);
//            skillFamilies.Add(secondaryFamily);
//            skillFamilies.Add(utilityFamily);
//            skillFamilies.Add(specialFamily);
//        }

//        // this could all be a lot cleaner but at least it's simple and easy to work with
//        internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
//        {
//            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

//            SkillFamily skillFamily = skillLocator.primary.skillFamily;

//            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
//            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
//            {
//                skillDef = skillDef,
//                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
//            };
//        }

//        internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
//        {
//            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

//            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

//            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
//            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
//            {
//                skillDef = skillDef,
//                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
//            };
//        }

//        internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
//        {
//            foreach (SkillDef i in skillDefs)
//            {
//                AddSecondarySkill(targetPrefab, i);
//            }
//        }

//        internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
//        {
//            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

//            SkillFamily skillFamily = skillLocator.utility.skillFamily;

//            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
//            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
//            {
//                skillDef = skillDef,
//                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
//            };
//        }

//        internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
//        {
//            foreach (SkillDef i in skillDefs)
//            {
//                AddUtilitySkill(targetPrefab, i);
//            }
//        }

//        internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
//        {
//            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

//            SkillFamily skillFamily = skillLocator.special.skillFamily;

//            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
//            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
//            {
//                skillDef = skillDef,
//                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
//            };
//        }

//        internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
//        {
//            foreach (SkillDef i in skillDefs)
//            {
//                AddSpecialSkill(targetPrefab, i);
//            }
//        }

//        internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile)
//        {
//            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

//            skillDef.skillName = skillNameToken;
//            skillDef.skillNameToken = skillNameToken;
//            skillDef.skillDescriptionToken = skillDescriptionToken;
//            skillDef.icon = skillIcon;

//            skillDef.activationState = state;
//            skillDef.activationStateMachineName = stateMachine;
//            skillDef.baseMaxStock = 1;
//            skillDef.baseRechargeInterval = 0;
//            skillDef.beginSkillCooldownOnSkillEnd = false;
//            skillDef.canceledFromSprinting = false;
//            skillDef.forceSprintDuringState = false;
//            skillDef.fullRestockOnAssign = true;
//            skillDef.interruptPriority = InterruptPriority.Any;
//            skillDef.resetCooldownTimerOnUse = false;
//            skillDef.isCombatSkill = true;
//            skillDef.mustKeyPress = false;
//            skillDef.cancelSprintingOnActivation = !agile;
//            skillDef.rechargeStock = 1;
//            skillDef.requiredStock = 0;
//            skillDef.stockToConsume = 0;

//            if (agile) skillDef.keywordTokens = new string[] { "KEYWORD_AGILE" };

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }

//        internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
//        {
//            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

//            skillDef.skillName = skillDefInfo.skillName;
//            skillDef.skillNameToken = skillDefInfo.skillNameToken;
//            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
//            skillDef.icon = skillDefInfo.skillIcon;

//            skillDef.activationState = skillDefInfo.activationState;
//            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
//            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
//            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
//            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
//            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
//            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
//            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
//            skillDef.interruptPriority = skillDefInfo.interruptPriority;
//            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
//            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
//            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
//            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
//            skillDef.rechargeStock = skillDefInfo.rechargeStock;
//            skillDef.requiredStock = skillDefInfo.requiredStock;
//            skillDef.stockToConsume = skillDefInfo.stockToConsume;

//            skillDef.keywordTokens = skillDefInfo.keywordTokens;

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }

//        internal static SkillDef CreateTrackingSkillDef(SkillDefInfo skillDefInfo)
//        {
//            HenryTrackingSkillDef skillDef = ScriptableObject.CreateInstance<HenryTrackingSkillDef>();

//            skillDef.skillName = skillDefInfo.skillName;
//            skillDef.skillNameToken = skillDefInfo.skillNameToken;
//            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
//            skillDef.icon = skillDefInfo.skillIcon;

//            skillDef.activationState = skillDefInfo.activationState;
//            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
//            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
//            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
//            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
//            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
//            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
//            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
//            skillDef.interruptPriority = skillDefInfo.interruptPriority;
//            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
//            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
//            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
//            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
//            skillDef.rechargeStock = skillDefInfo.rechargeStock;
//            skillDef.requiredStock = skillDefInfo.requiredStock;
//            skillDef.stockToConsume = skillDefInfo.stockToConsume;

//            skillDef.keywordTokens = skillDefInfo.keywordTokens;

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }

//        internal static SkillDef CreateEnergySkillDef(SkillDefInfo skillDefInfo)
//        {
//           NemryEnergySkillDef skillDef = ScriptableObject.CreateInstance<NemryEnergySkillDef>();

//            skillDef.skillName = skillDefInfo.skillName;
//            skillDef.skillNameToken = skillDefInfo.skillNameToken;
//            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
//            skillDef.icon = skillDefInfo.skillIcon;

//            skillDef.activationState = skillDefInfo.activationState;
//            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
//            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
//            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
//            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
//            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
//            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
//            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
//            skillDef.interruptPriority = skillDefInfo.interruptPriority;
//            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
//            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
//            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
//            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
//            skillDef.rechargeStock = skillDefInfo.rechargeStock;
//            skillDef.requiredStock = skillDefInfo.requiredStock;
//            skillDef.stockToConsume = skillDefInfo.stockToConsume;

//            skillDef.keywordTokens = skillDefInfo.keywordTokens;

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }

//        internal static SkillDef CreateFurySkillDef(SkillDefInfo skillDefInfo)
//        {
//            HenryFurySkillDef skillDef = ScriptableObject.CreateInstance<HenryFurySkillDef>();

//            skillDef.skillName = skillDefInfo.skillName;
//            skillDef.skillNameToken = skillDefInfo.skillNameToken;
//            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
//            skillDef.icon = skillDefInfo.skillIcon;

//            skillDef.activationState = skillDefInfo.activationState;
//            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
//            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
//            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
//            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
//            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
//            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
//            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
//            skillDef.interruptPriority = skillDefInfo.interruptPriority;
//            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
//            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
//            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
//            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
//            skillDef.rechargeStock = skillDefInfo.rechargeStock;
//            skillDef.requiredStock = skillDefInfo.requiredStock;
//            skillDef.stockToConsume = skillDefInfo.stockToConsume;

//            skillDef.keywordTokens = skillDefInfo.keywordTokens;

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }

//        internal static SkillDef CreateTrackingEnergySkillDef(SkillDefInfo skillDefInfo)
//        {
//            TrackingEnergySkillDef skillDef = ScriptableObject.CreateInstance<TrackingEnergySkillDef>();

//            skillDef.skillName = skillDefInfo.skillName;
//            skillDef.skillNameToken = skillDefInfo.skillNameToken;
//            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
//            skillDef.icon = skillDefInfo.skillIcon;

//            skillDef.activationState = skillDefInfo.activationState;
//            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
//            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
//            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
//            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
//            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
//            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
//            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
//            skillDef.interruptPriority = skillDefInfo.interruptPriority;
//            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
//            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
//            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
//            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
//            skillDef.rechargeStock = skillDefInfo.rechargeStock;
//            skillDef.requiredStock = skillDefInfo.requiredStock;
//            skillDef.stockToConsume = skillDefInfo.stockToConsume;

//            skillDef.keywordTokens = skillDefInfo.keywordTokens;

//            skillDefs.Add(skillDef);

//            return skillDef;
//        }
//    }
//}

//internal class SkillDefInfo
//{
//    public string skillName;
//    public string skillNameToken;
//    public string skillDescriptionToken;
//    public Sprite skillIcon;

//    public SerializableEntityStateType activationState;
//    public string activationStateMachineName;
//    public int baseMaxStock;
//    public float baseRechargeInterval;
//    public bool beginSkillCooldownOnSkillEnd;
//    public bool canceledFromSprinting;
//    public bool forceSprintDuringState;
//    public bool fullRestockOnAssign;
//    public InterruptPriority interruptPriority;
//    public bool resetCooldownTimerOnUse;
//    public bool isCombatSkill;
//    public bool mustKeyPress;
//    public bool cancelSprintingOnActivation;
//    public int rechargeStock;
//    public int requiredStock;
//    public int stockToConsume;

//    public string[] keywordTokens;
//}