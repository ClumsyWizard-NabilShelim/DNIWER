using Godot;
using System;

public class VineDoor : StaticBody2D
{
    public GameManager GM;
    public bool Active;
    private AnimationTree Animator;
    private AnimationNodeStateMachinePlayback stateMachine;
    private AudioStream Vine;
    private bool CanAnimate = true;

    public override void _Ready()
    {
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Vine = GD.Load<AudioStream>("res://Audio/vinedoor.wav");
        Animator = (AnimationTree)GetNode("Animator");
        Animator.Active = true;
        stateMachine = (AnimationNodeStateMachinePlayback)Animator.Get("parameters/playback");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Active)
        {
            if(stateMachine.GetCurrentNode() == "DoorDefault" && CanAnimate)
            {
                CanAnimate = false;
                stateMachine.Travel("DoorOpen");
            }
        }
        else
        {
            if(stateMachine.GetCurrentNode() == "DoorOpen" && CanAnimate)
            {
                CanAnimate = false;
                stateMachine.Travel("DoorClose");
            }
        }
    }

    private void Animate()
    {
        CanAnimate = true;
    }

    private void PlayeSoundEffect()
    {
        GM.PlayAudio(Vine);
    }
}
