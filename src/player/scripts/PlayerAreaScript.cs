using Godot;
using System;

public class PlayerAreaScript : Node
{
	[Signal] public delegate void GetHit(int damage);
	
	public override void _Ready()
	{
		this.Connect("GetHit", GetNode("/root/Player/MainPlayerBody"), "TakeDamage");
		
		this.Connect("body_entered", this, nameof(DetectCollition));
	}

	public void DetectCollition(PhysicsBody2D node)
	{
		Console.WriteLine(node.CollisionLayer);
		if (node.CollisionLayer == 4)
		{
			Console.WriteLine("Got Hit");
			EmitSignal(nameof(GetHit), node.Get("damage"));
			node.Call("AnnihilateNode");
		}
		
	}
}
