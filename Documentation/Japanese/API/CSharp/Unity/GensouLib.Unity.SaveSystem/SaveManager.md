# SaveManager

## 説明

`SaveManager` はゲームのセーブデータを管理するためのクラスで、データの保存、読み込み、削除、追加、変更、検索、ディレクトリ作成などの機能を提供します。

デフォルトではバイナリ形式でデータを保存しますが、`Json` 形式で保存したい場合は、[フレームワークツール](../GensouLib.Unity.Tools/PackageInstaller.md)または Unity Package Manager を使用して `Json.NET` パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効にしてください。

バイナリ形式でデータを保存する場合、保存するデータは `[Serializable]` 属性が付与されたクラスに格納する必要があります。

`Json` 形式でデータを保存する場合、Unity オブジェクトのシリアライズはサポートしていませんので、保存する Unity オブジェクトの状態（名前や位置など）を取り出して辞書に保存してください。

## 静的プロパティ

| [DataToSave](#savemanagerdatatosave) | 保存するデータ。 |
|:---|:---|
| [LoadedData](#savemanagerloadeddata) | 読み込まれたセーブデータ。 |
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

# SaveManager.LoadedData

`public static Dictionary<string, object> LoadedData`

## 説明

読み込まれたセーブデータを格納するプロパティです。キーはデータを検索するための識別子または `Json` ファイルに保存するためのキー名、値は対応するデータです。

---

# SaveManager.SavePath

`public static string SavePath`

## 説明

セーブデータの保存先ディレクトリ。

デフォルトは [`Application.persistentDataPath`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Application-persistentDataPath.html) ですが、他のパスに変更することもできます。

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

# SaveManager.GetSaveFileCount

`public static int GetSaveFileCount(string extension = ".sav")`

## パラメータ

| `extension` | 数を取得するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

セーブディレクトリ内で指定した拡張子を持つセーブファイルの数を取得します。

## 戻り値

指定した拡張子を持つセーブファイルがあればその数を、なければ `0` を返します。

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(object dataDictionary = null, string fileName = "SaveData.sav")`

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

読み込んだデータは [`LoadedData`](#savemanagerloadeddata) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedData`](#savemanagerloadeddata) プロパティは空の辞書になります。

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(object dataDictionary = null, string fileName = "SaveData.sav")`

## パラメータ

| `dataDictionary` | 追加するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 追加するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |

## 説明

指定したバイナリファイルにデータを追加します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが追加されます。

---

# SaveManager.GetDataFromBinary

`public static void GetDataFromBinary(string fileName = "SaveData.sav")`

## パラメータ

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

指定したバイナリファイルからデータを取得します。

データは [`LoadedData`](#savemanagerloadeddata) プロパティに格納されます。

---

# SaveManager.DeleteDataFromBinary

`public static void DeleteDataFromBinary(string fileName = "SaveData.sav")`

## パラメータ

| `fileName` | 削除するファイル名（拡張子込み）。デフォルトは `SaveData.sav`。 |
|:---|:---|

## 説明

指定したバイナリファイルからデータを削除します。

---

# SaveManager.SaveAsJson

`public static void SaveAsJson(object dataDictionary = null, string fileName = "SaveData.json")`

## パラメータ

| `dataDictionary` | 保存するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 保存するファイル名（拡張子込み）。デフォルトは `SaveData.json`。 |

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。 **

データを指定されたファイル名の `Json` ファイルとして保存します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが保存されます。

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.json")`

## パラメータ

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.json`。 |
|:---|:---|

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。 **

指定した `Json` ファイルからデータを読み込みます。

読み込んだデータは [`LoadedData`](#savemanagerloadeddata) プロパティに保存されます。

ファイルが存在しない場合、[`LoadedData`](#savemanagerloadeddata) プロパティは空の辞書になります。

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(object dataDictionary = null, string fileName = "SaveData.json")`

## パラメータ

| `dataDictionary` | 追加するデータの辞書。デフォルトは [`DataToSave`](#savemanagerdatatosave)。 |
|:---|:---|
| `fileName` | 追加するファイル名（拡張子込み）。デフォルトは `SaveData.json`。 |

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。 **

指定した `Json` ファイルにデータを追加します。

`dataDictionary` が空の場合、[`DataToSave`](#savemanagerdatatosave) のデータが追加されます。

---

# SaveManager.GetDataFromJson

`public static void GetDataFromJson(string fileName = "SaveData.json")`

## パラメータ

| `fileName` | 読み込むファイル名（拡張子込み）。デフォルトは `SaveData.json`。 |
|:---|:---|

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。 **

指定した `Json` ファイルからデータを取得します。

データは [`LoadedData`](#savemanagerloadeddata) プロパティに格納されます。

---

# SaveManager.DeleteDataFromJson

`public static void DeleteDataFromJson(string fileName = "SaveData.json")`

## パラメータ

| `fileName` | 削除するファイル名（拡張子込み）。デフォルトは `SaveData.json`。 |
|:---|:---|

## 説明

**使用前に`Json.NET`パッケージをインストールし、[フレームワーク設定](../GensouLib.Unity.Tools/FrameworkSettings.md)で有効化してください。 **

指定した `Json` ファイルからデータを削除します。
