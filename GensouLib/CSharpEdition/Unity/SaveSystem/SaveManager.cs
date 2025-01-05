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
    /// 存档管理器
    /// </summary>
    public class SaveManager
    {
        /// <summary> 
        /// 需要保存的数据
        /// </summary>
        public static Dictionary<string, object> DataToSave { get; set; } = new();

        /// <summary>
        /// 从二进制文件中加载的数据
        /// </summary>
        public static Dictionary<string, object> LoadedDataBinary { get => _LoadedDataBinary;}

        private static Dictionary<string, object> _LoadedDataBinary = new();

        private static Dictionary<string, object> _TempDataBinary = new();

#if ENABLE_JSONNET
        /// <summary>
        /// 从 JSON 文件中加载的数据。<br/>
        /// 如果键对应的值是包含多个属性的类或字典，请将其转换为 <see cref="JObject"/> 类型，然后使用 <see cref="JObject.Properties"/> 方法访问其属性。
        /// 示例：
        /// <code>
        /// JObject jObject = (JObject)LoadedDataJson["key"];
        /// foreach (var property in jObject.Properties())
        /// {
        ///     Debug.Log(property.Name + " : " + property.Value);
        /// }
        /// </code>
        /// 如果键对应的值是数组或列表，请将其转换为 <see cref="JArray"/> 类型，然后使用 <see cref="JArray.Children"/> 方法访问其元素。<br/>
        /// 示例：
        /// <code>
        /// JArray jArray = (JArray)LoadedDataJson["key"];
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
        /// 保存路径
        /// </summary>
        public static string SavePath { get; set; } = Application.persistentDataPath;

        /// <summary> 
        /// 创建目录
        /// </summary>
        /// <param name="directory">
        /// 目录路径
        ///</param>
        /// <param name="creatAtLocalLow">
        /// 是否创建在LocalLow目录下
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
        /// 删除存档目录
        /// </summary>  
        public static void DeleteSaveDirectory()
        {
            if (Directory.Exists(SavePath))
            {
                Directory.Delete(SavePath, true);
            }
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
            return File.Exists(Path.Combine(SavePath, fileName));
        }

        /// <summary> 
        /// 删除指定存档文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void DeleteSaveFile(string fileName)
        {
            if (File.Exists(Path.Combine(SavePath, fileName)))
            {
                File.Delete(Path.Combine(SavePath, fileName));
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
            foreach (string file in Directory.GetFiles(SavePath))
            {
                if (file.EndsWith(extension))
                {
                    File.Delete(file);
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
        /// 存档路径下的文件数量，如果不存在则返回0
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
        /// 保存为二进制文件
        /// </summary>
        /// <param name="dataDictionary">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void SaveAsBinary(Dictionary<string, object> dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            using FileStream fileStream = new(Path.Combine(SavePath, fileName), FileMode.Create);
            BinaryFormatter binaryFormatter = new();
            binaryFormatter.Serialize(fileStream, dataDictionary);
        }

        /// <summary> 
        /// 加载二进制文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回空字典
        /// </returns>
        public static Dictionary<string, object> LoadFromBinary(string fileName = "SaveData.sav")
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
        /// 保存为Json文件
        /// </summary>
        /// <param name="dataDictionary">
        /// 需要保存的数据，为<c>null</c>时使用<see cref="DataToSave"/>
        /// </param>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        public static void SaveAsJson(Dictionary<string, object> dataDictionary = null, string fileName = "SaveData.sav")
        {
            dataDictionary ??= DataToSave;
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(dataDictionary, settings);

            File.WriteAllText(Path.Combine(SavePath, fileName), json);
        }

        /// <summary> 
        /// 加载Json文件
        /// </summary>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 若存在则返回数据，否则返回空字典
        /// </returns>
        public static Dictionary<string, object> LoadFromJson(string fileName = "SaveData.sav")
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
        /// 从Json文件中获取数据
        /// 能够获取以下类型的值:
        /// <list type="bullet">
        /// <item>Json支持的简单值(long, string, double, bool)</item>
        /// <item>数组(List)</item>
        /// <item>字典(Dictionary)</item>
        /// </list>
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

                // 处理从 JSON 获取的集合（List<T>）
                if (value is Newtonsoft.Json.Linq.JArray array && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    // 获取集合元素的类型
                    Type itemType = typeof(T).GetGenericArguments()[0];

                    // 使用 JArray 的 ToObject 方法将 JSON 数组转为对应的 List<T> 类型
                    var list = array.ToObject(typeof(List<>).MakeGenericType(itemType));

                    _TempDataJson.Clear();
                    return (T)list;
                }

                // 处理字典类型（Dictionary<TKey, TValue>）
                if (value is Newtonsoft.Json.Linq.JObject obj && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type[] keyValueTypes = typeof(T).GetGenericArguments();
                    var dictionary = obj.ToObject(typeof(Dictionary<,>).MakeGenericType(keyValueTypes[0], keyValueTypes[1]));

                    _TempDataJson.Clear();
                    return (T)dictionary;
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

#endif        

    }
}