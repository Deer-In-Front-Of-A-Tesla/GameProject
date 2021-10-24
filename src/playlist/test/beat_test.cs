using Godot;
using System;

public class beat_test : Node2D
{
    [Export] public NodePath _song_player;

    [Export] public NodePath _export_label;

    private playlist player;
    private Label label;
    private playlist.GameSong currentSong;

    public override void _Ready()
    {
        player = GetNode<playlist>(_song_player);
        label = GetNode<Label>(_export_label);
        currentSong = player.PlaySong("1");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("ui_up", true))
        {
            label.Text = currentSong.GetCurrentStrength().ToString("F3");
            
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        var str = currentSong.GetCurrentStrength();
        if (str > 1)
        {
            //label.Text = str.ToString("F3");
            label.Visible = true;
        }
        else
        {
            //label.Visible = false;
        }
        
    }
}
