using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using GensouLib.GenScript.Interpreters;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 对话框功能
    /// </summary>
    public class TextboxFunctions : VisualNoveCore
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
        public static RectTransform LogContainter { get; set; }
        
        /// <summary>
        /// 历史记录文本预制体
        /// </summary>
        public static GameObject LogTextPrefab { get; set; }
        
        /// <summary>
        /// 历史记录滚动视图
        /// </summary>
        public static ScrollRect ScrollView { get; set; }
        
        /// <summary>
        /// 历史记录面板
        /// </summary>
        public static GameObject LogPanel { get; set; }
        
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
        /// <param name="logTextPrefab">历史记录文本预制体</param>
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
            RectTransform logContainter,
            GameObject logTextPrefab,
            ScrollRect scrollView,
            GameObject logPanel,
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
            LogTextPrefab = logTextPrefab;
            ScrollView = scrollView;
            LogPanel = logPanel;
            CloseLogPanelButton = closeLogPanelButton;
            if (CloseLogPanelButton!= null)
                CloseLogPanelButton.onClick.AddListener(HideHistory);
            if (LogButton!= null)
                LogButton.onClick.AddListener(ShowHistory);
            if (SaveButton!= null)
                SaveButton.onClick.AddListener(() => SaveLoadGameButtonClick(true));
            if (LoadButton!= null)
                LoadButton.onClick.AddListener(() => SaveLoadGameButtonClick(false));
            if (AutoButton!= null)
                AutoButton.onClick.AddListener(() => SwitchAutoPlay(!OnAutoPlay));
            if (SkipButton!= null)
                SkipButton.onClick.AddListener(() => SwitchSkip(!OnSkiping));
            if (SystemButton != null)
                SystemButton.onClick.AddListener(OpenConfigUi);
            if (TitleButton != null)
                TitleButton.onClick.AddListener(BackToTitle);
        }

        /// <summary>
        /// 返回标题
        /// </summary>
        public static void BackToTitle()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            SceneManager.LoadScene(TitleScene, LoadSceneMode.Single);
        }

        /// <summary>
        /// 唤起保存或加载游戏界面
        /// </summary>
        /// <param name="isSave">是否是保存</param>
        public static void SaveLoadGameButtonClick(bool isSave)
        {
            OnAutoPlay = false;
            OnSkiping = false;
            if (isSave) ScreenshotToRawImage.CaptureScreenshot();
            SaveLoadGame.IsSave = isSave;
            SaveLoadUiActive = true;
            SceneManager.LoadScene(SaveLoadScene, LoadSceneMode.Additive);
        }

        /// <summary>
        /// 显示历史记录面板
        /// </summary>
        public static void ShowHistory()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            foreach (Transform child in LogContainter)
            {
                Destroy(child.gameObject);
            }

            foreach (string log in History)
            {
                GameObject logText = Instantiate(LogTextPrefab, LogContainter);
                TextMeshProUGUI text = logText.GetComponent<TextMeshProUGUI>();
                RectTransform textRectTransform = logText.GetComponent<RectTransform>();
                text.text = log.Replace("\n", " ");
                
                text.enableAutoSizing = true;
                Vector2 size = textRectTransform.sizeDelta;
                size.x = LogContainter.sizeDelta.x;
                textRectTransform.sizeDelta = size;
            }
            LogPanelActive = true;
            LogPanel.SetActive(true);
            ScrollView.verticalNormalizedPosition = 0;
        }

        /// <summary>
        /// 隐藏历史记录面板
        /// </summary>
        public static void HideHistory()
        {
            LogPanelActive = false;
            LogPanel.SetActive(false);
        }

        /// <summary>
        /// 切换对话框可见性
        /// </summary>
        public static void SwitchTextboxVisibility()
        {
            OnAutoPlay = false;
            OnSkiping = false;
            TextBox.SetActive(!TextBox.activeSelf);
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
    
        /// <summary>
        /// 打开系统设置界面
        /// </summary>
        public static void OpenConfigUi()
        {
            ConfigUiActive = true;
            OnAutoPlay = false;
            OnSkiping = false;
            SceneManager.LoadScene("Config", LoadSceneMode.Additive);
        }
    }
}