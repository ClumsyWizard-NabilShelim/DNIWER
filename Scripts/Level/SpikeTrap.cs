using Godot;
using System;

public class SpikeTrap : Area2D
{
    public GameManager GM;
    public bool Active;
    private AnimationTree Animator;
    private AnimationNodeStateMachinePlayback stateMachine;
    private bool CanAnimate = true;
    private AudioStream SpikeOut;
    public override void _Ready()
    {
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        SpikeOut = GD.Load<AudioStream>("res://Audio/spiketrap.wav");
        Animator = (AnimationTree)GetNode("Animator");
        Animator.Active = true;
        stateMachine = (AnimationNodeStateMachinePlayback)Animator.Get("parameters/playback");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Active)
        {
            if(stateMachine.GetCurrentNode() == "SpikeDefault" && CanAnimate)
            {
                CanAnimate = false;
                stateMachine.Travel("SpikeOut");
            }
        }
        else
        {
            if(stateMachine.GetCurrentNode() == "SpikeOut" && CanAnimate)
            {
                CanAnimate = false;
                stateMachine.Travel("SpikeIn");
            }
        }
    }

    private void Animate()
    {
        CanAnimate = true;
    }

    private void PlayeSoundEffect()
    {
        GM.PlayAudio(SpikeOut);
    }
}
