using Godot;
using System;

public class Effect : Particles2D
{
    private Timer DestroyTimer;
    public override void _Ready()
    {
        DestroyTimer = (Timer)GetNode("DestroyTimer");
        DestroyTimer.Connect("timeout", this, "Destroy");
        DestroyTimer.Start();
        OneShot = true;
    }

    private void Destroy()
    {
        QueueFree();
    }
}
