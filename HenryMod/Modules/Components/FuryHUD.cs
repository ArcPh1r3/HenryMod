using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HenryMod.Modules.Components
{
    public class FuryHUD : MonoBehaviour
    {
        public GameObject furyGauge;
        public Image furyFill;

        private Color startColor;
        private Color endColor;
        private float currentFill;

        private HUD hud;

        private void Awake()
        {
            this.hud = this.GetComponent<HUD>();
            this.startColor = new Color(171, 115, 10);
            this.endColor = new Color(236, 82, 0);
        }

        private void FillGauge(float desiredFill)
        {
            if (desiredFill > this.currentFill)
            {
                this.currentFill += 15f * Time.deltaTime;
                if (this.currentFill > desiredFill) this.currentFill = desiredFill;
            }
            else
            {
                this.currentFill -= 15f * Time.deltaTime;
                if (this.currentFill < desiredFill) this.currentFill = desiredFill;
            }
        }

        public void Update()
        {
            if (this.hud.targetBodyObject)
            {
                HenryFuryComponent furyComponent = this.hud.targetBodyObject.GetComponent<HenryFuryComponent>();
                if (furyComponent)
                {
                    PlayerCharacterMasterController masterController = this.hud.targetMaster ? this.hud.targetMaster.playerCharacterMasterController : null;

                    if (this.furyGauge)
                    {
                        this.furyGauge.gameObject.SetActive(true);

                        float _fillAmount = furyComponent.currentFury / furyComponent.maxFury;
                        float oldFillAmount = this.furyFill.fillAmount;

                        this.FillGauge(_fillAmount);

                        this.furyFill.fillAmount = this.currentFill;

                        float r = Mathf.Lerp(this.startColor.r, this.endColor.r, this.currentFill);
                        float g = Mathf.Lerp(this.startColor.g, this.endColor.g, this.currentFill);
                        float b = Mathf.Lerp(this.startColor.b, this.endColor.b, this.currentFill);
                        Color desiredColor = new Color(r, g, b);
                        if (this.currentFill >= 1f) desiredColor = Color.cyan;

                        this.furyFill.color = desiredColor;
                    }
                }
                else
                {
                    if (this.furyGauge)
                    {
                        this.furyGauge.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}