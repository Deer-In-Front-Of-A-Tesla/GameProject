using Godot;
using System;

public class attackDummyTest : Node
{
    [Export] public int health = 100;

    public override void _Ready()
    {
        Console.WriteLine("Dummy ready with " + health + " health");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Console.WriteLine("Dummy hit with " + damage + " has " + health + " left");
    }

}
