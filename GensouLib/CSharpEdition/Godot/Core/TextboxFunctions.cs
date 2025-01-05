using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GensouLib.GenScript.Interpreters;
using Godot;

namespace GensouLib.Godot.Core
{
    /// <summary>
    /// 对话框功能
    /// </summary>
    public partial class TextboxFunctions : VisualNoveCore
    {
        /// <summary>
        /// 跳过对话按钮
        /// </summary>
        public static Button SkipButton { get; set; }
        
        /// <summary>
        /// 自动播放按钮
        /// </summary>
        public static Button AutoButton { get; set; }
        
        /// <summary>
        /// 保存游戏按钮
        /// </summary>
        public static Button SaveButton { get; set; }
        
        /// <summary>
        /// 加载游戏按钮
        /// </summary>
        public static Button LoadButton { get; set; }
        
        /// <summary>
        /// 历史记录按钮
        /// </summary>
        public static Button LogButton { get; set; }

        /// <summary>
        /// 系统设置按钮
        /// </summary>
        public static Button SystemButton { get; set; }
        
        /// <summary>
        /// 返回标题按钮
        /// </summary>
        public static Button TitleButton { get; set; }

        /// <summary>
        /// 历史记录容器
        /// </summary>
        public static VBoxContainer LogContainter { get; set; }

        /// <summary>
        /// 历史记录文本场景
        /// </summary>
        public static PackedScene LogTextScene { get; set; }
        
        /// <summary>
        /// 历史记录滚动视图
        /// </summary>
        public static ScrollContainer ScrollView { get; set; }
        
        /// <summary>
        /// 历史记录面板
        /// </summary>
        public static Panel LogPanel { get; set; }
        
        /// <summary>
        /// 关闭历史记录面板按钮
        /// </summary>
        public static Button CloseLogPanelButton { get; set; }
    
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="skipButton">跳过对话按钮</param>
        /// <param name="autoButton">自动播放按钮</param>
        /// <param name="saveButton">保存游戏按钮</param>
        /// <param name="loadButton">加载游戏按钮</param>
        /// <param name="logButton">历史记录按钮</param>
        /// <param name="systemButton">系统设置按钮</param>
        /// <param name="titleButton">返回标题按钮</param>
        /// <param name="logContainter">历史记录容器</param>
        /// <param name="logTextScene">历史记录场景</param>
        /// <param name="scrollView">历史记录滚动视图</param>
        /// <param name="logPanel">历史记录面板</param>
        /// <param name="closeLogPanelButton">关闭历史记录面板的按钮</param>
        public static void Init(
            Button skipButton,
            Button autoButton,
            Button saveButton,
            Button loadButton,
            Button logButton,
            Button systemButton,
            Button titleButton,
            VBoxContainer logContainter,
            PackedScene logTextScene,
            ScrollContainer scrollView,
            Panel logPanel,
            Button closeLogPanelButton
        )
        {
            SkipButton = skipButton;
            AutoButton = autoButton;
            SaveButton = saveButton;
            LoadButton = loadButton;
            LogButton = logButton;
            SystemButton = systemButton;
            TitleButton = titleButton;
            LogContainter = logContainter;
            LogTextScene = logTextScene;
            ScrollView = scrollView;
            LogPanel = logPanel;
            CloseLogPanelButton = closeLogPanelButton;
            if (SaveButton != null)
            {
                SaveButton.Pressed += () => SaveLoadGameButtonClick(true);
                ConnectButtonHoverSignal(SaveButton);
            }
            if (LoadButton != null)
            {
                LoadButton.Pressed += () => SaveLoadGameButtonClick(false);
                ConnectButtonHoverSignal(LoadButton);
            }
            if (LogButton != null)
            {
                LogButton.Pressed += ShowHistory;
                ConnectButtonHoverSignal(LogButton);
            }
            if (CloseLogPanelButton != null)
            {
                CloseLogPanelButton.Pressed += HideHistory;
                ConnectButtonHoverSignal(CloseLogPanelButton);
            }
            if (TitleButton != null)
            {
                TitleButton.Pressed += BackToTitle;
                ConnectButtonHoverSignal(TitleButton);
            }
            if (AutoButton != null)
            {
                AutoButton.Pressed += () => SwitchAutoPlay(true);
                ConnectButtonHoverSignal(AutoButton);
            }
            if (SkipButton != null)
            {
                SkipButton.Pressed += () => SwitchSkip(true);
                ConnectButtonHoverSignal(SkipButton);
            }
            if (SystemButton != null)
            {
                SystemButton.Pressed += () => OpenConfigUi();
                ConnectButtonHoverSignal(SystemButton);
            }
        }

