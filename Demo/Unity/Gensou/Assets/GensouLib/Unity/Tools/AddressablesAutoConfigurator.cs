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
        private string audioFolder = "Assets/Audio";       // 音频资源文件夹
        private string backgroundFolder = "Assets/Background"; // 背景资源文件夹
        private string characterFolder = "Assets/Character"; // 背景资源文件夹
        private string storyFolder = "Assets/Story";         // 故事资源文件夹

        private string audioGroupName = "Audio_Group";
        private string backgroundGroupName = "Background_Group";
        private string characterGroupName = "Character_Group";
        private string storyGroupName = "Story_Group";

        private readonly Dictionary<string, HashSet<string>> validExtensions = new()
        {
            { "Audio", new HashSet<string> { ".mp3", ".wav", ".ogg" } },
            { "Background", new HashSet<string> { ".png", ".jpg" } },
            { "Character", new HashSet<string> { ".png", ".jpg" } },
            { "Story", new HashSet<string> { ".txt"} }
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
            GUILayout.Label("Audio folder(音频文件夹):");
            audioFolder = EditorGUILayout.TextField(audioFolder);
            GUILayout.Label("Background folder(背景文件夹):");
            backgroundFolder = EditorGUILayout.TextField(backgroundFolder);
            GUILayout.Label("Character folder(立绘文件夹):");
            characterFolder = EditorGUILayout.TextField(characterFolder);
            GUILayout.Label("Story folder(故事文件夹):");
            storyFolder = EditorGUILayout.TextField(storyFolder);

            GUILayout.Space(10);

            // 分组名称
            GUILayout.Label("Audio group name(音频分组名称):");
            audioGroupName = EditorGUILayout.TextField(audioGroupName);
            GUILayout.Label("Backgrounds group name(背景分组名称):");
            backgroundGroupName = EditorGUILayout.TextField(backgroundGroupName);
            GUILayout.Label("Characters group name(立绘分组名称):");
            characterGroupName = EditorGUILayout.TextField(characterGroupName);
            GUILayout.Label("Stories group name(故事分组名称):");
            storyGroupName = EditorGUILayout.TextField(storyGroupName);

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
            AddressableAssetGroup audioGroup = GetOrCreateGroup(settings, audioGroupName);
            AddressableAssetGroup backgroundGroup = GetOrCreateGroup(settings, backgroundGroupName);
            AddressableAssetGroup characterGroup = GetOrCreateGroup(settings, characterGroupName);
            AddressableAssetGroup storyGroup = GetOrCreateGroup(settings, storyGroupName);

            // Step 3: 扫描各个文件夹并添加资源到对应分组
            AddAssetsToGroup(settings, audioFolder, audioGroup, "Audio");
            AddAssetsToGroup(settings, backgroundFolder, backgroundGroup, "Background");
            AddAssetsToGroup(settings, characterFolder, characterGroup, "Character");
            AddAssetsToGroup(settings, storyFolder, storyGroup, "Story");

            // 保存 Addressables 配置
            AssetDatabase.SaveAssets();
            Debug.Log("Addressables configuration complete! (Addressables 配置完成！)");
        }

        // 将资源添加到指定分组
        private void AddAssetsToGroup(AddressableAssetSettings settings, string folderPath, AddressableAssetGroup group, string typeDescription)
        {

            string typeDescriptionLocalized = typeDescription switch
            {
                "Audio" => "音频",
                "Background" => "背景",
                "Character" => "立绘",
                "Story" => "故事",
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