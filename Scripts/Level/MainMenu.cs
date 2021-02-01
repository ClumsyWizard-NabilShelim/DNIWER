using Godot;
using System;

public class MainMenu : Node2D
{
    public AudioStream SelectSound;
    private GameManager GM;

    public override void _Ready()
    {
        SelectSound = GD.Load<AudioStream>("res://Audio/select.wav");
        GM = (GameManager)GetNode("../GameManager");
    }

    private void _on_PlayButton_button_down()
    {
        GM.PlayAudio(SelectSound);
        GM.LoadScene("res://Scenes/CutScene1.tscn");
    }

    private void _on_Credit_button_down()
    {
        GM.PlayAudio(SelectSound);
        GM.LoadScene("res://Scenes/Credit.tscn");
    }
}
