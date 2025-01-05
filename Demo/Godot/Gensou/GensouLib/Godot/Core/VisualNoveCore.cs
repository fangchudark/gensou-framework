using GensouLib.GenScript;
using GensouLib.GenScript.Interpreters;
using GensouLib.Godot.Audio;
using GensouLib.Godot.SaveSystem;
using Godot;
using Godot.Collections;
using System.Linq;

namespace GensouLib.Godot.Core
{
    /// <summary>
    /// 核心功能
    /// </summary>
    public partial class VisualNoveCore : Node
    {
#region property
        /// <summary>
        /// 显示角色名的标签
        /// </summary>
        public static Label CharaterName {get; set;}

        /// <summary>
        /// 显示对话的标签
        /// </summary>
        public static Label DialogueText {get; set;}
        
        /// <summary>
        /// 显示左侧立绘的TextureRect
        /// </summary>
        public static TextureRect FigureLeft {get; set;}
        
        /// <summary>
        /// 显示中间立绘的TextureRect
        /// </summary>
        public static TextureRect FigureCenter {get; set;}

        /// <summary>
        /// 显示右侧立绘的TextureRect
        /// </summary>
        public static TextureRect FigureRight {get; set;}

        /// <summary>
        /// 显示头像的TextureRect
        /// </summary>
        public static TextureRect Portrait {get; set;}

        /// <summary>
        /// 显示背景的TextureRect
        /// </summary>
        public static TextureRect Background {get; set;}

        /// <summary>
        /// 任一处于节点树的节点
        /// </summary>
        public static Node GameManagerNode {get; set;}

        /// <summary>
        /// BGM播放器
        /// </summary>
        public static AudioStreamPlayer Bgm {get => AudioManager.BgmPlayer; set => AudioManager.BgmPlayer = value;}
        
        /// <summary>
        /// BGS播放器
        /// </summary>
        public static AudioStreamPlayer Bgs {get => AudioManager.BgsPlayer; set => AudioManager.BgsPlayer = value;}
        
        /// <summary>
        /// 音效播放器
        /// </summary>
        public static AudioStreamPlayer Se {get => AudioManager.SePlayer; set => AudioManager.SePlayer = value;}
        
        /// <summary>
        /// 语音播放器
        /// </summary>
        public static AudioStreamPlayer Voice {get => AudioManager.VoicePlayer; set => AudioManager.VoicePlayer = value;}
        
        /// <summary>
        /// 选择按钮的容器
        /// </summary>
        public static VBoxContainer ChoiceButtonContainer {get; set;}
        
        /// <summary>
        /// 选择按钮的场景
        /// </summary>
        public static PackedScene ChoiceButtonScene {get; set;}
        
        /// <summary>
        /// 对话框
        /// </summary>
        public static Panel TextBox {get; set;}
        
        /// <summary>
        /// 标题界面场景路径
        /// </summary>
        public static string TitleScenePath {get; set;} = "res://Scenes/Title.tscn";
        
        /// <summary>
        /// 存档界面场景路径
        /// </summary>
        public static string SaveLoadScenePath {get; set;} = "res://Scenes/SaveLoad.tscn";
        
        /// <summary>
        /// 主界面场景路径
        /// </summary>
        public static string MainScenePath {get; set;} = "res://Scenes/Main.tscn";
        
        /// <summary>
        /// 系统设置界面场景路径
        /// </summary>
        public static string ConfigScnenPath {get; set;} = "res://Scenes/Config.tscn";
        
        /// <summary>
        /// 立绘路径
        /// </summary>
        public static string FigurePath {get; set;} = "res://Assets/Figure/";
        
        /// <summary>
        /// 头像路径
        /// </summary>
        public static string PortraitPath {get; set;} = "res://Assets/Portrait/";
        
        /// <summary>
        /// 背景路径
        /// </summary>
        public static string BackgroundPath {get; set;} = "res://Assets/Background/";
        
