using GensouLib.Godot.Core;
using Godot;

public partial class SaveLoad : Node
{
    [Export]
    public int MaxSlots = 20;

    [Export]
    public PackedScene SaveSlotScene;
    
    [Export]
    public Button CloseButton;

    [Export]
    public Label PanelTitle;

    [Export]
    public VBoxContainer SaveSlotsContainer;

    [Export]
    public string TimestampNodePath = "Timestamp";

    [Export]
    public string DialogueNodePath = "Dialogue";

    [Export]
    public string ScreenshotNodePath = "Screenshot";

    public override void _EnterTree()
    {
        SaveLoadGame.Init(
            SaveSlotScene,
            CloseButton,
            PanelTitle,
            SaveSlotsContainer,
            MaxSlots,
            TimestampNodePath,
            DialogueNodePath,
            ScreenshotNodePath
        );
    }

    public override async void _Ready()
    {
        await SaveLoadGame.CreateSlots();
    }
}
