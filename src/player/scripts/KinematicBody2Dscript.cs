using Godot;
using System;

public class KinematicBody2Dscript : KinematicBody2D
{
	[Export] public Resource MainPlayer;

	private int speed;
	private int meleeDamage;
	private int maxHealth;
	private int maxShields;
	private int dashMod;
	private float dashTime;
	private double dashRecover;
	private Node attackArea;
	private AnimatedSprite animatedSprite;
	private AnimationPlayer blink;
	private Timer IFrameTimer;
	private Timer AttackTimer;

	
	
	private bool dashUp = true;
	private float dashRecharge = 0;
	private bool dashing = false;
	private bool attacking = false;
	private string lastDir = "Up";
	private int health;
	private int shields;
	private bool IFrame = false;
	private string idleDir = "Left";

	private int rangeDamage;
	private int rangeSpeed;
	private string attdir;
	private bool rangedAttacking = false;

	Vector2 velocity = new Vector2();

	private string findMouseRelativePosition()		// -2.6 top left, -0.53 top right
	{
		Vector2 mouseClick = this.GetGlobalMousePosition();
		float angle = mouseClick.AngleToPoint(this.GlobalPosition);
		if (-2.6 < angle && angle < -0.53) return "Up";
		if (-0.53 < angle && angle < 0.53) return "Right";
		if (0.53 < angle && angle < 2.6) return "Down";
		if (-2.6 > angle || angle > 2.6) return "Left";
		return "";
	}
	private void spawnBullet(float dir)
	{

		float songBeat = playlist.currentlyPlaying.GetCurrentStrength();
		
		if (songBeat <0.2) return;
		
		var scene = GD.Load<PackedScene>("res://src//player//scenes//Projectile.tscn");
		var instance = scene.Instance();
		instance.GetNode("Area2D").Set("bulletSpeed", rangeSpeed);
		instance.GetNode("Area2D").Set("dir", dir);
		instance.GetNode("Area2D").Set("damage", rangeDamage);
		if (songBeat == 0.5)
		{
			instance.GetNode("Area2D").Set("strength", "max");
		}
		switch (attdir)
		{
			case "Up":
				instance.GetNode("Area2D").Set("pos",((Position2D) GetNode("RangeAttackUp")).GlobalPosition);
				break;
			case "Right":
				instance.GetNode("Area2D").Set("pos",((Position2D) GetNode("RangeAttackRight")).GlobalPosition);
				break;
			case "Left":
				instance.GetNode("Area2D").Set("pos",((Position2D) GetNode("RangeAttackLeft")).GlobalPosition);
				break;
			case "Down":
				instance.GetNode("Area2D").Set("pos",((Position2D) GetNode("RangeAttackDown")).GlobalPosition);
				break;
		}
		this.GetParent().AddChild(instance);
		
	}
	
