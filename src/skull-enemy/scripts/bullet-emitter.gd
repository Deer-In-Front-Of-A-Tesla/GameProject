extends Node2D

export(Resource) onready var _playlist_reader = _playlist_reader as PlaylistReader;
export(NodePath) onready var _skull = get_node(_skull) as SkullEnemy;
export(PackedScene) onready var _bullet;

func _ready():
	_playlist_reader.connect("changed", self, "_strength_changed");

func _strength_changed() -> void:
	if(_skull.type == "GREEN"):
		green_emitter();
		return;
	red_emitter();
	

func green_emitter():
	if(_playlist_reader.current_beat_strength >= 0.7):
		spawn_bullet(90);
		spawn_bullet(-90);
		spawn_bullet(180);
		spawn_bullet(0);
	elif(_playlist_reader.current_beat_strength >= 0.5):
		spawn_bullet(90);
		
func red_emitter() -> void:
	if(_playlist_reader.current_beat_strength >= 0.5):
		spawn_bullet(90);
		spawn_bullet(-90);
		spawn_bullet(180);
		spawn_bullet(0);
	elif(_playlist_reader.current_beat_strength >= 0.3):
		spawn_bullet(90);

func spawn_bullet(rot: float) -> void:
	var spawned_bullet = _bullet.instance();
	spawned_bullet.sprite_color = _skull.get_color();
	spawned_bullet.rotation_degrees = rot;
	self.add_child(spawned_bullet);
