#if ENABLE_ADDRESSABLES == false
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GensouLib.Unity.ResourceLoader
{
    /// <summary>
    /// 资源加载器 <br/>
    /// Resource Loader
    /// </summary>
    /// <remarks>
    /// 提供同步和异步的资源加载方法。<br/>
    /// Provides synchronous and asynchronous resource loading methods. <br/>
    /// 提供释放已加载的资源的方法。<br/>
    /// Provides methods to release loaded resources. <br/>
    /// 提供获取已加载的资源的方法。<br/>
    /// Provides methods to retrieve loaded resources.
    /// </remarks>
    public class AssetLoader : MonoBehaviour
    {
        // 储存加载过的资源
        private static readonly Dictionary<string, object> loadedAssets = new();

        /// <summary>
        /// 根据加载方式生成完整的资源路径或地址。<br/>
        /// Generates the full resource path or address based on the loading method.
        /// </summary>
        /// <param name="category">
        /// 资源类别，例如 "UI"、"Backgrounds"。<br/>
        /// The resource category, such as "UI" or "Backgrounds".
        /// </param>
        /// <param name="name">
        /// 资源名称，不包含路径前缀或后缀，例如 "MainMenu"。<br/>
        /// The resource name without path prefix or suffix, such as "MainMenu".
        /// </param>
        /// <returns>
        /// 如果使用 UnityEngine.Resources 加载，返回拼接后的完整路径，例如 <c>UI/MainMenu</c>；<br/>
        /// If loading with UnityEngine.Resources, returns the full path, e.g., <c>UI/MainMenu</c>. <br/>
        /// 如果使用 Addressables 加载，直接返回资源地址。<br/>
        /// If loading with Addressables, returns the resource address directly.
        /// </returns>
        public static string GetResourcePath(string category, string name)
        {
            return $"{category}/{name}";
        }

        /// <summary>
        /// 获取已加载的资源。<br/>
        /// Retrieves the loaded asset.
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。<br/>
        /// The type of the asset, such as GameObject, Texture, etc.
        /// </typeparam>
        /// <param name="path">
        /// 资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the resource within the Resources folder, excluding file extensions and using forward slashes: <c>path/to/resource</c>.
        /// </param>
        /// <returns>
        /// 返回已加载的资源，类型为指定的 <typeparamref name="T"/>，如果资源未加载，则返回 null。<br/>
        /// Returns the loaded asset of type <typeparamref name="T"/>. If the resource is not loaded, returns null.
        /// </returns>
        public static T GetLoadedAsset<T>(string path) where T : UnityEngine.Object
        {
            return loadedAssets.TryGetValue(path, out var asset) ? asset as T : null;
        }

        /// <summary>
        /// 实例化游戏对象。如果资源已加载，直接实例化；如果未加载，按需加载并实例化。<br/>
        /// Instantiate the GameObject. If the resource is already loaded, it will be instantiated directly; if not, it will be loaded and then instantiated.
        /// </summary>
        /// <param name="path">
        /// 资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the resource within the Resources folder, excluding file extensions and using forward slashes: <c>path/to/resource</c>.
        /// </param>
        /// <param name="load">
        /// 当资源未加载时，是否加载并实例化。如果为 true，将尝试加载并实例化该资源；如果为 false，则仅在资源已加载时实例化。<br/>
        /// Whether to load and instantiate the resource if it is not already loaded. If true, the resource will be loaded and instantiated; if false, it will only be instantiated if already loaded.
        /// </param>
        /// <returns>
        /// 返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 <c>load</c> 为 false 时资源未加载，则返回 null。<br/>
        /// Returns the instantiated GameObject. If the resource is not loaded and loading fails, or if <c>load</c> is false and the resource is not loaded, it returns null.
        /// </returns>
        public static GameObject Instantiate(string path, bool load = false)
        {
            GameObject prefab = GetLoadedAsset<GameObject>(path);
            if (prefab == null)
            {
                if (load)
                {
                    LoadResource<GameObject>(path);
                    prefab = GetLoadedAsset<GameObject>(path);
                    if (prefab == null) return null;
                }
                else
                {
                    return null;
                }
            }
            return UnityEngine.Object.Instantiate(prefab);
        }

        /// <summary>
        /// 异步实例化游戏对象。如果资源已加载，直接实例化；如果未加载，按需加载并实例化。<br/>
        /// Asynchronously instantiates the GameObject. If the resource is already loaded, it will be instantiated directly; if not, it will be loaded and then instantiated.
        /// </summary>
        /// <param name="path">
        /// 资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the resource within the Resources folder, excluding file extensions and using forward slashes: <c>path/to/resource</c>.
        /// </param>
        /// <param name="load">
        /// 当资源未加载时，是否加载并实例化。如果为 true，将尝试加载并实例化该资源；如果为 false，则仅在资源已加载时实例化。<br/>
        /// Whether to load and instantiate the resource if it is not already loaded. If true, the resource will be loaded and instantiated; if false, it will only be instantiated if already loaded.
        /// </param>
        /// <returns>
        /// 返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 <c>load</c> 为 false 时资源未加载，则返回 null。<br/>
        /// Returns the instantiated GameObject. If the resource is not loaded and loading fails, or if <c>load</c> is false and the resource is not loaded, it returns null.
        /// </returns>
        public static async Task<GameObject> InstantiateAsync(string path, bool load = false)
        {
            GameObject prefab = GetLoadedAsset<GameObject>(path);
            if (prefab == null)
            {
                if (load)
                {
                    await LoadResourceAsync<GameObject>(path);
                    prefab = GetLoadedAsset<GameObject>(path);
                    if (prefab == null) return null;
                }
                else
                {
                    return null;
                }
            }
            var request = UnityEngine.Object.InstantiateAsync(prefab);
            await WaitForRequest(request);
            
            return request.Result[0];
        }

        /// <summary>
        /// 加载指定路径的资源。<br/>
        /// Load the resource at the specified path.
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。<br/>
        /// The type of the resource, such as GameObject, Texture, etc.
        /// </typeparam>
        /// <param name="path">
        /// 资源路径，Resources 文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the resource within the Resources folder, excluding file extensions, and using forward slashes: <c>path/to/resource</c>.
        /// </param>
        /// <remarks>
        /// 此方法同步加载指定资源，若资源已加载则立即返回。适用于 <c>UnityEngine.Resources</c> 资源加载方式。<br/>
        /// This method synchronously loads the specified resource. If the resource is already loaded, it returns immediately. Suitable for <c>UnityEngine.Resources</c> resource loading.
        /// </remarks>
        public static void LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (loadedAssets.ContainsKey(path)) return;

            T res = Resources.Load<T>(path);

            if (res != null)
            {
                loadedAssets.Add(path, res);
            }
            else
            {
                Debug.LogError($"Failed to load asset at {path} (加载位于 {path} 的资产失败)");
            }
        }

        /// <summary>
        /// 异步加载指定路径的资源。<br/>
        /// Asynchronously loads the resource at the specified path.
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。<br/>
        /// The type of the resource, e.g., GameObject, Texture, etc.
        /// </typeparam>
        /// <param name="path">
        /// 资源路径，Resources 文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the resource within the Resources folder, excluding file extensions, and using forward slashes: <c>path/to/resource</c>.
        /// </param>
        /// <returns>
        /// 一个表示异步加载状态的任务。<br/>
        /// A Task representing the asynchronous loading operation.
        /// </returns>
        /// <remarks>
        /// 该方法将异步加载指定的资源，并在加载完成后可以通过任务的状态检查结果，如果是已加载的资源将会立即返回。适用于 <c>UnityEngine.Resources</c> 资源加载方式。<br/>
        /// This method asynchronously loads the resource and allows checking the result once the task completes. If the resource is already loaded, it returns immediately. Suitable for <c>UnityEngine.Resources</c> resource loading.
        /// </remarks>
        public static async Task LoadResourceAsync<T>(string path) where T : UnityEngine.Object
        {
            if (loadedAssets.ContainsKey(path)) return;

            ResourceRequest request = Resources.LoadAsync<T>(path);
            await WaitForRequest(request);
            
            T res = request.asset as T;

            if (res != null)
            {
                loadedAssets.Add(path, res);
            }
            else
            {
                Debug.LogError($"Failed to load asset at {path} (加载位于 {path} 的资产失败)");
            }
        
        }

        // 将ResourceRequest转换为Task
        private static Task WaitForRequest(ResourceRequest request)
        {
            var tcs = new TaskCompletionSource<bool>();
            request.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }

        // 将AsyncInstantiateOperation<GameObject>转换为Task
        private static Task WaitForRequest(AsyncInstantiateOperation<GameObject> request)
        {
            var tcs = new TaskCompletionSource<bool>();
            request.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }

        /// <summary>
        /// 释放指定已加载的资源 <br/>
        /// Releases the specified loaded resource
        /// </summary>
        /// <param name="path">
        /// 需要释放的已加载资源的路径，Resources 文件夹中的资源路径，不包含文件扩展名，使用正斜杠：<c>path/to/resource</c>。<br/>
        /// The path of the loaded resource within the Resources folder to be released, excluding file extensions, and using forward slashes: <c>path/to/resource</c>. 
        /// </param>
        public static void ReleaseResource(string path)
        {
            if (loadedAssets.TryGetValue(path, out var res))
            {
                if (res is not GameObject) 
                {
                    Resources.UnloadAsset((Object)res);
                }
                loadedAssets.Remove(path);
            }
            else
            {
                Debug.LogWarning($"Loaded resource not found for release at {path} (在 {path} 没有找到可供释放的已加载资源)");
            }
        }
    }
}
#endif