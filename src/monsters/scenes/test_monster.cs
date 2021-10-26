using Godot;
using System;

public class test_monster : Node2D
{
    [Export] private NodePath _master;

    public dungeon_master master;
    
    public override void _Ready()
    {
        master = GetNode<dungeon_master>(_master);
        master.SpawnMonster("bat");
        master.song_player.PlaySong("1");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
