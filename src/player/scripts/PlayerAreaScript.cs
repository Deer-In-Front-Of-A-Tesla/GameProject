using Godot;
using System;

public class PlayerAreaScript : Area2D
{
	private int meleeDamage;
	private string lastAttackDir = "Right";


	public void DetectCollision(Node node)
	{
		node.Call("TakeDamage", meleeDamage);
	}

	public void SetMeleeDamage(int damage)
	{
		meleeDamage = damage;
	}

	public void LookForCollision(string direction, bool status)
	{

		if (status)
		{
			direction = lastAttackDir;
		} else lastAttackDir = direction;

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
		
		foreach (Node node in this.GetOverlappingBodies())
		{
			DetectCollision(node);
		}
	}
	
}