        /// <summary>
        /// BGM路径
        /// </summary>
        public static string BgmPath {get; set;} = "res://Assets/Bgm/";
        
        /// <summary>
        /// 声音路径
        /// </summary>
        public static string VocalPath {get; set;} = "res://Assets/Vocal/";

        /// <summary>
        /// 历史对话记录
        /// </summary>
        public static System.Collections.Generic.List<string> History {get; set;} = new();

        /// <summary>
        /// 文本显示控制器实例
        /// </summary>
        public static DisplayController Typewriter {get; set;}
        
        /// <summary>
        /// 文本显示速度
        /// </summary>
        public static float TextDisplaySpeed {get; set;} = 0.05f;

        /// <summary>
        /// 日志面板是否激活
        /// </summary>
        public static bool LogPanelActive { get; protected set; } = false;

        /// <summary>
        /// 存档界面是否激活
        /// </summary>
        public static bool SaveLoadUiActive { get; protected set; } = false;

        /// <summary>
        /// 是否在自动播放剧本
        /// </summary>
        public static bool OnAutoPlay { get; protected set; } = false;

        /// <summary>
        /// 自动播放剧本间隔
        /// </summary>
        public static int AutoPlayInterval { get; set; } = 1;

        /// <summary>
        /// 是否在跳过对话
        /// </summary>
        public static bool OnSkiping { get; protected set; } = false;

        /// <summary>
        /// 配置界面是否激活
        /// </summary>
        public static bool ConfigUiActive { get; protected set; } = false;
#endregion
    
        private static bool IsMouseHoveringButton = false;
        
        /// <summary>
        /// 配置界面根节点
        /// </summary>
        protected static Node ConfigSceneRootNode;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="characterName">显示角色名称的TextMeshProUGUI</param>
        /// <param name="dialogueText">显示对话文本的TextMeshProUGUI</param>
        /// <param name="typewriter">文本显示控制器实例</param>
        /// <param name="figureLeft">显示左侧立绘的Image</param>
        /// <param name="figureCenter">显示中间立绘的Image</param>
        /// <param name="figureRight">显示右侧立绘的Image</param>
        /// <param name="portrait">显示头像的Image</param>
        /// <param name="background">显示背景的Image</param>
        /// <param name="bgm">BGM播放器</param>
        /// <param name="bgs">BGS播放器</param>
        /// <param name="se">音效播放器</param>
        /// <param name="voice">语音播放器</param>
        /// <param name="gameManagerNode">任一处于节点树的节点</param>
        /// <param name="choiceButtonContainer">选择按钮的容器</param>
        /// <param name="choiceButtonScene">选择按钮场景</param>
        /// <param name="textBox">对话框</param>
        /// <param name="autoPlayInterval">自动播放剧本间隔</param>
        /// <param name="titleScenePath">标题界面场景路径</param>
        /// <param name="saveLoadScenePath">存档界面场景路径</param>
        /// <param name="mainScenePath">主界面场景路径</param>
        /// <param name="figurePath">立绘路径</param>
        /// <param name="portraitPath">头像路径</param>
        /// <param name="backgroundPath">背景路径</param>
        /// <param name="bgmPath">BGM路径</param>
        /// <param name="vocalPath">声音路径</param>
        public static void Init(
            Label charaterName,
            Label dialogueText,
            DisplayController typewriter,
            TextureRect figureLeft,
            TextureRect figureCenter,
            TextureRect figureRight,
            TextureRect portrait,
            TextureRect background,
            AudioStreamPlayer bgm,
            AudioStreamPlayer bgs,
            AudioStreamPlayer se,
            AudioStreamPlayer voice,
            Node gameManagerNode,
            VBoxContainer choiceButtonContainer,
            PackedScene choiceButtonScene,
            Panel textBox,
            int autoPlayInterval,
            string titleScenePath = "res://Scenes/Title.tscn",
            string saveLoadScenePath = "res://Scenes/SaveLoad.tscn",
            string mainScenePath = "res://Scenes/Main.tscn",
            string figurePath = "res://Assets/Figure/",
            string portraitPath = "res://Assets/Portrait/",
            string backgroundPath = "res://Assets/Background/",
            string bgmPath = "res://Assets/Bgm/",
            string vocalPath = "res://Assets/Vocal/"
        )
        {
            CharaterName = charaterName;
            DialogueText = dialogueText;
            Typewriter = typewriter;
            FigureLeft = figureLeft;
            FigureCenter = figureCenter;
            FigureRight = figureRight;
            Portrait = portrait;
            Background = background;
            Bgm = bgm;
            Bgs = bgs;
            Se = se;
            Voice = voice;
            GameManagerNode = gameManagerNode;
            ChoiceButtonContainer = choiceButtonContainer;
            ChoiceButtonScene = choiceButtonScene;
            TextBox = textBox;
            AutoPlayInterval = autoPlayInterval;
            TitleScenePath = titleScenePath;
            SaveLoadScenePath = saveLoadScenePath;
            MainScenePath = mainScenePath;
            FigurePath = figurePath;
            PortraitPath = portraitPath;
            BackgroundPath = backgroundPath;
            BgmPath = bgmPath;
            VocalPath = vocalPath;

            // 添加映射
            InputMap.ActionAddEvent(
                "ui_accept", 
                new InputEventMouseButton { 
                    ButtonIndex = MouseButton.Left 
                    }
                );
            InputMap.ActionAddEvent(
                "ui_accept", 
                new InputEventMouseButton { 
                    ButtonIndex = MouseButton.WheelDown 
                }
            );

            if (!InputMap.HasAction("show_history"))
            {
                InputMap.AddAction("show_history");
                InputMap.ActionAddEvent(
                    "show_history", 
                    new InputEventMouseButton { 
                        ButtonIndex = MouseButton.WheelUp 
                    }
                );
            }

            if (!InputMap.HasAction("hide_textbox"))
            {
                InputMap.AddAction("hide_textbox");
                InputMap.ActionAddEvent(
                    "hide_textbox",
                    new InputEventMouseButton {
                        ButtonIndex = MouseButton.Right
                    }
                );
            }
            // 添加节点上下文
            AudioManager.Init(gameManagerNode);
        }
        
