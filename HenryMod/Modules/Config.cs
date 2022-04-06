using BepInEx.Configuration;
using UnityEngine;

namespace HenryMod.Modules {
    public static class Config
    {
        public static ConfigEntry<bool> rampageEffects;
        public static ConfigEntry<bool> ExtraSkins;

        public static ConfigEntry<KeyCode> restKeybind;
        public static ConfigEntry<KeyCode> danceKeybind;

        public static void ReadConfig()
        {
            ExtraSkins
                = HenryPlugin.instance.Config.Bind<bool>("Henry", 
                                                         "Extra Skins",
                                                         false,
                                                         "A gift from rob. Enable extra skins from the game he's workin on.");

            rampageEffects = HenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Visuals", "Rampage VFX"), true, new ConfigDescription("Enable Rampage visual effects"));
            restKeybind = HenryPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Keybinds", "Rest"), KeyCode.Alpha1, new ConfigDescription("Keybind used to perform the Rest emote"));
            danceKeybind = HenryPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Keybinds", "Dance"), KeyCode.Alpha3, new ConfigDescription("Keybind used to perform the Dance emote"));
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return HenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return HenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}