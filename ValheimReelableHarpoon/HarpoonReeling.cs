using HarmonyLib;

namespace ValheimReelableHarpoon
{
    
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        private static bool _canReel;
        
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPrefix]
        private static void UpdateStatusEffectPostfix(SE_Harpooned __instance, float dt)
        {
            bool isUsing = ZInput.GetButton("Use") || ZInput.GetButton("JoyUse");
            bool isRunning = ZInput.GetButton("Run") || ZInput.GetButton("JoyRun");
            bool isFar = __instance.m_baseDistance > ValheimReelableHarpoonPlugin.ConfigMinDistance.Value;
            _canReel = isUsing && isRunning && isFar;
            
            if (_canReel)
            {
                // ValheimReelableHarpoonPlugin.Logger.LogInfo("Harpoon reeling!");
                __instance.m_baseDistance -= dt * ValheimReelableHarpoonPlugin.ConfigPullSpeed.Value;
            }
        }

        [HarmonyPatch(typeof(Utils), nameof(Utils.Pull)), HarmonyPrefix]
        private static void PullPrefix(ref bool noUpForce)
        {
            if (_canReel)
            {
                noUpForce = false;
            }
        }
    }  
}