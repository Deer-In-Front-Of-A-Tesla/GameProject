using Godot;
using System;


public class projectile_template : Node
{
    private int damage;
    private float movementSpeed;
    private SpriteFrames frames;
    
    public class projectile: AnimatedSprite
    {
        private float movementSpeed, rotation;
        public int damage;
        public projectile(float x, float y, float direction, int damage, float movementSpeed, SpriteFrames frames)
        {
            Position = new Vector2(x, y);
            Rotation = rotation = direction;
            Frames = frames;
            this.damage = damage;
            this.movementSpeed = movementSpeed;
        }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("Spawned at " + Position);
            Play("down");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            
            var moveBy = Vector2.Left.Rotated(rotation) * delta * movementSpeed;
            MoveLocalX(moveBy.x);
            MoveLocalY(moveBy.y);
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
