using Godot;
using System;

public class song_play : Node2D
{
	[Export] private NodePath _master;

	public dungeon_master master;
	
	public override void _Ready()
	{
		master = GetNode<dungeon_master>(_master);
		master.SpawnMonster("bat");
		// master.song_player.PlayRandom();
	}
}
