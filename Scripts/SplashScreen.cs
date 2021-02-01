using Godot;
using System;

public class SplashScreen : Control
{
    [Export] public string SceneName;
    private AnimationPlayer Animation;
    private Timer timer;

    public override void _Ready()
    {
        timer = (Timer)GetNode("Screen/Timer");
        timer.Start();
        Animation = (AnimationPlayer)GetNode("Screen/AnimationPlayer");
        timer.Connect("timeout", this, "Swicth");
    }

    private void Swicth()
    {
        Animation.Play("In");
    }
    private void ChangeScene()
    {
        GetTree().ChangeScene("res://Scenes/" + SceneName + ".tscn");
    }
}
