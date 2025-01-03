using GensouLib.GenScript;
using GensouLib.GenScript.Interpreters;
using GensouLib.Unity.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualNoveManager : MonoBehaviour
{
    public TextMeshProUGUI characterName = null;
    public TextMeshProUGUI dialogueText = null;
    public DisplayController typewriterEffect = null;
    public Image figureLeft = null;
    public Image figureCenter = null;
    public Image figureRight = null;
    public Image portrait = null;
    public Image background = null;
    public AudioSource bgm = null;
    public AudioSource bgs = null;
    public AudioSource voice = null;
    public AudioSource se = null;
    public VerticalLayoutGroup choiceButtonContainer = null;
    public Button choiceButtonPrefab = null;
    public GameObject textBox = null;
    public Button skipButton = null;
    public Button autoButton = null;
    public Button saveButton = null;
    public Button loadButton = null;
    public Button logButton = null;
    public Button systemButton = null;
    public Button titleButton = null;
    public RectTransform logContainer = null;
    public GameObject logTextPrefab = null;
    public ScrollRect ScrollView = null;
    public GameObject logPanel = null;
    public Button closeLogPanelButton = null;
    public Camera targetCamera = null;
    public int screenshotWidth = 1920;
    public int screenshotHeight = 1080;
    public int autoPlayInterval = 1;
    private void Awake()
    {
        VisualNoveCore.Init(
            characterName,
            dialogueText,
            typewriterEffect,
            figureLeft,
            figureCenter,
            figureRight,
            portrait,
            background,
            bgm,
            bgs,
            voice,
            se,
            choiceButtonContainer,
            choiceButtonPrefab,
            textBox,
            autoPlayInterval
        );
        TextboxFunctions.Init(
            skipButton,
            autoButton,
            saveButton,
            loadButton,
            logButton,
            systemButton,
            titleButton,
            logContainer,
            logTextPrefab,
            ScrollView,
            logPanel,
            closeLogPanelButton
        );
        ScreenshotToRawImage.Init(
            targetCamera, 
            screenshotWidth, 
            screenshotHeight
        );
        ScriptReader.ReadAndExecute("demo");
    }

    private void Update()
    {
        if (VisualNoveCore.ShouldExecuteNextLine())
        {
            VisualNoveCore.StopAutoPlayAndSkip();
            BaseInterpreter.ExecuteNextLine();
        }

        if (VisualNoveCore.ShouldShowHistory())
        {
            TextboxFunctions.ShowHistory();
        }

        if (VisualNoveCore.ShouldSwitchTextboxVisibility())
        {
            TextboxFunctions.SwitchTextboxVisibility();
        }
    }

}
