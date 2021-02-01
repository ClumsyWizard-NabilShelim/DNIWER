using Godot;
using System.Collections.Generic;

public class Tutorial : Area2D
{
    private GameManager GM;
    private bool ShowTutorial;
    private Node2D TutHolder;
    private List<Sprite> Tuts = new List<Sprite>();
    private Button Next;
    private Button Previous;
    private Button Close;
    private int tutIndex = 0;
    private AudioStream Click;
    public override void _Ready()
    {
        TutHolder = (Node2D)GetNode("TutorialHolder");
        for (int i = 0; i < TutHolder.GetChildCount(); i++)
        {
            Tuts.Add((Sprite)TutHolder.GetChild(i));
            Tuts[i].Visible = false;
        }
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        Click = GD.Load<AudioStream>("res://Audio/select.wav");
        TutHolder.Visible = false;
        Next = (Button)GetNode("Next");
        Next.Disabled = true;
        Next.Visible = false;
        Previous = (Button)GetNode("Previous");
        Previous.Disabled = true;
        Previous.Visible = false;
        Close = (Button)GetNode("Close");
        Close.Disabled = true;
        Close.Visible = false;
        PauseMode = PauseModeEnum.Process;
    }

    public override void _Process(float delta)
    {
        if(TutHolder.Visible)
        {
            if(Tuts.Count == 1)
            {
                Next.Disabled = true;
                Next.Visible = false;
                Previous.Disabled = false;
                Previous.Visible = false;
                Close.Disabled = false;
                Close.Visible = true;    
            }
            else
            {
                if(tutIndex == 0)
                {
                    Next.Disabled = false;
                    Next.Visible = true;
                    Previous.Disabled = true;
                    Previous.Visible = false;
                    Close.Disabled = true;
                    Close.Visible = false;    
                }
                else if(tutIndex > 0 && tutIndex < Tuts.Count - 1)
                {
                    Next.Disabled = false;
                    Next.Visible = true;
                    Previous.Disabled = false;
                    Previous.Visible = true;
                    Close.Disabled = true;
                    Close.Visible = false;    
                }
                else if(tutIndex == Tuts.Count - 1)
                {
                    Next.Disabled = true;
                    Next.Visible = false;
                    Previous.Disabled = false;
                    Previous.Visible = true;
                    Close.Disabled = false;
                    Close.Visible = true;    
                }
            }

        }
    }

    private void _on_Next_pressed()
    {
        GM.PlayAudio(Click);
        if(TutHolder.Visible && tutIndex < Tuts.Count - 1)
        {
            Tuts[tutIndex].Visible = false;
            tutIndex++;
            Tuts[tutIndex].Visible = true;
        }
    }

    private void _on_Previous_pressed()
    {
        GM.PlayAudio(Click);
        if(TutHolder.Visible && tutIndex > 0)
        {
            Tuts[tutIndex].Visible = false;
            tutIndex--;
            Tuts[tutIndex].Visible = true;
        }
    }

    private void _on_Close_pressed()
    {
        GM.PlayAudio(Click);
        tutIndex = 0;
        TutHolder.Visible = false;
        Next.Disabled = true;
        Next.Visible = false;
        Previous.Disabled = true;
        Previous.Visible = false;
        Close.Disabled = true;
        Close.Visible = false;
        GetTree().Paused = false;
        QueueFree();
    }

    private void _on_Tutorial_body_entered(Node body)
    {
        TutHolder.Visible = true;
        Tuts[tutIndex].Visible = true;
        Next.Visible = true;
        Next.Disabled = false;
        GetTree().Paused = true;
    }
}
