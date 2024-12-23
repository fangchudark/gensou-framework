using GensouLib.GenScript;
using Godot;

public partial class GenScriptInitialization : Node
{
    public override void _Ready()
    {
        ScriptReader.ReadAndExecute(this);
    }
}
