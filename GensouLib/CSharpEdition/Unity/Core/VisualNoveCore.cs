using System.Collections.Generic;
using System.IO;
using GensouLib.GenScript;
using GensouLib.GenScript.Interpreters;
using GensouLib.Unity.Audio;
using GensouLib.Unity.ResourceLoader;
using GensouLib.Unity.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 核心功能
    /// </summary>
    public class VisualNoveCore : MonoBehaviour
    {
#region property
        /// <summary>
        /// 显示角色名称的TextMeshProUGUI
        /// </summary>
        public static TextMeshProUGUI CharacterName { get; set; }
        
        /// <summary>
        /// 显示对话文本的TextMeshProUGUI
        /// </summary>
        public static TextMeshProUGUI DialogueText { get; set; }

        /// <summary>
        /// 显示左侧立绘的Image
        /// </summary>
        public static Image FigureLeft { get; set; }
        
        /// <summary>
        /// 显示中间立绘的Image
        /// </summary>
        public static Image FigureCenter { get; set; }

        /// <summary>
        /// 显示右侧立绘的Image
        /// </summary>
        public static Image FigureRight { get; set; }

        /// <summary>
        /// 显示头像的Image
        /// </summary>
        public static Image Portrait { get; set; }

        /// <summary>
        /// 显示背景的Image
        /// </summary>
        public static Image Background { get; set; }

        /// <summary>
        /// 背景音乐音源
        /// </summary>
        public static AudioSource Bgm { get => AudioManager.BgmSource; set => AudioManager.BgmSource = value; }
        
        /// <summary>
        /// 语音音源
        /// </summary>
        public static AudioSource Voice { get => AudioManager.VoiceSource; set => AudioManager.VoiceSource = value; }
        
        /// <summary>
        /// 背景声音音源
        /// </summary>
        public static AudioSource Bgs { get => AudioManager.BgsSource; set => AudioManager.BgsSource = value; }
        
        /// <summary>
        /// 音效音源
        /// </summary>
        public static AudioSource Se { get => AudioManager.SeSource; set => AudioManager.SeSource = value; }
        
        /// <summary>
        /// 选择按钮的容器
        /// </summary>
        public static VerticalLayoutGroup ChoiceButtonContainer { get; set; }
        
        /// <summary>
        /// 选择按钮的预制体
        /// </summary>
        public static Button ChoiceButtonPrefab { get; set; }
        
        /// <summary>
        /// 对话框
        /// </summary>
        public static GameObject TextBox { get; set; }
        
        /// <summary>
        /// 标题场景名称
        /// </summary>
        public static string TitleScene { get; set; } = "Title";

        /// <summary>
        /// 存档场景名称
        /// </summary>
        public static string SaveLoadScene { get; set; }  = "SaveLoad";

        /// <summary>
        /// 主场景名称
        /// </summary>
        public static string MainScene { get; set; } = "Main";
# if ENABLE_ADDRESSABLES == false

        /// <summary>
        /// 立绘路径
        /// </summary>
        public static string FigurePath { get; set; } = "Figure";

        /// <summary>
        /// 头像路径
        /// </summary>
        public static string PortraitPath { get; set; } = "Portrait";

        /// <summary>
        /// 背景路径
        /// </summary>
        public static string BackgroundPath { get; set; } = "Background";

        /// <summary>
        /// BGM路径
        /// </summary>
        public static string BgmPath { get; set; } = "Bgm";

        /// <summary>
        /// 声音路径
        /// </summary>
        public static string VocalPath { get; set; } = "Vocal";
#endif

        // /// <summary>
        // /// 当前剧本行（与脚本执行行无关）
        // /// </summary>
        // public static int CurrentLine { get; private set; }
        
        // /// <summary>
        // /// 当前剧本最大行（与脚本最大行无关）
        // /// </summary>
        // public static int CurrentDialogueMaxLine { get; private set; }

        // /// <summary>
        // /// 当前剧本数据（与脚本数据无关）
        // /// </summary>
        // public static List<DialogueInterpreter.Dialogue> CurrentStoryData { get; private set; } = new();

        /// <summary>
        /// 历史对话记录
        /// </summary>
        public static List<string> History { get; set;} = new();

        /// <summary>
        /// 文本显示控制器实例
        /// </summary>
        public static DisplayController Typewriter { get; set; }
        
        /// <summary>
        /// 文本显示速度
        /// </summary>
        public static float TextDisplaySpeed { get; set; } = 0.05f;

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
        /// <param name="bgm">背景音乐音源</param>
        /// <param name="voice">语音音源</param>
        /// <param name="bgs">背景声音音源</param>
        /// <param name="se">音效音源</param>
        /// <param name="choiceButtonContainer">选择按钮容器</param>
        /// <param name="choiceButtonPrefab">选择按钮预制件</param>
        /// <param name="textBox">对话框</param>
        /// <param name="autoPlayInterval">自动播放间隔</param>
        /// <param name="titleScene">标题场景名</param>
        /// <param name="saveLoadScene">存档场景名</param>
        /// <param name="mainScene">主场景名</param>
        public static void Init(
            TextMeshProUGUI characterName, 
            TextMeshProUGUI dialogueText, 
            DisplayController typewriter, 
            Image figureLeft, 
            Image figureCenter, 
            Image figureRight, 
            Image portrait, 
            Image background, 
            AudioSource bgm, 
            AudioSource voice, 
            AudioSource bgs, 
            AudioSource se, 
            VerticalLayoutGroup choiceButtonContainer, 
            Button choiceButtonPrefab,
            GameObject textBox,
            int autoPlayInterval,
            string titleScene = "Title",
            string saveLoadScene = "SaveLoad",
            string mainScene = "Main"
        )
        {
            CharacterName = characterName;
            DialogueText = dialogueText;
            Typewriter = typewriter;
            FigureLeft = figureLeft;    
            FigureCenter = figureCenter;
            FigureRight = figureRight;
            Portrait = portrait;
            Background = background;
            Bgm = bgm;
            Voice = voice;
            Bgs = bgs;
            Se = se;
            ChoiceButtonContainer = choiceButtonContainer;
            ChoiceButtonPrefab = choiceButtonPrefab;
            TextBox = textBox;
            AutoPlayInterval = autoPlayInterval;
            TitleScene = titleScene;
            SaveLoadScene = saveLoadScene;
            MainScene = mainScene;
        }
        
        /// <summary>
        /// 恢复脚本全局变量
        /// </summary>
        public static void RecoverGlobalVariables()
        {
#if ENABLE_JSONNET == false
            if (SaveManager.SaveExists("globalVariables.sav")) // 加载全局变量
            {
                foreach (var item in SaveManager.LoadFromBinary("globalVariables.sav"))
                {
                    VariableInterpreter.VariableList.TryAdd(item.Key, item.Value);
                    if (!VariableInterpreter.GlobalVariableList.Contains(item.Key))
                    {
                        VariableInterpreter.GlobalVariableList.Add(item.Key);
                    }
                }
                SaveManager.ClearLoadedDataBinary();
            }
#else
            if (SaveManager.SaveExists("globalVariables.json")) // 加载全局变量
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
#endif
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="saveName">存档名</param>
        public static void RecoverData(string saveName)
        {
            if (!SaveManager.SaveExists(saveName)) return;
#if ENABLE_JSONNET == false
            string scriptName = SaveManager.GetDataFromBinary<string>(saveName, "currentScript");
            int lineIdex = SaveManager.GetDataFromBinary<int>(saveName, "currentLine");
            ScriptReader.ReadAndExecute(scriptName, lineIdex);

            VariableInterpreter.VariableList = SaveManager.GetDataFromBinary<Dictionary<string, object>>(saveName, "scriptVariables"); // 恢复脚本变量

            RecoverGlobalVariables(); // 恢复全局变量

            DialogueInterpreter.CurrentDialogue = SaveManager.GetDataFromBinary<string>(saveName, "currentDialogue"); // 恢复对话内容
            DialogueText.text = DialogueInterpreter.CurrentDialogue;
            DialogueText.maxVisibleCharacters = DialogueInterpreter.CurrentDialogue.Length;

            DialogueInterpreter.CurrentSpeaker = SaveManager.GetDataFromBinary<string>(saveName, "currentSpeaker"); // 恢复对话人物
            CharacterName.text = DialogueInterpreter.CurrentSpeaker;

            ChoiceInterpreter.OnChoosing = SaveManager.GetDataFromBinary<bool>(saveName, "onChoosing"); // 恢复是否处于选择状态
            if (!ChoiceInterpreter.OnChoosing) ClearChoiceButtons(); // 清除选择按钮

            AudioManager.MasterVolume = SaveManager.GetDataFromBinary<float>(saveName, "masterVolume"); // 恢复音量

            string bgmName = SaveManager.GetDataFromBinary<string>(saveName, "currentBgm"); // 恢复BGM

            string bgsName = SaveManager.GetDataFromBinary<string>(saveName, "currentBgs"); // 恢复BGS

            string seName = SaveManager.GetDataFromBinary<string>(saveName, "currentSe"); // 恢复SE

            string voiceName = SaveManager.GetDataFromBinary<string>(saveName, "currentVoice"); // 恢复语音
            
            if (!string.IsNullOrEmpty(bgmName))
            {
#if ENABLE_ADDRESSABLES == false
                bgmName = string.Join("/", BgmPath, bgmName);
#endif
                AudioManager.PlayBgm(bgmName);
            }
            else
            {
                AudioManager.StopBgm();
            }

            if (!string.IsNullOrEmpty(bgsName))
            {
#if ENABLE_ADDRESSABLES == false
                bgsName = string.Join("/", VocalPath, bgsName);
#endif
                AudioManager.PlayBgs(bgsName);
            }
            else
            {
                AudioManager.StopBgs();
            }

            if (!string.IsNullOrEmpty(seName))
            {
#if ENABLE_ADDRESSABLES == false
                seName = string.Join("/", VocalPath, seName);
#endif
                AudioManager.PlaySe(seName);
            }
                
            if (!string.IsNullOrEmpty(voiceName))
            {
#if ENABLE_ADDRESSABLES == false
                voiceName = string.Join("/", VocalPath, voiceName);
#endif
                AudioManager.PlayVoice(voiceName);
            }
            else
            {
                AudioManager.StopVoice();
            }

            string leftFigureName = SaveManager.GetDataFromBinary<string>(saveName, "leftFigure"); // 恢复左侧角色

            string rightFigureName = SaveManager.GetDataFromBinary<string>(saveName, "rightFigure"); // 恢复右侧角色

            string centerFigureName = SaveManager.GetDataFromBinary<string>(saveName, "centerFigure"); // 恢复中间角色

            string portraitName = SaveManager.GetDataFromBinary<string>(saveName, "portrait"); // 恢复头像

            string backgroundName = SaveManager.GetDataFromBinary<string>(saveName, "background"); // 恢复背景

            if (!string.IsNullOrEmpty(leftFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                leftFigureName = string.Join("/", FigurePath, leftFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(leftFigureName);
                FigureLeft.sprite = AssetLoader.GetLoadedAsset<Sprite>(leftFigureName);
            }
            else
            {
                FigureLeft.sprite = null;
            }
                
            if (!string.IsNullOrEmpty(centerFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                rightFigureName = string.Join("/", FigurePath, rightFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(rightFigureName);
                FigureRight.sprite = AssetLoader.GetLoadedAsset<Sprite>(rightFigureName);
            }
            else
            {
                FigureRight.sprite = null;
            }
                
            if (!string.IsNullOrEmpty(rightFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                centerFigureName = string.Join("/", FigurePath, centerFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(centerFigureName);
                FigureCenter.sprite = AssetLoader.GetLoadedAsset<Sprite>(centerFigureName);
            }
            else
            {
                FigureCenter.sprite = null;
            }
                
            if (!string.IsNullOrEmpty(portraitName))
            {
#if ENABLE_ADDRESSABLES == false
                portraitName = string.Join("/", PortraitPath, portraitName);
#endif
                AssetLoader.LoadResource<Sprite>(portraitName);
                Portrait.sprite = AssetLoader.GetLoadedAsset<Sprite>(portraitName);
            }
            else
            {
                Portrait.sprite = null;
            }

            if (!string.IsNullOrEmpty(backgroundName))
            {
#if ENABLE_ADDRESSABLES == false
                backgroundName = string.Join("/", BackgroundPath, backgroundName);
#endif
                AssetLoader.LoadResource<Sprite>(backgroundName);
                Background.sprite = AssetLoader.GetLoadedAsset<Sprite>(backgroundName);
            }
            else
            {
                Background.sprite = null;
            }
#else
            string scriptName = SaveManager.GetDataFromJson<string>(saveName, "currentScript");
            int lineIdex = (int)SaveManager.GetDataFromJson<long>(saveName, "currentLine");
            ScriptReader.ReadAndExecute(scriptName, lineIdex);

            VariableInterpreter.VariableList = SaveManager.GetDataFromJson<Dictionary<string, object>>(saveName, "scriptVariables"); // 恢复脚本变量

            RecoverGlobalVariables(); // 恢复全局变量

            DialogueInterpreter.CurrentDialogue = SaveManager.GetDataFromJson<string>(saveName, "currentDialogue"); // 恢复对话内容
            DialogueText.text = DialogueInterpreter.CurrentDialogue;
            DialogueText.maxVisibleCharacters = DialogueInterpreter.CurrentDialogue.Length;

            DialogueInterpreter.CurrentSpeaker = SaveManager.GetDataFromJson<string>(saveName, "currentSpeaker"); // 恢复对话人物
            CharacterName.text = DialogueInterpreter.CurrentSpeaker;

            ChoiceInterpreter.OnChoosing = SaveManager.GetDataFromJson<bool>(saveName, "onChoosing"); // 恢复是否处于选择状态
            if (!ChoiceInterpreter.OnChoosing) ClearChoiceButtons(); // 清除选择按钮

            string bgmName = SaveManager.GetDataFromJson<string>(saveName, "currentBgm"); // 恢复BGM

            string bgsName = SaveManager.GetDataFromJson<string>(saveName, "currentBgs"); // 恢复BGS
            
            string seName = SaveManager.GetDataFromJson<string>(saveName, "currentSe"); // 恢复SE
            
            string voiceName = SaveManager.GetDataFromJson<string>(saveName, "currentVoice"); // 恢复语音
            
            if (!string.IsNullOrEmpty(bgmName))
            {
#if ENABLE_ADDRESSABLES == false
                bgmName = string.Join("/", BgmPath, bgmName);
#endif
                AudioManager.PlayBgm(bgmName);
            }
            else
            {
                AudioManager.StopBgm();
            }


            if (!string.IsNullOrEmpty(bgsName))
            {
#if ENABLE_ADDRESSABLES == false
                bgsName = string.Join("/", VocalPath, bgsName);
#endif
                AudioManager.PlayBgs(bgsName);
            }
            else
            {
                AudioManager.StopBgs();
            }


            if (!string.IsNullOrEmpty(seName))
            {
#if ENABLE_ADDRESSABLES == false
                seName = string.Join("/", VocalPath, seName);
#endif
                AudioManager.PlaySe(seName);
            }
                

            if (!string.IsNullOrEmpty(voiceName))
            {
#if ENABLE_ADDRESSABLES == false
                voiceName = string.Join("/", VocalPath, voiceName);
#endif
                AudioManager.PlayVoice(voiceName);
            }
            else
            {
                AudioManager.StopVoice();
            }

            string leftFigureName = SaveManager.GetDataFromJson<string>(saveName, "leftFigure"); // 恢复左侧角色

            string rightFigureName = SaveManager.GetDataFromJson<string>(saveName, "rightFigure"); // 恢复右侧角色

            string centerFigureName = SaveManager.GetDataFromJson<string>(saveName, "centerFigure"); // 恢复中间角色

            string portraitName = SaveManager.GetDataFromJson<string>(saveName, "portrait"); // 恢复头像

            string backgroundName = SaveManager.GetDataFromJson<string>(saveName, "background"); // 恢复背景

            if (!string.IsNullOrEmpty(leftFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                leftFigureName = string.Join("/", FigurePath, leftFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(leftFigureName);
                FigureLeft.sprite = AssetLoader.GetLoadedAsset<Sprite>(leftFigureName);
            }
                
            if (!string.IsNullOrEmpty(rightFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                rightFigureName = string.Join("/", FigurePath, rightFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(rightFigureName);
                FigureRight.sprite = AssetLoader.GetLoadedAsset<Sprite>(rightFigureName);
            }
            else
            {
                FigureRight.sprite = null;
            }

            if (!string.IsNullOrEmpty(centerFigureName))
            {
#if ENABLE_ADDRESSABLES == false
                centerFigureName = string.Join("/", FigurePath, centerFigureName);
#endif
                AssetLoader.LoadResource<Sprite>(centerFigureName);
                FigureCenter.sprite = AssetLoader.GetLoadedAsset<Sprite>(centerFigureName);
            }
            else
            {
                FigureCenter.sprite = null;
            }
                
            if (!string.IsNullOrEmpty(portraitName))
            {
#if ENABLE_ADDRESSABLES == false
                portraitName = string.Join("/", PortraitPath, portraitName);
#endif
                AssetLoader.LoadResource<Sprite>(portraitName);
                Portrait.sprite = AssetLoader.GetLoadedAsset<Sprite>(portraitName);
            }
            else
            {
                Portrait.sprite = null;
            }

            if (!string.IsNullOrEmpty(backgroundName))
            {
#if ENABLE_ADDRESSABLES == false
                backgroundName = string.Join("/", BackgroundPath, backgroundName);
#endif
                AssetLoader.LoadResource<Sprite>(backgroundName);
                Background.sprite = AssetLoader.GetLoadedAsset<Sprite>(backgroundName);
            }
            else
            {
                Background.sprite = null;
            }
#endif  
        }

        // /// <summary>
        // /// 加载剧本
        // /// </summary>
        // /// <remarks>
        // /// 不使用Genscript时，使用此方法来快速加载对话数据
        // /// </remarks>
        // /// <param name="path">文件路径</param>
        // /// <returns>对话数据</returns>
        // public static List<DialogueInterpreter.Dialogue> LoadStory(string path)
        // {
        //     if (!File.Exists(path)) // 检查文件是否存在
        //     {
        //         Debug.LogError("File not found: " + path);
        //         return new();
        //     }
        //     using StreamReader reader = new(path); // 读取文件
        //     List<DialogueInterpreter.Dialogue> dialogue = new();
        //     while (!reader.EndOfStream)
        //     {
        //         string line = reader.ReadLine();
        //         if (string.IsNullOrEmpty(line)) 
        //             continue;
        //         dialogue.Add(DialogueInterpreter.ParseDialogue(line));
        //     }
        //     CurrentLine = 0; // 重置当前行数
        //     CurrentDialogueMaxLine = dialogue.Count; // 记录最大行数
        //     CurrentStoryData = dialogue; // 记录剧本数据
        //     return dialogue; // 返回剧本数据
        // }

        // /// <summary>
        // /// 显示下一行
        // /// </summary>
        // /// <remarks>
        // /// 不使用Genscript时，使用此方法来显示下一行对话
        // /// </remarks>
        // public static void DisplayNextLine()
        // {
        //     if (CharacterName == null || DialogueText == null || Typewriter == null) // 检查实例是否存在
        //     {
        //         Debug.LogError("VisualNoveCore: Missing instance");
        //         return;
        //     }
        //     if (CurrentLine >= CurrentDialogueMaxLine) // 检查是否到达结尾
        //     {
        //         Debug.Log("End of dialogue");
        //         return;
        //     }
        //     if (Typewriter.IsTyping) // 检查是否正在打字
        //     {
        //         Typewriter.DisplayCompleteLine(); // 立即显示完整行
        //     }
        //     else
        //     {
        //         DialogueInterpreter.Dialogue data = CurrentStoryData[CurrentLine]; // 获取当前行数据
        //         CharacterName.text = data.Speaker; // 设置角色名
        //         Typewriter.DisplayLine(data.Content); // 开始打字
        //         CurrentLine++; // 增加当前行数
        //     }
        // }

        /// <summary>
        /// 显示当前行对话
        /// </summary>
        public static void DisplayCurrentLine()
        {
            if (CharacterName == null || DialogueText == null || Typewriter == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }
            if (Typewriter.IsTyping) // 检查是否正在打字
            {
                Typewriter.DisplayCompleteLine(); // 立即显示完整行
            }
            else
            {
                CharacterName.text = DialogueInterpreter.CurrentSpeaker; // 设置角色名
                Typewriter.DisplayLine(DialogueInterpreter.CurrentDialogue); // 开始打字
            }
        }

        /// <summary>
        /// 创建选择按钮
        /// </summary>
        /// <param name="choicesText">选择文本</param>
        /// <param name="spacing">间隔</param>
        public static void CreateChoiceButtons(string[] choicesText, int spacing)
        {
            if (ChoiceButtonContainer == null || ChoiceButtonPrefab == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }
            ClearChoiceButtons();
            ChoiceButtonContainer.spacing = spacing; // 设置按钮间距

            for (int i = 0; i < choicesText.Length; i++)
            {
                Button button = Instantiate(ChoiceButtonPrefab, ChoiceButtonContainer.transform); // 创建按钮
                button.name = "ChoiceButton" + i; // 设置按钮名称
                button.GetComponentInChildren<TextMeshProUGUI>().text = choicesText[i]; // 设置按钮文本
                int choiceIndex = i; // 捕获当前索引
                button.onClick.AddListener(() => { ChoiceInterpreter.SelectChoice(choiceIndex); }); // 设置按钮点击事件
            }
        }

        /// <summary>
        /// 清除选择按钮
        /// </summary>
        public static void ClearChoiceButtons()
        {
            if (ChoiceButtonContainer == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }

            foreach (Transform child in ChoiceButtonContainer.transform)
            {
                Destroy(child.gameObject); // 销毁所有按钮
            }
        }

        /// <summary>
        /// 显示或隐藏文本框
        /// </summary>
        /// <param name="show">是否显示</param>
        public static void ShowTextBox(bool show)
        {
            if (TextBox == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }

            TextBox.SetActive(show); // 设置文本框可见性
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <param name="size">大小</param>
        public static void SetFontSize(int size)
        {
            if (DialogueText == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }

            DialogueText.fontSize = size; // 设置字体大小
        }

        /// <summary>
        /// 点击的对象是否是按钮
        /// </summary>
        public static bool IsClickButton()
        {
            GameObject clickedObject = EventSystem.current.currentSelectedGameObject; // 获取当前选中对象
            if (clickedObject == null) // 检查是否有选中对象
                return false;
            return clickedObject.GetComponent<Button>() != null; // 检查是否为按钮
        }

        /// <summary>
        /// 是否应该执行下一行脚本
        /// </summary>
        public static bool ShouldExecuteNextLine()
        {
            return (Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse ScrollWheel") < 0) && 
                ParseScript.WaitClick && 
                !IsClickButton() && 
                !ChoiceInterpreter.OnChoosing &&
                !LogPanelActive &&
                TextBox.activeSelf &&
                !SaveLoadUiActive &&
                !ConfigUiActive;
        }

        /// <summary>
        /// 是否应该显示历史记录
        /// </summary>
        public static bool ShouldShowHistory()
        {
            return Input.GetAxis("Mouse ScrollWheel") > 0 &&
                !LogPanelActive &&
                !SaveLoadUiActive &&
                !ConfigUiActive;
        }

        /// <summary>
        /// 是否应该切换对话框可见性
        /// </summary>
        public static bool ShouldSwitchTextboxVisibility()
        {
            return Input.GetMouseButtonDown(1) &&
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
            SceneManager.UnloadSceneAsync("Config");
        }
    

    }
}