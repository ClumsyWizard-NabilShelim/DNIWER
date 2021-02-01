using Godot;
using System;

public class VineDoorPressurePad : Area2D
{
    public GameManager GM;
    private KinematicBody2D EnterBody;
    private AnimatedSprite GFX;
    [Export] public string Door;
    private VineDoor VD;
    public bool Active;
    private AudioStream Trigger;
    
    public override void _Ready()
    {
        VD = (VineDoor)GetNode("../" + Door);
        GFX = (AnimatedSprite)GetNode("GFX");
        GFX.Frame = 0;
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Trigger = GD.Load<AudioStream>("res://Audio/pressurepad.wav");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!Active)
        {
            VD.Active = false;
            GFX.Stop();
            GFX.Frame = 0;
        }
        else
        {
            VD.Active = true;
            GFX.Play();
        }
    }

    private void _on_VineDoorPressurePad_body_entered(Node body)
    {
        if(EnterBody == null)
        {
            GM.PlayAudio(Trigger);
            EnterBody = (KinematicBody2D)body;
            Active = true;
        }
    }

    private void _on_VineDoorPressurePad_body_exited(Node body)
    {
        KinematicBody2D ExitBody = (KinematicBody2D)body;

        if(ExitBody == EnterBody)
        {
            GM.PlayAudio(Trigger);
            Active = false;
            EnterBody = null;
        }
    }
}
