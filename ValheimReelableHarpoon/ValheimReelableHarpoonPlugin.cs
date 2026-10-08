using BepInEx;
using BepInEx.Logging;
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

        public void Awake()
        {
            logger.LogInfo("Valheim Reelable Harpoons loaded successfully!");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void Update()
        {
            if (ZInput.GetButtonDown("Use") && ZInput.GetButtonDown("Run"))
            {
                LogInfo("Use and Run buttons pressed.");
            }
        }
        
        public static void LogInfo(string message)
        {
            logger.LogInfo(message);
        }
    }
}