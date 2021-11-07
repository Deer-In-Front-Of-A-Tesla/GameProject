using Godot;
using System;
using System.Collections.Generic;
using gamejamproj.monsters.scripts;

public class dungeon_master : Node
{
	[Export]
	private Resource[] _monster_templates;

	[Export]
	private bool _autoload;

	[Export]
	private NodePath _dungeon_root_node;

	public Node dungeon_root_node;

	[Export]
	private NodePath _main_player;

	public KinematicBody2D player;

	[Export]
	private NodePath _song_player;

	public playlist song_player;


	private Dictionary<string, MonsterTemplate> monsterTemplates = new Dictionary<string, MonsterTemplate>();

	public void ReloadMonsterTemplates(bool autoload)
	{
		monsterTemplates.Clear();
		if (autoload)
		{
			var dir = new Directory();
			var path = "res://src/monsters/assets/monster_templates/";
			dir.Open(path);
			dir.ListDirBegin();
			string monsterName;
			do
			{
				monsterName = dir.GetNext();
				if (monsterName.StartsWith(".")) continue;
				if (monsterName == "") break;
				try
				{
					var newTemplate = new MonsterTemplate(ResourceLoader.Load(path + monsterName + "/" + monsterName + ".tres"), this);
					monsterTemplates.Add(newTemplate.id, newTemplate);
					GD.Print("Autoloaded " + monsterName);
				}
				catch (Exception e)
				{
					GD.PrintErr("Monster template invalid!\nMonster in question: " + path + monsterName + "/"
								+ monsterName + ".tres" + "\nReason: " + e.Message + "\nName: '" + monsterName + "'"
								);
				}
			} while (monsterName != "");
		}

		foreach (var template in _monster_templates)
		{
			var newTemplate = new MonsterTemplate(template, this);
			monsterTemplates[newTemplate.id] = newTemplate;
		}
	}

	public Monster SpawnMonster(string id)
	{
		var templateToSpawn = monsterTemplates[id];
		var newMonster = new Monster(templateToSpawn);
		dungeon_root_node.AddChild(newMonster);
		GD.Print("Spawning monster " + id);
		return newMonster;
	}

	public override void _Ready()
	{
		dungeon_root_node = GetNode<Node>(_dungeon_root_node);
		song_player = GetNode<playlist>(_song_player);
		player = GetNode<Node>(_main_player).GetNode<KinematicBody2D>("MainPlayerBody");
		ReloadMonsterTemplates(_autoload);
	}

}
