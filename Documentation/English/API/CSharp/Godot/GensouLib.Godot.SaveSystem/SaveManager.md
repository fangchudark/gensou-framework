# SaveManager

## Description

`SaveManager` is used to manage game save files, providing functionality for saving, reading, adding, deleting, modifying, querying, and creating directories.

All I/O operations of this class are implemented based on Godot's API. Please use forward slash `/` as the path separator for file paths.

## Static Properties

| [DataToSave](#savemanagerdatatosave) | Data to be saved. |
|:---|:---|
|[LoadedDataBinary](#savemanagerloadeddatabinary)|The data loaded from a binary file.|
|[LoadedDataJson](#savemanagerloadeddatajson)|The data loaded from a `JSON` file.|
| [SavePath](#savemannersavepath) | Save directory. |

## Static Methods

| [CreateDirectory](#savemanagercreatedirectory) | Creates a directory and changes the save path. |
|:---|:---|
| [DeleteSaveDirectory](#savemanagerdeletesavedirectory) | Deletes the save directory. |
| [SaveExists](#savemanagersaveexists) | Checks if a save file exists by file name. |
| [DeleteSaveFile](#savemanagerdeletesavefile) | Deletes a specific save file. |
| [DeleteAllSaveFiles](#savemanagerdeleteallsavefiles) | Deletes all save files in the directory with the specified extension. |
| [GetSaveFileCount](#savemanagergetsavefilecount) | Gets the number of save files in the directory with the specified extension. |
| [SaveAsBinary](#savemanagersaveasbinary) | Saves data as a binary file. |
| [LoadFromBinary](#savemanagerloadfrombinary) | Loads data from a binary file. |
| [AddDataToBinary](#savemanageradddatatobinary) | Adds data to a binary file. |
| [GetDataFromBinary](#savemanagergetdatafrombinary) | Retrieves data from a binary file. |
| [DeleteDataFromBinary](#savemanagerdeletedatafrombinary) | Deletes data from a binary file. |
| [SaveAsJson](#savemanagersaveasjson) | Saves data as a Json file. |
| [LoadFromJson](#savemanagerloadfromjson) | Loads data from a Json file. |
| [AddDataToJson](#savemanageradddatatojson) | Adds data to a Json file. |
| [GetDataFromJson](#savemanagergetdatafromjson) | Retrieves data from a Json file. |
| [DeleteDataFromJson](#savemanagerdeletedatafromjson) | Deletes data from a Json file. |

---

# SaveManager.DataToSave

`public static Dictionary<string, Variant> DataToSave`

## Description

This property stores the data to be saved. The key is used to identify the data or to specify the key name in the `Json` file, and the value is the corresponding data.

Before saving, ensure the data to be saved is stored in this dictionary.

---

# SaveManager.LoadedDataBinary

`public static Dictionary<string, Variant> LoadedDataBinary`

## Description

Read-only property, data loaded from binary file.

The key is the identifier used to find the data, and the value is the corresponding data.

If the value corresponding to the key is a `Godot` object, convert it to the `EncodedObjectAsId` type.

Then access the `EncodedObjectAsId.ObjectId` property to retrieve the object reference ID.

Finally, use the `GodotObject.InstanceFromId(ulong)` method to get the object instance from the reference ID.

```csharp
EncodedObjectAsId encodedObject = (EncodedObjectAsId)SaveManager.LoadedDataBinary["key"];
Node node = (Node)GodotObject.InstanceFromId(encodedObject.ObjectId);
```

---

# SaveManager.LoadedDataJson

`public static Dictionary<string, Variant> LoadedDataJson`

## Description

Read-only property, data loaded from `Json` file.

The key is the key name saved to the `Json` file, and the value is the corresponding data.

If the value corresponding to the key is a `Godot` object, use `Variant.AsUInt64()` to retrieve its reference ID.

Then use the `GodotObject.InstanceFromId(ulong)` method to get the object instance from the reference ID.

```csharp
ulong objectId = SaveManager.LoadedDataJson["key"].AsUInt64();
Node node = (Node)GodotObject.InstanceFromId(objectId);
```

---

# SaveManager.SavePath

`public static string SavePath`

## Description

This property specifies the save directory.

The default value is [absolute path of `user://`](https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user), but it can be modified to any other path.

---

# SaveManager.CreateDirectory

`public static void CreateDirectory(string directory, bool createAtLocalLow = false)`

## Parameters

| `directory` | The path of the directory to create. |
|:---|:---|
| `createAtLocalLow` | Whether to create the directory at `C:\Users\Username\AppData\LocalLow`. Default is `false`. |

## Description

Creates a directory and changes the current save path.

If `createAtLocalLow` is `true`, the directory will be created under `C:\Users\Username\AppData\LocalLow`, and the `directory` parameter should be a relative path.

---

# SaveManager.DeleteSaveDirectory

`public static void DeleteSaveDirectory()`

## Description

Deletes the save directory and all files under it.

---

# SaveManager.SaveExists

`public static bool SaveExists(string fileName)`

## Parameters

| `fileName` | The name of the save file, including the extension. |
|:---|:---|

## Description

Checks if a save file with the specified file name exists.

## Returns

Returns `true` if the specified save file exists in the save directory; otherwise, returns `false`.

---

# SaveManager.DeleteSaveFile

`public static void DeleteSaveFile(string fileName)`

## Parameters

| `fileName` | The name of the save file to delete, including the extension. |
|:---|:---|

## Description

Deletes the specified save file if it exists in the save directory.

---

# SaveManager.DeleteAllSaveFiles

`public static void DeleteAllSaveFiles(string extension = ".sav")`

## Parameters

| `extension` | The extension of the save files to delete. Default is `.sav`. |
|:---|:---|

## Description

Deletes all save files with the specified extension in the save directory.

---

# SaveManager.GetSaveFilesCount

`public static int GetSaveFilesCount(string extension = ".sav")`

## Parameters

| `extension` | The extension of the save files to count. Default is `.sav`. |
|:---|:---|

## Description

Gets the number of save files with the specified extension in the save directory.

## Returns

Returns the number of save files with the specified extension if they exist, otherwise returns `0`.

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## Parameters

| `dataDictionary` | The dictionary of data to save. Default is [`DataToSave`](#savemanagerdatatosave). |
|:---|:---|
| `fileName` | The name of the file to save, including the extension. Default is `SaveData.sav`. |

## Description

Saves the data as a binary file with the specified file name.

If `dataDictionary` is null, it will use [`DataToSave`](#savemanagerdatatosave) for saving.

---

# SaveManager.LoadFromBinary

`public static void LoadFromBinary(string fileName = "SaveData.sav")`

## Parameters

| `fileName` | The name of the file to read, including the extension. Default is `SaveData.sav`. |
|:---|:---|

## Description

Reads data from the specified binary file.

The data is saved in the [`LoadedDataBinary`](#savemanagerloadeddatabinary) property.

If the file does not exist, [`LoadedDataBinary`](#savemanagerloadeddatabinary) will be an empty dictionary.

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(string fileName, string key, Variant newData)`

## Parameters

| `fileName` | The name of the binary file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |
| `newData` | The data to add. |

## Description

Adds data to the specified binary file.

If the file does not exist, it will be created.

---

# SaveManager.GetDataFromBinary

`public static T GetDataFromBinary<[MustBeVariant] T>(string fileName, string key)`

## Parameters

| `T` | The type of data to retrieve. |
|:---|:---|
| `fileName` | The name of the binary file, including the extension. |
| `key` | The identifier used to find the data. |

## Description

Retrieves data from the specified binary file.

## Returns

If the file exists and contains the specified key, it returns the corresponding data; otherwise, it returns the `default` value.

---

# SaveManager.DeleteDataFromBinary

`public static void DeleteDataFromBinary(string fileName, string key)`

## Parameters

| `fileName` | The name of the binary file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |

## Description

Deletes the data from the binary file if it exists and contains the specified key.

---

# SaveManager.SaveAsJson

`public static void SaveAsJson(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## Parameters

| `dataDictionary` | The dictionary of data to save. Default is [`DataToSave`](#savemanagerdatatosave). |
|:---|:---|
| `fileName` | The name of the file to save, including the extension. Default is `SaveData.sav`. |

## Description

Saves the data as a `Json` file with the specified file name.

If `dataDictionary` is null, it will use [`DataToSave`](#savemanagerdatatosave) for saving.

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## Parameters

| `fileName` | The name of the file to read, including the extension. Default is `SaveData.sav`. |
|:---|:---|

## Description

Reads data from the specified `Json` file.

The data is saved in the [`LoadedDataJson`](#savemanagerloadeddatajson) property.

If the file does not exist, [`LoadedDataJson`](#savemanagerloadeddatajson) will be an empty dictionary.

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, Variant newData)`

## Parameters

| `fileName` | The name of the `Json` file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |
| `newData` | The data to add. |

## Description

Adds data to the specified `Json` file.

If the file does not exist, it will be created.

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<[MustBeVariant] T>(string fileName, string key)`

## Parameters

| `T` | The type of data to retrieve. |
|:---|:---|
| `fileName` | The name of the `Json` file, including the extension. |
| `key` | The identifier used to find the data. |

## Description

Retrieves data from the specified `Json` file.

## Returns

If the file exists and contains the specified key, it returns the corresponding data; otherwise, it returns the `default` value.


---

# SaveManager.DeleteDataFromJson

`public static void DeleteDataFromJson(string fileName, string key)`

## Parameters

| `fileName` | The name of the `Json` file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |

## Description

Deletes the data from the `Json` file if it exists and contains the specified key.





