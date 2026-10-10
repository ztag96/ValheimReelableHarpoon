using System;
using HarmonyLib;

namespace ValheimReelableHarpoon
{
    
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        private static bool _canReel;
        private static readonly int Ratio = 10;

        internal static Func<bool> GetInput = () =>
        {
            bool isUsing = ZInput.GetButton("Use") || ZInput.GetButton("JoyUse");
            bool isRunning = ZInput.GetButton("Run") || ZInput.GetButton("JoyRun");
            return isUsing && isRunning;
        };
            
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPrefix]
        private static void UpdateStatusEffectPostfix(SE_Harpooned __instance, float dt)
        {
            _canReel = GetInput();
            bool isFar = __instance.m_baseDistance > ValheimReelableHarpoonPlugin.ConfigMinDistance.Value;

            if (_canReel && isFar)
            {
                __instance.m_baseDistance -= dt * ValheimReelableHarpoonPlugin.ConfigPullSpeed.Value / Ratio;
            }
        }

        [HarmonyPatch(typeof(Utils), nameof(Utils.Pull)), HarmonyPrefix]
        private static void PullPrefix(ref bool noUpForce)
        {
            if (_canReel)
            {
                noUpForce = !ValheimReelableHarpoonPlugin.ConfigCanPullUp.Value;
            }
        }
    }  
}