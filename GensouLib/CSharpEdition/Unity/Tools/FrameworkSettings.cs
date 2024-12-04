#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace GensouLib.Unity.Tools
{
    public class FrameworkSettings : EditorWindow
    {
        private int selectedOptionIndex = 0;
        private int currentOptionIndex = 0;
        private bool enableNewtonsoftJson = false;
        private readonly string[] options = { "UnityEngine.Resources", "Addressables"};
        private const string PrefKey = "GensouFramework.ResourceLoadingMethod";
        private const string PrefKey1 = "GensouFramework.EnableNewtonsoftJson";

        [MenuItem("Tools/GensouFramework Settings")]
        public static void ShowWindow()
        {
            GetWindow<FrameworkSettings>("Gensou Framework Settings");
        }

        private void OnEnable()
        {
            // 加载保存的选项索引
            currentOptionIndex = EditorPrefs.GetInt(PrefKey, 0);
            enableNewtonsoftJson = EditorPrefs.GetBool(PrefKey1, false);
        }

        private void OnGUI()
        {
            GUILayout.Label("Resource loading method (资源加载方式)", EditorStyles.boldLabel);

            GUILayout.Space(10);

            GUILayout.Label($"Current loading method (当前的加载方式): \n{options[currentOptionIndex]}", EditorStyles.wordWrappedLabel);

            GUILayout.Space(10);

            GUILayout.Label("Switch loading method(切换加载方式)");
            // 下拉菜单
            selectedOptionIndex = EditorGUILayout.Popup(currentOptionIndex, options);

            GUILayout.Space(10);

            enableNewtonsoftJson = GUILayout.Toggle(enableNewtonsoftJson, "Enable Newtonsoft.Json (启用 Newtonsoft.Json)");

            GUILayout.Space(10);

            if (GUILayout.Button("Save settings (保存设置)"))
            {
                SaveSettings();
            }

            GUILayout.Space(10);

            // 重置按钮
            if (GUILayout.Button("Reset to default settings (重置为默认设置)"))
            {
                ResetToDefault();
            }
        }

        private void SaveSettings()
        {

            if (selectedOptionIndex == 1)
            {
                if (IsAddressablesInstalled())
                {
                    UpdateDirective("ENABLE_ADDRESSABLES", true);
                    currentOptionIndex = 1;
                }
                else
                {
                    // 弹出警告对话框
                    ShowDialog(
                        "Addressables Not Installed",
                        "The Addressables package is not installed. Please install it via the Unity Package Manager or framework tool : \"Package Installer\" before switching to Addressables."
                    );

                    // 恢复选项索引为当前索引
                    selectedOptionIndex = currentOptionIndex;                    
                }
            }
            else
            {
                UpdateDirective("ENABLE_ADDRESSABLES", false);
                currentOptionIndex = 0;
            }
            
            if (enableNewtonsoftJson)
            {
                if (IsNewtonsoftJsonInstalled())
                {
                    UpdateDirective("ENABLE_JSONNET", true);
                }
                else
                {
                    ShowDialog(
                        "Newtonsoft.Json Not Installed", 
                        "The Newtonsoft.Json package is not installed. Please install it via the Unity Package Manager or framework tool : \"Package Installer\" before enabling Newtonsoft.Json."
                    );
                    enableNewtonsoftJson = false;
                }
            }
            else
            {
                UpdateDirective("ENABLE_JSONNET", false);
            }

            EditorPrefs.SetInt(PrefKey, selectedOptionIndex);
            EditorPrefs.SetBool(PrefKey1, enableNewtonsoftJson);
            Debug.Log($"Resource loading method saved: {options[selectedOptionIndex]} (资源加载方式已保存: {options[selectedOptionIndex]})");
        }

        private bool IsAddressablesInstalled()
        {
            // 检查 Addressables 类是否存在
            return Type.GetType("UnityEngine.AddressableAssets.Addressables, Unity.Addressables") != null;
        }

        private void ResetToDefault()
        {
            selectedOptionIndex = 0;
            currentOptionIndex = 0;
            enableNewtonsoftJson = false;
            SaveSettings();
            Debug.Log("Already reset to default settings (已重置为默认设置)");
        }

        private bool IsNewtonsoftJsonInstalled()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Newtonsoft.Json");

            return assembly != null && assembly.GetType("Newtonsoft.Json.JsonConvert") != null;
        }

        public static void UpdateDirective(string directive, bool add)
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            if (add && !defines.Contains(directive))
            {
                defines = string.IsNullOrEmpty(defines) ? directive : $"{defines};{directive}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            }
            else if (!add && defines.Contains(directive))
            {
                string newDefines = string.Join(";", defines.Split(';').Where(d => d != directive));
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefines);
            }
        }

        private void ShowDialog(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }
    }
    
}
#endif