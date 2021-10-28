using Godot;
using System;

public class KinematicBody2Dscript : KinematicBody2D
{
	
	[Export] public Resource MainPlayer;
	
	private int speed;
	private int dashMod;
	private float dashTime;
	private double dashRecover;

	private bool dashUp = true;
	private float dashRecharge = 0;
	private bool canMove = true;
	private string lastDir = "Up";
	
	Vector2 velocity = new Vector2();

	private void GetInput()
	{
		velocity = new Vector2();

		if ((Input.IsActionPressed("dash") && dashUp) || !canMove)
		{
			Console.WriteLine("Dash");
			canMove = false;
			dashUp = false;
			switch (lastDir)
			{
				case "Right":
					velocity.x += 1;
					break;
				case "Left":
					velocity.x -= 1;
					break;
				case "Up":
					velocity.y -= 1;
					break;
				case "Down":
					velocity.y += 1;
					break;
			}
		}
		else if (canMove) { 
			if (Input.IsActionPressed("ui_right"))
			{
				velocity.x += 1;
				lastDir = "Right";
			}
			if (Input.IsActionPressed("ui_left"))
			{
				velocity.x -= 1;
				lastDir = "Left";
			}
			if (Input.IsActionPressed("ui_down"))
			{
				velocity.y += 1;
				lastDir = "Down";
			}
			if (Input.IsActionPressed("ui_up"))
			{
				velocity.y -= 1;
				lastDir = "Up";
			}
		}

		if (canMove)
		{
			velocity = velocity * speed;
		}
		else
		{
			velocity = velocity * speed * dashMod;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		
		var collisionInfo = MoveAndCollide(velocity * delta);
		if (collisionInfo != null)
		{
			velocity = velocity.Bounce(collisionInfo.Normal);
		}
		
		
		
			
	}
	public override void _Process(float delta)
	{
		//Literally exists to keep up with dash timings, because i cant be fucked to set up a timer.
		if (dashUp) return;
		if (dashTime <= dashRecharge )
		{
			dashUp = true;
			dashRecharge = 0;
		}
		else
		{
			dashRecharge += delta;
			if (dashRecharge >= dashRecover)
			{
				canMove = true;
			}
		}

	}

	public override void _Ready()
	{
		speed = (int)MainPlayer.Get("movement_speed");
		dashMod = (int)MainPlayer.Get("dash_speed_modification");
		dashTime = (float)MainPlayer.Get("dash_time");
		dashRecover = (float)MainPlayer.Get("dash_recover_time");
		MainPlayer.Connect("changed", this, nameof(_onChange));
		
		GD.Print($"HP IS: {MainPlayer.Get("hp")}");
		Console.WriteLine(speed);
	}

	private void _onChange() { // F no async
		GD.Print("something changed on player data!");
	}
}
