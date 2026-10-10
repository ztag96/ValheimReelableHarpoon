using System;
using HarmonyLib;
using UnityEngine;

namespace ValheimReelableHarpoon
{
    
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        private static bool _canReel;
        private static readonly int Ratio = 10;
        private static Rigidbody _body;

        internal static Func<bool> GetInput = () =>
        {
            bool isUsingJoy = ZInput.GetButton("JoyUse");
            bool isUsingKey = ZInput.GetButton("Use");
            bool isRunning = ZInput.GetButton("Run");
            return isUsingKey && isRunning || isUsingJoy;
        };
            
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPrefix]
        private static void UpdateStatusEffectPostfix(SE_Harpooned __instance, float dt)
        {
            _canReel = GetInput();
            bool isFar = __instance.m_baseDistance > ValheimReelableHarpoonPlugin.ConfigMinDistance.Value;

            if (_canReel && isFar)
            {
                __instance.m_baseDistance -= dt * ValheimReelableHarpoonPlugin.ConfigPullSpeed.Value / Ratio;
                _body = __instance.m_character.m_body;
            }
        }

        [HarmonyPatch(typeof(Utils), nameof(Utils.Pull)), HarmonyPrefix]
        private static void PullPrefix(Rigidbody body, ref bool noUpForce)
        {
            if (_canReel && body == _body)
            {
                noUpForce = !ValheimReelableHarpoonPlugin.ConfigCanPullUp.Value;
            }
        }
    }  
}