using HarmonyLib;
using UnityEngine;
using System;

namespace ValheimReelableHarpoon
{
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPrefix]
        private static void UpdateStatusEffectPostfix(SE_Harpooned __instance, float dt)
        {
            bool isUsing = ZInput.GetButton("Use") || ZInput.GetButton("JoyUse");
            bool isRunning = ZInput.GetButton("Run") || ZInput.GetButton("JoyRun");
            bool canReel = isUsing && isRunning;
            
            if (canReel)
            {
                ValheimReelableHarpoonPlugin.logger.LogInfo("Harpoon reeling!");
                __instance.m_baseDistance -= dt;
            }
        }
    }  
}