using Godot;
using System;

public class bulletScript : Area2D
{
    public int bulletSpeed = 100;
    public float dir = 0;
    public Vector2 pos = Vector2.Zero;
    public int damage = 0;
    public string stringOffsetDir = "Up";
    
    
    public override void _Ready()
    {
        this.GlobalPosition = pos;
        Rotation = dir;
    }

    public override void _PhysicsProcess(float delta)
    {
        Position += Transform.x * bulletSpeed * delta;
    }
}
