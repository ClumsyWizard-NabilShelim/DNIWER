using Godot;
using System;

public class Lever : Area2D
{
    public RewindObject RO;
    public Recorder recorder;
    public GameManager GM;
    private AnimatedSprite GFX;
    private bool PlayerIn;
    [Export] public string Platform;
    private MovingPlatform MP;
    [Export] public bool Active;
    private AnimatedSprite Outline;
    private AudioStream Trigger;
    private Sprite R;
    public override void _Ready()
    {
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");
        RO = (RewindObject)GetNode("RewindAble");
        R = (Sprite)GetNode("R");
        R.Visible = false;
        Outline = (AnimatedSprite)GetNode("RewindAble/Outline");
        MP = (MovingPlatform)GetNode("../" + Platform);
        GFX = (AnimatedSprite)GetNode("GFX");
        GFX.Frame = 0;
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Trigger = GD.Load<AudioStream>("res://Audio/lever.wav");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(RO.Selected)
        {
            Outline.Modulate = new Color(0.3f, 1f, 0f, 1);
            if(recorder.Record)
            {
                RO.Bool = Active;
            }
            else if(recorder.Rewind ||recorder.Forward)
            {
                Modulate = new Color(1f, 1f, 1f, 0.5f);
                Active = RO.Bool;
            } 
            if(!recorder.Rewind && !recorder.Forward)
            {
                Modulate = new Color(1f, 1f, 1f, 1f);
            }
        }
        else
        {
            Outline.Modulate = new Color(1f, 1f, 1f, 1);
        }

        if(PlayerIn)
        {
            if(Input.IsActionJustPressed("PickUp"))
            {
                Active = !Active;
                GM.PlayAudio(Trigger);
            }
        }

        MP.Move = Active;

        if(!Active)
        {
            Outline.Stop();
            Outline.Frame = 0;
            GFX.Stop();
            GFX.Frame = 0;
        }
        else
        {
            Outline.Play();
            GFX.Play();
        }
    }

    private void _on_Lever_body_entered(Node body)
    {
        KinematicBody2D K = (KinematicBody2D)body;
        if(K.GetCollisionLayerBit(1))
        {
            PlayerIn = true;
            R.Visible = true;
        }
        else
        {
            Active = !Active;
            GM.PlayAudio(Trigger);
        }
    }

    private void _on_Lever_body_exited(Node body)
    {
        KinematicBody2D K = (KinematicBody2D)body;
        if(K.GetCollisionLayerBit(1))
        {
            PlayerIn = false;
            R.Visible = false;
        }
    }
}
