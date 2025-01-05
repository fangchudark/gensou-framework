using Godot;
using GensouLib.Godot.Core;
using GensouLib.GenScript;

public partial class VisualNoveManager : Node
{
    [Export]
    public Label characterName = null;
    [Export]
    public Label dialogueText = null;
    [Export]
    public DisplayController typewriterEffect = null;
    [Export]
    public TextureRect figureLeft = null;
    [Export]
    public TextureRect figureCenter = null;
    [Export]
    public TextureRect figureRight = null;
    [Export]
    public TextureRect portrait = null;
    [Export]
    public TextureRect background = null;
    [Export]
    public AudioStreamPlayer bgm = null;
    [Export]
    public AudioStreamPlayer bgs = null;
    [Export]
    public AudioStreamPlayer voice = null;
    [Export]
    public AudioStreamPlayer se = null;
    [Export]
    public VBoxContainer choiceButtonContainer = null;
    [Export]
    public PackedScene choiceButtonScene = null;
    [Export]
    public Panel textBox = null;
    [Export]
    public Button skipButton = null;
    [Export]
    public Button autoButton = null;
    [Export]
    public Button saveButton = null;
    [Export]
    public Button loadButton = null;
    [Export]
    public Button logButton = null;
    [Export]
    public Button systemButton = null;
    [Export]
    public Button titleButton = null;
    [Export]
    public VBoxContainer logContainer = null;
    [Export]
    public PackedScene logTextScene = null;
    [Export]
    public ScrollContainer scrollView = null;
    [Export]
    public Panel logPanel = null;
    [Export]
    public Button closeLogPanelButton = null;
    [Export]
    public int autoPlayInterval = 1;

    public override void _EnterTree()
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
            se,
            voice,
            this,
            choiceButtonContainer,
            choiceButtonScene,
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
            logTextScene,
            scrollView,
            logPanel,
            closeLogPanelButton
        );
    }
    public override void _Ready()
    {
        ScriptReader.ReadAndExecute("demo");
    }

}
