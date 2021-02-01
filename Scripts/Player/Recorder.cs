using Godot;
using System;
using System.Collections.Generic;

public class Recorder : Node2D
{
    private GameManager GM;
    private int RewindCellCount = 4;
    public bool RewindCellLeft = true;
    public bool Record;
    public bool Rewind;
    public bool Forward;
    public bool Clear;
    public bool CanRewind;
    public bool SelectClose;

    private Timer RecordTimer;
    private Timer RewindTimer;
    private Timer ForwardTimer;
    private TextureRect RecordBar;
    private TextureRect RecordedBar;

    private float RecordDecreaseAmount;
    private float RewindDecreaseAmount;
    private TextureRect RecordButton;
    private TextureRect CancelButton;
    private TextureRect RewindButton;
    private TextureRect ForwardButton;

    private TextureRect Record_1;
    private TextureRect Recorded_1;
    private TextureRect Record_2;
    private TextureRect Recorded_2;
    private TextureRect Record_3;
    private TextureRect Recorded_3;
    private TextureRect Record_4;
    private TextureRect Recorded_4;

    private AudioStream ButtonPress;
    private AudioStream RecordingSound;
    private AudioStream RewindSound;
    private AudioStream ForwardSound;

    public override void _Ready()
    {
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        ButtonPress = GD.Load<AudioStream>("res://Audio/recorderpress.wav");
        RecordingSound = GD.Load<AudioStream>("res://Audio/recordingsound.wav");
        RewindSound = GD.Load<AudioStream>("res://Audio/rewind.wav");
        ForwardSound = GD.Load<AudioStream>("res://Audio/forward.wav");

        Record_1 = (TextureRect)GetNode("PlayerUI/RecordCell_1/RecordBar");
        Recorded_1 = (TextureRect)GetNode("PlayerUI/RecordCell_1/RecordedBar");

        Record_2 = (TextureRect)GetNode("PlayerUI/RecordCell_2/RecordBar");
        Recorded_2 = (TextureRect)GetNode("PlayerUI/RecordCell_2/RecordedBar");

        Record_3 = (TextureRect)GetNode("PlayerUI/RecordCell_3/RecordBar");
        Recorded_3 = (TextureRect)GetNode("PlayerUI/RecordCell_3/RecordedBar");

        Record_4 = (TextureRect)GetNode("PlayerUI/RecordCell_4/RecordBar");
        Recorded_4 = (TextureRect)GetNode("PlayerUI/RecordCell_4/RecordedBar");

        RecordBar = Record_4;
        RecordedBar = Recorded_4;

        RecordTimer = (Timer)GetNode("RecordTimer");
        RecordTimer.Connect("timeout", this, "RecordDone");

        RecordDecreaseAmount = RecordBar.RectSize.x / RecordTimer.WaitTime;

        Record_1.RectSize = new Vector2(0, Record_1.RectSize.y);
        Recorded_1.RectSize = new Vector2(0, Recorded_1.RectSize.y);

        Record_2.RectSize = new Vector2(0, Record_2.RectSize.y);
        Recorded_2.RectSize = new Vector2(0, Recorded_2.RectSize.y);

        Record_3.RectSize = new Vector2(0, Record_3.RectSize.y);
        Recorded_3.RectSize = new Vector2(0, Recorded_3.RectSize.y);

        Record_4.RectSize = new Vector2(0, Record_4.RectSize.y);
        Recorded_4.RectSize = new Vector2(0, Recorded_4.RectSize.y);

        RewindTimer = (Timer)GetNode("RewindTimer");
        RewindTimer.Connect("timeout", this, "RewindStop");
        RewindTimer.WaitTime = 0.001f;

        RecordButton = (TextureRect)GetNode("PlayerUI/RecordButton");
        CancelButton = (TextureRect)GetNode("PlayerUI/CancelButton");
        RewindButton = (TextureRect)GetNode("PlayerUI/RewindButton");
        ForwardButton = (TextureRect)GetNode("PlayerUI/ForwardButton");

        ForwardTimer = (Timer)GetNode("ForwardTimer");
        ForwardTimer.WaitTime = 0.001f;
        ForwardTimer.Connect("timeout", this, "ForwardStop");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(RewindCellLeft)
        {
            if(Input.IsActionJustPressed("F") && !CanRewind)
            {
                Recording();
            }
            if(Input.IsActionJustPressed("X") && !Rewind && !Record && CanRewind && CancelButton.Visible)
            {
                Cancel();
            }
            if(Input.IsActionJustReleased("X") && !Rewind && !Record && CancelButton.Visible)
            {
                CancelUp();
            }
            if(Input.IsActionJustPressed("G") && CanRewind && RewindButton.Visible)
            {
                Rewinding();
            }
            if(Input.IsActionJustPressed("H") && ForwardButton.Visible)
            {
                Forwarding();
            }

            if(Record)
            {
                if(!RecordTimer.IsStopped())
                {
                    if(RecordBar.RectSize.x < 52)
                    {
                        RecordBar.RectSize = new Vector2(RecordBar.RectSize.x + (RecordDecreaseAmount * delta), RecordBar.RectSize.y);
                        RecordedBar.RectSize = new Vector2(RecordedBar.RectSize.x + (RecordDecreaseAmount * delta), RecordedBar.RectSize.y);
                    }
                }
            }

            if(Rewind)
            {
                if(!RewindTimer.IsStopped())
                {
                    if(RewindDecreaseAmount == 0)
                        RewindDecreaseAmount = RecordBar.RectSize.x / RewindTimer.WaitTime;

                    RecordBar.RectSize = new Vector2(RecordBar.RectSize.x - (RewindDecreaseAmount * delta), RecordBar.RectSize.y);
                }
            }

            if(Forward)
            {
                if(!ForwardTimer.IsStopped())
                {
                    RecordBar.RectSize = new Vector2(RecordBar.RectSize.x + (RewindDecreaseAmount * delta), RecordBar.RectSize.y);
                }
            }    
        }
        else
        {
            Clear = true;
        }

        if(Clear)
        {
            RecordDone();
            RewindStop();
            ForwardStop();
            RewindTimer.Stop();
            RewindTimer.WaitTime = 0.001f;
            ForwardTimer.Stop();
            ForwardTimer.WaitTime = 0.001f;
            CanRewind = false;
            SelectClose = false;
            RewindDecreaseAmount = 0;
        }
    }

