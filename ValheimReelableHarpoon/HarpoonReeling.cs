using HarmonyLib;
using UnityEngine;

namespace ValheimReelableHarpoon
{
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        private static bool isAttackerSet = false;
        private static Rigidbody characterRB;

        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.SetAttacker)), HarmonyPrefix]
        private static void HarpoonedSetAttackerPrefix(SE_Harpooned __instance)
        {
            if (__instance.m_character == null)
            {
                ValheimReelableHarpoonPlugin.logger.LogWarning("Harpooned character is null!");
                return;
            }
            
            characterRB = __instance.m_character.GetComponent<Rigidbody>();
        }
        
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPostfix]
        private static void HarpoonedSetAttackerPostfix(SE_Harpooned __instance)
        {
            if (!isAttackerSet)
            {
                ValheimReelableHarpoonPlugin.logger.LogInfo($"SE_Harpooned.SetAttacker called. Postfix running...");
            }
            isAttackerSet = true;

            if (__instance.m_attacker.IsBlocking())
            {
                // Pull attached charcter
            }

            characterRB = null;
        }
    }
}