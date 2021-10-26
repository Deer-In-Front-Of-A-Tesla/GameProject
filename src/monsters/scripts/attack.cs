using Godot;

namespace gamejamproj.monsters.scripts
{
    public class attack
    {
        private projectile_template _projectileTemplate;
        private Monster monster;
        private dungeon_master dungeonMaster;
        private string type;

        public attack(projectile_template projectileTemplate, Monster _monster, dungeon_master _dungeonMaster, string _type)
        {
            _projectileTemplate = projectileTemplate;
            monster = _monster;
            dungeonMaster = _dungeonMaster;
            type = _type;
        }

        private void CreateProjectile()
        {
            CreateProjectile(monster.GlobalPosition.AngleToPoint(dungeonMaster.player.Position));

        }

        private void CreateProjectile(float direction)
        {
            var newProjectile = _projectileTemplate.Instantiate(monster.Position.x, monster.Position.y, direction);
            dungeonMaster.dungeon_root_node.AddChild(newProjectile);
        }
        
        public void Execute()
        {
            switch (type)
            {
                default:
                case "single":
                {
                    CreateProjectile();
                    break;
                }
            }
        }
    }
}