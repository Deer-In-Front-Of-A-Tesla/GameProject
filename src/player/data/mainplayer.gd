class_name mainplayer
extends Resource;

export(int) var hp = 100 setget _hp;
export(int) var maxhp = 100;
export(int) var movement_speed = 200;
export(int) var dash_speed_modification = 2;
export(float) var dash_time = 2;
export(float) var dash_recover_time = 0.5;

func _hp(newHp: int) -> void:
	hp = newHp;
	emit_changed();
