# SaveManager

Inherits: [Object](https://docs.godotengine.org/en/stable/classes/class_object.html)

## Description

`SaveManager` is used to manage game save files, providing functionality for saving, reading, adding, deleting, modifying, querying, and creating directories.

Please use forward slash `/` as the path separator for file paths.

## Static Properties

|[data_to_save](#savemanagerdata_to_save)| Data to be saved. |
|:---|:---|
|[loaded_data_binary](#savemanagerloaded_data_binary)|The data loaded from a binary file.|
|[loaded_data_json](#savemanagerloaded_data_json)|The data loaded from a `JSON` file.|
|[save_path](#savemanagersave_path)|Save directory. |

## Static Methods

|[create_directory](#savemanagercreate_directory)| Creates a directory and changes the save path. |
|:---|:---|
|[delete_save_directory](#savemanagerdelete_save_directory)|Deletes the save directory. |
|[save_exists](#savemanagersave_exists)|Checks if a save file exists by file name. |
|[delete_save_file](#savemanagerdelete_save_file)|Deletes a specific save file. |
|[delete_all_save_files](#savemanagerdelete_all_save_files)|Deletes all save files in the directory with the specified extension. |
|[get_save_file_count](#savemanagerget_save_file_count)|Gets the number of save files in the directory with the specified extension. |
|[save_as_binary](#savemanagersave_as_binary)|Saves data as a binary file. |
|[load_from_binary](#savemanagerload_from_binary)|Loads data from a binary file. |
|[add_data_to_binary](#savemanageradd_data_to_binary)|Adds data to a binary file. |
|[get_data_from_binary](#savemanagerget_data_from_binary)|Retrieves data from a binary file. |
|[delete_data_from_binary](#savemanagerdelete_data_from_binary)|Saves data as a Json file. |
|[save_as_json](#savemanagersave_as_json)|Loads data from a Json file. |
|[load_from_json](#savemanagerload_from_json)|Adds data to a Json file. |
|[add_data_to_json](#savemanageradd_data_to_json)|Retrieves data from a Json file. |
|[get_data_from_json](#savemanagerget_data_from_json)|Retrieves data from a Json file. |
|[delete_data_from_json](#savemanagerdelete_data_from_json)|Deletes data from a Json file. |

---

# SaveManager.data_to_save

`static var data_to_save: Dictionary`

## Description

This property stores the data to be saved. The key is used to identify the data or to specify the key name in the `Json` file, and the value is the corresponding data.

Before saving, ensure the data to be saved is stored in this dictionary.

---
 
# SaveManager.loaded_data_binary

`static var loaded_data_binary: Dictionary`

## Description

Read-only property, data loaded from binary file.

The key is the identifier used to find the data, and the value is the corresponding data.

If the value corresponding to the key is a `Godot` object, you need to first access the `EncodedObjectAsID.object_id` property to get the object's reference ID.

Then, use the `@GlobalScope.instance_from_id(int)` method to get the object instance based on the reference ID.

```gdscript
var instance_id = SaveManager.loaded_data_binary["key"].object_id
var node = instance_from_id(instance_id)
```

---

# SaveManager.loaded_data_json

`static var loaded_data_json: Dictionary`

## Description

Read-only property, data loaded from `Json` file.

The key is the key name saved to the `Json` file, and the value is the corresponding data.

If the key is a `Godot` object, first get its reference ID using `String.to_int()`.

Then use the `@GlobalScope.instance_from_id(int)` method to get the object instance based on the reference ID.

```gdscript
var instance_id = SaveManager.loaded_data_json["key"].to_int()
var node = instance_from_id(instance_id)
```

---

# SaveManager.save_path

`static var save_path: String`

## Description

This property specifies the save directory.

The default value is [absolute path of `user://`](https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user), but it can be modified to any other path.

---

# SaveManager.create_directory

`static func create_directory(directory: String, create_at_localLow: bool = false) -> void`

## Parameters

| `directory` | The path of the directory to create. |
|:---|:---|
| `create_at_localLow` | Whether to create the directory at `C:\Users\Username\AppData\LocalLow`. Default is `false`. |

## Description

Creates a directory and changes the current save path.

If `createAtLocalLow` is `true`, the directory will be created under `C:\Users\Username\AppData\LocalLow`, and the `directory` parameter should be a relative path.

---

# SaveManager.delete_save_directory

`static func delete_save_directory() -> void`

## Description

Deletes the save directory and all files under it.

---

# SaveManager.save_exists

`static func save_exists(file_name: String) -> bool`

## Parameters

| `file_name` | The name of the save file, including the extension. |
|:---|:---|

## Description

Checks if a save file with the specified file name exists.

## Returns

Returns `true` if the specified save file exists in the save directory; otherwise, returns `false`.

---

# SaveManager.delete_save_file

`static func delete_save_file(file_name: String) -> void`

## Parameters

|`file_name`| The name of the save file to delete, including the extension. |
|:---|:---|

## Description

Deletes the specified save file if it exists in the save directory.

---

# SaveManager.delete_all_save_files

`static func delete_all_save_files(extension: String = ".sav") -> void`

## Parameters

|`extension`| The extension of the save files to delete. Default is `.sav`. |
|:---|:---|

## Description

Deletes all save files with the specified extension in the save directory.

---

# SaveManager.get_save_files_count

`static func get_save_files_count(extension: String = ".sav") -> int`

## Parameters

| `extension` | The extension of the save files to count. Default is `.sav`. |
|:---|:---|

## Description

Gets the number of save files with the specified extension in the save directory.

## Returns

Returns the number of save files with the specified extension if they exist, otherwise returns `0`.

---

# SaveManager.save_as_binary

`static func save_as_binary(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## Parameters

|`data_dictionary` | The dictionary of data to save. Default is [`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`|The name of the file to save, including the extension. Default is `SaveData.sav`。|

## Description

Saves the data as a binary file with the specified file name.

---

# SaveManager.load_from_binary

`static func load_from_binary(file_name: String = "SaveData.sav") -> void`

## Parameters

| `file_name` | The name of the file to read, including the extension. Default is `SaveData.sav`. |
|:---|:---|

## Description

Reads data from the specified binary file.

The data is saved in the [`loaded_data_binary`](#savemanagerloaded_data_binary) property.

If the file does not exist, [`loaded_data_binary`](#savemanagerloaded_data_binary) will be an empty dictionary.

---

# SaveManager.add_data_to_binary

`static func add_data_to_binary(file_name: String, key: String, new_data: Variant) -> void`

## Parameters

| `file_name` | The name of the binary file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |
| `new_data` | The data to add. |

## Description

Adds data to the specified binary file.

If the file does not exist, it will be created.

---

# SaveManager.get_data_from_binary

`static func get_data_from_binary(file_name: String, key: String) -> Variant`

## 参数

|`file_name`| The name of the binary file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |

## Description

Retrieves data from the specified binary file.

## Returns

If the file exists and contains the specified key, it returns the corresponding data; otherwise, it returns `null`.

---

# SaveManager.delete_data_from_binary

`static func delete_data_from_binary(file_name: String, key: String) -> void`

## Parameters

| `fileName` | The name of the binary file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |

## Description

Deletes the data from the binary file if it exists and contains the specified key.

---

# SaveManager.save_as_json

`static func save_as_json(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## Parameters

|`data_dictionary` | The dictionary of data to save. Default is [`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`| The name of the file to save, including the extension. Default is `SaveData.sav`. |

## Description

Saves the data as a `Json` file with the specified file name.

---

# SaveManager.load_from_json

`static func load_from_json(file_name: String = "SaveData.sav") -> void`

## Parameters

| `file_name` | The name of the file to read, including the extension. Default is `SaveData.sav`. |
|:---|:---|

## Description

Reads data from the specified `Json` file.

The data is saved in the [`loaded_data_json`](#savemanagerloaded_data_json) property.

If the file does not exist, [`loaded_data_json`](#savemanagerloaded_data_json) will be an empty dictionary.

---

# SaveManager.add_data_to_json

`static func add_data_to_json(file_name: String, key: String, new_data: Variant) -> void`

## Parameters

| `file_name` | The name of the binary file, including the extension. |
|:---|:---|
| `key` | The key to save to the `Json` file. |
| `new_data` | The data to add. |

## Description

Adds data to the specified `Json` file.

If the file does not exist, it will be created.

---

# SaveManager.get_data_from_json

`static func get_data_from_json(file_name: String, key: String, is_object: bool = false) -> Variant`

## 参数

|`file_name`| The name of the binary file, including the extension. |
|:---|:---|
|`key`|The key to save to the `Json` file. |
|`is_object`|Whether the value to retrieve is a `Godot` object, defaults to `false`.|

## Description

Retrieves data from the specified `Json` file.

If `is_object` is `true`, it will try to convert the obtained `Json` value to a `Godot` object and return its instance.

**Set `is_object` to `true` only when the value corresponding to the key is a `Godot` object, otherwise it will cause the conversion to fail and return `null`.**

## 返回

If the file exists and contains the specified key, it returns the corresponding data; otherwise, it returns `null`.

When `is_object` is `true`, the `Godot` object instance is returned if the conversion is successful, otherwise `null` is returned.

---

# SaveManager.delete_data_from_json

`static func delete_data_from_json(file_name: String, key: String) -> void`

## Parameters

|`file_name`| The name of the `Json` file, including the extension. |
|:---|:---|
| `key` | The key to save to the `Json` file. |

## Description

Deletes the data from the `Json` file if it exists and contains the specified key.

