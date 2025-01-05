
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace GensouLib.Godot.SaveSystem
{
    public class SaveManager
    {
        /// <summary>
        /// 需要保存的数据
        /// </summary>
        public static Dictionary<string, Variant> DataToSave { get; set; } = new();

        /// <summary>
        /// 从二进制文件中加载的数据。<br/>
        /// 如果键对应的值是 Godot 对象，需要先将其转换为 <see cref="EncodedObjectAsId"/> 类型。<br/>
        /// 然后访问 <see cref="EncodedObjectAsId.ObjectId"/> 属性来获取对象的引用 ID。<br/>
        /// 最后，使用 <see cref="GodotObject.InstanceFromId(ulong)"/> 方法根据引用 ID 获取对象实例。<br/>
        /// 示例：<br/>
        /// <code>
        /// EncodedObjectAsId encodedObjectAsId = (EncodedObjectAsId)loadedData["MyObject"];
        /// Node myObject = (Node)GodotObject.InstanceFromId(encodedObjectAsId.ObjectId);
        /// </code>
        /// </summary>
        public static Dictionary<string, Variant> LoadedDataBinary { get => _LoadedDataBinary;}

        private static Dictionary<string, Variant> _LoadedDataBinary = new();

        private static Dictionary<string, Variant> _TempDataBinary = new();

        /// <summary>
        /// 从JSON文件中加载的数据
        /// </summary>
        public static  Dictionary<string, Variant> LoadedDataJson { get => _LoadedDataJson;}

        private static Dictionary<string, Variant> _LoadedDataJson = new();

        private static Dictionary<string, Variant> _TempDataJson = new();

        /// <summary>
        /// 保存路径
        /// </summary>
        public static string SavePath { get; set; } = ProjectSettings.GlobalizePath("user://");

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directory">
        /// 目录路径
        /// </param>
        /// <param name="createAtLocalLow">
        /// 是否创建在LocalLow目录下
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
        /// 删除存档目录
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
        /// 存档文件是否存在
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 如果存在返回true，否则返回false
        /// </returns>
        public static bool SaveExists(string fileName)
        {
            return FileAccess.FileExists(SavePath.PathJoin(fileName));
        }

        /// <summary>
        /// 删除指定存档文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void DeleteSaveFile(string fileName)
        {
            if (FileAccess.FileExists(SavePath.PathJoin(fileName)))
            {
                DirAccess.RemoveAbsolute(SavePath.PathJoin(fileName));
            }
        }

        /// <summary>
        /// 删除所有存档文件
        /// </summary>
        /// <param name="extension">
        /// 扩展名
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
        /// 获取存档文件数量
        /// </summary>
        /// <param name="extension">
        /// 扩展名
        /// </param>
        /// <returns>
        /// 存档路径下的文件数量，如果不存在则返回 0
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
        /// 保存为二进制文件
        /// </summary>
        /// <param name="data">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void SaveAsBinary(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Write);
            file.StoreVar(dataDictionary);
        }

        /// <summary>
        /// 加载二进制文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 若文件存在则返回数据，否则返回空字典
        /// </returns>
        public static Dictionary<string, Variant> LoadFromBinary(string fileName = "SaveData.sav")
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
            return _LoadedDataBinary;
        }

        /// <summary>
        /// 向二进制文件中添加数据
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
        /// </param>
        /// <param name="newData">
        /// 新数据
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
        /// 从二进制文件中获取数据
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值
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
        /// 从二进制文件中删除数据
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
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
        /// 清空加载的二进制数据
        /// </summary>
        public static void ClearLoadedDataBinary()
        {
            _LoadedDataBinary.Clear();
        }

        /// <summary>
        /// 保存为Json文件
        /// </summary>
        /// <param name="data">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void SaveAsJson(Dictionary<string, Variant> dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Write);
            file.StoreString(Json.Stringify(dataDictionary, "\t"));
        }

        /// <summary>
        /// 加载Json文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 若文件存在则返回数据，否则返回空字典
        /// </returns>
        public static Dictionary<string, Variant> LoadFromJson(string fileName = "SaveData.sav")
        {
            _TempDataJson.Clear();
            using var file = FileAccess.Open(SavePath.PathJoin(fileName), FileAccess.ModeFlags.Read);
            if (file == null)
            {
                _LoadedDataJson.Clear();
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
            return _LoadedDataJson;
        }

        /// <summary>
        /// 向Json文件中添加数据
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
        /// </param>
        /// <param name="newData">
        /// 新数据
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
        /// 从Json文件中获取数据
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值
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
                
                Json json = new();
                if (json.Parse(value.AsString()) == Error.Ok)
                {
                    _TempDataJson.Clear();
                    return json.Data.As<T>();
                }
                else if (typeof(T) == typeof(string))
                {
                    _TempDataJson.Clear();
                    return value.As<T>();
                }
            }
            _TempDataJson.Clear();
            return default;
        }

        /// <summary>
        /// 从Json文件中删除数据
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="key">
        /// 键
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

        /// <summary>
        /// 清空加载的Json数据
        /// </summary>        
        public static void ClearLoadedDataJson()
        {
            _LoadedDataJson.Clear();
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