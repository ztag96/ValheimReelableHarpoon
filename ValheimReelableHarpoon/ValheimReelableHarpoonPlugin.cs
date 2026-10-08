using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;

namespace ValheimReelableHarpoon
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class ValheimReelableHarpoonPlugin : BaseUnityPlugin
    {
        const string pluginGUID = "ztag96.ValheimReelableHarpoon";
        const string pluginName = "Reelable Harpoon";
        const string pluginVersion = "0.0.1";

        private readonly Harmony HarmonyInstance = new Harmony(pluginGUID);

        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);

        private ConfigEntry<float> _configPullSpeed;
        private ConfigEntry<float> _configMinDistance;
        private ConfigEntry<float> _configStaminaDrainPerSecond;
        private ConfigEntry<bool> _configIsWeightFactored;
        private ConfigEntry<bool> _configCanPullUp;

        public void Awake()
        {
            _configPullSpeed = Config.Bind<float>("General", "PullSpeed", 5f, "The speed at which the harpoon will reel in.");
            _configMinDistance = Config.Bind<float>("General", "MinDistance", 5f, "The closest distance a creature can be before reeling stops.");
            _configStaminaDrainPerSecond = Config.Bind<float>("General", "StaminaDrainPerSecond", 10f, "The amount of stamina drained per second while reeling in a creature. The default value matches the harpoon's normal stamina drains.");
            _configIsWeightFactored = Config.Bind<bool>("General", "IsWeightFactored", false, "Whether the stamina drain is factored by the creature's weight.");
            _configCanPullUp = Config.Bind<bool>("General", "CanPullUp", false, "Whether the harpoon can pull the target up. The harpoon normally does not do this.");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            logger.LogInfo("Valheim Reelable Harpoons loaded successfully!");
        }
    }
}