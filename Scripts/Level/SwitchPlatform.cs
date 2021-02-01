using Godot;
using System;

public class SwitchPlatform : Node2D
{
    public bool Switch;
    private Vector2 StartPos;
    private Vector2 EndPos;
    private float StartRot;
    private float EndRot;
    private StaticBody2D Platform;
    private Node2D PlatformMovePos;

    public override void _Ready()
    {
        Platform = (StaticBody2D)GetNode("Platform");
        PlatformMovePos = (Node2D)GetNode("PlatformMovePos");
        StartPos = Platform.Position;
        EndPos = PlatformMovePos.Position;
        StartRot = Platform.Rotation;
        EndRot = PlatformMovePos.Rotation;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Switch)
        {
            Switch = false;
            Platform.Position = EndPos;
            PlatformMovePos.Position = StartPos;
            Platform.Rotation = EndRot;
            PlatformMovePos.Rotation = StartRot;
            StartPos = Platform.Position;
            EndPos = PlatformMovePos.Position;
            StartRot = Platform.Rotation;
            EndRot = PlatformMovePos.Rotation;
        }
    }
}
