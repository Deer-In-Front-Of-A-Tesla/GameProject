extends Node2D

export(Color) onready var sprite_color;
export(int) onready var bullet_speed = 2;

onready var _sprite = $AnimatedSprite;

func _ready():
	_sprite.self_modulate = sprite_color;

func _physics_process(delta):
	self.position += Vector2(1, 0).rotated(self.rotation) * bullet_speed;


func _on_decay_timeout():
	self.queue_free();
