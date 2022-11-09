using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using UnityEngine.Rendering.PostProcessing;
using RoR2.Projectile;
using System;

namespace HenryMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        // particle effects
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;

        internal static GameObject punchSwingEffect;
        internal static GameObject punchImpactEffect;

        internal static GameObject fistBarrageEffect;

        internal static GameObject bombExplosionEffect;
        internal static GameObject bazookaExplosionEffect;
        internal static GameObject bazookaMuzzleFlash;
        internal static GameObject dustEffect;

        internal static GameObject frenzyChargeEffect;
        internal static GameObject frenzyShockwaveEffect;

        internal static GameObject muzzleFlashEnergy;
        internal static GameObject swordChargeEffect;
        internal static GameObject swordChargeFinishEffect;
        internal static GameObject minibossEffect;

        internal static GameObject energyBurstEffect;
        internal static GameObject smallEnergyBurstEffect;

        internal static GameObject spearSwingEffect;

        internal static GameObject nemSwordSwingEffect;
        internal static GameObject nemSwordHeavySwingEffect;
        internal static GameObject nemSwordStabSwingEffect;
        internal static GameObject nemSwordHitImpactEffect;

        internal static GameObject shotgunTracer;
        internal static GameObject energyTracer;

        // custom crosshair
        internal static GameObject bazookaCrosshair;

        // tracker
        internal static GameObject trackerPrefab;

        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        internal static NetworkSoundEventDef punchHitSoundEvent;
        internal static NetworkSoundEventDef nemSwordHitSoundEvent;

        #region Materials
        // vfx materials
        internal static Material supplyDropMat;
        internal static Material airStrikeMat;
        internal static Material crippleSphereMat;
        internal static Material areaIndicatorMat;
        internal static Material matMeteorIndicator;

        internal static Material matBlueLightningLong;
        internal static Material matYellowLightningLong;
        internal static Material matJellyfishLightning;
        internal static Material matJellyfishLightningLarge;
        internal static Material matMageMatrixDirectionalLightning;
        internal static Material matDistortion;
        internal static Material matMercSwipe;
        internal static Material matLoaderLightningSphere;
        internal static Material matHuntressSwingTrail;
        #endregion

        #region PostProcessing
        internal static PostProcessProfile grandParentPP;
        #endregion

        // lists of assets to add to contentpack
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        // cache these and use to create our own materials
        internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];

        internal static void Initialize()
        {
            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HenryMod.pleasechangethisnameinyourprojectorelseyouwillcauseconflicts"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("HenryMod.HenryBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        private static void GatherMaterials()
        {
            grandParentPP = Resources.Load<GameObject>("Prefabs/CharacterBodies/GrandParentBody").GetComponentInChildren<PostProcessVolume>().sharedProfile;
            supplyDropMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NetworkedObjects/CaptainSupplyDrops/CaptainSupplyDrop, Healing").transform.Find("Inactive").Find("Sphere, Outer").GetComponent<MeshRenderer>().material);
            airStrikeMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileGhosts/CaptainAirstrikeGhost1").transform.Find("Expander").Find("Sphere, Outer").GetComponent<MeshRenderer>().material);
            crippleSphereMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/TemporaryVisualEffects/CrippleEffect").transform.Find("Visual").GetChild(1).GetComponent<MeshRenderer>().material);
            areaIndicatorMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/SpiteBombDelayEffect").transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>().material);
            matBlueLightningLong = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect").transform.Find("Ring").GetComponent<ParticleSystemRenderer>().material);
            matJellyfishLightning = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/JellyfishNova").transform.Find("Lightning, Spark Center").GetComponent<ParticleSystemRenderer>().material);
            matJellyfishLightningLarge = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/VagrantCannonExplosion").transform.Find("Lightning, Radial").GetComponent<ParticleSystemRenderer>().material);
            matMageMatrixDirectionalLightning = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniImpactVFXLightningMage").transform.Find("Matrix, Directional").GetComponent<ParticleSystemRenderer>().material);
            matDistortion = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LoaderGroundSlam").transform.Find("Sphere, Distortion").GetComponent<ParticleSystemRenderer>().material);
            matMercSwipe = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab.transform.Find("Base").GetComponent<ParticleSystemRenderer>().material);
            matLoaderLightningSphere = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LoaderGroundSlam").transform.Find("Sphere, Expanding").GetComponent<ParticleSystemRenderer>().material);
            matYellowLightningLong = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/LoaderPylon").transform.Find("loader pylon").Find("LoaderPylonArmature").Find("ROOT").Find("ActiveParticles").Find("Sparks, Trail").GetComponent<ParticleSystemRenderer>().trailMaterial);
            matMeteorIndicator = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/MeteorStrikePredictionEffect").transform.Find("GroundSlamIndicator").GetComponent<MeshRenderer>().material);
            matHuntressSwingTrail = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/HuntressGlaiveSwing").GetComponentInChildren<ParticleSystemRenderer>().material);
        }

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Debug.LogError("There is no AssetBundle to load assets from.");
                return;
            }

            GatherMaterials();

            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");
            punchHitSoundEvent = CreateNetworkSoundEventDef("HenryPunchHit");
            nemSwordHitSoundEvent = CreateNetworkSoundEventDef("NemrySwordHit");

            dustEffect = LoadEffect("HenryDustEffect", false);
            bombExplosionEffect = LoadEffect("BombExplosionEffect", "HenryBombExplosion");
            bazookaExplosionEffect = LoadEffect("HenryBazookaExplosionEffect", "HenryBazookaExplosion");
            bazookaMuzzleFlash = LoadEffect("HenryBazookaMuzzleFlash", false);

            frenzyChargeEffect = LoadEffect("FrenzyChargeEffect", true);
            frenzyShockwaveEffect = LoadEffect("FrenzyShockwaveEffect");

            muzzleFlashEnergy = LoadEffect("NemryMuzzleFlashEnergy", true);
            minibossEffect = mainAssetBundle.LoadAsset<GameObject>("NemryMinibossIndicator");
            
            swordChargeFinishEffect = LoadEffect("SwordChargeFinishEffect");
            swordChargeEffect = mainAssetBundle.LoadAsset<GameObject>("SwordChargeEffect");

            if (swordChargeEffect)
            {
                swordChargeEffect.AddComponent<ScaleParticleSystemDuration>().particleSystems = swordChargeEffect.GetComponentsInChildren<ParticleSystem>();
                swordChargeEffect.GetComponent<ScaleParticleSystemDuration>().initialDuration = 1.5f;
            }

            if (bombExplosionEffect)
            {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };

                shakeEmitter = bazookaExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.4f;
                shakeEmitter.radius = 100f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 30f,
                    cycleOffset = 0f
                };
            }

            swordSwingEffect = Assets.LoadEffect("HenrySwordSwingEffect", true);
            swordSwingEffect.transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().material = matHuntressSwingTrail;
            swordSwingEffect.transform.Find("SwingTrail").Find("SwingTrail2").GetComponent<ParticleSystemRenderer>().material = matDistortion;

            swordHitImpactEffect = Assets.LoadEffect("ImpactHenrySlash");
            AddSimpleShakeEmitter(swordHitImpactEffect);

            punchSwingEffect = Assets.LoadEffect("HenryFistSwingEffect", true);
            punchImpactEffect = Assets.LoadEffect("ImpactHenryPunch");
            AddSimpleShakeEmitter(punchImpactEffect);

            fistBarrageEffect = Assets.LoadEffect("FistBarrageEffect", true);
            if (fistBarrageEffect) fistBarrageEffect.GetComponent<ParticleSystemRenderer>().material.shader = hotpoo;

            bazookaCrosshair = PrefabAPI.InstantiateClone(LoadCrosshair("ToolbotGrenadeLauncher"), "HenryBazookaCrosshair", false);
            CrosshairController crosshair = bazookaCrosshair.GetComponent<CrosshairController>();
            crosshair.skillStockSpriteDisplays = new CrosshairController.SkillStockSpriteDisplay[0];
            bazookaCrosshair.transform.Find("StockCountHolder").gameObject.SetActive(false);
            bazookaCrosshair.transform.Find("Image, Arrow (1)").gameObject.SetActive(true);
            crosshair.spriteSpreadPositions[0].zeroPosition = new Vector3(32f, 34f, 0f);
            crosshair.spriteSpreadPositions[2].zeroPosition = new Vector3(-32f, 34f, 0f);
            bazookaCrosshair.transform.GetChild(1).gameObject.SetActive(false);

            trackerPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/HuntressTrackingIndicator"), "HenryTrackerPrefab", false);
            trackerPrefab.transform.Find("Core Pip").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            trackerPrefab.transform.Find("Core Pip").localScale = new Vector3(0.15f, 0.15f, 0.15f);

            trackerPrefab.transform.Find("Core, Dark").gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            trackerPrefab.transform.Find("Core, Dark").localScale = new Vector3(0.1f, 0.1f, 0.1f);

            foreach(SpriteRenderer i in trackerPrefab.transform.Find("Holder").gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                if (i)
                {
                    i.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    i.color = Color.white;
                }
            }

            shotgunTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("HenryBulletTracer", true);

            if (!shotgunTracer.GetComponent<EffectComponent>()) shotgunTracer.AddComponent<EffectComponent>();
            if (!shotgunTracer.GetComponent<VFXAttributes>()) shotgunTracer.AddComponent<VFXAttributes>();
            if (!shotgunTracer.GetComponent<NetworkIdentity>()) shotgunTracer.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in shotgunTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    Material bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                }
            }

            AddNewEffectDef(shotgunTracer);

            spearSwingEffect = Assets.LoadEffect("NemrySpearSwingEffect");

            nemSwordSwingEffect = Assets.LoadEffect("NemrySwordSwingEffect", true);
            nemSwordStabSwingEffect = Assets.LoadEffect("NemrySwordStabSwingEffect", true);
            nemSwordHeavySwingEffect = Assets.LoadEffect("NemryHeavySwordSwingEffect", true);
            nemSwordHitImpactEffect = Assets.LoadEffect("ImpactNemrySlash");
            AddSimpleShakeEmitter(nemSwordHitImpactEffect);

            energyBurstEffect = LoadEffect("EnergyBurstEffect");
            smallEnergyBurstEffect = LoadEffect("EnergySmallBurstEffect");

            energyTracer = CreateTracer("TracerHuntressSnipe", "NemryEnergyTracer");

            LineRenderer line = energyTracer.transform.Find("TracerHead").GetComponent<LineRenderer>();
            Material tracerMat = UnityEngine.Object.Instantiate<Material>(line.material);
            line.startWidth *= 0.25f;
            line.endWidth *= 0.25f;
            // this did not work.
            //tracerMat.SetColor("_TintColor", new Color(78f / 255f, 80f / 255f, 111f / 255f));
            line.material = tracerMat;
        }

        private static void AddSimpleShakeEmitter(GameObject effectPrefab)
        {
            ShakeEmitter shakeEmitter = effectPrefab.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.15f;
            shakeEmitter.radius = 72f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = true;

            shakeEmitter.wave = new Wave
            {
                amplitude = 0.2f,
                frequency = 60f,
                cycleOffset = 0f
            };
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            /*NetworkSoundEventCatalog.getSoundEventDefs += delegate (List<NetworkSoundEventDef> list)
            {
                list.Add(networkSoundEventDef);
            };*/
            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }

        internal static Texture LoadCharacterIcon(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++)
            {
                if (assetNames[i].Contains(resourceName.ToLower()))
                {
                    assetExists = true;
                    i = assetNames.Length;
                }
            }

            if (!assetExists)
            {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
        }

        #region materials(old)
        private const string obsolete = "use `Materials.CreateMaterial` instead, or use the extension `Material.SetHotpooMaterial` directly on a material";
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName) => Materials.CreateHotpooMaterial(materialName);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission) => Materials.CreateHotpooMaterial(materialName);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor) => CreateMaterial(materialName, emission, emissionColor, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength) {
            return Materials.CreateHotpooMaterial(materialName)
                            .MakeUnique()
                            .SetEmission(emission, emissionColor)
                            .SetNormal(normalStrength);
        }
        #endregion materials(old)

    }
}