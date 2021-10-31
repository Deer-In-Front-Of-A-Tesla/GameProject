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
    private Timer IFrameTimer;

    private bool dashUp = true;
    private float dashRecharge = 0;
    private bool dashing = false;
    private bool attacking = false;
    private string lastDir = "Up";
    private int health;
    private int shields;
    private bool IFrame = false;
    private string idleDir = "Left";
    
    Vector2 velocity = new Vector2();

    private void GetInput()
    {
	    velocity = new Vector2();
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
				dashUp = true;
				dashing = false;
			}
		}
		else if (!dashing && !attacking) { 
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
            velocity = velocity.Bounce(collisionInfo.Normal);
        }
    }
    public override void _Process(float delta)
    {
        //Sprite Control
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
        animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
        //IFrameTimer = (Timer) GetNode("I-Frames");
        attackArea = GetNode("AttackArea");
        //IFrameTimer.WaitTime = (float)MainPlayer.Get("I_frame_time");
        //this.Connect("timeout", IFrameTimer, "stopIFrames");
        animatedSprite.Connect("animation_finished", this, "stopAttack");
        MainPlayer.Connect("changed", this, nameof(_onChange));
        
        GD.Print($"HP IS: {MainPlayer.Get("hp")}");
    }
    
    private void _onChange() { // F no async
	    GD.Print("something changed on player data!");
    }

    public void stopIFrames()
    {
	    IFrame = false;
	    //TODO: Stop animation blinking
    }
    
    public void TakeDamage(int damage)
    {
        shields -= damage;
        if (shields < 0)
        {
	        health += shields;
        }
        IFrameTimer.Start();
        IFrame = true;
        //TODO: Make animation blink
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
