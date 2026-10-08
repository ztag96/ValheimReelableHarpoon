using HarmonyLib;
using UnityEngine;
using System;

namespace ValheimReelableHarpoon
{
    [HarmonyPatch]
    internal class HarpoonReeling
    {
        [HarmonyPatch(typeof(SE_Harpooned), nameof(SE_Harpooned.UpdateStatusEffect)), HarmonyPostfix]
        private static void UpdateStatusEffectPostfix(SE_Harpooned __instance, float dt)
        {
            
            if (__instance.m_attacker == null)
            {
                ValheimReelableHarpoonPlugin.logger.LogWarning("Harpooned attacker is null!");
                return;
            }
            
            if(__instance.m_attacker != Player.m_localPlayer)
            {
                ValheimReelableHarpoonPlugin.logger.LogWarning("Harpooned attacker is not the local player!");
                return;
            }

            if (ZInput.GetButton("Use") || ZInput.GetButton("JoyUse"))
            {
                ValheimReelableHarpoonPlugin.logger.LogInfo("Harpoon reeling!");
                
                /*
                if(__instance.m_character == null)
                {
                    ValheimReelableHarpoonPlugin.logger.LogWarning("Harpooned character is null!");
                    return;
                }

                if (__instance.m_attacker == null)
                {
                    ValheimReelableHarpoonPlugin.logger.LogWarning("Harpooned attacker is null!");
                    return;
                }
                
                Rigidbody characterRb = __instance.m_character.GetComponent<Rigidbody>();
                Vector3 targetPos = __instance.m_attacker.transform.position;
                
                var pullPower = Pull(characterRb, targetPos, __instance.m_pullSpeed, __instance.m_pullForce, true, true, __instance.m_forcePower);

                ValheimReelableHarpoonPlugin.logger.LogInfo($"Pulling in direction: {pullPower.normalized} at force: {pullPower.magnitude}.");
                */
                
                // __instance.m_drainStaminaTimer += dt;
                // if (__instance.m_drainStaminaTimer > __instance.m_staminaDrainInterval && pullPower > 0f)
                // {
                //     ValheimReelableHarpoonPlugin.logger.LogInfo($"Pulling with {pullPower} power.");
                //     __instance.m_drainStaminaTimer = 0f;
                //     float stamina = __instance.m_staminaDrain * pullPower * __instance.m_character.GetMass();
                //     __instance.m_attacker.UseStamina(stamina);
                // }
            }
        }
    
        // Modified Pull from assembly_utils.Utils.Pull. I'm unsure what some of these original values were supposed to be named.
        // TODO: Patch over pull instead.
        private static Vector3 Pull(Rigidbody body, Vector3 target, float speed, float force, bool noUpForce = false, bool useForce = false, float power = 1f)
        {
            Vector3 normalized = (target - body.position).normalized;
            Vector3 val2 = Vector3.Project(body.linearVelocity, normalized.normalized);
            Vector3 val3 = normalized.normalized * speed - val2;
            if (noUpForce && val3.y > 0f)
            {
                val3.y = 0f;
            }
            ForceMode forceMode = (ForceMode)(useForce ? 1 : 2);
            Vector3 finalForce = val3 * Mathf.Clamp01(force);
            body.AddForce(finalForce, forceMode);
            return finalForce;
        }
    }  
}