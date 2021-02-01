using Godot;
using System;

public class ExitDoor : Area2D
{
    [Export] public string LevelName;
    private GameManager GM;
    public override void _Ready()
    {
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
    }

    private void _on_ExitDoor_body_entered(Node body)
    {
        PlayerMovement PM = (PlayerMovement)body;
        PM.StopMove = true;
        GM.LoadScene("res://Scenes/" + LevelName + ".tscn");
    }

}