    private void Recording()
    {
        if(Visible && RewindCellLeft)
        {
            SelectClose = true;
            GM.PlayAudio(ButtonPress);
            GM.RecordEffectIn();
            Record = !Record;
            if(!Record)
            {
                GM.StopAudio(RecordingSound);
                RecordDone();
            }
            else
            {
                GM.PlayAudio(RecordingSound);
                RecordTimer.Start();
            }
        }
    }

    public void DecreaseCells()
    {
        RewindCellCount--;
        TextureRect R = (TextureRect)RecordBar.GetParent();
        R.Modulate  = new Color(0.2f, 0.2f, 0.2f, 1f);
        RecordBar.RectSize = new Vector2(52, 20);
        
        if(RewindCellCount == 3)
        {
            RecordBar = Record_3;
            RecordedBar = Recorded_3;
        }
        else if(RewindCellCount == 2)
        {
            RecordBar = Record_2;
            RecordedBar = Recorded_2;
        }
        else if(RewindCellCount == 1)
        {
            RecordBar = Record_1;
            RecordedBar = Recorded_1;
        }
        else if(RewindCellCount == 0)
        {
            RewindCellLeft = false;
        }
    } 
    private void Cancel()
    {
        if(!Rewind && !Forward && RewindCellLeft)
        {
            Clear = true;
        }
    }

    private void CancelUp()
    {
        if(!Rewind && !Forward && RewindCellLeft)
        {
            GM.PlayAudio(ButtonPress);
            Clear = false;
            DecreaseCells();
            CancelButton.Visible = false;
            RecordButton.Visible = true;
        }
    }

    private void RecordDone()
    {
        if(!Clear)
            GM.RecordEffectOut();

        GM.StopAudio(RecordingSound);
        if(RewindCellLeft)
        {
            if(RewindTimer.WaitTime == 0.001f)
                RewindTimer.WaitTime = RecordTimer.WaitTime - RecordTimer.TimeLeft;
            if(ForwardTimer.WaitTime == 0.001f)
                ForwardTimer.WaitTime = RecordTimer.WaitTime - RecordTimer.TimeLeft;

            CanRewind = true;
            RecordTimer.Stop();
            CancelButton.Visible = true;
            RecordButton.Visible = false;
            Record = false;
        }
    }

    private void Rewinding()
    {
        if(Visible && CanRewind && RewindCellLeft && !Rewind)
        {
            GM.PlayAudio(RewindSound);
            GM.PlayAudio(ButtonPress);
            Rewind = true;
            if(RewindTimer.IsStopped())
                RewindTimer.Start();
        }
    }
    
    private void RewindStop()
    {
        GM.StopAudio(RewindSound);
        if(RewindCellLeft)
        {
            RewindButton.Visible = false;
            ForwardButton.Visible = true;
            Rewind = false;
        }
    }

    private void Forwarding()
    {
        if(Visible && !Rewind && RewindCellLeft && !Forward)
        {
            GM.PlayAudio(ForwardSound);
            GM.PlayAudio(ButtonPress);
            Forward = true;
            if(ForwardTimer.IsStopped())
                ForwardTimer.Start();
        }
    }

    private void ForwardStop()
    {
        GM.StopAudio(ForwardSound);
        if(RewindCellLeft)
        {
            RewindButton.Visible = true;
            ForwardButton.Visible = false;
            Forward = false;
        }
    }

}
