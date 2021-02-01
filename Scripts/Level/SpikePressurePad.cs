using Godot;
using System;

public class SpikePressurePad : Area2D
{
    public GameManager GM;
    public RewindObject RO;
    public Recorder recorder;
    private AnimatedSprite GFX;
    [Export] public string Trap;
    private SpikeTrap ST;
    public bool Active;
    private AnimatedSprite Outline;
    private AudioStream Trigger;
    public override void _Ready()
    {
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");
        RO = (RewindObject)GetNode("RewindAble");
        Outline = (AnimatedSprite)GetNode("RewindAble/Outline");
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Trigger = GD.Load<AudioStream>("res://Audio/pressurepad.wav");
        ST= (SpikeTrap)GetNode("../" + Trap);
        GFX = (AnimatedSprite)GetNode("GFX");
        GFX.Frame = 0;
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
        

        if(!Active)
        {
            ST.Active = false;
            Outline.Stop();
            Outline.Frame = 0;
            GFX.Stop();
            GFX.Frame = 0;
        }
        else
        {
            ST.Active = true;
            Outline.Play();
            GFX.Play();
        }
    }

    private void _on_SpikePressurePad_body_entered(Node body)
    {
        GM.PlayAudio(Trigger);
        Active = !Active;
    }
}
