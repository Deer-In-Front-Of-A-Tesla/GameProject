using Godot;
using System;

public class main : Node2D
{
    [Export]
    public NodePath _music;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var music = GetNode<GameMusic>(_music);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