        /// <summary>
        /// 恢复脚本全局变量
        /// </summary>
        public static void RecoverGlobalVariables()
        {
            if (SaveManager.SaveExists("globalVariables.json"))
            {
                foreach (var item in SaveManager.LoadFromJson("globalVariables.json"))
                {
                    VariableInterpreter.VariableList.TryAdd(item.Key, item.Value);
                    if (!VariableInterpreter.GlobalVariableList.Contains(item.Key))
                    {
                        VariableInterpreter.GlobalVariableList.Add(item.Key);
                    }
                }
                SaveManager.ClearLoadedDataJson();
            }
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="saveName">存档名</param>
        public static void RecoverData(string saveName)
        {
            if (!SaveManager.SaveExists(saveName)) return;
            string scriptName = SaveManager.GetDataFromJson<string>(saveName, "currentScript");
            int lineIndex = SaveManager.GetDataFromJson<int>(saveName, "currentLine");
            ScriptReader.ReadAndExecute(scriptName, lineIndex);

            VariableInterpreter.VariableList = VariableInterpreter.CovertGdDictToSysDict(
                                                    SaveManager.GetDataFromJson<Dictionary<string, Variant>>(saveName, "scriptVariables")
                                                );
            RecoverGlobalVariables();

            DialogueInterpreter.CurrentDialogue = SaveManager.GetDataFromJson<string>(saveName, "currentDialogue");
            DialogueText.Text = DialogueInterpreter.CurrentDialogue;
            DialogueText.VisibleCharacters = -1;

            DialogueInterpreter.CurrentSpeaker = SaveManager.GetDataFromJson<string>(saveName, "currentSpeaker");
            CharaterName.Text = DialogueInterpreter.CurrentSpeaker;

            ChoiceInterpreter.OnChoosing = SaveManager.GetDataFromJson<bool>(saveName, "onChoosing");
            if (!ChoiceInterpreter.OnChoosing) ClearChoiceButtons();
        
            string bgmName = SaveManager.GetDataFromJson<string>(saveName, "currentBgm");

            string bgsName = SaveManager.GetDataFromJson<string>(saveName, "currentBgs");

            string seName = SaveManager.GetDataFromJson<string>(saveName, "currentSe");

            string voiceName = SaveManager.GetDataFromJson<string>(saveName, "currentVoice");

            if (!string.IsNullOrEmpty(bgmName))
            {
                AudioManager.PlayBgm(BgmPath.PathJoin(bgmName));
            }
            else
            {
                AudioManager.StopBgm();
            }

            
            if (!string.IsNullOrEmpty(bgsName))
            {
                AudioManager.PlayBgs(VocalPath.PathJoin(bgsName));
            }
            else
            {
                AudioManager.StopBgs();
            }

            
            if (!string.IsNullOrEmpty(seName))
            {
                AudioManager.PlaySe(VocalPath.PathJoin(seName));
            }

            
            if (!string.IsNullOrEmpty(voiceName))
            {
                AudioManager.PlayVoice(VocalPath.PathJoin(voiceName));
            }
            else
            {
                AudioManager.StopVoice();
            }

            string leftFigureName = SaveManager.GetDataFromJson<string>(saveName, "leftFigure");
            
            string centerFigureName = SaveManager.GetDataFromJson<string>(saveName, "centerFigure");
            
            string rightFigureName = SaveManager.GetDataFromJson<string>(saveName, "rightFigure");
            
            string portraitName = SaveManager.GetDataFromJson<string>(saveName, "portrait");
            
            string backgroundName = SaveManager.GetDataFromJson<string>(saveName, "background");

            if (!string.IsNullOrEmpty(leftFigureName))
            {
                FigureLeft.Texture = ResourceLoader.Load<Texture2D>(FigurePath.PathJoin(leftFigureName));
            }
            else
            {
                FigureLeft.Texture = null;
            }

            if (!string.IsNullOrEmpty(centerFigureName))
            {
                FigureCenter.Texture = ResourceLoader.Load<Texture2D>(FigurePath.PathJoin(centerFigureName));
            }
            else
            {
                FigureCenter.Texture = null;
            }

            if (!string.IsNullOrEmpty(rightFigureName))
            {
                FigureRight.Texture = ResourceLoader.Load<Texture2D>(FigurePath.PathJoin(rightFigureName));
            }
            else
            {
                FigureRight.Texture = null;
            }

            if (!string.IsNullOrEmpty(portraitName))
            {
                Portrait.Texture = ResourceLoader.Load<Texture2D>(PortraitPath.PathJoin(portraitName));
            }
            else
            {
                Portrait.Texture = null;
            }

            if (!string.IsNullOrEmpty(backgroundName))
            {
                Background.Texture = ResourceLoader.Load<Texture2D>(BackgroundPath.PathJoin(backgroundName));
            }
            else
            {
                Background.Texture = null;
            }

        }

        /// <summary>
        /// 显示当前行对话
        /// </summary>
        public static void DisplayCurrentLine()
        {
            if (CharaterName == null || DialogueText == null || Typewriter == null)
            {
                GD.PushError("VisualNoveCore: Missing instance");
                return;
            }
            if (Typewriter.IsTyping)
            {
                Typewriter.DisplayCompleteLine();
            }
            else
            {
                CharaterName.Text = DialogueInterpreter.CurrentSpeaker;
                Typewriter.DisplayLine(DialogueInterpreter.CurrentDialogue);
            }
        }

        /// <summary>
        /// 创建选择按钮
        /// </summary>
        /// <param name="choicesText">选择文本</param>
        /// <param name="spacing">间隔</param>
        public static void CreateChoiceButtons(string[] choicesText, int spacing)
        {
            if (ChoiceButtonContainer == null || ChoiceButtonScene == null)
            {
                GD.PushError("VisualNoveCore: Missing instance");
                return;
            }
            ClearChoiceButtons();
            ChoiceButtonContainer.AddThemeConstantOverride("separation", spacing);

            for(int i = 0; i < choicesText.Length; i++)
            {
                Button button = ChoiceButtonScene.Instantiate<Button>();
                ChoiceButtonContainer.AddChild(button);
                button.Name = "ChoiceButton_" + i;
                button.Text = choicesText[i];
                int choiceIndex = i;
                button.Pressed += () => { ChoiceInterpreter.SelectChoice(choiceIndex); };
            }
        }

        /// <summary>
        /// 显示或隐藏文本框
        /// </summary>
        /// <param name="show">是否显示</param>
        public static void ShowTextBox(bool show)
        {
            if (TextBox == null)
            {
                GD.PushError("VisualNoveCore: Missing instance");
                return;
            }

            TextBox.Visible = show;
        }

        /// <summary>
        /// 清除选择按钮
        /// </summary>
        public static void ClearChoiceButtons()
        {
            if (ChoiceButtonContainer == null)
            {
                GD.PushError("VisualNoveCore: Missing instance");
                return;
            }

            foreach(Button button in ChoiceButtonContainer.GetChildren().Cast<Button>())
            {
                button.QueueFree();
            }
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <param name="size">大小</param>
        public static void SetFontSize(int size)
        {
            if (DialogueText == null)
            {
                GD.PushError("VisualNoveCore: Missing instance");
                return;
            }

            DialogueText.AddThemeFontSizeOverride("font_size", size);
        }

        public override void _Input(InputEvent @event)
        {
            if (ShouldExecuteNextLine())
            {
                BaseInterpreter.ExecuteNextLine();
            }
            if (ShouldShowHistory())
            {
                TextboxFunctions.ShowHistory();
            }
            if (ShouldSwitchTextboxVisibility())
            {
                TextboxFunctions.SwitchTextboxVisibility();
            }
        }
        
        /// <summary>
        /// 鼠标进入按钮
        /// </summary>
        public static void MouseEnteredButton()
            => IsMouseHoveringButton = true;

        /// <summary>
        /// 鼠标退出按钮
        /// </summary>
        public static void MouseExitedButton()
            => IsMouseHoveringButton = false;

        /// <summary>
        /// 是否应该执行下一行脚本
        /// </summary>
        public static bool ShouldExecuteNextLine()
        {
            return Input.IsActionJustPressed("ui_accept") &&
                !IsMouseHoveringButton &&
                ParseScript.WaitClick &&
                !ChoiceInterpreter.OnChoosing &&
                !LogPanelActive &&
                TextBox.Visible &&
                !SaveLoadUiActive &&
                !ConfigUiActive;
        }

        /// <summary>
        /// 是否应该显示历史记录
        /// </summary>
        public static bool ShouldShowHistory()
        {
            return Input.IsActionJustPressed("show_history") &&
                !LogPanelActive &&
                !SaveLoadUiActive &&
                !ConfigUiActive;
        }

        /// <summary>
        /// 是否应该切换对话框可见性
        /// </summary>
        public static bool ShouldSwitchTextboxVisibility()
        {
            return Input.IsActionJustPressed("hide_textbox") &&
                !LogPanelActive &&
                !SaveLoadUiActive &&
                !ConfigUiActive &&
                !ParseScript.ScriptHidedTextbox;
        }
    
        /// <summary>
        /// 停止自动播放和跳过对话
        /// </summary>
        public static void StopAutoPlayAndSkip()
        {
            OnAutoPlay = false;
            OnSkiping = false;
        }
    
        /// <summary>
        /// 关闭配置界面
        /// </summary>
        public static void CloseConfigUi()
        {
            ConfigUiActive = false;
            ConfigSceneRootNode.QueueFree();
        }
    }
}