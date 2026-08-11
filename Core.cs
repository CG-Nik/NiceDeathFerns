using Alta.Intelligence;
using Alta.Networking;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(NiceDeathFerns.Core), "NiceDeathFerns", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace NiceDeathFerns
{
    [HarmonyPatch(typeof(NetworkPrefab), "Initialize")]
    public class InitializePatch
    {
        static void Postfix(NetworkPrefab __instance)
        {
            if (__instance.Hash == 37642u // this is Death Fern
                || __instance.Hash == 24402u // this is Vine Attack Fern
                // || __instance.Hash == 65004u // this is Mother Fern
                )
            {
                GameObject.Destroy(__instance.gameObject);
            }
        }
    }

    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}