	private void GetInput()
	{
		velocity = new Vector2();
		if (Input.IsActionPressed("ranged_attack") && rangedAttacking == false && attacking == false && dashing == false)
		{
			attdir = findMouseRelativePosition();
			spawnBullet(this.GetGlobalMousePosition().AngleToPoint(this.GlobalPosition));
			rangedAttacking = true;
			AttackTimer.Start();
			switch (attdir)
			{
				case "Right":
					animatedSprite.Play("attack_right");
					break;
				case "Left":
					animatedSprite.Play("attack_left");
					break;
				case "Up":
					animatedSprite.Play("attack_up");
					break;
				case "Down":
					animatedSprite.Play("attack_down");
					break;
			}
			
		}
		if (Input.IsActionPressed("attack") && attacking == false && dashing == false)
		{
			attacking = true;
			if (Input.IsActionPressed("ui_right"))
			{
				startAttack("Right");
			} else 
			if (Input.IsActionPressed("ui_left"))
			{
				startAttack("Left");
			} else 
			if (Input.IsActionPressed("ui_down"))
			{
				startAttack("Down");
			} else
			if (Input.IsActionPressed("ui_up"))
			{
				startAttack("Up");
			}
			else
			{
				switch (lastDir)
				{
					case "Right":
						startAttack("Right");
						break;
					case "Left":
						startAttack("Left");
						break;
					case "Up":
						startAttack("Up");
						break;
					case "Down":
						startAttack("Down");
						break;
					case "UpRight":
						startAttack("Right");
						break;
					case "UpLeft":
						startAttack("Left");
						break;
					case "DownLeft":
						startAttack("Left");
						break;
					case "DownRight":
						startAttack("Right");
						break;
				}
			}
			
		}
		else 
		if ((Input.IsActionPressed("dash") && dashUp))
		{
			dashing = true;
			dashUp = false;
			this.SetCollisionMaskBit(3, false);
			this.SetCollisionMaskBit(2, false);
			if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_down"))
			{
				idleDir = "Left";
				lastDir = "DownLeft";
			} else 
			if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up"))
			{
				idleDir = "Left";
				lastDir = "UpLeft";
			} else 
			if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_down"))
			{
				idleDir = "Right";
				lastDir = "DownRight";
			} else 
			if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up"))
			{
				idleDir = "Right";
				lastDir = "UpRight";
			} else 
			if (Input.IsActionPressed("ui_right"))
			{
				idleDir = "Right";
				lastDir = "Right";
			} else 
			if (Input.IsActionPressed("ui_left"))
			{
				idleDir = "Left";
				lastDir = "Left";
			} else 
			if (Input.IsActionPressed("ui_down"))
			{
				lastDir = "Down";
			} else
			if (Input.IsActionPressed("ui_up"))
			{
				lastDir = "Up";
			}
			else
			{
				this.SetCollisionMaskBit(3, true);
				this.SetCollisionMaskBit(2, true);
				dashUp = true;
				dashing = false;
			}
		}
		else if (!dashing && !attacking ) { 
			if (Input.IsActionPressed("ui_right"))
			{
				idleDir = "Right";
				lastDir = "Right";
				velocity.x += 1;
			}
			if (Input.IsActionPressed("ui_left"))
			{
				idleDir = "Left";
				lastDir = "Left";
				velocity.x -= 1;
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
		else
		if (dashing)
		{
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
				case "UpRight":
					velocity.y -= 1;
					velocity.x += 1;
					break;
				case "UpLeft":
					velocity.y -= 1;
					velocity.x -= 1;
					break;
				case "DownLeft":
					velocity.y += 1;
					velocity.x -= 1;
					break;
				case "DownRight":
					velocity.y += 1;
					velocity.x += 1;
					break;
			}
		}

		if (!dashing)
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
			if (collisionInfo.Collider.GetType() == typeof(projectile_template.projectile))
			{
				var projectile = ((projectile_template.projectile) collisionInfo.Collider);
				TakeDamage(projectile.damage);
				projectile.AnnihilateNode();

			}
			else
			{
				velocity = velocity.Bounce(collisionInfo.Normal);
			}
		}
		
		//Anim Blinking
		
	}
	public override void _Process(float delta)
	{
		//Sprite Control
		if (!rangedAttacking && animatedSprite.Frame == 0)
		{
			if (!attacking && !dashing)
			{
				if (Input.IsActionPressed("ui_right"))
				{
					animatedSprite.Play("run_right");
				} else 
				if (Input.IsActionPressed("ui_left"))
				{
					animatedSprite.Play("run_left");
				} else 
				if (Input.IsActionPressed("ui_up"))
				{
					animatedSprite.Play("run_up");
				} else 
				if (Input.IsActionPressed("ui_down"))
				{
					animatedSprite.Play("run_down");
				} else 
				if (idleDir == "Left")
				{
					animatedSprite.Play("idle_left");
				}
				else
				{
					animatedSprite.Play("idle_right");
				}
			}
			else 
			if (dashing)
			{
				switch (lastDir)
				{
					case "Right":
						animatedSprite.Play("dash_right");
						break;
					case "Left":
						animatedSprite.Play("dash_left");
						break;
					case "Up":
						animatedSprite.Play("dash_up");
						break;
					case "Down":
						animatedSprite.Play("dash_down");
						break;
					case "UpRight":
						animatedSprite.Play("dash_right");
						break;
					case "UpLeft":
						animatedSprite.Play("dash_left");
						break;
					case "DownLeft":
						animatedSprite.Play("dash_left");
						break;
					case "DownRight":
						animatedSprite.Play("dash_right");
						break;
				}
			}
		}

		if (IFrame)
		{
			blink.Play("blink");
		}
		
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
				dashing = false;
				this.SetCollisionMaskBit(3, true);
				this.SetCollisionMaskBit(2, true);
			}
		}

	}
	
	public override void _Ready()
	{
		speed = (int)MainPlayer.Get("movement_speed");
		meleeDamage = (int)MainPlayer.Get("melee_damage");
		dashMod = (int)MainPlayer.Get("dash_speed_modification");
		dashTime = (float)MainPlayer.Get("dash_time");
		dashRecover = (float)MainPlayer.Get("dash_recover_time");
		
		rangeSpeed = (int)MainPlayer.Get("bullet_speed");
		Console.WriteLine(rangeSpeed);
		rangeDamage = (int)MainPlayer.Get("range_damage");
		
		animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
		animatedSprite.Connect("animation_finished", this, "stopAttack");
		
		attackArea = GetNode("AttackArea");
		attackArea.Call("SetMeleeDamage", meleeDamage);
		
		AttackTimer = (Timer) GetNode("RangeCooldown");
		AttackTimer.WaitTime = (float)MainPlayer.Get("AttackTime");
		AttackTimer.Connect("timeout", this, "RangeCooledDown");
		
		IFrameTimer = (Timer) GetNode("IFrameTimer");
		IFrameTimer.WaitTime = (float)MainPlayer.Get("I_frame_time");
		IFrameTimer.Connect("timeout", this, "stopIFrames");

		blink = (AnimationPlayer) GetNode("BlinkPlayer");
		
		
		MainPlayer.Connect("changed", this, nameof(_onChange));
	}

	private void RangeCooledDown()
	{
		rangedAttacking = false;
	}
	
	private void _onChange() { // F no async
		GD.Print("something changed on player data!");
	}

  public void stopIFrames()
  {
	IFrame = false;
  }

  public void TakeDamage(int damage)
  {
	if (IFrame) return;

	  shields -= damage;
	  if (shields < 0)
	  {
		health += shields;
	  }
	  IFrameTimer.Start();
	  IFrame = true;
  }
	public void startAttack(string direction)
	{
		attackArea.Call("LookForCollision", direction, false);
		switch (direction)
		{
			case "Up":
				animatedSprite.Play("attack_up");
				break;
			case "Down":
				animatedSprite.Play("attack_down");
				break;
			case "Right":
				animatedSprite.Play("attack_right");
				break;
			case "Left":
				animatedSprite.Play("attack_left");
				break;
		}
	}
	public void stopAttack()
	{
		if (attacking)
		{
			attacking = false;
			
			attackArea.Call("LookForCollision", "", true);
			if (idleDir == "Left")
			{
				animatedSprite.Play("idle_left");
			}
			else
			{
				animatedSprite.Play("idle_right");
			}
		}
	}
}
