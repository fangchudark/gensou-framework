using System;
using Godot.Collections;
using Godot;
using System.Threading.Tasks;
using GensouLib.Godot.SaveSystem;
using GensouLib.GenScript.Interpreters;
using GensouLib.GenScript;
using GensouLib.Godot.Audio;

namespace GensouLib.Godot.Core
{
    /// <summary>
    /// 保存/加载游戏
    /// </summary>
    public partial class SaveLoadGame : VisualNoveCore
    {
        /// <summary>
        /// 最大存档槽位数
        /// </summary>
        public static int MaxSlots { get; set; } = 10;

        /// <summary>
        /// 存档槽位场景
        /// </summary>
        public static PackedScene SaveSlotScene { get; set; }
    
        /// <summary>
        /// 关闭存档界面的按钮
        /// </summary>    
        public static Button CloseButton { get; set; }

        /// <summary>
        /// 存档界面的标题
        /// </summary>
        public static Label PanelTitle { get; set; }

        /// <summary>
        /// 存档槽位容器
        /// </summary>
        public static VBoxContainer SaveSlotContainer { get; set; }

        /// <summary>
        /// 是否是保存游戏
        /// </summary>
        public static bool IsSave { get; set; } = false;

        /// <summary>
        /// 显示时间戳的节点路径
        /// </summary>
        public static string TimestampNodePath { get; set; } = "Timestamp";

        /// <summary>
        /// 显示当前对话文本的节点路径
        /// </summary>
        public static string DialogueNodePath { get; set; } = "Dialogue";

        /// <summary>
        /// 显示当前截图的节点路径
        /// </summary>
        public static string ScreenShotNodePath { get; set; } = "ScreenShot";
        
        /// <summary>
        /// 存档/加载界面根节点
        /// </summary>
        public static Node SaveLoadSceneRootNode { get; set; }
        
        private static readonly Dictionary<string, Variant> Data = new();
    
        private static System.Collections.Generic.List<Button> SaveSlots { get; set; } = new();
    
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="saveSlotScene">存档槽位场景</param>
        /// <param name="closeButton">关闭存档界面的按钮</param>
        /// <param name="panelTitle">存档界面的标题</param>
        /// <param name="saveSlotContainer">存档槽位容器</param>
        /// <param name="maxSlots">最大存档槽位数</param>
        /// <param name="timestampNodePath">显示时间戳的节点路径</param>
        /// <param name="dialogueNodePath">显示当前对话文本的节点路径</param>
        /// <param name="screenShotNodePath">显示当前截图的节点路径</param>
        public static void Init(
            PackedScene saveSlotScene,
            Button closeButton,
            Label panelTitle,
            VBoxContainer saveSlotContainer,
            int maxSlots = 10,
            string timestampNodePath = "Timestamp",
            string dialogueNodePath = "Dialogue",
            string screenShotNodePath = "screenShot"
        )
        {
            SaveSlotScene = saveSlotScene;
            CloseButton = closeButton;
            PanelTitle = panelTitle;
            SaveSlotContainer = saveSlotContainer;
            MaxSlots = maxSlots;
            TimestampNodePath = timestampNodePath;
            DialogueNodePath = dialogueNodePath;
            ScreenShotNodePath = screenShotNodePath;
            if (CloseButton!= null)
                CloseButton.Pressed += CloseMenu;
        }

        /// <summary>
        /// 创建存档槽位
        /// </summary>
        /// <returns>异步任务</returns>
        public static async Task CreateSlots()
        {
            PanelTitle.Text = IsSave? "Save Game" : "Load Game";
            SaveSlots.Clear();
            for (int i = 0; i < MaxSlots; i++)
            {
                Button saveSlot = SaveSlotScene.Instantiate<Button>();
                SaveSlotContainer.AddChild(saveSlot); // 添加到容器
                SaveSlots.Add(saveSlot); // 添加到列表
                int slotIndex = i;
                if (IsSave) saveSlot.Pressed += () => SaveGame(slotIndex); // 绑定保存事件
                else saveSlot.Pressed += () => LoadGame(slotIndex); // 绑定加载事件
                ReadSaves(slotIndex); // 读取存档位信息
                await SaveLoadSceneRootNode.ToSignal(SaveLoadSceneRootNode.GetTree(), "process_frame");
            }
        }

