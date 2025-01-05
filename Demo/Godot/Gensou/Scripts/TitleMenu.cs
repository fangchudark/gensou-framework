using GensouLib.Godot.Core;
using Godot;
using GodotPlugins.Game;
using System;

public partial class TitleMenu : Node
{
    [Export]
    public string MainScenePath = "res://Scenes/Main.tscn";

    [Export]
    public string SaveLoadScenePath = "res://Scenes/SaveLoad.tscn";
    
    [Export]
    public string ConfigScenePath = "res://Scenes/Config.tscn";

    public override void _Ready()
    {
        SaveLoadGame.LoadConfig();
        VisualNoveCore.RecoverGlobalVariables();
    }

    private void OnNewGameButtonPressed()
    {
        GetTree().ChangeSceneToFile(MainScenePath);
    }

    private void OnLoadButtonPressed()
    {
        SaveLoadGame.ShowSaveLoadMenu(GetTree().Root, SaveLoadScenePath);
    }

    private void OnConfigButtonPressed()
    {
        TextboxFunctions.OpenConfigUi(ConfigScenePath, GetTree().Root); 
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
