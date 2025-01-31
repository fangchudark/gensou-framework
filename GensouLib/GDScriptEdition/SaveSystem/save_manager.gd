## 存档管理器
class_name SaveManager extends Object

## 需要保存的数据
static var data_to_save: Dictionary = {}

## 从二进制文件中加载的数据。[br]
## 如果键对应的值是 Godot 对象，需要访问 [member EncodedObjectAsID.object_id] 属性来获取对象的引用 ID。[br]
## 然后，使用 [method @GlobalScope.instance_from_id] 方法根据引用 ID 获取对象实例。[br]
## [br]
## 示例： [br]
## [codeblock]
## var instance_id = SaveManager.loaded_data_binary["key"].object_id
## var node = instance_form_id(instance_id)
## [/codeblock]
static var loaded_data_binary: Dictionary = {}:
    get:
        return _loaded_data_binary
        
static var _loaded_data_binary: Dictionary = {}

static var _temp_data_binary: Dictionary = {}

## 从JSON文件中加载的数据 [br]
## 如果键对应的值是 Godot 对象，首先使用 [method String.to_int] 获取其引用 ID。[br]
## 然后，使用 [method @GlobalScope.instance_from_id] 方法根据引用 ID 获取对象实例。[br]
## [br]
## 示例： [br]
## [codeblock]
## var instance_id = SaveManager.loaded_data_json["key"].to_int()
## var node = instance_form_id(instance_id)
## [/codeblock]
static var loaded_data_json: Dictionary = {}:
    get:
        return _loaded_data_json

static var _loaded_data_json: Dictionary = {}

static var _temp_data_json: Dictionary = {}

## 保存路径
static var save_path: String = ProjectSettings.globalize_path("user://")

static func _static_init() -> void:
    loaded_data_binary.make_read_only()
    loaded_data_json.make_read_only()

## 创建目录 [br]
## [br]
## [param directory] : [br]
## 目录路径 [br]
## [param create_at_localLow] : [br]
## 是否创建在LocalLow目录下
static func create_directory(directory: String, create_at_localLow: bool = false) -> void:
    if create_at_localLow:
        var app_data_localLow = OS.get_environment("localappdata") + "Low"
        directory = app_data_localLow.replace("\\", "/").path_join(directory)
    if not DirAccess.dir_exists_absolute(directory):
        DirAccess.make_dir_recursive_absolute(directory)
        save_path = directory

## 删除存档目录
static func delete_save_directory() -> void:
    if DirAccess.dir_exists_absolute(save_path):
        _delete_directory_recursive(save_path)

static func _delete_directory_recursive(path: String) -> void:
    if (
        path == ProjectSettings.globalize_path("user://logs") or 
        path == ProjectSettings.globalize_path("user://shader_cache") or 
        path == ProjectSettings.globalize_path("user://vulkan")
    ):
        return
    for directory: String in DirAccess.get_directories_at(path):
        _delete_directory_recursive(path.path_join(directory))
    for file: String in DirAccess.get_files_at(path):
        DirAccess.remove_absolute(path.path_join(file))
    DirAccess.remove_absolute(path)

## 存档文件是否存在 
## [br]
## [param file_name] : [br]
## 文件名 [br]
## [br]
## 如果文件存在返回[code]true[/code]，否则返回[code]false[/code]
static func save_exists(file_name: String) -> bool:
    return FileAccess.file_exists(save_path.path_join(file_name))

## 删除指定存档文件
## [br]
## [param file_name] : [br]
## 文件名
static func delete_save_file(file_name: String) -> void:
    if FileAccess.file_exists(save_path.path_join(file_name)):
        DirAccess.remove_absolute(save_path.path_join(file_name))

## 删除所有存档文件 [br]
## [br]
## [param file_extension] : [br]
## 扩展名
static func delete_all_save_files(extension: String = ".sav") -> void:
    for file: String in DirAccess.get_files_at(save_path):
        if file.ends_with(extension):
            DirAccess.remove_absolute(save_path.path_join(file))

## 获取存档文件数量 [br]
## [br]
## [param extension] : [br]
## 扩展名 [br]
## [br]
## 返回存档路径下的文件数量，如果不存在则返回0 
static func get_save_files_count(extension: String = ".sav") -> int:
    var count = 0
    for file: String in DirAccess.get_files_at(save_path):
        if file.ends_with(extension):
            count += 1
    return count

