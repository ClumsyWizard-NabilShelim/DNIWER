using Godot;
using System;

public class Credit : Control
{
    private GameManager GM;
    public AudioStream SelectSound;
    public override void _Ready()
    {
        SelectSound = GD.Load<AudioStream>("res://Audio/select.wav");
        GM = (GameManager)GetNode("GameManager");
    }

    private void _on_Twitter_button_down()
    {
        GM.PlayAudio(SelectSound);
        OS.ShellOpen("https://twitter.com/KnightDevGames");
    }

    private void _on_Instagram_button_down()
    {
        GM.PlayAudio(SelectSound);
        OS.ShellOpen("https://www.instagram.com/Knight_Dev_Games/");
    }

    private void _on_Website_button_down()
    {
        GM.PlayAudio(SelectSound);
        OS.ShellOpen("https://knightdevgames.netlify.app");
    }

    private void _on_MainMenu_button_down()
    {
        GM.PlayAudio(SelectSound);
        GM.LoadScene("res://Scenes/MainMenu.tscn");
    }
}
