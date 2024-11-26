# AssetLoader

Inherits: [MonoBehaviour](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.html)

## Description

`AssetLoader` is a utility class designed to unify resource loading and management, supporting two resource loading methods on the Unity platform:

 - `Addressables`: The recommended resource loading method. Requires installing the `Addressables` package via Unity's Package Manager.

 - `UnityEngine.Resources`: The native Unity resource loading method, suitable for simple projects but requires resources to be stored in the `Assets/Resources` folder.

## Static Methods

| [GetResourcePath](#assetloadergetresourcepath) | Generates a complete resource path or address based on the loading method. |
|------------------------------------------------|---------------------------------------------------------------------------|
| [GetLoadedAsset](#assetloadergetloadedasset)   | Retrieves a loaded resource.                                              |
| [Instantiate](#assetloaderinstantiate)         | Loads and instantiates a game object as needed.                          |
| [InstantiateAsync](#assetloaderinstantiateasync)| Asynchronously loads and instantiates a game object as needed.           |
| [LoadResource](#assetloaderloadresource)       | Loads a resource at a specified address.                                  |
| [LoadResourceAsync](#assetloaderloadresourceasync)| Asynchronously loads a resource at a specified address.                 |
| [ReleaseResource](#assetloaderreleaseresource) | Releases a loaded resource.                                               |

---

# AssetLoader.GetResourcePath

`public static string GetResourcePath(string category, string name)`

## Parameters

| `category` | Resource category, e.g., "UI", "Backgrounds".        |
|------------|-----------------------------------------------------|
| `name`     | Resource name, without path prefix or suffix, e.g., "MainMenu". |

## Description

Returns the full resource path based on the loading method.

## Returns

- If using `UnityEngine.Resources`, returns a concatenated path, e.g., `UI/MainMenu`.
- If using `Addressables`, returns the resource address directly.

---

# AssetLoader.GetLoadedAsset

`public static T GetLoadedAsset<T>(string address) where T : UnityEngine.Object`  
`public static T GetLoadedAsset<T>(string path) where T : UnityEngine.Object`

## Parameters

| `T`        | Resource type, e.g., GameObject, Texture, etc.        |
|------------|-----------------------------------------------------|
| `address`  | (For `Addressables`) The resource address defined in Addressables. |
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |

## Description

Retrieves a loaded resource and specifies its type. Parameters vary depending on the loader used.

## Returns

Returns the loaded resource of type `T`. If the resource is not loaded, returns `null`.

---

# AssetLoader.Instantiate

`public static GameObject Instantiate(string address, bool load = false)`  
`public static GameObject Instantiate(string path, bool load = false)`

## Parameters

| `address`  | (For `Addressables`) The resource address defined in Addressables. |
|------------|-------------------------------------------------------------------|
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |
| `load`     | Whether to load and instantiate the resource if it is not already loaded. Default is `false`. |

## Description

Synchronously instantiates a game object in the scene. Determines whether to load and instantiate the resource if it is not already loaded.

## Returns

Returns the instantiated game object. If the resource is not loaded and loading fails, or if `load` is `false` and the resource is not loaded, returns `null`.

---

# AssetLoader.InstantiateAsync

`public static async Task<GameObject> InstantiateAsync(string address, bool load = false)`  
`public static async Task<GameObject> InstantiateAsync(string path, bool load = false)`

## Parameters

| `address`  | (For `Addressables`) The resource address defined in Addressables. |
|------------|-------------------------------------------------------------------|
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |
| `load`     | Whether to load and instantiate the resource if it is not already loaded. Default is `false`. |

## Description

Asynchronously instantiates a game object in the scene. Determines whether to load and instantiate the resource if it is not already loaded.

## Returns

Returns the instantiated game object. If the resource is not loaded and loading fails, or if `load` is `false` and the resource is not loaded, returns `null`.

---

# AssetLoader.LoadResource

`public static void LoadResource<T>(string address) where T : UnityEngine.Object`  
`public static void LoadResource<T>(string path) where T : UnityEngine.Object`

## Parameters

| `T`        | Resource type, e.g., GameObject, Texture, etc.       |
|------------|-----------------------------------------------------|
| `address`  | (For `Addressables`) The resource address defined in Addressables. |
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |

## Description

Synchronously loads a specified resource. If the resource is already loaded, it will be returned immediately.

---

# AssetLoader.LoadResourceAsync

`public static async Task LoadResourceAsync<T>(string address) where T : UnityEngine.Object`  
`public static async Task LoadResourceAsync<T>(string path) where T : UnityEngine.Object`

## Parameters

| `T`        | Resource type, e.g., GameObject, Texture, etc.       |
|------------|-----------------------------------------------------|
| `address`  | (For `Addressables`) The resource address defined in Addressables. |
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |

## Description

Asynchronously loads a specified resource. If the resource is already loaded, it will be returned immediately.

## Returns

A task representing the asynchronous load operation.

---

# AssetLoader.ReleaseResource

`public static void ReleaseResource(string address)`  
`public static void ReleaseResource(string path)`

## Parameters

| `address`  | (For `Addressables`) The resource address to release, as defined in Addressables. |
|------------|----------------------------------------------------------------------------------|
| `path`     | (For `UnityEngine.Resources`) The resource path within the Resources folder, excluding file extension, and using forward slashes: `path/to/resource`. |

## Description

Releases a loaded resource from memory.

For `UnityEngine.Resources`, this method cannot release game objects. To release game objects, use `UnityEngine.Destroy` or switch to `Addressables`.

