using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.IO;
using HarmonyLib;
using System.Reflection;
using ServerSync;

namespace ValheimReelableHarpoon
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInIncompatibility("com.kaden.harpoonreelin")]
    public class ValheimReelableHarpoonPlugin : BaseUnityPlugin
    {
        private const string PluginGuid = "ztag96.ValheimReelableHarpoon";
        private const string PluginName = "Valheim Reelable Harpoon";
        private const string PluginVersion = "1.0.0";
        private const string ConfigFileName = PluginGuid + ".cfg";
        private const float DefaultPullSpeed = 1f;
        private const float DefaultMinDistance = 5f;
        private const bool DefaultCanPullUp = true;
        private const bool DefaultIsLocked = true;

        internal static ConfigEntry<float> ConfigPullSpeed;
        internal static ConfigEntry<float> ConfigMinDistance;
        internal static ConfigEntry<bool> ConfigCanPullUp;
        private static ConfigEntry<bool> ConfigIsLocked;
        
        public static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(PluginName);
        static readonly string ConfigFileFullPath =
            Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        
        private readonly Harmony _harmonyInstance = new Harmony(PluginGuid);

        private static ConfigSync _configSync;

        private  void Awake()
        {
            _configSync = new ConfigSync("ztag96.ValheimReelableHarpoon")
            {
                DisplayName = PluginName, CurrentVersion = PluginVersion, MinimumRequiredVersion = PluginVersion
            };
            
            Config.SaveOnConfigSet = false;

            ConfigIsLocked = Config.Bind<bool>("Admin", "IsLocked", DefaultIsLocked, "Whether the config is locked and cannot be edited.");
            _configSync.AddLockingConfigEntry(ConfigIsLocked);
            ConfigPullSpeed = ConfigBind<float>("General", "PullSpeed", DefaultPullSpeed, "The speed at which the harpoon will reel in.");
            ConfigMinDistance = ConfigBind<float>("General", "MinDistance", DefaultMinDistance,
                "The closest distance a creature can be before reeling stops.");
            ConfigCanPullUp = ConfigBind<bool>("General", "CanPullUp", DefaultCanPullUp, "Whether the harpoon can pull creatures up (or down). The harpoon cannot do this normally.");
            
            Logger.LogDebug($"Saving config file to: {ConfigFileFullPath}");
            Config.Save();
            Config.SaveOnConfigSet = true;
            SetupWatcher();
            
            Logger.LogDebug($"Config value PullSpeed: {ConfigPullSpeed.Value}");
            Logger.LogDebug($"Config value MinDistance: {ConfigMinDistance.Value}");
            Logger.LogDebug($"Config value CanPullUp: {ConfigCanPullUp.Value}");
            Logger.LogDebug($"Config value ConfigIsLocked: {ConfigIsLocked.Value}");

            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Valheim Reelable Harpoons loaded successfully! Thank you!");
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            var watcher = new FileSystemWatcher(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
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
        
        ConfigEntry<T> ConfigBind<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);
            SyncedConfigEntry<T> syncedConfigEntry = _configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        
        ConfigEntry<T> ConfigBind<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => ConfigBind(group, name, value, new ConfigDescription(description), synchronizedSetting);
    }
}