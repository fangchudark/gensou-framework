using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

#if ENABLE_JSONNET
using Newtonsoft.Json;
#endif

namespace GensouLib.Unity.SaveSystem
{
    /// <summary>
    /// 存档管理器
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
        /// 已读取的数据 <br/>
        /// The data that has been read
        /// </summary>
        public static Dictionary<string, object> LoadedData { get => _LoadedData; }

        private static Dictionary<string, object> _LoadedData = new();

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
        /// 读取二进制文件 <br/>
        /// Read a binary file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void LoadFromBinary(string fileName = "SaveData.sav")
        {
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                using FileStream fileStream = new(Path.Combine(SavePath, fileName), FileMode.Open);
                BinaryFormatter binaryFormatter = new();
                _LoadedData = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
            }
            else
            {
                _LoadedData.Clear(); // 清空字典表示未读取到数据
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
            LoadFromBinary(fileName);
            if (_LoadedData.ContainsKey(key))
            {
                _LoadedData[key] = newData;
            }
            else
            {
                _LoadedData.Add(key, newData);
            }
            SaveAsBinary(_LoadedData, fileName);
            _LoadedData.Clear();
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
        public static T GetDataFromBinary<T>(string fileName, string key)
        {
            LoadFromBinary(fileName);

            if (_LoadedData.ContainsKey(key))
            {
                T data = (T)_LoadedData[key];
                _LoadedData.Clear();
                return data;
            }
            _LoadedData.Clear();
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
            LoadFromBinary(fileName);

            if (_LoadedData.ContainsKey(key))
            {
                _LoadedData.Remove(key);
                SaveAsBinary(_LoadedData, fileName);
            }
            
            _LoadedData.Clear();
            
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
        /// 读取Json文件 <br/>
        /// Read a Json file
        /// </summary>
        /// <param name="fileName">
        /// 文件名 <br/>
        /// The file name
        /// </param>
        public static void LoadFromJson(string fileName = "SaveData.sav")
        {
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                string json = File.ReadAllText(Path.Combine(SavePath, fileName));
                _LoadedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            else
            {
                _LoadedData.Clear(); // 清空字典表示未读取到数据
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
            LoadFromJson(fileName);
            if (_LoadedData.ContainsKey(key))
            {
                _LoadedData[key] = newData;
            }
            else
            {
                _LoadedData.Add(key, newData);
            }
            SaveAsJson(_LoadedData, fileName);
            _LoadedData.Clear();
        }

        /// <summary> 
        /// 从Json文件中获取数据 <br/>
        /// Get data from a Json file
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
        public static T GetDataFromJson<T>(string fileName, string key)
        {
            LoadFromJson(fileName);

            if (_LoadedData.ContainsKey(key))
            {
                T data = (T)_LoadedData[key];
                _LoadedData.Clear();
                return data;
            }
            _LoadedData.Clear();
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
            LoadFromJson(fileName);

            if (_LoadedData.ContainsKey(key))
            {
                _LoadedData.Remove(key);
                SaveAsJson(_LoadedData, fileName);
            }
            
            _LoadedData.Clear();
            
        }
        
#endif        

    }
}