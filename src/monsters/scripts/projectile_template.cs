using Godot;
using System;


public class projectile_template : Node
{
    private int damage;
    private float movementSpeed;
    private SpriteFrames frames;
    
    public class projectile: KinematicBody2D
    {
        private float movementSpeed, rotation;
        public int damage;
        private AnimatedSprite animation;
        public projectile(float x, float y, float direction, int damage, float movementSpeed, SpriteFrames frames)
        {
            Position = new Vector2(x, y);
            Rotation = direction - (float)Math.PI / 2;

            var shape = new CollisionShape2D();
            shape.Shape = new CapsuleShape2D();

            AddChild(shape);
            SetCollisionMaskBit(0, false);
            CollisionLayer = 4;

            animation = new AnimatedSprite();
            AddChild(animation);
            animation.Frames = frames;
            this.damage = damage;
            this.movementSpeed = movementSpeed;
        }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("Spawned at " + Position);
            animation.Play("down");
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            MoveLocalY(-movementSpeed * delta);
        }

        public void AnnihilateNode()
        {
            GetParent().RemoveChild(this);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public projectile_template(Resource _source)
    {
        damage = (int) _source.Get("damage");
        movementSpeed = (float) _source.Get("movement_speed");
        frames = (SpriteFrames) _source.Get("frames");
    }

    public projectile Instantiate(float x, float y, float direction)
    {
        return new projectile(x, y, direction, damage, movementSpeed, frames);
    }
    
}