        private static void ReadSaves(int slotIndex)
        {
            string saveName = "save" + slotIndex + ".json";
            if (SaveManager.SaveExists(saveName))
            {
                Button slot = SaveSlots[slotIndex];
                TextureRect screenshot = slot.GetNode<TextureRect>(ScreenShotNodePath);
                Label timestamp = slot.GetNode<Label>(TimestampNodePath);
                Label dialogue = slot.GetNode<Label>(DialogueNodePath);
                screenshot.Texture = ScreenshotToTextureRect.LoadScreenshotFormBytes( // 加载截图并赋值
                                        SaveManager.GetDataFromJson<byte[]>(saveName, "screenshot") // 从json文件中读取截图数据
                                    );
                timestamp.Text = SaveManager.GetDataFromJson<string>(saveName, "timestamp"); // 从json文件中读取时间戳
                dialogue.Text = SaveManager.GetDataFromJson<string>(saveName, "currentDialogue"); // 从json文件中读取对话内容
            }
        }

        private static void SaveGame(int slotIndex)
        {
            Button slot = SaveSlots[slotIndex]; // 获取槽位对象
            TextureRect screenshot = slot.GetNode<TextureRect>(ScreenShotNodePath); // 获取截图对象
            screenshot.Texture = ScreenshotToTextureRect.Screenshot; // 赋值截图
            Label timestamp = slot.GetNode<Label>(TimestampNodePath); // 获取时间戳对象
            timestamp.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 赋值时间戳
            Label dialogue = slot.GetNode<Label>(DialogueNodePath); // 获取对话信息对象
            dialogue.Text = DialogueInterpreter.CurrentDialogue; // 赋值对话信息
        
            // 需要存储的数据：脚本的变量、截图、脚本的当前执行行总行数等信息、对话框的文本内容、主场景音频、立绘等
            Data.Add("slotIndex", slotIndex); // 存档位索引

            byte[] screenshotBytes = ScreenshotToTextureRect.GetScreenshotBytes(); // 获取截图字节数据
        
            Data.Add("screenshot", screenshotBytes); // 存档位截图数据

            Data.Add("timestamp", timestamp.Text); // 存档位时间戳

            Data.Add("currentScript", ScriptReader.CurrentScriptName); // 当前脚本名

            Data.Add("currentLine", BaseInterpreter.CurrentLine - 1); // 当前执行行索引
        
            Data.Add("scriptVariables", VariableInterpreter.GetLocalVariablesGD()); // 脚本变量
        
            Data.Add("currentDialogue", DialogueInterpreter.CurrentDialogue); // 当前对话内容
        
            Data.Add("currentSpeaker", DialogueInterpreter.CurrentSpeaker); // 当前对话人物名
        
            Data.Add("onChoosing", ChoiceInterpreter.OnChoosing); // 是否处于选择状态
        
            if (AudioManager.BgmPlayer.Stream == null) Data.Add("currentBgm", ""); // 背景音乐
            else Data.Add("currentBgm", AudioManager.BgmPlayer.Stream.ResourcePath.GetFile());
        
            if (AudioManager.BgsPlayer.Stream == null) Data.Add("currentBgs", ""); // 背景音乐
            else Data.Add("currentBgs", AudioManager.BgsPlayer.Stream.ResourcePath.GetFile());

            if (AudioManager.SePlayer.Stream == null) Data.Add("currentSe", ""); // 音效
            else Data.Add("currentSe", AudioManager.SePlayer.Stream.ResourcePath.GetFile());

            if (AudioManager.VoicePlayer.Stream == null) Data.Add("currentVoice", ""); // 语音
            else Data.Add("currentVoice", AudioManager.VoicePlayer.Stream.ResourcePath.GetFile());

            if (FigureLeft.Texture == null) Data.Add("leftFigure", ""); // 立绘
            else Data.Add("leftFigure", FigureLeft.Texture.ResourcePath.GetFile());

            if (FigureRight.Texture == null) Data.Add("rightFigure", ""); // 立绘
            else Data.Add("rightFigure", FigureRight.Texture.ResourcePath.GetFile());

            if (FigureCenter.Texture == null) Data.Add("centerFigure", ""); // 立绘
            else Data.Add("centerFigure", FigureCenter.Texture.ResourcePath.GetFile());
        
            if (Portrait.Texture == null) Data.Add("portrait", ""); // 肖像
            else Data.Add("portrait", Portrait.Texture.ResourcePath.GetFile());

            if (Background.Texture == null) Data.Add("background", ""); // 背景
            else Data.Add("background", Background.Texture.ResourcePath.GetFile());

            // 保存数据到json文件
            SaveManager.SaveAsJson(Data, "save" + slotIndex + ".json");
            SaveManager.SaveAsJson(VariableInterpreter.GetGlobalVariablesGD(), "globalVariables.json");
            Data.Clear();
        }

