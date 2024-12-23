#if UNITY_EDITOR && ENABLE_ADDRESSABLES
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using System.Collections.Generic;

namespace GensouLib.Unity.Tools
{
    public class AddressablesAutoConfigurator : EditorWindow
    {
        private string uiFolder = "Assets/UI";             // UI资源文件夹
        private string audioFolder = "Assets/Audio";       // 音频资源文件夹
        private string backgroundsFolder = "Assets/Backgrounds"; // 背景资源文件夹
        private string charactersFolder = "Assets/Characters"; // 背景资源文件夹

        private string uiGroupName = "UI_Group";
        private string audioGroupName = "Audio_Group";
        private string backgroundsGroupName = "Backgrounds_Group";
        private string charactersGroupName = "Characters_Group";

        private readonly Dictionary<string, HashSet<string>> validExtensions = new()
        {
            { "UI", new HashSet<string> { ".prefab" } },
            { "Audio", new HashSet<string> { ".mp3", ".wav", ".ogg" } },
            { "Backgrounds", new HashSet<string> { ".png", ".jpg" } },
            { "Characters", new HashSet<string> { ".png", ".jpg" } }
        };

        [MenuItem("Tools/Configure Addressables")]
        public static void ShowWindow()
        {
            GetWindow(typeof(AddressablesAutoConfigurator));
        }

        private void OnGUI()
        {
            GUILayout.Label("Addressables auto configurator(自动配置 Addressables)", EditorStyles.boldLabel);

            // 目标文件夹路径
            GUILayout.Label("UI folder(UI 文件夹):");
            uiFolder = EditorGUILayout.TextField(uiFolder);
            GUILayout.Label("Audio folder(音频文件夹):");
            audioFolder = EditorGUILayout.TextField(audioFolder);
            GUILayout.Label("Backgrounds folder(背景文件夹):");
            backgroundsFolder = EditorGUILayout.TextField(backgroundsFolder);
            GUILayout.Label("Characters folder(立绘文件夹):");
            charactersFolder = EditorGUILayout.TextField(charactersFolder);

            GUILayout.Space(10);

            // 分组名称
            GUILayout.Label("UI group name(UI 分组名称):");
            uiGroupName = EditorGUILayout.TextField(uiGroupName);
            GUILayout.Label("Audio group name(音频分组名称):");
            audioGroupName = EditorGUILayout.TextField(audioGroupName);
            GUILayout.Label("Backgrounds group name(背景分组名称):");
            backgroundsGroupName = EditorGUILayout.TextField(backgroundsGroupName);
            GUILayout.Label("Characters group name(立绘分组名称):");
            charactersGroupName = EditorGUILayout.TextField(charactersGroupName);

            GUILayout.Space(10);

            if (GUILayout.Button("Configure Addressables(配置 Addressables)"))
            {
                ConfigureAddressables();
            }
        }

        private void ConfigureAddressables()
        {
            // Step 1: 检查并初始化 Addressables 设置
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            // Step 2: 获取或创建每种资源的分组
            AddressableAssetGroup uiGroup = GetOrCreateGroup(settings, uiGroupName);
            AddressableAssetGroup audioGroup = GetOrCreateGroup(settings, audioGroupName);
            AddressableAssetGroup backgroundsGroup = GetOrCreateGroup(settings, backgroundsGroupName);
            AddressableAssetGroup charactersGroup = GetOrCreateGroup(settings, charactersGroupName);

            // Step 3: 扫描各个文件夹并添加资源到对应分组
            AddAssetsToGroup(settings, uiFolder, uiGroup, "UI");
            AddAssetsToGroup(settings, audioFolder, audioGroup, "Audio");
            AddAssetsToGroup(settings, backgroundsFolder, backgroundsGroup, "Backgrounds");
            AddAssetsToGroup(settings, charactersFolder, charactersGroup, "Characters");

            // 保存 Addressables 配置
            AssetDatabase.SaveAssets();
            Debug.Log("Addressables configuration complete! (Addressables 配置完成！)");
        }

        // 将资源添加到指定分组
        private void AddAssetsToGroup(AddressableAssetSettings settings, string folderPath, AddressableAssetGroup group, string typeDescription)
        {

            string typeDescriptionLocalized = typeDescription switch
            {
                "UI" => "UI",
                "Audio" => "音频",
                "Backgrounds" => "背景",
                "Characters" => "立绘",
                _ => typeDescription // 如果没有匹配，则保留原有字符串
            };

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($" Invalid {typeDescription} folder: {folderPath} ({typeDescriptionLocalized} 文件夹无效: {folderPath})");
                return;
            }

            string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { folderPath });
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                if (Directory.Exists(assetPath))
                {
                    continue; // 跳过文件夹
                }

                // 验证资源文件的扩展名
                if (!IsValidAssetExtension(assetPath, typeDescription))
                {
                    Debug.LogWarning($"Invalid file type: {assetPath} (无效的文件类型: {assetPath})");
                    continue; // 跳过无效文件
                }

                AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);

                // 设置资源的键为文件名（没有后缀）
                string assetKey = Path.GetFileNameWithoutExtension(assetPath);
                entry.SetAddress(assetKey);

                Debug.Log($"Added {typeDescription} resource: {assetPath} (key: {assetKey}) (已添加 {typeDescriptionLocalized} 资源: {assetPath} (键: {assetKey}))");
            }
        }

        // 创建分组（若不存在）
        private AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string groupName)
        {
            var group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, true, null);
                group.AddSchema<BundledAssetGroupSchema>();
            }
            return group;
        }

        // 根据资源类型验证扩展名
        private bool IsValidAssetExtension(string assetPath, string typeDescription)
        {
            string extension = Path.GetExtension(assetPath).ToLower();

            // 检查字典中是否存在该资源类型，并验证文件扩展名是否在有效扩展名集合中
            if (validExtensions.ContainsKey(typeDescription))
            {
                return validExtensions[typeDescription].Contains(extension);
            }

            // 如果字典中没有该类型的资源，默认返回 false
            return false;
        }

    }
}
#endif