using Godot;
using System.Collections.Generic;

public class RewindObject : Area2D
{
    private Recorder recorder;
    private GameManager GM;
    [Export] private bool RecordPosition;
    [Export] private bool RecordScale;
    [Export] private bool RecordBool;

    private bool MouseOnTop;
    public bool Selected = false;

    private List<Vector2> RewindPositions = new List<Vector2>();
    public Vector2 Pos;
    private int RewindPosIndex = 0;

    private List<Vector2> RewindScales = new List<Vector2>();
    public Vector2 Sca;
    private int RewindScaleIndex = 0;

    private List<bool> RewindBools = new List<bool>();
    public bool Bool;
    private int RewindBoolIndex = 0;

    private List<Vector2> ForwardPositions = new List<Vector2>();
    private int ForwardPosIndex = 1;

    private List<Vector2> ForwardScales = new List<Vector2>();
    private int ForwardScaleIndex = 1;

    private List<bool> ForwardBools = new List<bool>();
    private int ForwardBoolIndex = 1;
    private AudioStream SelectSound;
    public override void _Ready()
    {
        GM = (GameManager)GetNode(GetTree().CurrentScene.GetPath() + "/GameManager");
        recorder = (Recorder)GetNode(GetTree().CurrentScene.GetPath() + "/PlayerCanvas");

        SelectSound = GD.Load<AudioStream>("res://Audio/recordselect.wav");
    }
    public override void _Process(float delta)
    {
        if(MouseOnTop && recorder.RewindCellLeft && !recorder.SelectClose)
        {   
            if(Input.IsActionJustReleased("SelectObject"))
            {
                GM.PlayAudio(SelectSound);
                Selected = !Selected;
                if(Selected)
                {
                    recorder.DecreaseCells();
                }
                else
                {
                    Selected = false;
                }
            }   
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Selected)
        {
            if(recorder.Record)
            {
                if(RecordPosition)
                {
                    RewindPositions.Insert(0, Pos);
                    ForwardPositions = RewindPositions;
                }
                if(RecordScale)
                {
                    RewindScales.Insert(0, Sca);
                    ForwardScales = RewindScales;
                }
                if(RecordBool)
                {
                    RewindBools.Insert(0, Bool);
                    ForwardBools = RewindBools;
                }
            }
            else if(recorder.Rewind)
            {
                if(RecordPosition)
                {
                    ForwardPosIndex = 1;
                    if(RewindPosIndex < RewindPositions.Count)
                    {
                        Pos = RewindPositions[RewindPosIndex];
                        RewindPosIndex++;
                    }
                    else
                    {
                        RewindPosIndex = 0;
                    }
                }
                if(RecordScale)
                {
                    ForwardScaleIndex = 1;
                    if(RewindScaleIndex < RewindScales.Count)
                    {
                        Sca = RewindScales[RewindScaleIndex];
                        RewindScaleIndex++;
                    }
                    else
                    {
                        RewindScaleIndex = 0;
                    }
                }
                if(RecordBool)
                {
                    ForwardBoolIndex = 1;
                    if(RewindBoolIndex < RewindBools.Count)
                    {
                        Bool = RewindBools[RewindBoolIndex];
                        RewindBoolIndex++;
                    }
                    else
                    {
                        RewindBoolIndex = 0;
                    }
                }
            }
            else if(recorder.Forward)
            {
                if(RecordPosition)
                {
                    RewindPosIndex = 0;
                    if(ForwardPosIndex <= ForwardPositions.Count)
                    {
                        Pos = ForwardPositions[ForwardPositions.Count - ForwardPosIndex];
                        ForwardPosIndex++;
                    }
                    else
                    {
                        ForwardPosIndex = 1;
                    }
                }
                if(RecordScale)
                {
                    RewindScaleIndex = 0;
                    if(ForwardScaleIndex <= ForwardScales.Count)
                    {
                        Sca = ForwardScales[ForwardScales.Count - ForwardScaleIndex];
                        ForwardScaleIndex++;
                    }
                    else
                    {
                        ForwardScaleIndex = 1;
                    }
                }
                if(RecordBool)
                {
                    RewindBoolIndex = 0;
                    if(ForwardBoolIndex <= ForwardBools.Count)
                    {
                        Bool = ForwardBools[ForwardBools.Count - ForwardBoolIndex];
                        ForwardBoolIndex++;
                    }
                    else
                    {
                        ForwardBoolIndex = 1;
                    }
                }
            }
        }

        if(recorder.Clear)
        {
            RewindPositions.Clear();
            RewindScales.Clear();
            RewindBools.Clear();
            ForwardPositions.Clear();
            ForwardScales.Clear();
            ForwardBools.Clear();
            Selected = false;
        }
    }

    private void _on_RewindAble_mouse_entered()
    {
        MouseOnTop = true;
    }

    private void _on_RewindAble_mouse_exited()
    {
        MouseOnTop = false;
    }
}
