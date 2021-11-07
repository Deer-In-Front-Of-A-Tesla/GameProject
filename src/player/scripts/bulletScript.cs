using Godot;
using System;

public class bulletScript : Area2D
{
	public string strength = "mid";
	public int bulletSpeed = 100;
	public float dir = 0;
	public Vector2 pos = Vector2.Zero;
	public int damage = 0;
	public string stringOffsetDir = "Up";
	
	
	public override void _Ready()
	{
		this.GlobalPosition = pos;
		Rotation = dir;
		if (strength == "mid")
		{
			((Sprite) GetNode("midStrength")).Visible = true;
		}
		else
		{
			((Sprite) GetNode("maxStrength")).Visible = true;
		}

		Connect("body_entered", this, "detectCollision");
	}

	public void detectCollision(Node node)
	{
		node.Call("TakeDamage", damage);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Position += Transform.x * bulletSpeed * delta;
	}
}
