using Godot;
using System;

public class playlist : Node2D
{
	[Export]
	public Resource _music;

	public override void _Ready()
	{
//		var music = GetNode(_music);
		GD.Print(_music.Get("name"));
		GD.Print(_music.Get("beats_per_tact"));
	}
}
