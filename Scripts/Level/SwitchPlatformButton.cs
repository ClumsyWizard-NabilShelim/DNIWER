using Godot;
using System;

public class SwitchPlatformButton : Area2D
{
    private GameManager GM;
    private AnimatedSprite GFX;
    [Export] public string Platform;
    private SwitchPlatform SP;
    public bool Active;
    private AudioStream Trigger;
    public override void _Ready()
    {
        SP = (SwitchPlatform)GetNode("../" + Platform);
        GFX = (AnimatedSprite)GetNode("GFX");
        GFX.Frame = 0;
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Trigger = GD.Load<AudioStream>("res://Audio/pickup.wav");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!Active)
        {
            GFX.Stop();
            GFX.Frame = 0;
        }
        else
        {
            GFX.Play();
        }
    }

    private void _on_SwitchPlatformButton_body_entered(Node body)
    {
        GM.PlayAudio(Trigger);
        SP.Switch = !SP.Switch;
        Active = !Active;
    }
}