## 保存为二进制文件 [br]
## [br]
## [param data_dictionary] : [br]
## 需要保存的数据字典，默认为[member data_to_save] [br]
## [param file_name] : [br]
## 文件名
static func save_as_binary(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void:
    var file = FileAccess.open(save_path.path_join(file_name), FileAccess.WRITE)
    file.store_var(data_dictionary)
    file.close()

## 加载二进制文件 [br]
## Load a binary file. [br]
## [br]
## [param file_name] : [br]
## 文件名
static func load_from_binary(file_name: String = "SaveData.sav") -> Dictionary:
    _temp_data_binary.clear()
    var file = FileAccess.open(save_path.path_join(file_name), FileAccess.READ)
    if file:
        _loaded_data_binary = file.get_var()
        _temp_data_binary = _loaded_data_binary
        file.close()
    else:
        _loaded_data_binary.clear()
    return _loaded_data_binary

## 向二进制文件中添加数据 [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## [param key] : [br]
## 键 [br]
## [param new_data] : [br]
## 新数据
static func add_data_to_binary(file_name: String, key: String, new_data: Variant) -> void:
    if _loaded_data_binary.size() == 0 or _loaded_data_binary != _temp_data_binary:
        load_from_binary(file_name)
    _temp_data_binary[key] = new_data
    save_as_binary(_temp_data_binary, file_name)
    _temp_data_binary.clear()

## 从二进制文件中获取数据 [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## [param key] : [br]
## 键 [br]
## [br]
## 返回指定键的值，如果不存在则返回[code]null[/code]
static func get_data_from_binary(file_name: String, key: String) -> Variant:
    if _loaded_data_binary.size() == 0 or _loaded_data_binary != _temp_data_binary:
        load_from_binary(file_name)
    if _temp_data_binary.has(key):
        var data = _temp_data_binary[key]
        _temp_data_binary.clear()
        if data is Object:
            return instance_from_id(data.object_id)       
        return data
    _temp_data_binary.clear()    
    return null

## 从二进制文件中删除数据 [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## [param key] : [br]
## 键
static func delete_data_from_binary(file_name: String, key: String) -> void:
    if _loaded_data_binary.size() == 0 or _loaded_data_binary != _temp_data_binary:
        load_from_binary(file_name)
    if _temp_data_binary.has(key):
        _temp_data_binary.erase(key)
        save_as_binary(_temp_data_binary, file_name)
    _temp_data_binary.clear()

## 清空加载的二进制数据
static func clear_loaded_binary_data() -> void:
    _loaded_data_binary.clear()

## 保存为JSON文件 [br]
## [br]
## [param data_dictionary] : [br]
## 需要保存的数据字典，默认为[member data_to_save] [br]
## [param file_name] : [br]
## 文件名
static func save_as_json(data_dictionary: Dictionary = data_to_save, file_name: String = "SaveData.sav") -> void:
    var file = FileAccess.open(save_path.path_join(file_name), FileAccess.WRITE)
    file.store_string(JSON.stringify(data_dictionary, "\t"))
    file.close()

## 加载JSON文件 [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
static func load_from_json(file_name: String = "SaveData.sav") -> Dictionary:
    _temp_data_json.clear()
    var file = FileAccess.open(save_path.path_join(file_name), FileAccess.READ)
    if not file:
        _loaded_data_json.clear()   

    var json: JSON = JSON.new()
    var result: Error = json.parse(file.get_as_text())
    if result == OK:
        _loaded_data_json = json.data
        _temp_data_json = _loaded_data_json
    else:
        _loaded_data_json.clear()
    file.close()
    return _loaded_data_json

## 向JSON文件中添加数据 [br]
## Add data to a JSON file. [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## The file name. [br]
## [param key] : [br]
## 键 [br]
## The key. [br]
## [param new_data] : [br]
## 新数据 [br]
## The new data.
static func add_data_to_json(file_name: String, key: String, new_data: Variant) -> void:
    if _loaded_data_json.size() == 0 or _loaded_data_json != _temp_data_json:
        load_from_json(file_name)
    _temp_data_json[key] = new_data
    save_as_json(_temp_data_json, file_name)
    _temp_data_json.clear()

## 从JSON文件中获取数据 [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## [param key] : [br]
## 键 [br]
## [param is_object] : [br]
## 需要获取的值是否为Godot对象 [br]
## [br]
## 返回指定键的值，如果不存在则返回[code]null[/code]。 
static func get_data_from_json(file_name: String, key: String, is_object: bool = false) -> Variant:
    if _loaded_data_json.size() == 0 or _loaded_data_json != _temp_data_json:
        load_from_json(file_name)
    if _temp_data_json.has(key):
        var data = _temp_data_json[key]
        if is_object:
            _temp_data_json.clear()
            return instance_from_id(data.to_int())
        
        var json: JSON = JSON.new()
        if json.parse(str(data)) == OK:
            _temp_data_json.clear()
            return json.data
        elif data is String:
            _temp_data_json.clear()
            return data
    _temp_data_json.clear()    
    return null

## 从JSON文件中删除数据 [br]
## Delete data from a JSON file. [br]
## [br]
## [param file_name] : [br]
## 文件名 [br]
## The file name. [br]
## [param key] : [br]
## 键 [br]
## The key.
static func delete_data_from_json(file_name: String, key: String) -> void:
    if _loaded_data_json.size() == 0 or _loaded_data_json != _temp_data_json:
        load_from_json(file_name)
    if _temp_data_json.has(key):
        _temp_data_json.erase(key)
        save_as_json(_temp_data_json, file_name)
    _temp_data_json.clear()

## 清空加载的Json数据
static func clear_loaded_json_data() -> void:
    _loaded_data_json.clear()
