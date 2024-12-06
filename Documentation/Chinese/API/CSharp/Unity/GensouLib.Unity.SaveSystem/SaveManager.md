# SaveManager

## 描述

`SaveManager` 用于管理游戏存档，提供保存、读取、删增改查、创建目录等功能。

默认使用二进制保存数据，如果需要使用`Json`保存请使用[框架工具](../GensouLib.Unity.Tools/PackageInstaller.md)或Unity Package Manager安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。

使用二进制保存数据时，请将需要保存的数据存入标记为`[Serializable]`属性的类中。

使用`Json`保存数据时，不支持序列化Unity对象，请将需要保存的Unity对象的状态如名称，位置等数据提取出来并存入字典中。

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

`public static Dictionary<string, object> DataToSave`

## 描述

需要保存的数据，键为用于查找数据的标识或保存到`Json`文件的键名，值为对应的数据。

在保存数据之前请将需要保存的数据存入该字典。

---

# SaveManager.LoadedDataBinary

`public static Dictionary<string, object> LoadedDataBinary`

## 描述

只读属性，从二进制文件中加载的数据。

键为用于查找数据的标识，值为对应的数据。

---

# SaveManager.LoadedDataJson

`public static Dictionary<string, object> LoadedDataJson`

## 描述

只读属性，从`Json`文件中加载的数据。

键为保存到`Json`文件的键名，值为对应的数据。

如果键对应的值是包含多个属性的类或字典，请将其转换为 `JObject` 类型，然后使用`JObject.Properties` 方法访问其属性。

```csharp
JObject jObject = (JObject)SaveManager.LoadedDataJson["key"];
foreach (var property in jObject.Properties())
{
    // 处理属性
    Debug.Log(property.Name + " : " + property.Value);
}
```

如果键对应的值是数组或列表，请将其转换为 `JArray` 类型，然后使用`JArray.Children` 方法访问其元素。

```csharp
JArray jArray = (JArray)SaveManager.LoadedDataJson["key"];
foreach (var item in jArray.Children())
{
    // 处理元素
    Debug.Log(item.ToString());
}
```

---

# SaveManager.SavePath

`public static string SavePath`

## 描述

存档目录。

默认值为[`Application.persistentDataPath`](https://docs.unity.cn/cn/2022.3/ScriptReference/Application-persistentDataPath.html)，可修改为其他路径。

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

# SaveManager.GetSaveFileCount

`public static int GetSaveFileCount(string extension = ".sav")`

## 参数

|`extension`|要获取数量的存档文件扩展名，默认为`.sav`。|
|:---|:---|

## 描述

获取存档目录下指定拓展名的存档文件数量。

## 返回

如果存档目录下存在指定拓展名的存档文件则返回数量，否则返回`0`。

---

# SaveManager.SaveAsBinary

`public static void SaveAsBinary(object dataDictionary = null, string fileName = "SaveData.sav")`

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

`public static void AddDataToBinary(string fileName, string key, object newData)`

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

`public static T GetDataFromBinary<T>(string fileName, string key)`

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

`public static void SaveAsJson(object dataDictionary = null, string fileName = "SaveData.sav")`

## 参数

|`dataDictionary`|要保存的数据字典，默认为[`DataToSave`](#savemanagerdatatosave)。|
|:---|:---|
|`fileName`|要保存的文件名，包含扩展名，默认为`SaveData.sav`。|

## 描述

**使用前请安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。**

保存数据为指定文件名的`Json`文件。

如果`dataDictionary`为空则使用[`DataToSave`](#savemanagerdatatosave)保存数据。

---

# SaveManager.LoadFromJson

`public static void LoadFromJson(string fileName = "SaveData.sav")`

## 参数

|`fileName`|要读取的文件名，包含扩展名，默认为`SaveData.sav`。|
|:---|:---|

## 描述

**使用前请安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。**

读取指定文件名的`Json`文件中的数据。

并将读取的数据保存到[`LoadedDataJson`](#savemanagerloadeddatajson)属性中。

如果文件不存在则[`LoadedDataJson`](#savemanagerloadeddatajson)属性为空字典。

---

# SaveManager.AddDataToJson

`public static void AddDataToJson(string fileName, string key, object newData)`

## 参数

|`fileName`|要添加数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|
|`newData`|要添加的数据。|

## 描述

**使用前请安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。**

向指定文件名的`Json`文件中添加数据。

如果文件不存在则创建文件。

---

# SaveManager.GetDataFromJson

`public static T GetDataFromJson<T>(string fileName, string key)`

## 参数

|`T`|要获取数据的类型。|
|:---|:---|
|`fileName`|要获取数据的`Json`文件名，包含扩展名。|
|`key`|保存到`Json`文件的键名。|

## 描述

**使用前请安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。**

从指定文件名的`Json`文件中获取数据。

此方法只能获取 Json 能够序列化的基本类型数据，如字符串、数字（`long`、`double`）和布尔值。

**不能直接获取 C# 集合类型、Unity 对象或自定义类。**

如果需要获取，请通过访问[`LoadedDataJson`](#savemanagerloadeddatajson)属性获取。

从 `JSON` 文件获取数据时，`Json.NET` 对应的类型映射规则如下：

- C# 集合：

    - 字典：`JObject`
  
    - 数组：`JArray`
  
    - 列表：`JArray`

- 自定义类：`JObject`

- 基本数据类型：

    - 整型：`long`

    - 浮点型：`double`

    - 字符串：`string`
    
    - 布尔值：`bool`

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

**使用前请安装`Json.NET`包并在[框架设置](../GensouLib.Unity.Tools/FrameworkSettings.md)中启用它。**

如果文件存在且包含指定键则从文件中删除数据。





