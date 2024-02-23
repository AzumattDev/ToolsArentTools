using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ToolsArentTools
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ToolsArentToolsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "ToolsArentTools";
        internal const string ModVersion = "1.0.1";
        internal const string Author = "Azumatt";
        private const string ModGUID = $"{Author}.{ModName}";
        private static string ConfigFileName = $"{ModGUID}.cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        private readonly Harmony _harmony = new(ModGUID);
        public static readonly ManualLogSource ToolsArentToolsLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);


        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
        }
    }
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    static class ZNetSceneAwakePatch
    {
        static void Postfix(ZNetScene __instance)
        {
            foreach (GameObject fab in __instance.m_prefabs.Where(fab => fab.name is "Hammer" or "Cultivator" or "Hoe" || fab.name.ToLower().Contains("pickaxe")))
            {
                if (fab.TryGetComponent(out ItemDrop itemDrop))
                {
                    itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
                }
            }
        }
    }
}