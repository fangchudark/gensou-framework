# SaveManager

## Description

`SaveManager` is used to manage game save files, providing functionality for saving, reading, adding, deleting, modifying, querying, and creating directories.

By default, data is saved in binary format. If you prefer to save in `Json` format, please install the `Json.NET` package via [framework tools](../GensouLib.Unity.Tools/PackageInstaller.md) or Unity Package Manager, and enable it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).

When saving data in binary format, store the data in classes marked with the `[Serializable]` attribute.

When saving in `Json` format, Unity objects cannot be serialized. Extract necessary states such as names, positions, etc., and store them in a dictionary.

## Static Properties

| [DataToSave](#savemanagerdatatosave) | Data to be saved. |
|:---|:---|
| [LoadedData](#savemanagerloadeddata) | Loaded save data. |
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

`public static Dictionary<string, object> DataToSave`

## Description

This property stores the data to be saved. The key is used to identify the data or to specify the key name in the `Json` file, and the value is the corresponding data.

Before saving, ensure the data to be saved is stored in this dictionary.

---

# SaveManager.LoadedData

`public static Dictionary<string, object> LoadedData`

## Description

A read-only property that contains the loaded save data.

The key is used to identify the data or specify the key name in the `Json` file, and the value is the corresponding data.

---

# SaveManager.SavePath

`public static string SavePath`

## Description

This property specifies the save directory.

The default value is [`Application.persistentDataPath`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Application-persistentDataPath.html), but it can be modified to any other path.

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

# SaveManager.GetSaveFileCount

`public static int GetSaveFileCount(string extension = ".sav")`

## Parameters

| `extension` | The extension of the save files to count. Default is `.sav`. |
|:---|:---|

## Description

Gets the number of save files with the specified extension in the save directory.

## Returns

Returns the number of save files with the specified extension if they exist, otherwise returns `0`.

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(object dataDictionary = null, string fileName = "SaveData.sav")`

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

The data is saved in the [`LoadedData`](#savemanagerloadeddata) property.

If the file does not exist, [`LoadedData`](#savemanagerloadeddata) will be an empty dictionary.

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(string fileName, string key, object newData)`

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

`public static T GetDataFromBinary<T>(string fileName, string key)`

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

`public static void SaveAsJson(object dataDictionary = null, string fileName = "SaveData.sav")`

## Parameters

| `dataDictionary` | The dictionary of data to save. Default is [`DataToSave`](#savemanagerdatatosave). |
|:---|:---|
| `fileName` | The name of the file to save, including the extension. Default is `SaveData.sav`. |

## Description

**Before using this, make sure you have installed the `Json.NET` package and enabled it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).**

Saves the data as a `Json` file with the specified file name.

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## Parameters

| `fileName` | The name of the file to read, including the extension. Default is `SaveData.sav`. |
|:---|:---|

## Description

**Before using this, make sure you have installed the `Json.NET` package and enabled it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).**

Reads data from the specified `Json` file.

The data is saved in the [`LoadedData`](#savemanagerloadeddata) property.

If the file does not exist, [`LoadedData`](#savemanagerloadeddata) will be an empty dictionary.

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, object newData)`

## Parameters

| `fileName` | The name of the `Json` file, including the extension. |
|:---|:---|
| `key` | The identifier used to find the data. |
| `newData` | The data to add. |

## Description

**Before using this, make sure you have installed the `Json.NET` package and enabled it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).**

Adds data to the specified `Json` file.

If the file does not exist, it will be created.

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<T>(string fileName, string key)`

## Parameters

| `T` | The type of data to retrieve. |
|:---|:---|
| `fileName` | The name of the `Json` file, including the extension. |
| `key` | The identifier used to find the data. |

## Description

**Before using this, make sure you have installed the `Json.NET` package and enabled it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).**

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

**Before using this, make sure you have installed the `Json.NET` package and enabled it in the [framework settings](../GensouLib.Unity.Tools/FrameworkSettings.md).**

Deletes the data from the `Json` file if it exists and contains the specified key.
