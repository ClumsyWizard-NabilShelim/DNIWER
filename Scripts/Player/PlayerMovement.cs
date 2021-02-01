using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{
    private AudioStreamPlayer Walk;
    public bool StopMove = false;
    private bool GushingAgainstWall;
    public GameManager GM;
    public RewindObject RO;
    public Recorder recorder;
   //PHYSICS
    private Vector2 UP = new Vector2(0, -1);
    private float Friction = 0.7f;
    private float GravityScale;
    [Export] private float DefaultGravityScale = 50;

    //MOVEMENT
    private Vector2 MoveDir = Vector2.Zero;
    public Node2D GFX;

    //GROUND MOVEMENT
    private float InputX;
    [Export] private float MoveSpeed = 250;
    [Export] private float JumpForce = -600;

    //DETECTION
    private RayCast2D GroundCheck;
    private bool GroundHit = true;

    //WALL CHECK
    private RayCast2D WallCheck;

    //WALL JUMP
    private Vector2 WallJumpElivation = new Vector2(1.1f, -1.8f);
    [Export] private float WallJumpForce = 300;

    //ANIMATION
    private AnimationPlayer Animator;
    // private AnimationNodeStateMachinePlayback stateMachine;
    private bool GrabbedOntoWall;
    private AudioStream Jump;
    private Sprite Outline;
    
    public override void _Ready()
    {
        Walk = (AudioStreamPlayer)GetNode("Walk");
        GravityScale = DefaultGravityScale;
        GFX = (Node2D)GetNode("GFX");
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Jump = GD.Load<AudioStream>("res://Audio/jump.wav");
        Animator = (AnimationPlayer)GetNode("Animator");
        // Animator.Active = true;
        // stateMachine = (AnimationNodeStateMachinePlayback)Animator.Get("parameters/playback");
        RO = (RewindObject)GetNode("RewindAble");
        Outline = (Sprite)GetNode("RewindAble/Outline");
        WallCheck = (RayCast2D)GetNode("GFX/Raycasts/WallCheck");
        WallCheck.Enabled = true;
        GroundCheck = (RayCast2D)GetNode("GFX/Raycasts/GroundCheck");
        GroundCheck.Enabled = true;
    }

    public override void _Process(float delta)
    {
        //Player input
        InputX = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
    }
    public override void _PhysicsProcess(float delta)
    {
        Outline.Scale = new Vector2(GFX.Scale.x, Outline.Scale.y);
        //Gravity
        if(RO.Selected)
        {
            Outline.Modulate = new Color(0.3f, 1f, 0f, 1);
            if(!recorder.Rewind && !recorder.Forward)
            {
                MoveDir.y += GravityScale;
                Movement();
            }
            if(recorder.Record)
            {
                RO.Pos = GlobalPosition;
                RO.Sca = GFX.Scale;
            }
            else if(recorder.Rewind ||recorder.Forward)
            {
                Modulate = new Color(1f, 1f, 1f, 0.5f);
                GlobalPosition = RO.Pos;
                GFX.Scale = RO.Sca;
            }       
        }
        else 
        {
            Outline.Modulate = new Color(1f, 1f, 1f, 1);
            MoveDir.y += GravityScale;
            Movement();
        }

        //Ground Check
        if(GroundCheck.IsColliding())
        {
            if(Animator.CurrentAnimation == "WallGrab")
                Animator.Play("Idel");
            if(GroundHit)
            {
                GroundHit = false;
            }

            if(InputX == 0)
            {
                MoveDir.x = Mathf.Lerp(MoveDir.x, 0, Friction);
            }

            if(Input.IsActionJustPressed("ui_select"))
            {
                GM.PlayAudio(Jump);
                MoveDir.y = JumpForce;
            }
            if(WallCheck.IsColliding())
            {
                GushingAgainstWall = true;
            }
            else
            {
                GushingAgainstWall = false;
                GrabbedOntoWall = false;
            }
        }
        else
        {
            //Wall check
            if(WallCheck.IsColliding())
            {
                if(!GushingAgainstWall)
                {
                    Animator.Play("WallGrab");
                    if(Input.IsActionJustPressed("ui_select"))
                    {
                        GravityScale = DefaultGravityScale;
                        GM.PlayAudio(Jump);
                        MoveDir = new Vector2(WallJumpElivation.x * -GFX.Scale.x, WallJumpElivation.y) * WallJumpForce;
                        GFX.Scale = new Vector2(-GFX.Scale.x, GFX.Scale.y);
                    }else
                    {
                        GrabbedOntoWall = true;
                        MoveDir = Vector2.Zero;
                        GravityScale = 0;
                    }
                }
            }
            else
            {
                if(GravityScale == 0)
                {
                    GravityScale = DefaultGravityScale;
                }
                GrabbedOntoWall= false;
                GushingAgainstWall = false;
            }

            GroundHit = true;

            if(Input.IsActionJustReleased("ui_up") && MoveDir.y < JumpForce/2)
            {
                MoveDir.y = JumpForce/3;
            }

            if(InputX == 0)
            {
                MoveDir.x = Mathf.Lerp(MoveDir.x, 0, 0.01f);
            }
        }
        //Moving the player
        if(RO.Selected)
        {
            if(!recorder.Rewind && !recorder.Forward)
            {
                Modulate = new Color(1f, 1f, 1f, 1f);
                if(!StopMove)
                    MoveDir = MoveAndSlide(MoveDir, UP);
            }
        }
        else
        {
            if(!StopMove)
                MoveDir = MoveAndSlide(MoveDir, UP);
        }


        
    }

    private void Movement()
    {
        if(InputX != 0)
        {
            if(!Walk.Playing && !GroundHit)
            {
                Walk.Playing = true;
            }
            if(GroundHit)
            {
                Walk.Playing = false;
            }
            GFX.Scale = new Vector2(InputX, GFX.Scale.y);
            if(!GrabbedOntoWall)
            {
                MoveDir.x = Mathf.Lerp(MoveDir.x, InputX * MoveSpeed, 0.1f);
            }
        }
        else
        {
            Walk.Playing = false;
        }
    }

    public void KnockBack(float Force, Vector2 Dir)
    {
        MoveDir = -Dir * Force;
    }
}
