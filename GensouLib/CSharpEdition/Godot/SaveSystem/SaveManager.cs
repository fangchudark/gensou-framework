
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace GensouLib.Godot.SaveSystem
{
    public class SaveManager
    {
        /// <summary>
        /// 需要保存的数据 <br/>
        /// The data to save.
        /// </summary>
        public static Dictionary<string, Variant> DataToSave { get; set; } = new();

        /// <summary>
        /// 从二进制文件中加载的数据。<br/>
        /// The data loaded from a binary file.<br/>
        /// 如果键对应的值是 Godot 对象，需要先将其转换为 <see cref="EncodedObjectAsId"/> 类型。<br/>
        /// If the value corresponding to the key is a Godot object, convert it to the <see cref="EncodedObjectAsId"/> type.<br/>
        /// 然后访问 <see cref="EncodedObjectAsId.ObjectId"/> 属性来获取对象的引用 ID。<br/>
        /// Then access the <see cref="EncodedObjectAsId.ObjectId"/> property to retrieve the object reference ID.<br/>
        /// 最后，使用 <see cref="GodotObject.InstanceFromId(ulong)"/> 方法根据引用 ID 获取对象实例。<br/>
        /// Finally, use the <see cref="GodotObject.InstanceFromId(ulong)"/> method to get the object instance from the reference ID.<br/>
        /// 示例：<br/>
        /// Example:<br/>
        /// <code>
        /// EncodedObjectAsId encodedObjectAsId = (EncodedObjectAsId)SaveManager.LoadedDataBinary["MyObject"];
        /// Node myObject = (Node)GodotObject.InstanceFromId(encodedObjectAsId.ObjectId);
        /// </code>
        /// </summary>
        public static Dictionary<string, Variant> LoadedDataBinary { get => _LoadedDataBinary;}

        private static Dictionary<string, Variant> _LoadedDataBinary = new();

        private static Dictionary<string, Variant> _TempDataBinary = new();

        /// <summary>
        /// 从 JSON 文件中加载的数据。<br/>
        /// The data loaded from a JSON file.<br/>
        /// 如果键对应的值是 Godot 对象，首先使用 <see cref="Variant.AsUInt64()"/> 获取其引用 ID。<br/>
        /// If the value corresponding to the key is a Godot object, use <see cref="Variant.AsUInt64()"/> to retrieve its reference ID.<br/>
        /// 然后使用 <see cref="GodotObject.InstanceFromId(ulong)"/> 方法根据引用 ID 获取对象实例。<br/>
        /// Then use the <see cref="GodotObject.InstanceFromId(ulong)"/> method to get the object instance from the reference ID.<br/>
        /// 示例：<br/>
        /// Example:<br/>
        /// <code>
        /// ulong objectId = SaveManager.LoadedDataJson["MyObject"].AsUInt64();
        /// Node myObject = (Node)GodotObject.InstanceFromId(objectId);
        /// </code>
        /// </summary>
        public static  Dictionary<string, Variant> LoadedDataJson { get => _LoadedDataJson;}

        private static Dictionary<string, Variant> _LoadedDataJson = new();

        private static Dictionary<string, Variant> _TempDataJson = new();

        /// <summary>
        /// 保存路径 <br/>
        /// The path to save the data.
        /// </summary>
        public static string SavePath { get; set; } = ProjectSettings.GlobalizePath("user://");

        /// <summary>
        /// 创建目录 <br/>
        /// Create a directory.
        /// </summary>
        /// <param name="directory">
        /// 目录路径 <br/>
        /// The directory path.
        /// </param>
        /// <param name="createAtLocalLow">
        /// 是否创建在LocalLow目录下 <br/>
        /// Whether to create the directory at the LocalLow directory.
        /// </param>
        public static void CreateDirectory(string directory, bool createAtLocalLow = false)
        {
            if (createAtLocalLow)
            {
                string appDataLocalLow = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) +"Low";
                directory = appDataLocalLow.Replace('\\', '/').PathJoin(directory);
            }
            
            if (!DirAccess.DirExistsAbsolute(directory))
            {
                DirAccess.MakeDirRecursiveAbsolute(directory);
                SavePath = directory;
            }
        }

        /// <summary> 
        /// 删除存档目录 <br/>
        /// Delete the save directory
        /// </summary>  
        public static void DeleteSaveDirectory()
        {
            if (DirAccess.DirExistsAbsolute(SavePath))
            {
                DeleteDirectoryRecursive(SavePath);
            }
        }

        private static void DeleteDirectoryRecursive(string path)
        {
            // 跳过 Godot 重要目录：日志、着色器缓存、vulkan
            if (path == ProjectSettings.GlobalizePath("user://logs") ||
                path == ProjectSettings.GlobalizePath("user://shader_cache") ||
                path == ProjectSettings.GlobalizePath("user://vulkan"))
            {
                return;
            }

            foreach (string directory in DirAccess.GetDirectoriesAt(path))
            {
                DeleteDirectoryRecursive(path.PathJoin(directory));  // 递归删除子目录
            }
            foreach (string file in DirAccess.GetFilesAt(path))
            {
                DirAccess.RemoveAbsolute(path.PathJoin(file));
            }
            DirAccess.RemoveAbsolute(path);
        }

        /// <summary>
        /// 存档文件是否存在 <br/>
        /// Whether the save file exists.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <returns>
        /// 如果存在返回true，否则返回false <br/>
        /// Returns true if the file exists, false otherwise
        /// </returns>
        public static bool SaveExists(string fileName)
        {
            return FileAccess.FileExists(SavePath.PathJoin(fileName));
        }

        /// <summary>
        /// 删除指定存档文件 <br/>
        /// Delete the specified save file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        public static void DeleteSaveFile(string fileName)
        {
            if (FileAccess.FileExists(SavePath.PathJoin(fileName)))
            {
                DirAccess.RemoveAbsolute(SavePath.PathJoin(fileName));
            }
        }

        /// <summary>
        /// 删除所有存档文件 <br/>
        /// Delete all save files.
        /// </summary>
        /// <param name="extension">
        /// 扩展名 <br/>
        /// The extension.
        /// </param>
        public static void DeleteAllSaveFiles(string extension = ".sav")
        {
            foreach (string file in DirAccess.GetFilesAt(SavePath))
            {
                if (file.EndsWith(extension))
                {
                    DirAccess.RemoveAbsolute(SavePath.PathJoin(file));
                }
            }
        }

        /// <summary>
        /// 获取存档文件数量 <br/>
        /// Get the number of save files.
        /// </summary>
        /// <param name="extension">
        /// 扩展名 <br/>
        /// The extension.
        /// </param>
        /// <returns>
        /// 存档路径下的文件数量，如果不存在则返回0 <br/>
        /// The number of files in the save path, 0 if it does not exist.
        /// </returns>
        public static int GetSaveFilesCount(string extension = ".sav")
        {
            int count = 0;
            foreach (string file in DirAccess.GetFilesAt(SavePath))
            {
                if (file.EndsWith(extension))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 保存为二进制文件 <br/>
        /// Save as a binary file.
        /// </summary>
        /// <param name="data">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/> <br/>
        /// The data to save, <c>null</c> means using <see cref="DataToSave"/>.
        /// </param>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        public static void SaveAsBinary(Variant? data = null, string fileName = "SaveData.sav")
        {
            data ??= DataToSave;
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Write);
            file.StoreVar((Variant)data);
        }

        /// <summary>
        /// 加载二进制文件 <br/>
        /// Load a binary file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        public static void LoadFromBinary(string fileName = "SaveData.sav")
        {
            _TempDataBinary.Clear();
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Read);
            if (file != null)
            {
                _LoadedDataBinary = (Dictionary<string, Variant>)file.GetVar();
                _TempDataBinary = _LoadedDataBinary;
            }
            else
            {
                _LoadedDataBinary.Clear();
            }
        }

        /// <summary>
        /// 向二进制文件中添加数据 <br/>
        /// Add data to a binary file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        /// <param name="newData">
        /// 新数据 <br/>
        /// The new data.
        /// </param>
        public static void AddDataToBinary(string fileName, string key, Variant newData)
        {
            if (_LoadedDataBinary.Count == 0 || !AreDictionariesContentEqual(_LoadedDataBinary, _TempDataBinary))
            {
                LoadFromBinary(fileName);
            }
            if (_TempDataBinary.ContainsKey(key))
            {
                _TempDataBinary[key] = newData;
            }
            else
            {
                _TempDataBinary.Add(key, newData);
            }
            SaveAsBinary(_TempDataBinary, fileName);
            _TempDataBinary.Clear();
        }

        /// <summary>
        /// 从二进制文件中获取数据 <br/>
        /// Get data from a binary file.
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型 <br/>
        /// The data type
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值 <br/>
        /// If the data exists, return the data, otherwise return the default value.
        /// </returns>
        public static T GetDataFromBinary<[MustBeVariant] T>(string fileName, string key)
        {
            if (_LoadedDataBinary.Count == 0 || !AreDictionariesContentEqual(_LoadedDataBinary, _TempDataBinary))
            {
                LoadFromBinary(fileName);
            }

            if (_TempDataBinary.ContainsKey(key))
            {
                Variant value = _TempDataBinary[key];
                if (typeof(GodotObject).IsAssignableFrom(typeof(T)))
                {
                    EncodedObjectAsId objectAsId = (EncodedObjectAsId)value;
                    _TempDataBinary.Clear();
                    return (T)(object)GodotObject.InstanceFromId(objectAsId.ObjectId);
                }
                T data = value.As<T>();

                if (data != null)
                {
                    _TempDataBinary.Clear();
                    return data;
                }
            }
            _TempDataBinary.Clear();
            return default;
        }

        /// <summary>
        /// 从二进制文件中删除数据 <br/>
        /// Delete data from a binary file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        public static void DeleteDataFromBinary(string fileName, string key)
        {
            if (_LoadedDataBinary.Count == 0 || !AreDictionariesContentEqual(_LoadedDataBinary, _TempDataBinary))
            {
                LoadFromBinary(fileName);
            }

            if (_TempDataBinary.ContainsKey(key))
            {
                _TempDataBinary.Remove(key);
                SaveAsBinary(_TempDataBinary, fileName);
            }

            _TempDataBinary.Clear();
        }

        /// <summary>
        /// 保存为Json文件 <br/>
        /// Save as a Json file.
        /// </summary>
        /// <param name="data">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/> <br/>
        /// The data to save, <c>null</c> means using <see cref="DataToSave"/>.
        /// </param>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        public static void SaveAsJson(Variant? data = null, string fileName = "SaveData.sav")
        {
            data ??= DataToSave;
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Write);
            file.StoreString(Json.Stringify((Variant)data, "\t"));
        }

        /// <summary>
        /// 加载Json文件 <br/>
        /// Load a Json file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        public static void LoadFromJson(string fileName = "SaveData.sav")
        {
            _TempDataJson.Clear();
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Read);
            if (file == null)
            {
                _LoadedDataJson.Clear();
                return;
            }
        
            Json json = new();
            Error result = json.Parse(file.GetAsText());
            if (result == Error.Ok)
            {
                _LoadedDataJson = (Dictionary<string, Variant>)json.Data;
                _TempDataJson = _LoadedDataJson;
            }
            else
            {
                _LoadedDataJson.Clear();
            }
        }

        /// <summary>
        /// 向Json文件中添加数据 <br/>
        /// Add data to a Json file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        /// <param name="newData">
        /// 新数据 <br/>
        /// The new data.
        /// </param>
        public static void AddDataToJson(string fileName, string key, Variant newData)
        {
            if (_LoadedDataJson.Count == 0 || !AreDictionariesContentEqual(_LoadedDataJson, _TempDataJson))
            {
                LoadFromJson(fileName);
            }
            if (_TempDataJson.ContainsKey(key))
            {
                _TempDataJson[key] = newData;
            }
            else
            {
                _TempDataJson.Add(key, newData);
            }
            SaveAsJson(_TempDataJson, fileName);
            _TempDataJson.Clear();
        }

        /// <summary>
        /// 从Json文件中获取数据 <br/>
        /// Get data from a Json file.
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型 <br/>
        /// The data type
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值 <br/>
        /// If the data exists, return the data, otherwise return the default value.
        /// </returns>
        public static T GetDataFromJson<[MustBeVariant] T>(string fileName, string key)
        {
            if (_LoadedDataJson.Count == 0 || !AreDictionariesContentEqual(_LoadedDataJson, _TempDataJson))
            {
                LoadFromJson(fileName);
            }
            if (_TempDataJson.ContainsKey(key))
            {
                Variant value = _TempDataJson[key];
                if (typeof(GodotObject).IsAssignableFrom(typeof(T)))
                {
                    _TempDataJson.Clear();
                    return (T)(object)GodotObject.InstanceFromId(value.AsUInt64());
                }
                T data = value.As<T>();
                if (data != null)
                {
                    _TempDataJson.Clear();
                    return data;
                }
            }
            _TempDataJson.Clear();
            return default;
        }

        /// <summary>
        /// 从Json文件中删除数据 <br/>
        /// Delete data from a Json file.
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name.
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key.
        /// </param>
        public static void DeleteDataFromJson(string fileName, string key)
        {
            if (_LoadedDataJson.Count == 0 || !AreDictionariesContentEqual(_LoadedDataJson, _TempDataJson))
            {
                LoadFromJson(fileName);
            }
            if (_TempDataJson.ContainsKey(key))
            {
                _TempDataJson.Remove(key);
                SaveAsJson(_TempDataJson, fileName);
            }

            _TempDataJson.Clear();
        }

        private static bool AreDictionariesContentEqual(Dictionary<string, Variant> dic1, Dictionary<string, Variant> dic2)
        {
            if (dic1.Count != dic2.Count)
            {
                return false;
            }

            return dic1.OrderBy(kv => kv.Key).SequenceEqual(dic2.OrderBy(kv => kv.Key));
        }

    }
}