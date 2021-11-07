using Godot;
using System;

public class bulletScript : Area2D
{
    public string strength = "mid";
    public int bulletSpeed = 100;
    public float dir = 0;
    public Vector2 pos = Vector2.Zero;
    public int damage = 2;
    public string stringOffsetDir = "Up";
    
    
    public override void _Ready()
    {
        this.GlobalPosition = pos;
        Rotation = dir;
        if (strength == "mid")
        {
            ((Sprite) GetNode("midStrength")).Visible = true;
            damage = damage / 2;
        }
        else
        {
            ((Sprite) GetNode("maxStrength")).Visible = true;
        }

        Connect("body_entered", this, "detectCollision");
    }

    public void detectCollision(PhysicsBody2D node)
    {
        if (node.CollisionLayer == 2)
        {
            annihilateNode();
        }
        else
        {
            node.Call("TakeDamage", damage);
            annihilateNode();
        }
        
    }
    
    public override void _PhysicsProcess(float delta)
    {
        Position += Transform.x * bulletSpeed * delta;
    }

    public void annihilateNode()
    {
        QueueFree();
    }
}
