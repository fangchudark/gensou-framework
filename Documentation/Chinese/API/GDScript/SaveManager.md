# SaveManager

继承：[Object](https://docs.godotengine.org/zh-cn/stable/classes/class_object.html)

## 描述

`SaveManager` 用于管理游戏存档，提供保存、读取、删增改查、创建目录等功能。

文件路径请使用正斜杠`/`作为路径分隔符。

## 静态属性

|[data_to_save](#savemanagerdata_to_save)|需要保存的数据。|
|:---|:---|
|[loaded_data_binary](#savemanagerloaded_data_binary)|从二进制文件中加载的数据。|
|[loaded_data_json](#savemanagerloaded_data_json)|从`Json`文件中加载的数据。|
|[save_path](#savemanagersave_path)|存档目录。|

## 静态方法

|[create_directory](#savemanagercreate_directory)|创建目录，并更改存档目录。|
|:---|:---|
|[delete_save_directory](#savemanagerdelete_save_directory)|删除存档目录。|
|[save_exists](#savemanagersave_exists)|检查指定文件名的存档是否存在。|
|[delete_save_file](#savemanagerdelete_save_file)|删除指定存档文件。|
|[delete_all_save_files](#savemanagerdelete_all_save_files)|根据扩展名删除存档目录下所有存档文件。|
|[get_save_file_count](#savemanagerget_save_file_count)|根据扩展名获取存档目录下存档文件数量。|
|[save_as_binary](#savemanagersave_as_binary)|保存数据为二进制文件。|
|[load_from_binary](#savemanagerload_from_binary)|读取二进制文件中的数据。|
|[add_data_to_binary](#savemanageradd_data_to_binary)|向二进制文件中添加数据。|
|[get_data_from_binary](#savemanagerget_data_from_binary)|从二进制文件中获取数据。|
|[delete_data_from_binary](#savemanagerdelete_data_from_binary)|从二进制文件中删除数据。|
|[save_as_json](#savemanagersave_as_json)|保存数据为Json文件。|
|[load_from_json](#savemanagerload_from_json)|读取Json文件中的数据。|
|[add_data_to_json](#savemanageradd_data_to_json)|向Json文件中添加数据。|
|[get_data_from_json](#savemanagerget_data_from_json)|从Json文件中获取数据。|
|[delete_data_from_json](#savemanagerdelete_data_from_json)|从Json文件中删除数据。|

---

# SaveManager.data_to_save

`static var data_to_save: Dictionary`

## 描述

需要保存的数据，键为用于查找数据的标识或保存到`Json`文件的键名，值为对应的数据。

在保存数据之前请将需要保存的数据存入该字典。

---
 
# SaveManager.loaded_data_binary

`static var loaded_data_binary: Dictionary`

## 描述

只读属性，从二进制文件中加载的数据。

键为用于查找数据的标识，值为对应的数据。

如果键对应的值是 `Godot` 对象，需要先访问`EncodedObjectAsID.object_id`属性来获取对象的引用 ID。

然后，使用`@GlobalScope.instance_from_id(int)`方法根据引用 ID 获取对象实例。

```gdscript
var instance_id = SaveManager.loaded_data_binary["key"].object_id
var node = instance_from_id(instance_id)
```

---

# SaveManager.loaded_data_json

`static var loaded_data_json: Dictionary`

## 描述

只读属性，从`Json`文件中加载的数据。

键为保存到`Json`文件的键名，值为对应的数据。

如果键对应的值是 `Godot` 对象，首先使用`String.to_int()` 获取其引用 ID。

然后使用`@GlobalScope.instance_from_id(int)`方法根据引用 ID 获取对象实例。

```gdscript
var instance_id = SaveManager.loaded_data_json["key"].to_int()
var node = instance_from_id(instance_id)
```

---

# SaveManager.save_path

`static var save_path: String`

## 描述

存档目录。

默认值为[`user://`的绝对路径](https://docs.godotengine.org/zh-cn/stable/tutorials/io/data_paths.html#accessing-persistent-user-data-user)，可修改为其他路径。

---

# SaveManager.create_directory

`static func create_directory(directory: String, create_at_localLow: bool = false) -> void`

## 参数

|`directory`|要创建的目录路径。|
|:---|:---|
|`create_at_localLow`|是否创建在`C:\Users\用户名\AppData\LocalLow`目录下，默认为`false`。|

## 描述

创建目录，并更改当前存档目录。

如果`create_at_localLow`为`true`则在`C:\Users\用户名\AppData\LocalLow`下创建目录，此时`directory`参数应为相对路径。

---

# SaveManager.delete_save_directory

`static func delete_save_directory() -> void`

## 描述

删除存档目录及其下所有文件。

---

# SaveManager.save_exists

`static func save_exists(file_name: String) -> bool`

## 参数

|`file_name`|要检查的存档文件名，包含扩展名。|
|:---|:---|

## 描述

检查指定文件名的存档是否存在。

## 返回

如果存档目录下存在指定存档则返回`true`，否则返回`false`。

---

# SaveManager.delete_save_file

`static func delete_save_file(file_name: String) -> void`

## 参数

|`file_name`|要删除的存档文件名，包含扩展名。|
|:---|:---|

## 描述

如果存档目录下存在指定存档文件则删除它。

---

# SaveManager.delete_all_save_files

`static func delete_all_save_files(extension: String = ".sav") -> void`

## 参数

|`extension`|要删除的存档文件扩展名，默认为`.sav`。|
|:---|:---|

## 描述

删除存档目录下所有指定拓展名的存档文件。

---

# SaveManager.get_save_files_count

`static func get_save_files_count(extension: String = ".sav") -> int`

## 参数

|`extension`|要获取数量的存档文件扩展名，默认为`.sav`。|
|:---|:---|

## 描述

获取存档目录下指定拓展名的存档文件数量。

## 返回

如果存档目录下存在指定拓展名的存档文件则返回数量，否则返回`0`。

---

# SaveManager.save_as_binary

`static func save_as_binary(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## 参数

|`data_dictionary`|要保存的数据字典，默认为[`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`|要保存的文件名，包含扩展名，默认为`SaveData.sav`。|

## 描述

保存数据为指定文件名的二进制文件。

如果`data_dictionary`为空则使用[`data_to_save`](#savemanagerdata_to_save)保存数据。

---

# SaveManager.load_from_binary

`static func load_from_binary(file_name: String = "SaveData.sav") -> void`

## 参数

|`file_name`|要读取的文件名，包含扩展名，默认为`SaveData.sav`。|
|:---|:---|

## 描述

读取指定文件名的二进制文件中的数据。

并将读取的数据保存到[`loaded_data_binary`](#savemanagerloaded_data_binary)属性中。

如果文件不存在则[`loaded_data_binary`](#savemanagerloaded_data_binary)属性为空字典。

---

# SaveManager.add_data_to_binary

`static func add_data_to_binary(file_name: String, key: String, new_data: Variant) -> void`

## 参数

|`file_name`|要添加数据的二进制文件名，包含扩展名。|
|:---|:---|
|`key`|用于查找数据的标识。|
|`new_data`|要添加的数据。|

## 描述

向指定文件名的二进制文件中添加数据。

如果文件不存在则创建文件。

---

# SaveManager.get_data_from_binary

`static func get_data_from_binary(file_name: String, key: String) -> Variant`

## 参数

|`file_name`|要获取数据的二进制文件名，包含扩展名。|
|:---|:---|
|`key`|用于查找数据的标识。|

## 描述

从指定文件名的二进制文件中获取数据。

## 返回

如果文件存在且包含指定键则返回对应数据，否则返回`null`。

---

# SaveManager.delete_data_from_binary

`static func delete_data_from_binary(file_name: String, key: String) -> void`

## 参数

|`file_name`|要删除数据的二进制文件名，包含扩展名。|
|:---|:---|
|`key`|用于查找数据的标识。|

## 描述

如果文件存在且包含指定键则从文件中删除数据。

---

# SaveManager.save_as_json

`static func save_as_json(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void`

## 参数

|`data_dictionary`|要保存的数据字典，默认为[`data_to_save`](#savemanagerdata_to_save)。|
|:---|:---|
|`file_name`|要保存的文件名，包含扩展名，默认为`SaveData.sav`。|

## 描述

保存数据为指定文件名的`Json`文件。

如果`data_dictionary`为空则使用[`data_to_save`](#savemanagerdata_to_save)保存数据。

---

# SaveManager.load_from_json

`static func load_from_json(file_name: String = "SaveData.sav") -> void`

## 参数

|`file_name`|要读取的文件名，包含扩展名，默认为`SaveData.sav`。|
|:---|:---|

## 描述

读取指定文件名的`Json`文件中的数据。

并将读取的数据保存到[`loaded_data_json`](#savemanagerloaded_data_json)属性中。

如果文件不存在则[`loaded_data_json`](#savemanagerloaded_data_json)属性为空字典。

---

# SaveManager.add_data_to_json

`static func add_data_to_json(file_name: String, key: String, new_data: Variant) -> void`

## 参数

|`file_name`|要添加数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|
|`new_data`|要添加的数据。|

## 描述

向指定文件名的`Json`文件中添加数据。

如果文件不存在则创建文件。

---

# SaveManager.get_data_from_json

`static func get_data_from_json(file_name: String, key: String, is_object: bool = false) -> Variant`

## 参数

|`file_name`|要获取数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|
|`is_object`|需要获取的值是否为`Godot`对象，默认为`false`。|

## 描述

从指定文件名的`Json`文件中获取数据。

如果`is_object`为`true`则会尝试将获取的`Json`值转换为`Godot`对象，并返回其实例。

**请在明确键对应的值是`Godot`对象时才将`is_object`设置为`true`，否则会导致转换失败，并返回`null`。**

## 返回

如果文件存在且包含指定键则返回对应数据，否则返回`null`。

当`is_object`为`true`时，若转换成功则返回`Godot`对象实例，否则返回`null`。

---

# SaveManager.delete_data_from_json

`static func delete_data_from_json(file_name: String, key: String) -> void`

## 参数

|`file_name`|要删除数据的`Json`文件名，包含扩展名。|
|:---|:---|
|`key`|保存到`Json`文件的键名。|

## 描述

如果文件存在且包含指定键则从文件中删除数据。
