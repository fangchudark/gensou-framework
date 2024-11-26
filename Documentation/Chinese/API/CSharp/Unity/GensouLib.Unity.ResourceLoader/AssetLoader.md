# AssetLoader

继承：[MonoBehaviour](https://docs.unity.cn/cn/2022.3/ScriptReference/MonoBehaviour.html)

## 描述

`AssetLoader` 是一个用于统一资源加载和管理的工具类，提供了对 Unity 平台上两种资源加载方式的支持：

 - `Addressables`：推荐使用的资源加载方式。需要在 Unity 包管理器中额外安装 `Addressables` 包。

 - `UnityEngine.Resources`：Unity 原生支持的资源加载方式，适合简单项目，但要求资源存放在 `Assets/Resources` 文件夹下。

## 静态方法

|[GetResourcePath](#assetloadergetresourcepath)|根据加载方式生成完整的资源路径或地址。|
|:---|:---|
|[GetLoadedAsset](#assetloadergetloadedasset)|获取已加载的资源。|
|[Instantiate](#assetloaderinstantiate)|按需加载并实例化游戏对象。|
|[InstantiateAsync](#assetloaderinstantiateasync)|异步按需加载并实例化游戏对象。|
|[LoadResource](#assetloaderloadresource)|加载指定地址的资源。|
|[LoadResourceAsync](#assetloaderloadresourceasync)|异步加载指定地址的资源。|
|[ReleaseResource](#assetloaderreleaseresource)|释放指定已加载的资源。|

---

# AssetLoader.GetResourcePath

`public static string GetResourcePath(string category, string name)`

## 参数

|`category`|资源类别，例如 "UI"、"Backgrounds"。|
|:---|:---|
|`name`|资源名称，不包含路径前缀或后缀，例如 "MainMenu"。|

## 描述

根据不同的资源加载方式，返回不同的资源路径

## 返回

- 如果使用 `UnityEngine.Resources` 加载，返回拼接后的完整路径，例如 `UI/MainMenu`；

- 如果使用 `Addressables` 加载，直接返回资源地址。

---

# AssetLoader.GetLoadedAsset

`public static T GetLoadedAsset<T>(string address) where T : UnityEngine.Object`  
`public static T GetLoadedAsset<T>(string path) where T : UnityEngine.Object`

## 参数

|`T`|资源类型，例如 GameObject、Texture 等。|
|:---|:---|
|`address`|(`Addressables`加载器)资源地址，Addressables 中定义的地址。|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|

## 描述

获取已加载资源并指定其类型，不同的加载器，传入的参数不同。

## 返回

返回已加载的资源，类型为指定的`T` ，如果资源未加载，则返回 `null`。

---

# AssetLoader.Instantiate

`public static GameObject Instantiate(string address, bool load = false)`  
`public static GameObject Instantiate(string path, bool load = false)`

## 参数

|`address`|(`Addressables`加载器)资源地址，Addressables 中定义的地址。|
|:---|:---|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|
|`load`|当资源未加载时，是否加载并实例化。如果为 `true`，将尝试加载并实例化该资源；如果为 `false`，则仅在资源已加载时实例化。默认值`false`。|

## 描述

同步实例化游戏对象到场景中，根据需求来决定是否在资源未加载时，加载资源并实例化

## 返回

返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 `load` 为 `false` 时资源未加载，则返回 `null`。

# AssetLoader.InstantiateAsync

`public static async Task<GameObject> InstantiateAsync(string address, bool load = false)`  
`public static async Task<GameObject> InstantiateAsync(string path, bool load = false)`

## 参数

|`address`|(`Addressables`加载器)资源地址，Addressables 中定义的地址。|
|:---|:---|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|
|`load`|当资源未加载时，是否加载并实例化。如果为 `true`，将尝试加载并实例化该资源；如果为 `false`，则仅在资源已加载时实例化。默认值`false`。|

## 描述

异步实例化游戏对象到场景中，根据需求来决定是否在资源未加载时，加载资源并实例化

## 返回

返回实例化的游戏对象，如果指定资源未加载且加载失败，或当 `load` 为 `false` 时资源未加载，则返回 `null`。

# AssetLoader.LoadResource

`public static void LoadResource<T>(string address) where T : UnityEngine.Object`  
`public static void LoadResource<T>(string path) where T : UnityEngine.Object`

## 参数

|`T`|资源类型，例如 GameObject、Texture 等。|
|:---|:---|
|`address`|(`Addressables`加载器)资源地址，Addressables 中定义的地址。|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|

## 描述

同步加载指定资源，若资源已加载则立即返回

# AssetLoader.LoadResourceAsync

`public static async Task LoadResourceAsync<T>(string address) where T : UnityEngine.Object`  
`public static async Task LoadResourceAsync<T>(string path) where T : UnityEngine.Object`

## 参数

|`T`|资源类型，例如 GameObject、Texture 等。|
|:---|:---|
|`address`|(`Addressables`加载器)资源地址，Addressables 中定义的地址。|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|

## 描述

异步加载指定的资源，并在加载完成后可以通过任务的状态检查结果，如果是已加载的资源将会立即返回。

## 返回

一个表示异步加载状态的任务

# AssetLoader.ReleaseResource

`public static void ReleaseResource(string address)`  
`public static void ReleaseResource(string path)`

## 参数

|`address`|(`Addressables`加载器)需要释放的已加载资源的地址，Addressables 中定义的地址。|
|:---|:---|
|`path`|(`UnityEngine.Resources`加载器)资源路径，Resources文件夹中的资源路径，不包含文件扩展名，使用正斜杠：`path/to/resource`。|

## 描述

从内存释放一个已加载的资源。

在使用`UnityEngine.Resources`加载器的情况下，该方法不能用以释放游戏对象，如果想要释放游戏对象请直接使用`UnityEngine.Destroy`方法或改用`Addressables`加载器