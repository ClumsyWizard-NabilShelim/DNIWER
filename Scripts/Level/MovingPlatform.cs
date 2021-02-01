using Godot;
using System.Collections.Generic;

public class MovingPlatform : KinematicBody2D
{
    //private RewindAble RA;
    private Recorder recorder;
    [Export] public float MoveSpeed;
    [Export] public Vector2[] MovePos;
    private Vector2 TargetMovePos;
    private int I;
    private bool Flip;
    public bool Move = false;
    public override void _Ready()
    {
        I = 1;
      //  RA = (RewindAble)GetNode("RewindAble");
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");
        TargetMovePos = MovePos[I];
    }

    public override void _PhysicsProcess(float delta)
    {
        // if(!RA.Deselect)
        // {
            if(!recorder.Rewind)
            {
                MovePlat(delta);
            }
      //  }
      //  else
      //  {
            MovePlat(delta);
       // }
    }

    private void MovePlat(float _delta)
    {
        if(Move)
        {
            if(GlobalPosition.DistanceTo(MovePos[I]) > 0.2f)
            {
                GD.Print(MovePos.Length);
                GlobalPosition = new Vector2(Mathf.MoveToward(GlobalPosition.x, MovePos[I].x, MoveSpeed * _delta), Mathf.MoveToward(GlobalPosition.y, MovePos[I].y, MoveSpeed * _delta));
            }
            else
            {
                if(I == MovePos.Length - 1)
                    Flip = true;
                if(I == 0)
                    Flip = false;

                if(Flip)
                    I--;
                else
                    I++;
            }
        }
    }
}
