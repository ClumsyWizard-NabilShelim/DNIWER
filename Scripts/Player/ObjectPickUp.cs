using Godot;
using System;

public class ObjectPickUp : Area2D
{
    private PlayerMovement PM;
    private float ThrowForce = 700f;
    private Position2D HoldPos;
    private Gravity Crate;
    private bool Holding;
    private AudioStream PickUp;

    public override void _Ready()
    {
        PM = (PlayerMovement)GetNode("../");
        PickUp = GD.Load<AudioStream>("res://Audio/pickup.wav");
        HoldPos = (Position2D)GetNode("../GFX/HoldPos");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(PM.RO.Selected)
        {
            if(!PM.recorder.Rewind && !PM.recorder.Forward)
            {
                PickUP();
            }
        }
        else
        {
            PickUP();
        }
    }

    private void PickUP()
    {
        if(Crate != null)
        {
            if(Input.IsActionJustPressed("PickUp"))
            {
                Holding = !Holding;
                if(!Holding)
                {
                    PM.GM.PlayAudio(PickUp);
                    Crate.NoGravity = false;
                    Crate.R.Visible = true;
                }
                else
                {
                    PM.GM.PlayAudio(PickUp);
                    Crate.NoGravity = true;
                }
            }
            if(Input.IsActionJustPressed("Throw"))
            {
                Holding = false;
                Vector2 ThrowDir = (GetGlobalMousePosition() - this.GlobalPosition).Normalized();
                Crate.Throw(ThrowForce, ThrowDir);
                PM.KnockBack(ThrowForce, ThrowDir);
                Crate.NoGravity = false;
                Crate.GlobalPosition = HoldPos.GlobalPosition;
                Crate.R.Visible = false;
                Crate = null;
            }

            if(Holding && !PM.recorder.Rewind && !PM.recorder.Forward)
            {
                Crate.GlobalPosition = HoldPos.GlobalPosition;//new Vector2(Mathf.Lerp(Crate.GlobalPosition.x, HoldPos.GlobalPosition.x, 0.5f * delta), Mathf.Lerp(Crate.GlobalPosition.y, HoldPos.GlobalPosition.y, 0.5f * delta));
            }
        }
    }

    private void _on_PickRadius_area_entered(Area2D area)
    {
        Crate = (Gravity)area.GetParent();
        Crate.R.Visible = true;
    }

    private void _on_PickRadius_area_exited(Area2D area)
    {
        if(Crate != null)
            Crate.R.Visible = false;

        Crate = null;
        Holding = false;
    }
}