        private static void ConnectButtonHoverSignal(Button button)
        {
            button.MouseEntered += MouseEnteredButton;
            button.MouseExited += MouseExitedButton;
        }

        /// <summary>
        /// 返回标题
        /// </summary>
        public static void BackToTitle()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            GameManagerNode.GetTree().ChangeSceneToFile(TitleScenePath);
        }

        /// <summary>
        /// 唤起保存或加载游戏界面
        /// </summary>
        /// <param name="isSave">是否是保存</param>
        public static void SaveLoadGameButtonClick(bool isSave)
        {
            OnAutoPlay = false;
            OnSkiping = false;
            if (isSave) ScreenshotToTextureRect.CaptureScreenshot();
            SaveLoadGame.IsSave = isSave;
            SaveLoadUiActive = true;
            PackedScene saveLoadScene = ResourceLoader.Load<PackedScene>(SaveLoadScenePath);
            SaveLoadGame.SaveLoadSceneRootNode = saveLoadScene.Instantiate();
            GameManagerNode.GetTree().Root.AddChild(SaveLoadGame.SaveLoadSceneRootNode);
        }
    
        /// <summary>
        /// 打开系统设置界面
        /// </summary>
        /// <param name="configScenePath">系统设置场景路径</param>
        /// <param name="node">任意节点</param>
        public static void OpenConfigUi(string configScenePath = "res://Scenes/Config.tscn", Node node = null)
        {
            ConfigUiActive = true;
            OnAutoPlay = false;
            OnSkiping = false;
            if (node != null) 
            {
                PackedScene configScene = ResourceLoader.Load<PackedScene>(configScenePath);
                ConfigSceneRootNode = configScene.Instantiate();
                node.GetTree().Root.AddChild(ConfigSceneRootNode);
            }
            else 
            {
                PackedScene configScene = ResourceLoader.Load<PackedScene>(ConfigScnenPath);
                ConfigSceneRootNode = configScene.Instantiate();
                GameManagerNode.GetTree().Root.AddChild(ConfigSceneRootNode);
            }
        }

        /// <summary>
        /// 显示历史记录面板
        /// </summary>
        public static void ShowHistory()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            foreach (Node child in LogContainter.GetChildren())
            {
                child.QueueFree();
            }

            foreach (string log in History)
            {
                Label logText = LogTextScene.Instantiate<Label>();
                LogContainter.AddChild(logText);
                logText.Text = log.Replace("\n", " ");
            }

            LogPanelActive = true;
            LogPanel.Visible = true;
        }

        /// <summary>
        /// 隐藏历史记录面板
        /// </summary>
        public static void HideHistory()
        {
            LogPanelActive = false;
            LogPanel.Visible = false;
        }

        /// <summary>
        /// 切换对话框可见性
        /// </summary>
        public static void SwitchTextboxVisibility()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            TextBox.Visible = !TextBox.Visible;
        }
    
        /// <summary>
        /// 切换自动播放
        /// </summary>
        /// <param name="isOn">是否开启</param>
        public static void SwitchAutoPlay(bool isOn)
        {
            OnAutoPlay = isOn;
            if (OnAutoPlay && !Typewriter.IsTyping && !ChoiceInterpreter.OnChoosing)
                BaseInterpreter.ExecuteNextLine();
        }

        /// <summary>
        /// 切换跳过对话
        /// </summary>
        /// <param name="isOn">是否开启</param>
        public static void SwitchSkip(bool isOn)
        {
            OnSkiping = isOn;
            if (OnSkiping && !Typewriter.IsTyping && !ChoiceInterpreter.OnChoosing)
                BaseInterpreter.ExecuteNextLine();
        }
    }
}