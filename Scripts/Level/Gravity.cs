using Godot;
using System;

public class Gravity : KinematicBody2D
{
    public bool NoGravity;
    private float GravityValue = 50;
    private float DefaultGravity;
    private bool GroundHit = true;
    private Vector2 MoveDir;
    private CollisionShape2D Col;
    public Sprite R;
    public Recorder recorder;
    private RewindObject RO;
    private Sprite Outline;
    public override void _Ready()
    {
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");
        RO = (RewindObject)GetNode("RewindAble");
        Outline = (Sprite)GetNode("RewindAble/Outline");
        R = (Sprite)GetNode("R");
        R.Visible = false;
        Col = (CollisionShape2D)GetNode("Collider");
        DefaultGravity = GravityValue;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(RO.Selected)
        {
            Outline.Modulate = new Color(0.3f, 1f, 0f, 1);
            if(recorder.Forward)
            {
                Col.Disabled = true;
            }
            else
            {
                Col.Disabled = false;
            }
            if(recorder.Record)
            {
                RO.Pos = GlobalPosition;
            }
            else if(recorder.Rewind || recorder.Forward)
            {
                Modulate = new Color(1f, 1f, 1f, 0.5f);
                GlobalPosition = RO.Pos;
            }     
            if(!recorder.Forward && !recorder.Rewind)
            {
                Modulate = new Color(1f, 1f, 1f, 1f);
            }  
        }
        else 
        {
            Col.Disabled = false;
            Outline.Modulate = new Color(1f, 1f, 1f, 1);
        }
        
        if(NoGravity)
        {
            GravityValue = 0;
            //Col.Disabled = true;
            R.Visible = false;
        }
        else
        {
            if(GravityValue == 0)
            {
                //Col.Disabled = false;
                GravityValue = DefaultGravity;
            }
        }
        if(IsOnFloor())
        {
            MoveDir.x = 0;
            if(GroundHit)
            {
                GroundHit = false;
            }
        }
        else
        {
            GroundHit = true;
        }
        if(RO.Selected)
        {
            if(!recorder.Rewind && !recorder.Forward)
            {
                MoveDir.y += GravityValue;
                MoveDir = MoveAndSlide(MoveDir, Vector2.Up);
            }
            else
            {
                if(NoGravity)
                    NoGravity = false;
            }
        }
        else
        {
            MoveDir.y += GravityValue;
            MoveDir = MoveAndSlide(MoveDir, Vector2.Up);
        }
    }

    public void Throw(float Force, Vector2 Dir)
    {
        MoveDir = Dir * Force;
    }
}
