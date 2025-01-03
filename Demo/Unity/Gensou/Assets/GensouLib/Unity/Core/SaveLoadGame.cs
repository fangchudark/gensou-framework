using System.Collections.Generic;
using GensouLib.GenScript.Interpreters;
using GensouLib.Unity.SaveSystem;
using GensouLib.Unity.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using GensouLib.GenScript;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 保存/加载游戏
    /// </summary>
    public class SaveLoadGame : VisualNoveCore
    {
        /// <summary>
        /// 最大存档槽位数
        /// </summary>
        public static int MaxSolts { get; set; } = 10;
        
        /// <summary>
        /// 存档槽位预制体
        /// </summary>
        public static GameObject SaveSlotPrefab { get; set; }
        
        /// <summary>
        /// 关闭存档界面的按钮
        /// </summary>
        public static Button CloseButton { get; set; }
        
        /// <summary>
        /// 存档界面的标题
        /// </summary>
        public static TextMeshProUGUI PanelTitle { get; set; }
        
        /// <summary>
        /// 存档槽位容器
        /// </summary>
        public static RectTransform SaveSlotContainer { get; set; }

        /// <summary>
        /// 是否是保存游戏
        /// </summary>
        public static bool IsSave { get; set; } = false;

        /// <summary>
        /// 显示时间戳的游戏对象名
        /// </summary>
        public static string TimestampGameObjectName { get; set; } = "Timestamp";
        
        /// <summary>
        /// 显示当前对话文本的游戏对象名
        /// </summary>
        public static string DialogueGameObjectName { get; set; } = "Dialogue";
        
        /// <summary>
        /// 显示当前截图的游戏对象名
        /// </summary>
        public static string ScreenShotGameObjectName { get; set; } = "Screenshot";

        private static readonly Dictionary<string, object> Data  = new();

        private static List<GameObject> SaveSlots { get; set; } = new();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="saveSlotPrefab">存档槽位预制体</param>
        /// <param name="closeButton">关闭存档界面的按钮</param>
        /// <param name="panelTitle">存档界面的标题</param>
        /// <param name="saveSlotContainer">存档槽位容器</param>
        /// <param name="maxSolts">最大存档槽位数</param>
        /// <param name="timestampGameObjectName">显示时间戳的游戏对象名</param>
        /// <param name="dialogueGameObjectName">显示当前对话文本的游戏对象名</param>
        /// <param name="screenShotGameObjectName">显示当前截图的游戏对象名</param>
        public static void Init(
            GameObject saveSlotPrefab,
            Button closeButton,
            TextMeshProUGUI panelTitle,
            RectTransform saveSlotContainer,
            int maxSolts = 10,
            string timestampGameObjectName = "Timestamp",
            string dialogueGameObjectName = "Dialogue",
            string screenShotGameObjectName = "Screenshot"
        )
        {
            SaveSlotPrefab = saveSlotPrefab;
            CloseButton = closeButton;
            PanelTitle = panelTitle;
            SaveSlotContainer = saveSlotContainer;
            MaxSolts = maxSolts;
            TimestampGameObjectName = timestampGameObjectName;
            DialogueGameObjectName = dialogueGameObjectName;
            ScreenShotGameObjectName = screenShotGameObjectName;
            if (CloseButton!= null) CloseButton.onClick.AddListener(CloseMenu);
        }

        /// <summary>
        /// 创建存档槽位
        /// </summary>
        /// <returns>异步任务</returns>
        public static async Task CreateSolts()
        {
            PanelTitle.text = IsSave? "Save Game" : "Load Game";
            foreach (GameObject saveSlot in SaveSlots)
            {
                Destroy(saveSlot);
                await Task.Yield();
            }
            SaveSlots.Clear();
            for (int i = 0; i < MaxSolts; i++)
            {
                GameObject saveSlot = Instantiate(SaveSlotPrefab, SaveSlotContainer); // 创建槽位对象
                SaveSlots.Add(saveSlot); // 添加到槽位列表
                int slotIndex = i; // 保存槽位索引
                RawImage screenshot = saveSlot.transform.Find(ScreenShotGameObjectName).GetComponent<RawImage>(); // 获取截图对象
                if (IsSave) saveSlot.GetComponent<Button>().onClick.AddListener(() => SaveGame(slotIndex, screenshot)); // 绑定槽位按钮事件
                else saveSlot.GetComponent<Button>().onClick.AddListener(() => LoadGame(slotIndex)); // 绑定槽位按钮事件
                ReadSaves(
                    slotIndex, 
                    screenshot, 
                    saveSlot.transform.Find(TimestampGameObjectName).GetComponent<TextMeshProUGUI>(), 
                    saveSlot.transform.Find(DialogueGameObjectName).GetComponent<TextMeshProUGUI>()
                ); // 读取存档位信息
                await Task.Yield();
            }
        }

        private static void ReadSaves(int slotIndex, RawImage screenshot, TextMeshProUGUI time, TextMeshProUGUI dialogue)
        {

#if ENABLE_JSONNET == false
            string saveName = "save" + slotIndex + ".sav";
#else
            string saveName = "save" + slotIndex + ".json";
#endif
            if (SaveManager.SaveExists(saveName))
            {
#if ENABLE_JSONNET == false
                screenshot.texture = ScreenshotToRawImage.LoadScreenshotFormBytes( // 加载截图并赋值给RawImage
                                        SaveManager.GetDataFromBinary<byte[]>(saveName, "screenshot") // 从json文件中读取截图数据
                                     );
                time.text = SaveManager.GetDataFromBinary<string>(saveName, "timestamp"); // 从json文件中读取时间戳
                dialogue.text = SaveManager.GetDataFromBinary<string>(saveName, "currentDialogue"); // 从json文件中读取对话框内容
#else
                screenshot.texture = ScreenshotToRawImage.LoadScreenshotFormBytes( // 加载截图并赋值给RawImage
                                        Convert.FromBase64String( // 将Base64字符串转换为字节数组
                                            SaveManager.GetDataFromJson<string>(saveName, "screenshot") // 从json文件中读取截图数据
                                        )
                                     );
                time.text = SaveManager.GetDataFromJson<string>(saveName, "timestamp"); // 从json文件中读取时间戳
                dialogue.text = SaveManager.GetDataFromJson<string>(saveName, "currentDialogue"); // 从json文件中读取对话框内容
#endif
            }
        }

        private static void SaveGame(int slotIndex, RawImage screenshot)
        {
            screenshot.texture = ScreenshotToRawImage.Screenshot; // 获取截图并赋值给RawImage
            GameObject solt = SaveSlots[slotIndex]; // 获取槽位对象
            TextMeshProUGUI time = solt.transform.Find(TimestampGameObjectName).GetComponent<TextMeshProUGUI>(); // 获取时间戳对象
            time.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 设置时间戳
            TextMeshProUGUI dialogue = solt.transform.Find(DialogueGameObjectName).GetComponent<TextMeshProUGUI>(); // 获取对话框信息对象
            dialogue.text = DialogueInterpreter.CurrentDialogue; // 设置对话框内容

            // 需要存储的数据：脚本的变量、截图、脚本的当前执行行总行数等信息、对话框的文本内容、主场景音频、立绘等
            Data.Add("slotIndex", slotIndex); // 保存槽位索引

            byte[] screenshotBytes = ScreenshotToRawImage.Screenshot.EncodeToPNG(); // 截图转换为字节数组

            Data.Add("screenshot", screenshotBytes); // 保存截图

            Data.Add("timestamp", time.text ); // 保存时间戳

            Data.Add("currentScript", ScriptReader.CurrentScriptName); // 保存当前脚本名

            Data.Add("currentLine", BaseInterpreter.CurrentLine - 1); // 保存当前执行行索引

            Data.Add("scriptVariables", VariableInterpreter.GetLocalVariables()); // 保存脚本的变量

            Data.Add("currentDialogue", DialogueInterpreter.CurrentDialogue); // 保存对话框内容

            Data.Add("currentSpeaker", DialogueInterpreter.CurrentSpeaker); // 保存当前对话者

            Data.Add("OnChoosing", ChoiceInterpreter.OnChoosing); // 保存是否处于选择状态

            if (AudioManager.BgmClip == null) Data.Add("currentBgm", ""); // 保存当前BGM
            else Data.Add("currentBgm", AudioManager.BgmClip.name);

            if (AudioManager.BgsClip == null) Data.Add("currentBgs", ""); // 保存当前BGS
            else Data.Add("currentBgs", AudioManager.BgsClip.name);

            if (AudioManager.SeClip == null) Data.Add("currentSe", ""); // 保存当前SE
            else Data.Add("currentSe", AudioManager.SeClip.name);

            if (AudioManager.VoiceClip == null) Data.Add("currentVoice", ""); // 保存当前语音
            else Data.Add("currentVoice", AudioManager.VoiceClip.name);

            if (FigureLeft.sprite == null) Data.Add("leftFigure", ""); // 保存左侧立绘
            else Data.Add("leftFigure", FigureLeft.sprite.name);

            if (FigureRight.sprite == null) Data.Add("rightFigure", ""); // 保存右侧立绘
            else Data.Add("rightFigure", FigureRight.sprite.name);

            if (FigureCenter.sprite == null) Data.Add("centerFigure", ""); // 保存中心立绘
            else Data.Add("centerFigure", FigureCenter.sprite.name);

            if (Portrait.sprite == null) Data.Add("portrait", ""); // 保存小头像
            else Data.Add("portrait", Portrait.sprite.name);

            if (Background.sprite == null) Data.Add("background", ""); // 保存背景
            else Data.Add("background", Background.sprite.name);

            
#if ENABLE_JSONNET == false
            SaveManager.SaveAsBinary(Data, "save" + slotIndex + ".sav"); // 保存存档文件
            SaveManager.SaveAsBinary(VariableInterpreter.GetGlobalVariables(), "globalVariables.sav"); // 保存全局变量文件
#else
            SaveManager.SaveAsJson(Data, "save" + slotIndex + ".json"); // 保存存档文件
            SaveManager.SaveAsJson(VariableInterpreter.GetGlobalVariables(), "globalVariables.json"); // 保存全局变量文件
#endif
            Data.Clear(); // 清空数据字典
        }

        private static void LoadGame(int slotIndex)
        {
#if ENABLE_JSONNET == false
            string saveName = "save" + slotIndex + ".sav"; // 存档文件名
#else
            string saveName = "save" + slotIndex + ".json"; // 存档文件名
#endif
            if (SaveManager.SaveExists(saveName)) // 如果存档文件存在
            {
                if (SceneManager.GetActiveScene().name != MainScene) // 如果当前场景不是主场景
                {
                    SceneManager.LoadSceneAsync(MainScene).completed += _ =>  // 加载主场景
                    {
                        RecoverData(saveName); // 加载存档数据
                        BaseInterpreter.ExecuteNextLine(); // 继续执行脚本
                    };
                }
                else
                {
                    SceneManager.UnloadSceneAsync(SaveLoadScene).completed += _ => // 卸载存档场景
                    {
                        SaveLoadUiActive = false; // 关闭存档界面
                        RecoverData(saveName); // 加载存档数据
                        BaseInterpreter.ExecuteNextLine(); // 继续执行脚本
                    };
                }
                
            }
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        public static void SaveConfig()
        {
            Dictionary<string, object> config = new()
            {
                { "masterVolume", AudioManager.MasterVolume},
                { "bgmVolume", AudioManager.BgmVolume / AudioManager.MasterVolume},
                { "bgsVolume", AudioManager.BgsVolume / AudioManager.MasterVolume},
                { "seVolume", AudioManager.SeVolume / AudioManager.MasterVolume},
                { "voiceVolume", AudioManager.VoiceVolume / AudioManager.MasterVolume},
                { "autoPlaySpeed", AutoPlayInterval},
                { "textDisplaySpeed", TextDisplaySpeed},
                { "displayMode", Screen.fullScreenMode.ToString()}
            };

#if ENABLE_JSONNET == false
            SaveManager.SaveAsBinary(config, "config.sav"); // 保存配置文件
#else
            SaveManager.SaveAsJson(config, "config.json"); // 保存配置文件
#endif
        }

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        public static void LoadConfig()
        {
#if ENABLE_JSONNET == false
            string fileNmae = "config.sav";
#else
            string fileNmae = "config.json";
#endif
            if (SaveManager.SaveExists(fileNmae)) // 如果配置文件存在
            {
#if ENABLE_JSONNET == false
                Dictionary<string, object> config = SaveManager.LoadFromBinary(fileNmae); // 读取配置文件
                if (config.ContainsKey("masterVolume")) AudioManager.MasterVolume = Convert.ToSingle(config["masterVolume"]); // 设置音量
                if (config.ContainsKey("bgmVolume")) AudioManager.BgmVolume = Convert.ToSingle(config["bgmVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("bgsVolume")) AudioManager.BgsVolume = Convert.ToSingle(config["bgsVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("seVolume")) AudioManager.SeVolume = Convert.ToSingle(config["seVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("voiceVolume")) AudioManager.VoiceVolume = Convert.ToSingle(config["voiceVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("autoPlaySpeed")) AutoPlayInterval = Convert.ToInt32(config["autoPlaySpeed"]);
                if (config.ContainsKey("textDisplaySpeed")) Typewriter.waitngSeconds = Convert.ToSingle(config["textDisplaySpeed"]);
                if (config.ContainsKey("displayMode"))
                {
                    string mode = config["displayMode"].ToString();
                    Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), mode);
                }
#else
                Dictionary<string, object> config = SaveManager.LoadFromJson(fileNmae); // 读取配置文件
                if (config.ContainsKey("masterVolume")) AudioManager.MasterVolume = Convert.ToSingle(config["masterVolume"]); // 设置音量
                if (config.ContainsKey("bgmVolume")) AudioManager.BgmVolume = Convert.ToSingle(config["bgmVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("bgsVolume")) AudioManager.BgsVolume = Convert.ToSingle(config["bgsVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("seVolume")) AudioManager.SeVolume = Convert.ToSingle(config["seVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("voiceVolume")) AudioManager.VoiceVolume = Convert.ToSingle(config["voiceVolume"]) * AudioManager.MasterVolume;
                if (config.ContainsKey("autoPlaySpeed")) AutoPlayInterval = Convert.ToInt32(config["autoPlaySpeed"]);
                if (config.ContainsKey("textDisplaySpeed")) TextDisplaySpeed = Convert.ToSingle(config["textDisplaySpeed"]);
                if (config.ContainsKey("displayMode"))
                {
                    string mode = config["displayMode"].ToString();
                    Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), mode);
                }
#endif
            }
        }

        /// <summary>
        /// 关闭存档界面
        /// </summary>
        public static void CloseMenu()
        {
            SceneManager.UnloadSceneAsync(SaveLoadScene).completed += _ =>
            {
                SaveLoadUiActive = false;
            };
        }
    }
}