using Godot;
using System;

public class attackDummyTest : Node
{
	[Export] public int health = 100;
	private KinematicBody2D playerBody;

	public override void _Ready()
	{
		Console.WriteLine("Dummy ready with " + health + " health");
		playerBody = (KinematicBody2D) this.GetParent().GetNode("Player/MainPlayerBody");
		Console.WriteLine(playerBody.Position);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		Console.WriteLine("Dummy hit with " + damage + " has " + health + " left");
	}

}
