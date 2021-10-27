using Godot;
using System;

public class KinematicBody2Dscript : KinematicBody2D
{

    [Export] public Resource MainPlayer;
    
    private int speed;
    private int dashMod;
    private float dashTime;
    private double dashRecover;
    private AnimatedSprite animatedSprite;

	private bool dashUp = true;
	private float dashRecharge = 0;
	private bool canMove = true;
	private string lastDir = "Up";
	
	Vector2 velocity = new Vector2();

	private void GetInput()
	{
		velocity = new Vector2();

        if ((Input.IsActionPressed("dash") && dashUp))
        {
            Console.WriteLine("Dash");
            canMove = false;
            dashUp = false;
            if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_down"))
            {
                lastDir = "DownLeft";
            } else 
            if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up"))
            {
                lastDir = "UpLeft";
            } else 
            if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_down"))
            {
                lastDir = "DownRight";
            } else 
            if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up"))
            {
                lastDir = "UpRight";
            } else 
            if (Input.IsActionPressed("ui_right"))
            {
                lastDir = "Right";
            } else 
            if (Input.IsActionPressed("ui_left"))
            {
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
                canMove = true;
            }
        }
        else if (canMove) { 
            if (Input.IsActionPressed("ui_right"))
            {
                velocity.x += 1;
            }
            if (Input.IsActionPressed("ui_left"))
            {
                velocity.x -= 1;
            }
            if (Input.IsActionPressed("ui_down"))
            {
                velocity.y += 1;
            }
            if (Input.IsActionPressed("ui_up"))
            {
                velocity.y -= 1;
            }
        }
        else
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
        //Sprite Control
        if (canMove)
        {
            if (Input.IsActionPressed("ui_right"))
            {
                animatedSprite.Play("run_sideways");
            } else 
            if (Input.IsActionPressed("ui_left"))
            {
                animatedSprite.Play("run_sideways");
            } else 
            if (Input.IsActionPressed("ui_up"))
            {
                animatedSprite.Play("run_up");
            } else 
            if (Input.IsActionPressed("ui_down"))
            {
                animatedSprite.Play("run_down");
            } else
            {
                animatedSprite.Play("idle");
            }
        }
        else
        {
            switch (lastDir)
            {
                case "Right":
                    animatedSprite.Play("dash_sideways");
                    break;
                case "Left":
                    animatedSprite.Play("dash_sideways");
                    break;
                case "Up":
                    animatedSprite.Play("dash_up");
                    break;
                case "Down":
                    animatedSprite.Play("dash_down");
                    break;
                case "UpRight":
                    animatedSprite.Play("dash_sideways");
                    break;
                case "UpLeft":
                    animatedSprite.Play("dash_sideways");
                    break;
                case "DownLeft":
                    animatedSprite.Play("dash_sideways");
                    break;
                case "DownRight":
                    animatedSprite.Play("dash_sideways");
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
        animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
    }

    public void TakeDamage(int damage)
    {
        Console.WriteLine(damage);
        Console.WriteLine("Taken Damage");
    }
}
