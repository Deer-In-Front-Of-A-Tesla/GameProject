using System;
using Godot;

namespace gamejamproj.monsters.scripts
{
	public class MonsterTemplate
	{
		public string id { get; } = "0";
		public int health { get; } = 1;
		public float movementSpeed { get; } = 50;
		public SpriteFrames spriteFrames;
		public dungeon_master master;
		
		public string attackType;
		public projectile_template attackProjectileTemplate;

		public MonsterTemplate(Resource _source, dungeon_master _master)
		{
			id = (string)_source.Get("id");
			health = (int)_source.Get("health");
			movementSpeed = (float)_source.Get("movement_speed");
			spriteFrames = (SpriteFrames)_source.Get("texture");
			
			attackType = (string) _source.Get("attack_tyoe");
			Resource projectileRes = (Resource) _source.Get("projectile");
			attackProjectileTemplate = new projectile_template(projectileRes);
			
			master = _master;
		}
	}

	public class Monster : KinematicBody2D
	{
		private dungeon_master master;
		
		private int health;
		private float movementSpeed;
		private MonsterTemplate sourceTemplate;
		private Sprite player;
		private attack attack;
		private AnimatedSprite animation;
		
		public Monster( MonsterTemplate template)
		{
			master = template.master;
			sourceTemplate = template;
			health = template.health;
			movementSpeed = template.movementSpeed;
			attack = new attack(template.attackProjectileTemplate, this, master, template.attackType);
			
			animation = new AnimatedSprite();
			animation.Frames = template.spriteFrames;
			AddChild(animation);
			
			var shape = new CollisionShape2D();
			shape.Shape = new CapsuleShape2D();
			SetCollisionMaskBit(0, false);
			CollisionLayer = 4;
			AddChild(shape);
		}

		public string GetDirectionString(Vector2 dir)
		{
			if (Math.Abs(dir.x) >= Math.Abs(dir.y))
			{
				if (dir.x <= 0) return "left";
				return "right";
			}
			if (dir.y >= 0) return "down";
			return "up";
		}

		public void ExecuteAttack(float beatStrength)
		{
			attack.Execute();
		}

		public void TakeDamage(int amount)
		{
			health -= amount;
			if (health <= 0)
			{
				GetParent().RemoveChild(this);
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			base._PhysicsProcess(delta);
			float moveTowards = GetAngleTo(master.player.GlobalPosition);
			float distance = master.player.GlobalPosition.DistanceTo(GlobalPosition);
			Vector2 movement = new Vector2(1, 0).Rotated(moveTowards) * movementSpeed;
			
			if (distance < 300)
			{
				movement = movement.Rotated((float) Math.PI);
			}
			else if (distance > 500)
			{
				
			}
			else
			{
				movement =  movement.Rotated((float)(Math.PI*0.5)) ;
			}

			animation.Play(GetDirectionString(movement));
			var moved = MoveAndSlide(movement);
			if (Math.Abs(moved.DistanceTo(Vector2.Zero) - 100) > 2)
			{
				GD.Print(moved, "    ", moved.DistanceTo(Vector2.Zero));
			}
			
			
			// GD.Print("Rotation to  " + moveTowards);
			// GD.Print("Monster moving to " + movement);
		}

		public override void _Ready()
		{
			base._Ready();
			master.song_player.Connect("GameBeat", this, nameof(ExecuteAttack));
		}
	}
}
