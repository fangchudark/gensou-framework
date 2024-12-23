# SaveManager

## 説明

`SaveManager` はゲームのセーブデータを管理するためのクラスで、データの保存、読み込み、削除、追加、変更、検索、ディレクトリ作成などの機能を提供します。

デフォルトではバイナリ形式でデータを保存しますが、`Json` 形式で保存したい場合は、[フレームワークツール](../GensouLib.Unity.Tools/PackageInstaller.md)または Unity Package Manager を使用して `Json.NET` パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効にしてください。

バイナリ形式でデータを保存する場合、保存するデータは `[Serializable]` 属性が付与されたクラスに格納する必要があります。

`Json` 形式でデータを保存する場合、Unity オブジェクトのシリアライズはサポートしていませんので、保存する Unity オブジェクトの状態（名前や位置など）を取り出して辞書に保存してください。

## 静的プロパティ

| [DataToSave](#savemanagerdatatosave) | 保存するデータ。 |
|:---|:---|
|[LoadedDataBinary](#savemanagerloadeddatabinary)|バイナリファイルから読み込まれたデータ。|
|[LoadedDataJson](#savemanagerloadeddatajson)|`Json`ファイルから読み込まれたデータ。|
| [SavePath](#savemangersavepath) | セーブデータのディレクトリ。 |

## 静的メソッド

| [CreateDirectory](#savemanagercreatedirectory) | ディレクトリを作成し、セーブディレクトリを変更する。 |
|:---|:---|
| [DeleteSaveDirectory](#savemanagerdeletesavedirectory) | セーブディレクトリを削除する。 |
| [SaveExists](#savemanagersaveexists) | 指定したファイル名のセーブデータが存在するか確認する。 |
| [DeleteSaveFile](#savemanagerdeletesavefile) | 指定したセーブファイルを削除する。 |
| [DeleteAllSaveFiles](#savemanagerdeleteallsavefiles) | 指定した拡張子のセーブファイルをすべて削除する。 |
| [GetSaveFileCount](#savemanagergetsavefilecount) | 指定した拡張子のセーブファイル数を取得する。 |
| [SaveAsBinary](#savemanagersaveasbinary) | バイナリファイルとしてデータを保存する。 |
| [LoadFromBinary](#savemanagerloadfrombinary) | バイナリファイルからデータを読み込む。 |
| [AddDataToBinary](#savemanageradddatatobinary) | バイナリファイルにデータを追加する。 |
| [GetDataFromBinary](#savemanagergetdatafrombinary) | バイナリファイルからデータを取得する。 |
| [DeleteDataFromBinary](#savemanagerdeletedatafrombinary) | バイナリファイルからデータを削除する。 |
| [SaveAsJson](#savemanagersaveasjson) | Jsonファイルとしてデータを保存する。 |
| [LoadFromJson](#savemanagerloadfromjson) | Jsonファイルからデータを読み込む。 |
| [AddDataToJson](#savemanageradddatatojson) | Jsonファイルにデータを追加する。 |
| [GetDataFromJson](#savemanagergetdatafromjson) | Jsonファイルからデータを取得する。 |
| [DeleteDataFromJson](#savemanagerdeletedatafromjson) | Jsonファイルからデータを削除する。 |

---

# SaveManager.DataToSave

`public static Dictionary<string, object> DataToSave`

## 説明

保存するデータ。キーはデータを検索するための識別子または `Json` ファイルに保存するためのキー名、値は対応するデータです。

データを保存する前に、この辞書に保存するデータを格納してください。

---

# SaveManager.LoadedDataBinary

`public static Dictionary<string, object> LoadedDataBinary`

## 説明

読み取り専用属性、バイナリ ファイルからロードされたデータ。

キーはデータの検索に使用される識別子で、値は対応するデータです。

---

# SaveManager.LoadedDataJson

`public static Dictionary<string, object> LoadedDataJson`

## 説明

読み取り専用プロパティ。データは `Json` ファイルからロードされます。

キーは `Json` ファイルに保存されたキー名で、値は対応するデータです。

キーに対応する値が複数のプロパティを含むクラスまたはディクショナリである場合、それを `JObject` 型に変換し、`JObject.Properties` メソッドを使用してそのプロパティにアクセスします。

```csharp
JObject jObject = (JObject)SaveManager.LoadedDataJson["key"];
foreach (var property in jObject.Properties())
{
    // do something with property
    Debug.Log(property.Name + " : " + property.Value);
}
```

キーに対応する値が配列またはリストの場合は、それを `JArray` 型に変換し、`JArray.Children` メソッドを使用してその要素にアクセスします。

```csharp
JArray jArray = (JArray)SaveManager.LoadedDataJson["key"];
foreach (var item in jArray.Children())
{
    // do something with item
    Debug.Log(item.ToString());
}
```

---

# SaveManager.SavePath

`public static string SavePath`

## 説明

セーブデータの保存先ディレクトリ。

デフォルトは [`Application.persistentDataPath`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Application-persistentDataPath.html) ですが、他のパスに変更することもできます。

---

# SaveManager.CreateDirectory

`public static void CreateDirectory(string directory, bool createAtLocalLow = false)`

## パラメーター

| `directory` | 作成するディレクトリのパス。 |
|:---|:---|
| `createAtLocalLow` | `C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成するかどうか。デフォルトは `false`。 |

## 説明

ディレクトリを作成し、現在のセーブディレクトリを変更します。

`createAtLocalLow` が `true` の場合、`C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成します。この場合、`directory` パラメーターは相対パスで指定する必要があります。

---

# SaveManager.DeleteSaveDirectory

`public static void DeleteSaveDirectory()`

## 説明

セーブディレクトリとその中のすべてのファイルを削除します。

---

# SaveManager.SaveExists

`public static bool SaveExists(string fileName)`

## パラメーター

| `fileName` | チェックするセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したファイル名のセーブデータが存在するか確認します。

## 戻り値

セーブディレクトリに指定したセーブデータが存在すれば `true` を、存在しなければ `false` を返します。

---

# SaveManager.DeleteSaveFile

`public static void DeleteSaveFile(string fileName)`

## パラメーター

| `fileName` | 削除するセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したセーブファイルを削除します。

---

# SaveManager.DeleteAllSaveFiles

`public static void DeleteAllSaveFiles(string extension = ".sav")`

## パラメーター

| `extension` | 削除するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

指定した拡張子のセーブファイルをセーブディレクトリ内からすべて削除します。

---

# SaveManager.GetSaveFilesCount

`public static int GetSaveFilesCount(string extension = ".sav")`

## パラメーター

| `extension` | 数を取得するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

セーブディレクトリ内で指定した拡張子を持つセーブファイルの数を取得します。

## 戻り値

指定した拡張子を持つセーブファイルがあればその数を、なければ `0` を返します。

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(Dictionary<string, object> dataDictionary = null, string fileName = "SaveData.sav")`

## パラメーター

| `dataDictionary` | 保存するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 保存するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |

## 説明

データを指定されたファイル名のバイナリファイルとして保存します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが保存されます。

---

# SaveManager.LoadFromBinary

`public static void LoadFromBinary(string fileName = "SaveData.sav")`

## パラメーター

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

指定したバイナリファイルからデータを読み込みます。

読み込んだデータは [`LoadedDataBinary`](#savemanagerloadeddatabinary) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedDataBinary`](#savemanagerloadeddatabinary) プロパティは空の辞書になります。

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(string fileName, string key, object newData)`

## パラメーター

|`fileName`|データが追加されるバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。 |
|`newData`|追加されるデータ。 |

## 説明

指定されたファイル名のバイナリ ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.GetDataFromBinary

`public static T GetDataFromBinary<T>(string fileName, string key)`

## パラメーター

|`T`|取得するデータのタイプ。 |
|:---|:---|
|`fileName`|データを取得するバイナリ ファイルの名前 (拡張子を含む)。 |
|`key`|データの検索に使用される識別子。 |

## 説明

指定されたファイル名のバイナリ ファイルからデータを取得します。

## 戻り値

ファイルが存在し、指定されたキーが含まれている場合は、対応するデータが返されます。それ以外の場合は、`default` 値が返されます。

---

# SaveManager.DeleteDataFromBinary

`public static void DeleteDataFromBinary(string fileName, string key)`

## パラメーター

|`fileName`|データを削除するバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。 |

## 説明

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。

---

# SaveManager.SaveAsJson

`public static void SaveAsJson(Dictionary<string, object> dataDictionary = null, string fileName = "SaveData.sav")`

## パラメーター

| `dataDictionary` | 保存するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 保存するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。**

データを指定されたファイル名の `Json` ファイルとして保存します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが保存されます。

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## パラメーター

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。**

指定した `Json` ファイルからデータを読み込みます。

読み込んだデータは [`LoadedDataJson`](#savemanagerloadeddatajson) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedDataJson`](#savemanagerloadeddatajson) プロパティは空の辞書になります。

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, object newData)`

## パラメーター

|`fileName`|データが追加される `Json` ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|`Json` ファイルに保存されたキーの名前。 |
|`newData`|追加されるデータ。 |

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。**

指定したファイル名で `Json` ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<T>(string fileName, string key)`

## パラメーター

|`T`|取得するデータのタイプ。 |
|:---|:---|
|`fileName`|データを取得する `Json` ファイルの名前 (拡張子を含む)。 |
|`key`|`Json` ファイルに保存されたキーの名前。 |

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。**

指定した `Json` ファイルからデータを取得します。

このメソッドは、文字列、数値 (`long`、`double`)、ブール値など、Json がシリアル化できる基本的な型のデータのみを取得できます。

**C# コレクション タイプ、Unity オブジェクト、またはカスタム クラスを直接取得することはできません。**

取得が必要な場合は、[`LoadedDataJson`](#savemanagerloadeddatajson)プロパティにアクセスして取得してください。

`JSON` ファイルからデータを取得する場合、`Json.NET` の対応する型マッピング ルールは次のとおりです。

- C# コレクション:

 - 辞書: `JObject`

 - 配列: `JArray`

 - リスト: `JArray`

- カスタム クラス: `JObject`

- 基本的なデータ型:

 - 整数型: `long`

 - 浮動小数点型: `double`

 - 文字列: `string`

 - ブール値: `bool`

## 戻り値

ファイルが存在し、指定されたキーが含まれている場合は、対応するデータが返されます。それ以外の場合は、`default`値が返されます。

---

# SaveManager.DeleteDataFromJson

`public static void DeleteDataFromJson(string fileName, string key)`

## パラメーター

| `fileName` | 削除するファイル名（拡張子込み）。|
|:---|:---|
|`key`|`Json`ファイルに保存するキー名。|

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。**

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。
