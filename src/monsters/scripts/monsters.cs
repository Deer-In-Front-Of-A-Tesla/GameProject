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

    public class Monster : AnimatedSprite
    {
        private dungeon_master master;
        
        private int health;
        private float movementSpeed;
        private MonsterTemplate sourceTemplate;
        private Sprite player;
        private attack attack;
        
        public Monster( MonsterTemplate template)
        {
            Frames = template.spriteFrames;
            master = template.master;
            sourceTemplate = template;
            health = template.health;
            movementSpeed = template.movementSpeed;
            attack = new attack(template.attackProjectileTemplate, this, master, template.attackType);
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
            GD.Print("Attacking!!!!! Very furiously!!!!!!!! In the future!!!!!!!!!!!!!!");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            float moveTowards = GetAngleTo(master.player.GlobalPosition) + (float)(Math.PI);
            
            var tempvec = new Vector2(0, 1);
            var movement =  tempvec.Rotated(moveTowards) * movementSpeed * delta;
            MoveLocalX(movement.x);
            MoveLocalY(movement.y);
            Play(GetDirectionString(movement));
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