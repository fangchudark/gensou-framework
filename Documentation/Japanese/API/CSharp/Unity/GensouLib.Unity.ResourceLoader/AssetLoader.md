# AssetLoader

継承: [MonoBehaviour](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.html)

## 説明

`AssetLoader` は、リソースのロードと管理を統一するためのユーティリティクラスで、Unity プラットフォームで次の 2 種類のリソースロード方式をサポートしています:

- `Addressables`：推奨されるリソースロード方式。Unity のパッケージマネージャーから `Addressables` パッケージをインストールする必要があります。

- `UnityEngine.Resources`：Unity に標準搭載されたリソースロード方式。簡単なプロジェクトに適していますが、リソースを `Assets/Resources` フォルダーに配置する必要があります。

## 静的メソッド

| [GetResourcePath](#assetloadergetresourcepath) | 読み込み方法に基づいて完全なリソースパスまたはアドレスを生成します。 |
|-----------------------------------------------|---------------------------------------------------------------|
| [GetLoadedAsset](#assetloadergetloadedasset)   | 読み込まれたリソースを取得します。                             |
| [Instantiate](#assetloaderinstantiate)         | 必要に応じてリソースを読み込み、ゲームオブジェクトをインスタンス化します。 |
| [InstantiateAsync](#assetloaderinstantiateasync)| 非同期でリソースを読み込み、ゲームオブジェクトをインスタンス化します。 |
| [LoadResource](#assetloaderloadresource)       | 指定されたアドレスのリソースを読み込みます。                    |
| [LoadResourceAsync](#assetloaderloadresourceasync)| 指定されたアドレスのリソースを非同期で読み込みます。         |
| [ReleaseResource](#assetloaderreleaseresource) | 指定された読み込まれたリソースを解放します。                    |

---

# AssetLoader.GetResourcePath

`public static string GetResourcePath(string category, string name)`

## パラメーター

| `category` | リソースのカテゴリ、例: "UI"、"Backgrounds"。          |
|------------|--------------------------------------------------------|
| `name`     | リソース名（パスのプレフィックスやサフィックスなし）、例: "MainMenu"。 |

## 説明

異なるリソース読み込み方法に基づいて、完全なリソースパスを返します。

## 戻り値

- `UnityEngine.Resources` を使用する場合、結合されたパス（例: `UI/MainMenu`）を返します。
- `Addressables` を使用する場合、リソースのアドレスをそのまま返します。

---

# AssetLoader.GetLoadedAsset

`public static T GetLoadedAsset<T>(string address) where T : UnityEngine.Object`  
`public static T GetLoadedAsset<T>(string path) where T : UnityEngine.Object`

## パラメーター

| `T`        | リソースの型（例: GameObject、Texture など）。         |
|------------|--------------------------------------------------------|
| `address`  | （`Addressables` 用）Addressables に定義されたリソースアドレス。|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。 |

## 説明

読み込まれたリソースを取得し、その型を指定します。使用するローダーによって異なるパラメーターを渡します。

## 戻り値

指定された型 `T` の読み込まれたリソースを返します。リソースが読み込まれていない場合は `null` を返します。

---

# AssetLoader.Instantiate

`public static GameObject Instantiate(string address, bool load = false)`  
`public static GameObject Instantiate(string path, bool load = false)`

## パラメーター

| `address`  | （`Addressables` 用）Addressables に定義されたリソースアドレス。|
|------------|------------------------------------------------------------------|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。|
| `load`     | リソースが読み込まれていない場合に、読み込んでインスタンス化するかどうか。デフォルトは `false`。 |

## 説明

同期的にゲームオブジェクトをシーンにインスタンス化します。必要に応じて、リソースを読み込みインスタンス化します。

## 戻り値

インスタンス化されたゲームオブジェクトを返します。リソースが読み込まれておらず、読み込みに失敗した場合、または `load` が `false` の場合は `null` を返します。

---

# AssetLoader.InstantiateAsync

`public static async Task<GameObject> InstantiateAsync(string address, bool load = false)`  
`public static async Task<GameObject> InstantiateAsync(string path, bool load = false)`

## パラメーター

| `address`  | （`Addressables` 用）Addressables に定義されたリソースアドレス。|
|------------|------------------------------------------------------------------|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。|
| `load`     | リソースが読み込まれていない場合に、読み込んでインスタンス化するかどうか。デフォルトは `false`。 |

## 説明

非同期的にゲームオブジェクトをシーンにインスタンス化します。必要に応じて、リソースを読み込みインスタンス化します。

## 戻り値

インスタンス化されたゲームオブジェクトを返します。リソースが読み込まれておらず、読み込みに失敗した場合、または `load` が `false` の場合は `null` を返します。

---

# AssetLoader.LoadResource

`public static void LoadResource<T>(string address) where T : UnityEngine.Object`  
`public static void LoadResource<T>(string path) where T : UnityEngine.Object`

## パラメーター

| `T`        | リソースの型（例: GameObject、Texture など）。         |
|------------|--------------------------------------------------------|
| `address`  | （`Addressables` 用）Addressables に定義されたリソースアドレス。|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。 |

## 説明

指定されたリソースを同期的に読み込みます。リソースがすでに読み込まれている場合は、即座に返されます。

---

# AssetLoader.LoadResourceAsync

`public static async Task LoadResourceAsync<T>(string address) where T : UnityEngine.Object`  
`public static async Task LoadResourceAsync<T>(string path) where T : UnityEngine.Object`

## パラメーター

| `T`        | リソースの型（例: GameObject、Texture など）。         |
|------------|--------------------------------------------------------|
| `address`  | （`Addressables` 用）Addressables に定義されたリソースアドレス。|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。 |

## 説明

指定されたリソースを非同期で読み込みます。リソースがすでに読み込まれている場合は、即座に返されます。

## 戻り値

非同期読み込み操作を表すタスク。

---

# AssetLoader.ReleaseResource

`public static void ReleaseResource(string address)`  
`public static void ReleaseResource(string path)`

## パラメーター

| `address`  | （`Addressables` 用）解放する読み込まれたリソースのアドレス。|
|------------|------------------------------------------------------------|
| `path`     | （`UnityEngine.Resources` 用）Resources フォルダ内のリソースパス（ファイル拡張子なし）、例: `path/to/resource`。|

## 説明

読み込まれたリソースをメモリから解放します。

- `UnityEngine.Resources` を使用する場合、このメソッドではゲームオブジェクトを解放できません。ゲームオブジェクトを解放するには、`UnityEngine.Destroy` を直接使用するか、`Addressables` を使用してください。
