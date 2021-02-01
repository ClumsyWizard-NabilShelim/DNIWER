using Godot;
using System;

public class CutScene : Control
{
    [Export] private String SceneName;
    private void ChangeScene()
    {
        GetTree().ChangeScene("res://Scenes/" + SceneName + ".tscn");
    }
}
