using Godot;
using System;

public class GameManager : Node2D
{
    private AnimationPlayer Anim;
    private bool Restart;
    private bool CanPause;
    private string Path;
    private AudioStreamPlayer SFX1; 
    private AudioStreamPlayer SFX2; 
    private AudioStreamPlayer SFX3; 
    public AudioStream SelectSound;
    private Timer DeathTimer;
    public override void _Ready()
    {
        DeathTimer = (Timer)GetNode("DeathTimer");
        DeathTimer.Connect("timeout", this, "Reload");
        SelectSound = GD.Load<AudioStream>("res://Audio/select.wav");        SFX1 = (AudioStreamPlayer)GetNode("SFX_1");
        SFX2 = (AudioStreamPlayer)GetNode("SFX_2");
        SFX3 = (AudioStreamPlayer)GetNode("SFX_3");
        Anim = (AnimationPlayer)GetNode("Animations");
        CanPause = true;
    }

    public void PlayAudio(AudioStream SFX)
    {
        if(SFX1.Stream == null)
        {
            SFX1.Stream = SFX;
            SFX1.Play();
        }
        else if(SFX2.Stream == null)
        {
            SFX2.Stream = SFX;
            SFX2.Play();
        }else
        {
            SFX3.Stream = SFX;
            SFX3.Play();
        }
    }

    public void StopAudio(AudioStream CurrentlyPlaying)
    {
        if(SFX1.Stream == CurrentlyPlaying)
        {
            SFX1.Stream = null;
        }
        else if(SFX2.Stream == CurrentlyPlaying)
        {
            SFX2.Stream = null;
        }
        else if(SFX3.Stream == CurrentlyPlaying)
        {
            SFX3.Stream = null;
        }

    }
    public void LoadScene(string LoadPath = "", bool RestartLevel = false)
    {
        Restart = RestartLevel;
        Path = LoadPath;
        Anim.Play("FadeIn");
    }

    private void Load()
    {
        if(GetTree().Paused)
        {
            GetTree().Paused = false;
        }
        if(Restart)
        {
            GetTree().ReloadCurrentScene();
        }
        else
        {
            GetTree().ChangeScene(Path);
        }
    }

    private void _on_Resume_button_down()
    {
        GetTree().Paused = false;
        PlayAudio(SelectSound);
        Anim.Play("PauseOut");
    }

    private void PauseEnd()
    {
        CanPause = true;
    }

    private void _on_PauseButton_button_down()
    {
        PlayAudio(SelectSound);
        if(CanPause)
        {
            GetTree().Paused = true;
            CanPause = false;
            Anim.Play("PauseIn");
        }
    }

    private void _on_MainMenu_button_down()
    {
        PlayAudio(SelectSound);
        LoadScene("res://Scenes/MainMenu.tscn");
    }

    private void _on_SFX_1_finished()
    {
        SFX1.Stream = null;
    }

    private void _on_SFX_2_finished()
    {
        SFX2.Stream = null;
    }

    private void _on_SFX_3_finished()
    {
        SFX3.Stream = null;
    }

    private void _on_RestartButton_button_down()
    {
        PlayAudio(SelectSound);
        LoadScene("", true);
    }

    public void RecordEffectIn()
    {
        Anim.Play("RecordEffectIn");
    }

    public void RecordEffectOut()
    {
        Anim.Play("RecordEffectOut");
    }
    public void LateLoad()
    {
        DeathTimer.Start();
    }

    private void Reload()
    {
        LoadScene("", true);
    }
}
