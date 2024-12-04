#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace GensouLib.Unity.Tools
{
    public class PackageInstaller : EditorWindow
    {
        [MenuItem("Tools/Install Packages")]
        public static void ShowWindow()
        {
            GetWindow<PackageInstaller>("Install Packages");
        }

        private void OnGUI()
        {
            GUILayout.Label("Install Packages", EditorStyles.boldLabel);

            // 安装 Newtonsoft.Json
            if (GUILayout.Button("Install Newtonsoft.Json"))
            {
                InstallPackage("com.unity.nuget.newtonsoft-json", "Newtonsoft.Json");
            }

            GUILayout.Space(10);

            // 安装 Addressables
            if (GUILayout.Button("Install Addressables"))
            {
                InstallPackage("com.unity.addressables", "Addressables");
            }
        }

        private void InstallPackage(string packageName, string packageDisplayName)
        {
            if (!IsPackageInstalled(packageName))
            {
                UnityEditor.PackageManager.Client.Add(packageName);
            }
            else
            {
                ShowDialog($"{packageDisplayName} Already Installed", $"{packageDisplayName} is already installed.");
            }
        }
        private bool IsPackageInstalled(string packageName)
        {
            if (packageName == "com.unity.nuget.newtonsoft-json")
            {
                return IsNewtonsoftJsonInstalled();
            }
            else if (packageName == "com.unity.addressables")
            {
                return IsAddressablesInstalled();
            }
            else
            {
                return false;
            }
        }

        private bool IsNewtonsoftJsonInstalled()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Newtonsoft.Json");

            return assembly != null && assembly.GetType("Newtonsoft.Json.JsonConvert") != null;
        }

        private bool IsAddressablesInstalled()
        {
         
            return Type.GetType("UnityEngine.AddressableAssets.Addressables, Unity.Addressables") != null;
        }

        private void ShowDialog(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }
    }
}
#endif
