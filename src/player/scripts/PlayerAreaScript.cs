using Godot;
using System;

public class PlayerAreaScript : Node
{
	private int meleeDamage;
	private string lastAttackDir;
	
	public override void _Ready()
	{
		meleeDamage = (int) this.GetParent().Get("meleeDamage");
		
		this.Connect("body_entered", this, nameof(DetectCollision));
	}

	public void DetectCollision(PhysicsBody2D node)
	{
		if (node.CollisionLayer == 16)
		{
			node.Call("TakeDamage", meleeDamage);
		}
	}

	public void LookForCollision(string direction, bool status)
	{
		if (status == false) direction = lastAttackDir; 
		else lastAttackDir = direction ;
		
		if (direction == "Up")
		{
			CollisionPolygon2D node = (CollisionPolygon2D) GetNode("AttackUpCollision");
			node.Disabled = status;
		} else
		if (direction == "Down")
		{
			CollisionPolygon2D node = (CollisionPolygon2D) GetNode("AttackDownCollision");
			node.Disabled = status;
		} else
		if (direction == "Right")
		{
			CollisionPolygon2D node = (CollisionPolygon2D) GetNode("AttackRightCollision");
			node.Disabled = status;
		} else
		if (direction == "Left")
		{
			CollisionPolygon2D node = (CollisionPolygon2D) GetNode("AttackLeftCollision");
			node.Disabled = status;
		}
	}
}
