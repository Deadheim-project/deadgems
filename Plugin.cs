using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;
using ItemManager;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.IO;
using System;

namespace DeadGems
{
    [BepInPlugin(PluginGUID, PluginGUID, Version)]

    public class Plugin : BaseUnityPlugin
    {
        ConfigSync configSync = new ConfigSync(PluginGUID) { DisplayName = PluginGUID, CurrentVersion = Version, MinimumRequiredVersion = Version };

        public const string Version = "1.0.0";
        public const string PluginGUID = "Detalhes.DeadGems";
      
        public static ConfigEntry<string> BlockedRockAndTreeDropAtEndAge;

        public static GameObject dontCraftPrefab;

        Harmony _harmony = new Harmony(PluginGUID);

        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        [HarmonyPatch(typeof(ConfigSync), "RPC_ConfigSync")]
        public static class RPC_ConfigSync
        {
            [HarmonyPriority(Priority.Last)]
            private static void Postfix()
            {
                if (ZNet.instance.IsServer()) return;
            }
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")]
        public static class OnSpawned
        {
            [HarmonyPriority(Priority.Last)]
            private static void Postfix()
            {
            }
        }

        static AssetBundle bundle;
        private void Awake()
        {
            Config.SaveOnConfigSet = true;
       
            BlockedRockAndTreeDropAtEndAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilEndAge", "",
                "End");

            _harmony.PatchAll();

            bundle = GetAssetBundleFromResources("deadgems");
            CreatePrefab("cyanGemArmor");
            CreatePrefab("cyanGemBowsAndArtifacts");
            CreatePrefab("cyanGemCapeAndBelt");
            CreatePrefab("cyanGemOneHanded");
            CreatePrefab("cyanGemShields");
            CreatePrefab("cyanGemTwoHanded");

            CreatePrefab("redGemArmor");
            CreatePrefab("redGemBowsAndArtifacts");
            CreatePrefab("redGemCapeAndBelt");
            CreatePrefab("redGemOneHanded");
            CreatePrefab("redGemShields");
            CreatePrefab("redGemTwoHanded");

            CreatePrefab("greenGemArmor");
            CreatePrefab("greenGemBowsAndArtifacts");
            CreatePrefab("greenGemCapeAndBelt");
            CreatePrefab("greenGemOneHanded");
            CreatePrefab("greenGemShields");
            CreatePrefab("greenGemTwoHanded");

            CreatePrefab("blueGemArmor");
            CreatePrefab("blueGemBowsAndArtifacts");
            CreatePrefab("blueGemCapeAndBelt");
            CreatePrefab("blueGemOneHanded");
            CreatePrefab("blueGemShields");
            CreatePrefab("blueGemTwoHanded");

            CreatePrefab("purpleGemArmor");
            CreatePrefab("purpleGemBowsAndArtifacts");
            CreatePrefab("purpleGemCapeAndBelt");
            CreatePrefab("purpleGemOneHanded");
            CreatePrefab("purpleGemShields");
            CreatePrefab("purpleGemTwoHanded");

            CreatePrefab("crystalGemArmor");
            CreatePrefab("crystalGemBowsAndArtifacts");
            CreatePrefab("crystalGemCapeAndBelt");
            CreatePrefab("crystalGemOneHanded");
            CreatePrefab("crystalGemShields");
            CreatePrefab("crystalGemTwoHanded");

            CreatePrefab("roseGemArmor");
            CreatePrefab("roseGemBowsAndArtifacts");
            CreatePrefab("roseGemCapeAndBelt");
            CreatePrefab("roseGemOneHanded");
            CreatePrefab("roseGemShields");
            CreatePrefab("roseGemTwoHanded");
                          
            CreatePrefab("yellowGemArmor");
            CreatePrefab("yellowGemBowsAndArtifacts");
            CreatePrefab("yellowGemTwoHanded");
            CreatePrefab("yellowGemCapeAndBelt");
            CreatePrefab("yellowGemOneHanded");
            CreatePrefab("yellowGemShields");
        }

        private void CreatePrefab(string name)
        {
            GameObject obj = new Item("deadgems", name).Prefab;
            var itemdrop = obj.GetComponent<ItemDrop>();
            itemdrop.m_itemData.m_shared.m_name = FirstCharToUpper(SplitCamelCase(obj.name));
            itemdrop.m_itemData.m_shared.m_description = FirstCharToUpper(SplitCamelCase(obj.name));
            itemdrop.m_itemData.m_shared.m_icons[0] = bundle.LoadAsset<Sprite>(name +".png");
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input)) return "";
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static AssetBundle GetAssetBundleFromResources(string fileName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = executingAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(fileName));
            using Stream stream = executingAssembly.GetManifestResourceStream(text);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}
