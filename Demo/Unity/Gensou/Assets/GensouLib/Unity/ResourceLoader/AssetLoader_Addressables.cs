#if ENABLE_ADDRESSABLES
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GensouLib.Unity.ResourceLoader
{
    /// <summary>
    /// 资源加载器
    /// </summary>
    public class AssetLoader : MonoBehaviour
    {
        // 储存加载过的资源
        private static readonly Dictionary<string, object> loadedAssets = new();


        /// <summary>
        /// 获取已加载的资源。
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。
        /// </typeparam>
        /// <param name="address">
        /// 资源地址，Addressables 中定义的地址。
        /// </param>
        /// <returns>
        /// 返回已加载的资源，类型为指定的 <typeparamref name="T"/>，如果资源未加载，则返回 null。
        /// </returns>
        public static T GetLoadedAsset<T>(string address) where T : UnityEngine.Object
        {
            return loadedAssets.TryGetValue(address, out var asset) ? asset as T : null;
        }

        /// <summary>
        /// 实例化游戏对象。如果资源已加载，直接实例化；如果未加载，按需加载并实例化。
        /// </summary>
        /// <param name="address">
        /// 资源地址，Addressables 中定义的地址。
        /// </param>
        /// <param name="load">
        /// 当资源未加载时，是否加载并实例化。如果为 true，将尝试加载并实例化该资源；如果为 false，则仅在资源已加载时实例化。
        /// </param>
        /// <returns>
        /// 返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 <c>load</c> 为 false 时资源未加载，则返回 null。
        /// </returns>
        public static GameObject Instantiate(string address, bool load = false)
        {
            GameObject prefab = GetLoadedAsset<GameObject>(address);
            if (prefab == null)
            {
                if (load)
                {
                    var handle = Addressables.InstantiateAsync(address).WaitForCompletion();
                    if (handle != null) loadedAssets.Add(address, handle);
                    else Debug.LogError($"Failed to load addressable asset: {address} (加载Addressable资产失败：{address})");
                    return handle;
                }
                else
                {
                    return null;
                }
            }
            return UnityEngine.Object.Instantiate(prefab);
        }

        /// <summary>
        /// 异步实例化游戏对象。如果资源已加载，直接实例化；如果未加载，按需加载并实例化。
        /// </summary>
        /// <param name="address">
        /// 资源地址，Addressables 中定义的地址。
        /// </param>
        /// <param name="load">
        /// 当资源未加载时，是否加载并实例化。如果为 true，将尝试加载并实例化该资源；如果为 false，则仅在资源已加载时实例化。
        /// </param>
        /// <returns>
        /// 返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 <c>load</c> 为 false 时资源未加载，则返回 null。
        /// </returns>
        public static async Task<GameObject> InstantiateAsync(string address, bool load = false)
        {
            GameObject prefab = GetLoadedAsset<GameObject>(address);
            if (prefab == null)
            {
                if (load)
                {
                    var handle = Addressables.InstantiateAsync(address);
                    await handle.Task;

                    if (handle.Status == AsyncOperationStatus.Succeeded) 
                    {
                        loadedAssets.Add(address, handle.Result);
                        return handle.Result;
                    }
                    else 
                    {
                        Debug.LogError($"Failed to load addressable asset: {address} (加载Addressable资产失败：{address})");
                        return null;
                    }
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

        // 将AsyncInstantiateOperation<GameObject>转换为Task
        private static Task WaitForRequest(AsyncInstantiateOperation<GameObject> request)
        {
            var tcs = new TaskCompletionSource<bool>();
            request.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }

        /// <summary>
        /// 加载指定地址的资源。
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。
        /// </typeparam>
        /// <param name="address">
        /// 资源地址，Addressables 中定义的地址。
        /// </param>
        /// <remarks>
        /// 此方法同步加载指定资源，若资源已加载则立即返回。适用于 <c>Addressables</c> 资源加载方式。
        /// </remarks>
        public static void LoadResource<T>(string address) where T : UnityEngine.Object
        {
            if (loadedAssets.ContainsKey(address)) return;

            var handle = Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
                
            if (handle != null)
            {
                loadedAssets.Add(address, handle);
            }
            else
            {
                Debug.LogError($"Failed to load addressable asset: {address} (加载Addressable资产失败：{address})");
            }
        }

        /// <summary>
        /// 异步加载指定地址的资源。
        /// </summary>
        /// <typeparam name="T">
        /// 资源类型，例如 GameObject、Texture 等。
        /// </typeparam>
        /// <param name="address">
        /// 资源地址，Addressables 中定义的地址。
        /// </param>
        /// <returns>
        /// 一个表示异步加载状态的任务。
        /// </returns>
        /// <remarks>
        /// 该方法将异步加载指定的资源，并在加载完成后可以通过任务的状态检查结果，如果是已加载的资源将会立即返回。适用于 <c>Addressables</c> 资源加载方式。
        /// </remarks>
        public static async Task LoadResourceAsync<T>(string address) where T : UnityEngine.Object
        {
            if (loadedAssets.ContainsKey(address)) return;
         
            var res = Addressables.LoadAssetAsync<T>(address);
            await res.Task;
                
            if (res.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAssets.Add(address, res.Result);
            }
            else
            {
                Debug.LogError($"Failed to load addressable asset: {address} (加载Addressable资产失败：{address})");
            }
            
        }

        /// <summary>
        /// 释放指定已加载的资源
        /// </summary>
        /// <param name="address">
        /// 需要释放的已加载资源的地址
        /// </param>
        public static void ReleaseResource(string address)
        {
            if (loadedAssets.TryGetValue(address, out var asset))
            {
                Addressables.Release(asset);
                loadedAssets.Remove(address);
            }
            else
            {
                Debug.LogWarning($"Resource not found for release: {address} (未找到可供释放的资源：{address})");
            }
        }
    }
}
#endif
