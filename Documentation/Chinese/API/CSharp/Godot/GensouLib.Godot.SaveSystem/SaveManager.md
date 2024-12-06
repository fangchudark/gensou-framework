# SaveManager

## 描述

`SaveManager` 用于管理游戏存档，提供保存、读取、删增改查、创建目录等功能。

该类的所有 I/O 操作均基于Godot的API实现，文件路径请使用正斜杠`/`作为路径分隔符。

## 静态属性

|[DataToSave](#savemanagerdatatosave)|需要保存的数据。|
|:---|:---|
|[LoadedDataBinary](#savemanagerloadeddatabinary)|从二进制文件中加载的数据。|
|[LoadedDataJson](#savemanagerloadeddatajson)|从`Json`文件中加载的数据。|
|[SavePath](#savemanagersavepath)|存档目录。|

## 静态方法

|[CreateDirectory](#savemanagercreatedirectory)|创建目录，并更改存档目录。|
|:---|:---|
|[DeleteSaveDirectory](#savemanagerdeletesavedirectory)|删除存档目录。|
|[SaveExists](#savemanagersaveexists)|检查指定文件名的存档是否存在。|
|[DeleteSaveFile](#savemanagerdeletesavefile)|删除指定存档文件。|
|[DeleteAllSaveFiles](#savemanagerdeleteallsavefiles)|根据扩展名删除存档目录下所有存档文件。|
|[GetSaveFileCount](#savemanagergetsavefilecount)|根据扩展名获取存档目录下存档文件数量。|
|[SaveAsBinary](#savemanagersaveasbinary)|保存数据为二进制文件。|
|[LoadFromBinary](#savemanagerloadfrombinary)|读取二进制文件中的数据。|
|[AddDataToBinary](#savemanageradddatatobinary)|向二进制文件中添加数据。|
|[GetDataFromBinary](#savemanagergetdatafrombinary)|从二进制文件中获取数据。|
|[DeleteDataFromBinary](#savemanagerdeletedatafrombinary)|从二进制文件中删除数据。|
|[SaveAsJson](#savemanagersaveasjson)|保存数据为Json文件。|
|[LoadFromJson](#savemanagerloadfromjson)|读取Json文件中的数据。|
|[AddDataToJson](#savemanageradddatatojson)|向Json文件中添加数据。|
|[GetDataFromJson](#savemanagergetdatafromjson)|从Json文件中获取数据。|
|[DeleteDataFromJson](#savemanagerdeletedatafromjson)|从Json文件中删除数据。|

---

# SaveManager.DataToSave

`public static Dictionary<string, Variant> DataToSave`

## 描述

需要保存的数据，键为用于查找数据的标识或保存到`Json`文件的键名，值为对应的数据。

在保存数据之前请将需要保存的数据存入该字典。

---

# SaveManager.LoadedDataBinary

`public static Dictionary<string, Variant> LoadedDataBinary`

## 描述

只读属性，从二进制文件中加载的数据。

键为用于查找数据的标识，值为对应的数据。

如果键对应的值是 `Godot` 对象，需要先将其转换为`EncodedObjectAsId`类型。

然后访问`EncodedObjectAsId.ObjectId`属性来获取对象的引用 ID。

最后，使用`GodotObject.InstanceFromId(ulong)`方法根据引用 ID 获取对象实例。

```csharp
EncodedObjectAsId encodedObject = (EncodedObjectAsId)SaveManager.LoadedDataBinary["key"];
Node node = (Node)GodotObject.InstanceFromId(encodedObject.ObjectId);
```

---

# SaveManager.LoadedDataJson

`public static Dictionary<string, Variant> LoadedDataJson`

## 描述

只读属性，从`Json`文件中加载的数据。

键为保存到`Json`文件的键名，值为对应的数据。

如果键对应的值是 `Godot` 对象，首先使用`Variant.AsUInt64()` 获取其引用 ID。

然后使用`GodotObject.InstanceFromId(ulong)`方法根据引用 ID 获取对象实例。

```csharp
ulong objectId = SaveManager.LoadedDataJson["key"].AsUInt64();
Node node = (Node)GodotObject.InstanceFromId(objectId);
```

---

# SaveManager.SavePath

`public static string SavePath`

## 描述

存档目录。

默认值为[`user://`的绝对路径](https://docs.godotengine.org/zh-cn/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user)，可修改为其他路径。

---

# SaveManager.CreateDirectory

`public static void CreateDirectory(string directory, bool createAtLocalLow = false)`

## 参数

|`directory`|要创建的目录路径。|
|:---|:---|
|`createAtLocalLow`|是否创建在`C:\Users\用户名\AppData\LocalLow`目录下，默认为`false`。|

## 描述

创建目录，并更改当前存档目录。

如果`createAtLocalLow`为`true`则在`C:\Users\用户名\AppData\LocalLow`下创建目录，此时`directory`参数应为相对路径。

---

# SaveManager.DeleteSaveDirectory

`public static void DeleteSaveDirectory()`

## 描述

删除存档目录及其下所有文件。

---

# SaveManager.SaveExists

`public static bool SaveExists(string fileName)`

## 参数

|`fileName`|要检查的存档文件名，包含扩展名。|
|:---|:---|

## 描述

检查指定文件名的存档是否存在。

## 返回

如果存档目录下存在指定存档则返回`true`，否则返回`false`。

---

# SaveManager.DeleteSaveFile

`public static void DeleteSaveFile(string fileName)`

## 参数

|`fileName`|要删除的存档文件名，包含扩展名。|
|:---|:---|

## 描述

如果存档目录下存在指定存档文件则删除它。

---

# SaveManager.DeleteAllSaveFiles

`public static void DeleteAllSaveFiles(string extension = ".sav")`

## 参数

|`extension`|要删除的存档文件扩展名，默认为`.sav`。|
|:---|:---|

## 描述

删除存档目录下所有指定拓展名的存档文件。

---

# SaveManager.GetSaveFilesCount

`public static int GetSaveFilesCount(string extension = ".sav")`

## 参数

|`extension`|要获取数量的存档文件扩展名，默认为`.sav`。|
|:---|:---|

## 描述

获取存档目录下指定拓展名的存档文件数量。

## 返回

如果存档目录下存在指定拓展名的存档文件则返回数量，否则返回`0`。

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## 参数

|`dataDictionary`|要保存的数据字典，默认为[`DataToSave`](#savemanagerdatatosave)。|
|:---|:---|
|`fileName`|要保存的文件名，包含扩展名，默认为`SaveData.sav`。|

## 描述

保存数据为指定文件名的二进制文件。

如果`dataDictionary`为空则使用[`DataToSave`](#savemanagerdatatosave)保存数据。

---

# SaveManager.LoadFromBinary

`public static void LoadFromBinary(string fileName = "SaveData.sav")`

## 参数

|`fileName`|要读取的文件名，包含扩展名，默认为`SaveData.sav`。|
|:---|:---|

## 描述

读取指定文件名的二进制文件中的数据。

并将读取的数据保存到[`LoadedDataBinary`](#savemanagerloadeddatabinary)属性中。

如果文件不存在则[`LoadedDataBinary`](#savemanagerloadeddatabinary)属性为空字典。

---

# SaveManager.AddDataToBinary

`public static void AddDataToBinary(string fileName, string key, Variant newData)`

## 参数

|`fileName`|要添加数据的二进制文件名，包含扩展名。|
|:---|:---|
|`key`|用于查找数据的标识。|
|`newData`|要添加的数据。|

## 描述

向指定文件名的二进制文件中添加数据。

如果文件不存在则创建文件。

---

# SaveManager.GetDataFromBinary

`public static T GetDataFromBinary<[MustBeVariant] T>(string fileName, string key)`

## 参数

|`T`|要获取数据的类型。|
|:---|:---|
|`fileName`|要获取数据的二进制文件名，包含扩展名。|
|`key`|用于查找数据的标识。|

## 描述

从指定文件名的二进制文件中获取数据。

## 返回

如果文件存在且包含指定键则返回对应数据，否则返回`default`值。

---

# SaveManager.DeleteDataFromBinary

`public static void DeleteDataFromBinary(string fileName, string key)`

## 参数

|`fileName`|要删除数据的二进制文件名，包含扩展名。|
|:---|:---|
|`key`|用于查找数据的标识。|

## 描述

如果文件存在且包含指定键则从文件中删除数据。

---

# SaveManager.SaveAsJson

`public static void SaveAsJson(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")`

## 参数

|`dataDictionary`|要保存的数据字典，默认为[`DataToSave`](#savemanagerdatatosave)。|
|:---|:---|
|`fileName`|要保存的文件名，包含扩展名，默认为`SaveData.sav`。|

## 描述

保存数据为指定文件名的`Json`文件。

如果`dataDictionary`为空则使用[`DataToSave`](#savemanagerdatatosave)保存数据。

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## 参数

|`fileName`|要读取的文件名，包含扩展名，默认为`SaveData.sav`。|
|:---|:---|

## 描述

读取指定文件名的`Json`文件中的数据。

并将读取的数据保存到[`LoadedDataJson`](#savemanagerloadeddatajson)属性中。

如果文件不存在则[`LoadedDataJson`](#savemanagerloadeddatajson)属性为空字典。

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, Variant newData)`

## 参数

|`fileName`|要添加数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|
|`newData`|要添加的数据。|

## 描述

向指定文件名的`Json`文件中添加数据。

如果文件不存在则创建文件。

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<[MustBeVariant] T>(string fileName, string key)`

## 参数

|`T`|要获取数据的类型。|
|:---|:---|
|`fileName`|要获取数据的`Json`文件名，包含扩展名。|
|`key`|保存到`Json`文件的键名。|

## 描述

从指定文件名的`Json`文件中获取数据。

## 返回

如果文件存在且包含指定键则返回对应数据，否则返回`default`值。

---

# SaveManager.DeleteDataFromJson

`public static void DeleteDataFromJson(string fileName, string key)`

## 参数

|`fileName`|要删除数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|

## 描述

如果文件存在且包含指定键则从文件中删除数据。





