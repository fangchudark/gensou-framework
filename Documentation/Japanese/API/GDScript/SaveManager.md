# SaveManager

継承: [Object](https://docs.godotengine.org/ja/stable/classes/class_object.html)

## 説明

`SaveManager` はゲームのセーブデータを管理するためのクラスで、データの保存、読み込み、削除、追加、変更、検索、ディレクトリ作成などの機能を提供します。

ファイル パスのパス区切り文字としてスラッシュ `/` を使用してください。

## 静的プロパティ

|[data_to_save](#savemanagerdata_to_save)| 保存するデータ。 |
|:---|:---|
|[loaded_data_binary](#savemanagerloaded_data_binary)|バイナリファイルから読み込まれたデータ。|
|[loaded_data_json](#savemanagerloaded_data_json)|`Json`ファイルから読み込まれたデータ。|
|[save_path](#savemanagersave_path)| セーブデータのディレクトリ。 |

## 静的メソッド

|[create_directory](#savemanagercreate_directory)| ディレクトリを作成し、セーブディレクトリを変更する。 |
|:---|:---|
|[delete_save_directory](#savemanagerdelete_save_directory)|セーブディレクトリを削除する。 |
|[save_exists](#savemanagersave_exists)|指定したファイル名のセーブデータが存在するか確認する。 |
|[delete_save_file](#savemanagerdelete_save_file)|指定したセーブファイルを削除する。 |
|[delete_all_save_files](#savemanagerdelete_all_save_files)|指定した拡張子のセーブファイルをすべて削除する。 |
|[get_save_file_count](#savemanagerget_save_file_count)| 指定した拡張子のセーブファイル数を取得する。 |
|[save_as_binary](#savemanagersave_as_binary)|バイナリファイルとしてデータを保存する。 |
|[load_from_binary](#savemanagerload_from_binary)|バイナリファイルからデータを読み込む。 |
|[add_data_to_binary](#savemanageradd_data_to_binary)|バイナリファイルにデータを追加する。 |
|[get_data_from_binary](#savemanagerget_data_from_binary)|バイナリファイルからデータを取得する。 |
|[delete_data_from_binary](#savemanagerdelete_data_from_binary)|バイナリファイルからデータを削除する。 |
|[save_as_json](#savemanagersave_as_json)|Jsonファイルとしてデータを保存する。 |
|[load_from_json](#savemanagerload_from_json)|Jsonファイルからデータを読み込む。 |
|[add_data_to_json](#savemanageradd_data_to_json)|Jsonファイルにデータを追加する。 |
|[get_data_from_json](#savemanagerget_data_from_json)|Jsonファイルからデータを取得する。 |
|[delete_data_from_json](#savemanagerdelete_data_from_json)|Jsonファイルからデータを削除する。 |

---

# SaveManager.data_to_save

`static var data_to_save: Dictionary`

## 説明

保存するデータ。キーはデータを検索するための識別子または `Json` ファイルに保存するためのキー名、値は対応するデータです。

データを保存する前に、この辞書に保存するデータを格納してください。

---
 
# SaveManager.loaded_data_binary

`static var loaded_data_binary: Dictionary`

## 説明

読み取り専用属性、バイナリ ファイルからロードされたデータ。

キーはデータの検索に使用される識別子で、値は対応するデータです。

キーに対応する値が `Godot`オブジェクトの場合は、最初に `EncodedObjectAsID.object_id` プロパティにアクセスしてオブジェクトの参照 ID を取得する必要があります。

次に、`@GlobalScope.instance_from_id(int)` メソッドを使用して、参照 ID に基づいてオブジェクト インスタンスを取得します。

```gdscript
var instance_id = SaveManager.loaded_data_binary["key"].object_id
var node = instance_from_id(instance_id)
```

---

# SaveManager.loaded_data_json

`static var loaded_data_json: Dictionary`

## 説明

読み取り専用プロパティ。データは `Json` ファイルからロードされます。

キーは `Json` ファイルに保存されたキー名で、値は対応するデータです。

キーに対応する値が `Godot` オブジェクトの場合、まず `String.to_int()` を使用してその参照 ID を取得します。

次に、`@GlobalScope.instance_from_id(int)` メソッドを使用して、参照 ID に基づいてオブジェクト インスタンスを取得します。

```gdscript
var instance_id = SaveManager.loaded_data_json["key"].to_int()
var node = instance_from_id(instance_id)
```

---

# SaveManager.save_path

`static var save_path: String`

## 説明

セーブデータの保存先ディレクトリ。

デフォルト値は[`user://`の絶対パス](https://docs.godotengine.org/ja/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user)であり、他のパスに変更できます。


---

# SaveManager.create_directory

`static func create_directory(directory: String, create_at_localLow: bool = false) -> void`

## パラメータ

| `directory` | 作成するディレクトリのパス。 |
|:---|:---|
| `create_at_localLow` | `C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成するかどうか。デフォルトは `false`。 |

## 説明

ディレクトリを作成し、現在のセーブディレクトリを変更します。

`create_at_localLow` が `true` の場合、`C:\Users\ユーザー名\AppData\LocalLow` にディレクトリを作成します。この場合、`directory` パラメータは相対パスで指定する必要があります。

---

# SaveManager.delete_save_directory

`static func delete_save_directory() -> void`

## 説明

セーブディレクトリとその中のすべてのファイルを削除します。

---

# SaveManager.save_exists

`static func save_exists(file_name: String) -> bool`

## パラメータ

|`file_name`| チェックするセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したファイル名のセーブデータが存在するか確認します。

## 戻り値

セーブディレクトリに指定したセーブデータが存在すれば `true` を、存在しなければ `false` を返します。

---

# SaveManager.delete_save_file

`static func delete_save_file(file_name: String) -> void`

## パラメータ

|`file_name`| 削除するセーブファイル名（拡張子込み）。 |
|:---|:---|

## 説明

指定したセーブファイルを削除します。

---

# SaveManager.delete_all_save_files

`static func delete_all_save_files(extension: String = ".sav") -> void`

## パラメータ

| `extension` | 削除するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

指定した拡張子のセーブファイルをセーブディレクトリ内からすべて削除します。

---

# SaveManager.get_save_files_count

`static func get_save_files_count(extension: String = ".sav") -> int`

## パラメータ

| `extension` | 数を取得するセーブファイルの拡張子。デフォルトは `.sav`。 |
|:---|:---|

## 説明

セーブディレクトリ内で指定した拡張子を持つセーブファイルの数を取得します。

## 戻り値

指定した拡張子を持つセーブファイルがあればその数を、なければ `0` を返します。

---

# SaveManager.save_as_binary

`static func save_as_binary(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## パラメータ

|`data_dictionary`|保存するデータの辞書。デフォルトは[`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`|保存するファイル名（拡張子込み）。デフォルトは`SaveData.sav`。|

## 描述

データを指定されたファイル名のバイナリファイルとして保存します。

---

# SaveManager.load_from_binary

`static func load_from_binary(file_name: String = "SaveData.sav") -> void`

## パラメータ

|`file_name`|読み込むファイル名（拡張子込み）。デフォルトは`SaveData.sav`。|
|:---|:---|

## 説明

指定したバイナリファイルからデータを読み込みます。

読み込んだデータは[`loaded_data_binary`](#savemanagerloaded_data_binary)プロパティに保存されます。

ファイルが存在しない場合、[`loaded_data_binary`](#savemanagerloaded_data_binary)プロパティは空の辞書になります。

---

# SaveManager.add_data_to_binary

`static func add_data_to_binary(file_name: String, key: String, new_data: Variant) -> void`

## パラメータ

|`file_name`|データが追加されるバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。|
|`new_data`|追加されるデータ。|

## 説明

指定されたファイル名のバイナリ ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.get_data_from_binary

`static func get_data_from_binary(file_name: String, key: String) -> Variant`

## パラメータ

|`file_name`|データを取得するバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。 |

## 説明

指定されたファイル名のバイナリ ファイルからデータを取得します。

## 戻り値

ファイルが存在し、指定されたキーが含まれている場合は、対応するデータが返されます。それ以外の場合は、`null` が返されます。

---

# SaveManager.delete_data_from_binary

`static func delete_data_from_binary(file_name: String, key: String) -> void`

## パラメータ

|`file_name`|データを削除するバイナリ ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|データの検索に使用される識別子。|

## 説明

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。

---

# SaveManager.save_as_json

`static func save_as_json(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## パラメータ

|`data_dictionary`|保存するデータの辞書。デフォルトは[`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`|保存するファイル名（拡張子込み）。デフォルトは`SaveData.sav`。|

## 説明

データを指定されたファイル名の `Json` ファイルとして保存します。

---

# SaveManager.load_from_json

`static func load_from_json(file_name: String = "SaveData.sav") -> void`

## パラメータ

|`file_name`|読み込むファイル名（拡張子込み）。デフォルトは`SaveData.sav`。|
|:---|:---|

## 説明

指定した `Json` ファイルからデータを読み込みます。

読み込んだデータは[`loaded_data_json`](#savemanagerloaded_data_json)プロパティに保存されます。

ファイルが存在しない場合、[`loaded_data_json`](#savemanagerloaded_data_json)プロパティは空の辞書になります。

---

# SaveManager.add_data_to_json

`static func add_data_to_json(file_name: String, key: String, new_data: Variant) -> void`

## パラメータ

|`file_name`|データが追加される`Json`ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|`Json`ファイルに保存されたキーの名前。|
|`new_data`|追加されるデータ。|

## 説明

指定したファイル名で`Json`ファイルにデータを追加します。

ファイルが存在しない場合は作成します。

---

# SaveManager.get_data_from_json

`static func get_data_from_json(file_name: String, key: String, is_object: bool = false) -> Variant`

## パラメータ

|`file_name`|データを取得する`Json`ファイルの名前 (拡張子を含む)。 |
|:---|:---|
|`key`|`Json` ファイルに保存されたキーの名前。 |
|`is_object`|取得すべき値が `Godot` オブジェクトかどうかです。デフォルトは `false` です。|

## 説明

指定した `Json` ファイルからデータを取得します。

`is_object` が `true` の場合、取得した `Json` 値を `Godot` オブジェクトに変換し、そのインスタンスを返そうとします。

**キーに対応する値が `Godot` オブジェクトであることが明らかな場合にのみ、`is_object` を `true` に設定してください。そうでない場合、変換は失敗し、`null` が返されます。**

## 戻り値

ファイルが存在し、指定されたキーが含まれている場合は、対応するデータが返されます。それ以外の場合は、`null`が返されます。

`is_object` が `true` の場合、変換が成功した場合は `Godot` オブジェクト インスタンスが返され、そうでない場合は `null` が返されます。

---

# SaveManager.delete_data_from_json

`static func delete_data_from_json(file_name: String, key: String) -> void`

## パラメータ

|`file_name`|削除するファイル名（拡張子込み）。|
|:---|:---|
|`key`|`Json`ファイルに保存するキー名。|

## 説明

ファイルが存在し、指定されたキーが含まれている場合は、ファイルからデータを削除します。