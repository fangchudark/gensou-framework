using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

#if ENABLE_JSONNET
using Newtonsoft.Json;
#endif

namespace GensouLib.Unity.SaveSystem
{
    /// <summary>
    /// 存档管理器 <br/>
    /// Save manager
    /// </summary>
    /// <remarks>
    /// 该类用于管理存档文件，包括读取、保存、删除等操作。<br/>
    /// This class is used to manage save files, including reading, saving, and deleting operations.<br/>
    /// 默认使用二进制格式保存数据。如果需要使用JSON格式保存数据，请在框架设置中开启它。<br/>
    /// The default format used to save data is binary. If you need to use JSON format, please enable it in the framework settings.<br/>
    /// 不支持序列化Unity对象。请将需要保存的数据提取出来并存入字典中。<br/>
    /// Does not support serialization of Unity objects. Please extract the data to be saved out of the object and put it into a dictionary.<br/>
    /// </remarks>
    public class SaveManager
    {
        /// <summary> 
        /// 需要保存的数据 <br/>
        /// The data to save
        /// </summary>
        public static Dictionary<string, object> DataToSave { get; set; } = new();

        /// <summary>
        /// 从二进制文件中加载的数据 <br/>
        /// The data loaded from a binary file.
        /// </summary>
        public static Dictionary<string, object> LoadedDataBinary { get => _LoadedDataBinary;}

        private static Dictionary<string, object> _LoadedDataBinary = new();

        private static Dictionary<string, object> _TempDataBinary = new();

#if ENABLE_JSONNET
        /// <summary>
        /// 从 JSON 文件中加载的数据。<br/>
        /// The data loaded from a JSON file.<br/>
        /// 如果键对应的值是包含多个属性的类或字典，请将其转换为 <see cref="JObject"/> 类型，然后使用 <see cref="JObject.Properties"/> 方法访问其属性。<br/>
        /// If the value corresponding to the key is a class or dictionary containing multiple properties, convert it to a <see cref="JObject"/> type and access its properties using the <see cref="JObject.Properties"/> method.<br/>
        /// 示例：<br/>
        /// Example:
        /// <code>
        /// JObject jObject = (JObject)SaveManager.LoadedDataJson["key"];
        /// foreach (var property in jObject.Properties())
        /// {
        ///     Debug.Log(property.Name + " : " + property.Value);
        /// }
        /// </code>
        /// 如果键对应的值是数组或列表，请将其转换为 <see cref="JArray"/> 类型，然后使用 <see cref="JArray.Children"/> 方法访问其元素。<br/>
        /// If the value corresponding to the key is an array or list, convert it to a <see cref="JArray"/> type and use the <see cref="JArray.Children"/> method to access its elements.<br/>
        /// 示例：<br/>
        /// Example:
        /// <code>
        /// JArray jArray = (JArray)SaveManager.LoadedDataJson["key"];
        /// foreach (var item in jArray.Children())
        /// {
        ///     Debug.Log(item.ToString());
        /// }
        /// </code>
        /// </summary>
        public static Dictionary<string, object> LoadedDataJson { get => _LoadedDataJson; }

        private static Dictionary<string, object> _LoadedDataJson = new();

        private static Dictionary<string, object> _TempDataJson = new();
#endif

        /// <summary> 
        /// 保存路径 <br/>
        /// The path to save the data 
        /// </summary>
        public static string SavePath { get; set; } = Application.persistentDataPath;

        /// <summary> 
        /// 创建目录 <br/>
        /// Create a directory
        /// </summary>
        /// <param name="directory">
        /// 目录路径 <br/>
        /// The directory path
        ///</param>
        /// <param name="creatAtLocalLow">
        /// 是否创建在LocalLow目录下 <br/>
        /// Whether to create the directory in the LocalLow directory
        ///</param>
        public static void CreatDirectory(string directory, bool creatAtLocalLow = false)
        {
            if (!Directory.Exists(directory))
            {
                if (creatAtLocalLow)
                {
                    string appDataLocalLow = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "Low";
                    directory = Path.Combine(appDataLocalLow, directory);
                }
                Directory.CreateDirectory(directory);
                SavePath = directory;
            }
        }
        
        /// <summary> 
        /// 删除存档目录 <br/>
        /// Delete the save directory
        /// </summary>  
        public static void DeleteSaveDirectory()
        {
            if (Directory.Exists(SavePath))
            {
                Directory.Delete(SavePath, true);
            }
        }

        /// <summary> 
        /// 存档文件是否存在 <br/>
        /// Whether the data exists
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <returns>
        /// 如果存在返回true，否则返回false <br/>
        /// Returns true if the file exists, false otherwise
        /// </returns>
        public static bool SaveExists(string fileName)
        {
            return File.Exists(Path.Combine(SavePath, fileName));
        }

        /// <summary> 
        /// 删除指定存档文件 <br/>
        /// Delete a specified save file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void DeleteSaveFile(string fileName)
        {
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                File.Delete(Path.Combine(SavePath, fileName));
            }
        }

