using Godot;
using System;

// This scene is to test beat settings of a song. Loads one song, plays it, and shows the strength of your beat
// any time you press the arrow up key. Can be used to roughly calibrate song offsets and test the parameters that
// go into strength calculation
// Written by Red

public class beat_test : Node2D
{
	[Export] public NodePath _song_player;

	[Export] public NodePath _export_label;

	private playlist player;
	private Label label;

	public override void _Ready()
	{
		player = GetNode<playlist>(_song_player);
		label = GetNode<Label>(_export_label);
		player.PlayRandom();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_up", true))
		{
			label.Text = player.GetCurrentSong().GetCurrentStrength().ToString("F3");
			player.GetCurrentSong().AdjustPersonalOffset();
		}
		base._Input(@event);
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		var song = player.GetCurrentSong();
		if (song != null)
		{
			var str = song.GetCurrentStrength();
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
}