        private static void LoadGame(int slotIndex)
        {
            string saveName = "save" + slotIndex + ".json";

            if (SaveManager.SaveExists(saveName))
            {
                // 读取数据
                if (SaveLoadSceneRootNode.GetTree().CurrentScene.SceneFilePath != MainScenePath) // 不是主场景
                {
                    SaveLoadSceneRootNode.GetTree().CurrentScene.QueueFree(); // 销毁当前场景
                    PackedScene scene = ResourceLoader.Load<PackedScene>(MainScenePath); // 加载主场景
                    Node node = scene.Instantiate(); // 实例化主场景
                    SaveLoadSceneRootNode.GetTree().Root.AddChild(node); // 添加到根节点
                    SaveLoadSceneRootNode.GetTree().CurrentScene = node; // 设置为当前场景
                    RecoverData(saveName);
                    BaseInterpreter.ExecuteNextLine();
                    SaveLoadUiActive = false;
                    SaveLoadSceneRootNode.QueueFree();
                }
                else // 是主场景
                {
                    RecoverData(saveName);
                    SaveLoadUiActive = false;
                    SaveLoadSceneRootNode.QueueFree();
                }
            }
        }

        /// <summary>
        /// 关闭存档界面
        /// </summary>
        public static void CloseMenu()
        {
            SaveLoadUiActive = false;
            SaveLoadSceneRootNode.QueueFree();
        }
        
        /// <summary>
        /// 保存游戏配置
        /// </summary>
        public static void SaveConfig()
        {
            Dictionary<string, Variant> config = new()
            {
                { "masterVolume", AudioManager.MasterVolume},
                { "bgmVolume", AudioManager.BgmVolume},
                { "bgsVolume", AudioManager.BgsVolume},
                { "seVolume", AudioManager.SeVolume},
                { "voiceVolume", AudioManager.VoiceVolume},
                { "autoPlaySpeed", AutoPlayInterval},
                { "textDisplaySpeed", TextDisplaySpeed},
                { "displayMode", DisplayServer.WindowGetMode(0).ToString()}
            };    
            SaveManager.SaveAsJson(config, "config.json"); // 保存配置文件
        }
        
        /// <summary>
        /// 加载游戏配置
        /// </summary>
        public static void LoadConfig()
        {
            string fileNmae = "config.json";
            if (SaveManager.SaveExists(fileNmae)) // 如果配置文件存在
            {
                Dictionary<string, Variant> config = SaveManager.LoadFromJson(fileNmae); // 读取配置文件
                if (config.ContainsKey("masterVolume")) AudioManager.MasterVolume = config["masterVolume"].AsSingle(); // 设置音量
                if (config.ContainsKey("bgmVolume")) AudioManager.BgmVolume = config["bgmVolume"].AsSingle();
                if (config.ContainsKey("bgsVolume")) AudioManager.BgsVolume = config["bgsVolume"].AsSingle();
                if (config.ContainsKey("seVolume")) AudioManager.SeVolume = config["seVolume"].AsSingle();
                if (config.ContainsKey("voiceVolume")) AudioManager.VoiceVolume = config["voiceVolume"].AsSingle();
                if (config.ContainsKey("autoPlaySpeed")) AutoPlayInterval = config["autoPlaySpeed"].AsInt32();
                if (config.ContainsKey("textDisplaySpeed")) TextDisplaySpeed = config["textDisplaySpeed"].AsSingle();
                if (config.ContainsKey("displayMode"))
                {
                    string mode = config["displayMode"].ToString();
                    DisplayServer.WindowSetMode((DisplayServer.WindowMode)Enum.Parse(typeof(DisplayServer.WindowMode), mode));
                }
            }
        }

        /// <summary>
        /// 打开存档/加载界面
        /// </summary>
        /// <param name="node">父节点</param>
        /// <param name="scenePath">存档/加载界面场景路径</param>
        public static void ShowSaveLoadMenu(Node node, string scenePath)
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
            SaveLoadSceneRootNode = scene.Instantiate();
            node.AddChild(SaveLoadSceneRootNode);
            SaveLoadUiActive = true;
        }
    }
}