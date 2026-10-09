using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.IO;
using HarmonyLib;
using System.Reflection;
// using ServerSync;

namespace ValheimReelableHarpoon
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class ValheimReelableHarpoonPlugin : BaseUnityPlugin
    {
        const string PluginGuid = "ztag96.ValheimReelableHarpoon";
        const string PluginName = "Reelable Harpoon";
        const string PluginVersion = "0.0.1";
        const string ConfigFileName = PluginGuid + ".cfg";
        const float DefaultMinDistance = 5f;
        const float DefaultPullSpeed = 5f;

        internal static ConfigEntry<float> ConfigPullSpeed;
        internal static ConfigEntry<float> ConfigMinDistance;
        // internal static ConfigEntry<float> ConfigStaminaDrainPerSecond;
        // internal static ConfigEntry<bool> ConfigIsWeightFactored;
        // internal static ConfigEntry<bool> ConfigCanPullUp;
        
        public static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(PluginName);
        static readonly string ConfigFileFullPath =
            Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        readonly Harmony _harmonyInstance = new Harmony(PluginGuid);

        /*
        private ServerSync.ConfigSync _configSync = new ServerSync.ConfigSync(PluginGuid)
        {
            DisplayName = PluginName, CurrentVersion = PluginVersion
        };

        */
        public void Awake()
        {
            Config.SaveOnConfigSet = false;

            ConfigPullSpeed = Config.Bind<float>("General", "PullSpeed", DefaultPullSpeed, "The speed at which the harpoon will reel in.");
            ConfigMinDistance = Config.Bind<float>("General", "MinDistance", DefaultMinDistance,
                "The closest distance a creature can be before reeling stops.");
            // ConfigStaminaDrainPerSecond = Config.Bind<float>("General", "StaminaDrainPerSecond", 10f,
            //     "Currently nonfunctional. The amount of stamina drained per second while reeling in a creature. The default value matches the harpoon's normal stamina drains.");
            // ConfigIsWeightFactored = Config.Bind<bool>("General", "IsWeightFactored", false,
            //     "Currently nonfunctional. Whether the stamina drain is factored by the creature's weight.");
            // ConfigCanPullUp = Config.Bind<bool>("General", "CanPullUp", false,
            //     "Currently nonfunctional. Whether the harpoon can pull the target up. The harpoon normally does not do this.");

            Logger.LogDebug($"Saving config file to: {ConfigFileFullPath}");
            Config.Save();
            Config.SaveOnConfigSet = true;
            SetupWatcher();

            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Valheim Reelable Harpoons loaded successfully! Thank you!");
        }

        void OnDestroy()
        {
            Config.Save();
        }

        void SetupWatcher()
        {
            var watcher = new FileSystemWatcher(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                Logger.LogDebug("Attempting to reload configuration...");
                Config.Reload();
            }
            catch
            {
                Logger.LogError($"There was an issue loading {ConfigFileName}");
            }
        }
        
        /*
        ConfigEntry<T> ConfigBind<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = _configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        
        ConfigEntry<T> ConfigBind<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => ConfigBind(group, name, value, new ConfigDescription(description), synchronizedSetting);
    */
    }
}