using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules {

    internal static class Materials {

        private static List<Material> cachedMaterials = new List<Material>();

        public static Material CreateHotpooMaterial(string materialName) {
            Material tempMat = cachedMaterials.Find(mat => {
                materialName.Replace(" (Instance)", "");
                return mat.name == materialName;
            });
            if (tempMat) {
                //Debug.Log("returning cached material for " + materialName);
                return tempMat;
            }

            //Material mat = UnityEngine.Object.Instantiate<Material>(Assets.commandoMat);
            tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat) {
                Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                return new Material(Assets.hotpoo);
            }

            return tempMat.SetHotpooMaterial();
        }

        private static Material CreateHotpooMaterial(Material tempMat) {
            if (cachedMaterials.Contains(tempMat)) {

                //Debug.Log("returning cached material for " + tempMat);
                return tempMat;
            }
            return tempMat.SetHotpooMaterial();
        }

        public static Material SetHotpooMaterial(this Material tempMat) {
            if (cachedMaterials.Contains(tempMat)) {

                //Debug.Log("returning cached material for " + tempMat);
                return tempMat;
            }

            //Debug.Log("creating hotpoo material with " + tempMat);

            float? bumpScale = null;
            Color? emissionColor = null;

            //grab values before the shader changes
            if (tempMat.IsKeywordEnabled("_NORMALMAP")) {
                bumpScale = tempMat.GetFloat("_BumpScale");
            }
            if (tempMat.IsKeywordEnabled("_EMISSION")) {
                emissionColor = tempMat.GetColor("_EmissionColor");
            }

            tempMat.shader = Assets.hotpoo;

            //apply values after shader is set
            tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
            tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            tempMat.EnableKeyword("DITHER");

            if (tempMat.IsKeywordEnabled("LIMBREMOVAL")) {
                tempMat.SetInt("_LimbRemovalOn", 1);
            }

            if (bumpScale != null) {
                tempMat.SetFloat("_NormalStrength", (float)bumpScale);
            }
            if (emissionColor != null) {
                tempMat.SetColor("_EmColor", (Color)emissionColor);
                tempMat.SetFloat("_EmPower", 1);
            }

            if (tempMat.IsKeywordEnabled("CULL")) {
                tempMat.SetInt("_Cull", 0);
            }

            cachedMaterials.Add(tempMat);
            return tempMat;
        }

        /// <summary>
        /// makes this a unique material if we already have this material cached (i.e. you want a changed variant). New material will not be cached
        /// If it was not cached in the first place, simply returns as it is unique.
        /// </summary>
        public static Material MakeUnique(this Material material) {

            if (cachedMaterials.Contains(material)) {

                //Debug.Log("cloning cached material for " + material.name + " and making unique");
                return new Material(material);
            }
            return material;
        }

        public static Material SetColor(this Material material, Color color) {
            material.SetColor("_Color", color);
            return material;
        }

        public static Material SetNormal(this Material material, float normalStrength = 1) {
            material.SetFloat("_NormalStrength", normalStrength);
            return material;
        }

        public static Material SetEmission(this Material material) => SetEmission(material, 1);
        public static Material SetEmission(this Material material, float emission) {
            material.SetFloat("_EmPower", emission);
            return material;
        }
        public static Material SetEmission(this Material material, float emission, Color emissionColor) {
            material.SetFloat("_EmPower", emission);
            material.SetColor("_EmColor", emissionColor);
            return material;
        }
        public static Material SetCull(this Material material, bool cull = false) {
            material.SetInt("_Cull", cull ? 1 : 0);
            return material;
        }
    }
}