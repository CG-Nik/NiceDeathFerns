using Alta.Chunks;
using Alta.Intelligence;
using Alta.Mechanisms;
using Alta.Networking;
using Alta.Networking.Servers;
using Alta.StatSystem;
using HarmonyLib;
using MelonLoader;
using System.Text;
using UnityEngine;

[assembly: MelonInfo(typeof(NiceDeathFerns.Core), "NiceDeathFerns", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace NiceDeathFerns
{
    public class InitializePatch
    {
        internal static void Postfix(NetworkPrefab __instance)
        {
            System.Random rng = new();
            float random = rng.NextFloat();
            if
            (
                (__instance.Hash == 37642u && random <= Core.melonPrefEntry_DestroyDeathFernChance.Value)
                || (__instance.Hash == 24402u && random <= Core.melonPrefEntry_DestroyVineAttackFernChance.Value)
                || (__instance.Hash == 65004u && random <= Core.melonPrefEntry_DestroyMotherFernChance.Value)
            )
            {
                GameObject.Destroy(__instance.gameObject);
            }
        }
    }

    public class Core : MelonMod
    {
        private MelonPreferences_Category melonPrefCategory;
        internal static MelonPreferences_Entry<float> melonPrefEntry_DestroyDeathFernChance;
        internal static MelonPreferences_Entry<float> melonPrefEntry_DestroyVineAttackFernChance;
        internal static MelonPreferences_Entry<float> melonPrefEntry_DestroyMotherFernChance;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");

            melonPrefCategory = MelonPreferences.CreateCategory("NiceDeathFerns");
            melonPrefEntry_DestroyDeathFernChance = melonPrefCategory.CreateEntry<float>("DestroyDeathFernChance", 0.95f);
            melonPrefEntry_DestroyVineAttackFernChance = melonPrefCategory.CreateEntry<float>("DestroyVineAttackFernChance", 0.95f);
            melonPrefEntry_DestroyMotherFernChance = melonPrefCategory.CreateEntry<float>("DestroyMotherFernChance", 0f);
            MelonPreferences.Save();

            ServerHandler.Initialized += ServerInitialized;
        }

        private void ServerInitialized(ServerHandler serverHandler)
        {
            HarmonyInstance.Patch(AccessTools.Method(typeof(NetworkPrefab), "Initialize"), postfix: new HarmonyMethod(typeof(InitializePatch), nameof(InitializePatch.Postfix)));
        }
    }
}