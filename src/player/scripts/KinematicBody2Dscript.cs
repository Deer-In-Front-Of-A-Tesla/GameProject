using Godot;
using System;

public class KinematicBody2Dscript : KinematicBody2D
{
    [Export] public int speed = 200;
    [Export] public int dashMod = 40;
    [Export] public float dashTime = 2;
    [Export] public double dashRecover = 0.5;

    private bool dashUp = true;
    private float dashRecharge = 0;
    private bool canMove = true;
    private string lastDir = "Up";
    
    Vector2 velocity = new Vector2();

    private void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("dash") && dashUp)
        {
            Console.WriteLine("Dash");
            canMove = false;
            dashUp = false;
            switch (lastDir)
            {
                case "Right":
                    velocity.x += dashMod;
                    break;
                case "Left":
                    velocity.x -= dashMod;
                    break;
                case "Up":
                    velocity.y -= dashMod;
                    break;
                case "Down":
                    velocity.y += dashMod;
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
        
        velocity = velocity * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        var collisionInfo = MoveAndCollide(velocity * delta);
        if (collisionInfo != null)
        {
            Console.WriteLine("Collide");
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
}
