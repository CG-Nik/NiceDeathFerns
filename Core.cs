using Alta.Networking;
using Alta.Networking.Servers;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(NiceDeathFerns.Core), "NiceDeathFerns", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace NiceDeathFerns
{
    public class InitializePatch
    {
        internal static void Postfix(NetworkPrefab __instance)
        {
            if ((__instance.Hash == 37642u && Core.melonPrefEntry_DestroyDeathFerns.Value)
                || (__instance.Hash == 24402u && Core.melonPrefEntry_DestroyVineAttackFerns.Value)
                || (__instance.Hash == 65004u && Core.melonPrefEntry_DestroyMotherFerns.Value)
                )
            {
                GameObject.Destroy(__instance.gameObject);
            }
        }
    }

    public class Core : MelonMod
    {
        private MelonPreferences_Category melonPrefCategory;
        internal static MelonPreferences_Entry<bool> melonPrefEntry_DestroyDeathFerns;
        internal static MelonPreferences_Entry<bool> melonPrefEntry_DestroyVineAttackFerns;
        internal static MelonPreferences_Entry<bool> melonPrefEntry_DestroyMotherFerns;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");

            melonPrefCategory = MelonPreferences.CreateCategory("NiceDeathFerns");
            melonPrefEntry_DestroyDeathFerns = melonPrefCategory.CreateEntry<bool>("DestroyDeathFerns", true);
            melonPrefEntry_DestroyVineAttackFerns = melonPrefCategory.CreateEntry<bool>("DestroyVineAttackFerns", true);
            melonPrefEntry_DestroyMotherFerns = melonPrefCategory.CreateEntry<bool>("DestroyMotherFerns", false);

            ServerHandler.Initialized += ServerInitialized;
        }

        private void ServerInitialized(ServerHandler serverHandler)
        {
            HarmonyInstance.Patch(AccessTools.Method(typeof(NetworkPrefab), "Initialize"), postfix: new HarmonyMethod(typeof(InitializePatch), nameof(InitializePatch.Postfix)));
        }
    }
}