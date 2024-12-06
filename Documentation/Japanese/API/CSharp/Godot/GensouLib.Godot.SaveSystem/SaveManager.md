# SaveManager

## 説明

`SaveManager` はゲームのセーブデータを管理するためのクラスで、データの保存、読み込み、削除、追加、変更、検索、ディレクトリ作成などの機能を提供します。

このクラスのすべての I/O 操作は Godot の API に基づいて実装されています。ファイル パスのパス区切り文字としてスラッシュ `/` を使用してください。

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

`public static Dictionary<string, Variant> DataToSave`

## 説明

保存するデータ。キーはデータを検索するための識別子または `Json` ファイルに保存するためのキー名、値は対応するデータです。

データを保存する前に、この辞書に保存するデータを格納してください。

---

# SaveManager.LoadedDataBinary

`public static Dictionary<string, Variant> LoadedDataBinary`

## 説明

読み取り専用属性、バイナリ ファイルからロードされたデータ。

キーはデータの検索に使用される識別子で、値は対応するデータです。

キーに対応する値が `Godot` オブジェクトの場合、最初に `EncodedObjectAsId` 型に変換する必要があります。

次に、`EncodedObjectAsId.ObjectId` プロパティにアクセスして、オブジェクトの参照 ID を取得します。

最後に、`GodotObject.InstanceFromId(ulong)` メソッドを使用して、参照 ID に基づいてオブジェクト インスタンスを取得します。

```csharp
EncodedObjectAsId encodedObject = (EncodedObjectAsId)SaveManager.LoadedDataBinary["key"];
Node node = (Node)GodotObject.InstanceFromId(encodedObject.ObjectId);
```

---

# SaveManager.LoadedDataJson

`public static Dictionary<string, Variant> LoadedDataJson`

## 説明

読み取り専用プロパティ。データは `Json` ファイルからロードされます。

キーは `Json` ファイルに保存されたキー名で、値は対応するデータです。

キーに対応する値が `Godot` オブジェクトの場合、まず `Variant.AsUInt64()` を使用してその参照 ID を取得します。

次に、`GodotObject.InstanceFromId(ulong)` メソッドを使用して、参照 ID に基づいてオブジェクト インスタンスを取得します。

```csharp
ulong objectId = SaveManager.LoadedDataJson["key"].AsUInt64();
Node node = (Node)GodotObject.InstanceFromId(objectId);
```

---

# SaveManager.SavePath

`public static string SavePath`

## 説明

セーブデータの保存先ディレクトリ。

デフォルト値は[`user://`の絶対パス](https://docs.godotengine.org/ja/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user)であり、他のパスに変更できます。

---

# SaveManager.CreateDirectory

`public static void CreateDirectory(string directory, bool createAtLocalLow = false)`

## パラメータ

| `directory` | 作成するディレクトリのパス。 |
|:---|:---|
| `createAtLocalLow` | `C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成するかどうか。デフォルトは `false`。 |

## 説明

ディレクトリを作成し、現在のセーブディレクトリを変更します。

`createAtLocalLow` が `true` の場合、`C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成します。この場合、`directory` パラメータは相対パスで指定する必要があります。

---

# SaveManager.DeleteSaveDirectory

`public static void DeleteSaveDirectory()`

## 説明

セーブディレクトリとその中のすべてのファイルを削除します。

---

# SaveManager.SaveExists

`public static bool SaveExists(string fileName)`

## パラメータ

| `fileName` | チェックするセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したファイル名のセーブデータが存在するか確認します。

## 戻り値

セーブディレクトリに指定したセーブデータが存在すれば `true` を、存在しなければ `false` を返します。

---

# SaveManager.DeleteSaveFile

`public static void DeleteSaveFile(string fileName)`

## パラメータ

| `fileName` | 削除するセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したセーブファイルを削除します。

---

# SaveManager.DeleteAllSaveFiles

`public static void DeleteAllSaveFiles(string extension = ".sav")`

## パラメータ

| `extension` | 削除するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

指定した拡張子のセーブファイルをセーブディレクトリ内からすべて削除します。

---

# SaveManager.GetSaveFilesCount

`public static int GetSaveFilesCount(string extension = ".sav")`

## パラメータ

| `extension` | 数を取得するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

セーブディレクトリ内で指定した拡張子を持つセーブファイルの数を取得します。

## 戻り値

指定した拡張子を持つセーブファイルがあればその数を、なければ `0` を返します。

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## パラメータ

| `dataDictionary` | 保存するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 保存するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |

## 説明

データを指定されたファイル名のバイナリファイルとして保存します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが保存されます。

---

# SaveManager.LoadFromBinary

`public static void LoadFromBinary(string fileName = "SaveData.sav")`

## パラメータ

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

指定したバイナリファイルからデータを読み込みます。

読み込んだデータは [`LoadedDataBinary`](#savemanagerloadeddatabinary) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedDataBinary`](#savemanagerloadeddatabinary) プロパティは空の辞書になります。

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(string fileName, string key, Variant newData)`

## パラメータ

|`fileName`|データが追加されるバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。 |
|`newData`|追加されるデータ。 |

## 説明

指定されたファイル名のバイナリ ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.GetDataFromBinary

`public static T GetDataFromBinary<[MustBeVariant] T>(string fileName, string key)`

## パラメータ

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

## パラメータ

|`fileName`|データを削除するバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。 |

## 説明

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。

---

# SaveManager.SaveAsJson

`public static void SaveAsJson(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## パラメータ

| `dataDictionary` | 保存するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 保存するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |

## 説明

データを指定されたファイル名の `Json` ファイルとして保存します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが保存されます。

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## パラメータ

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

指定した `Json` ファイルからデータを読み込みます。

読み込んだデータは [`LoadedDataJson`](#savemanagerloadeddatajson) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedDataJson`](#savemanagerloadeddatajson) プロパティは空の辞書になります。

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, Variant newData)`

## パラメータ

|`fileName`|データが追加される `Json` ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|`Json` ファイルに保存されたキーの名前。 |
|`newData`|追加されるデータ。 |

## 説明

指定したファイル名で `Json` ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<[MustBeVariant] T>(string fileName, string key)`

## パラメータ

|`T`|取得するデータのタイプ。 |
|:---|:---|
|`fileName`|データを取得する `Json` ファイルの名前 (拡張子を含む)。 |
|`key`|`Json` ファイルに保存されたキーの名前。 |

## 説明

指定した `Json` ファイルからデータを取得します。

## 戻り値

ファイルが存在し、指定されたキーが含まれている場合は、対応するデータが返されます。それ以外の場合は、`default`値が返されます。

---

# SaveManager.DeleteDataFromJson

`public static void DeleteDataFromJson(string fileName, string key)`

## パラメータ

| `fileName` | 削除するファイル名（拡張子込み）。|
|:---|:---|
|`key`|`Json`ファイルに保存するキー名。|

## 説明

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。