        /// <summary> 
        /// 删除所有存档文件 <br/>
        /// Delete all save files
        /// </summary>
        /// <param name="extension">
        /// 扩展名 <br/>
        /// The extension
        /// </param>
        public static void DeleteAllSaveFiles(string extension = ".sav")
        {
            foreach (string file in Directory.GetFiles(SavePath))
            {
                if (file.EndsWith(extension))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary> 
        /// 获取存档文件数量 <br/>
        /// Get the number of save files
        /// </summary>
        /// <param name="extension">
        /// 扩展名 <br/>
        /// The extension
        /// </param>
        /// <returns>
        /// 存档路径下的文件数量，如果不存在则返回0 <br/>
        /// The number of files in the save path, 0 if it does not exist.
        /// </returns>
        public static int GetSaveFileCount(string extension = ".sav")
        {
            int count = 0;
            foreach (string file in Directory.GetFiles(SavePath))
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
        /// Save as a binary file
        /// </summary>
        /// <param name="dataDictionary">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/> <br/>
        /// The data to save, <c>null</c> means using <see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void SaveAsBinary(object dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            using FileStream fileStream = new(Path.Combine(SavePath, fileName), FileMode.Create);
            BinaryFormatter binaryFormatter = new();
            binaryFormatter.Serialize(fileStream, dataDictionary);
        }

        /// <summary> 
        /// 加载二进制文件 <br/>
        /// Load a binary file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void LoadFromBinary(string fileName = "SaveData.sav")
        {
            _TempDataBinary.Clear();
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                using FileStream fileStream = new(Path.Combine(SavePath, fileName), FileMode.Open);
                BinaryFormatter binaryFormatter = new();
                _LoadedDataBinary = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
                _TempDataBinary = _LoadedDataBinary;
            }
            else
            {
                _LoadedDataBinary.Clear(); // 清空字典表示未读取到数据
            }
        }

        /// <summary> 
        /// 向二进制文件中添加数据 <br/>
        /// Add data to a binary file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
        /// </param>
        /// <param name="newData">
        /// 新数据 <br/>
        /// The new data
        /// </param>
        public static void AddDataToBinary(string fileName, string key, object newData)
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
        /// Get data from a binary file
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型 <br/>
        /// The data type
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值 <br/>
        /// If the data exists, return the data, otherwise return the default value.
        /// </returns>
        public static T GetDataFromBinary<T>(string fileName, string key)
        {
            if (_LoadedDataBinary.Count == 0 || !AreDictionariesContentEqual(_LoadedDataBinary, _TempDataBinary))
            {
                LoadFromBinary(fileName);
            }
            
            if (_TempDataBinary.ContainsKey(key))
            {
                T data = (T)_TempDataBinary[key];
                _TempDataBinary.Clear();
                return data;
            }
            _TempDataBinary.Clear();
            return default;
        }

        /// <summary> 
        /// 从二进制文件中删除数据 <br/>
        /// Delete data from a binary file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
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
        private static bool AreDictionariesContentEqual(Dictionary<string, object> dic1, Dictionary<string, object> dic2)
        {
            if (dic1.Count != dic2.Count)
            {
                return false;
            }

            return dic1.OrderBy(kv => kv.Key).SequenceEqual(dic2.OrderBy(kv => kv.Key));
        }
#if ENABLE_JSONNET

        /// <summary> 
        /// 保存为Json文件 <br/>
        /// Save as a Json file
        /// </summary>
        /// <param name="dataDictionary">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/> <br/>
        /// The data to save, <c>null</c> means using <see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void SaveAsJson(object dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(dataDictionary, settings);

            File.WriteAllText(Path.Combine(SavePath, fileName), json);
        }

        /// <summary> 
        /// 加载Json文件 <br/>
        /// Load a Json file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void LoadFromJson(string fileName = "SaveData.sav")
        {
            _TempDataJson.Clear();
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                string json = File.ReadAllText(Path.Combine(SavePath, fileName));
                _LoadedDataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                _TempDataJson = _LoadedDataJson;
            }
            else
            {
                _LoadedDataJson.Clear(); // 清空字典表示未读取到数据
            }
        }

        /// <summary> 
        /// 向Json文件中添加数据 <br/>
        /// Add data to a Json file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
        /// </param>
        /// <param name="newData">
        /// 新数据 <br/>
        /// The new data
        /// </param>
        public static void AddDataToJson(string fileName, string key, object newData)
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
        /// Get data from a Json file <br/>
        /// 只能获取Json能够写入的单一类型，如字符串、数字(long, double)、布尔值。<br/>
        /// Can only get single-type data that can be written in Json, such as strings, numbers(long, double), and booleans.<br/>
        /// 不能获取集合、Unity对象和自定义类。<br/>
        /// Cannot get collections, Unity objects, and custom classes.<br/>
        /// 如果需要获取，请通过访问<see cref="LoadedDataJson"/>属性获取。<br/>
        /// If you need to get it, please access the <see cref="LoadedDataJson"/> property.<br/>
        /// </summary>
        /// <typeparam name="T">
        /// 数据类型 <br/>
        /// The data type
        /// </typeparam>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回默认值 <br/>
        /// If the data exists, return the data, otherwise return the default value.
        /// </returns>
        public static T GetDataFromJson<T>(string fileName, string key)
        {
            if (_LoadedDataJson.Count == 0 || !AreDictionariesContentEqual(_LoadedDataJson, _TempDataJson))
            {
                LoadFromJson(fileName);
            }

            if (_TempDataJson.ContainsKey(key))
            {
                var value = _TempDataJson[key];

                if (value is T data)
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
        /// Delete data from a Json file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        /// <param name="key">
        /// 键 <br/>
        /// The key
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
        
#endif        

    }
}