class_name mainplayer
extends Resource;

export(int) var hp = 100 setget _hp;
export(int) var shield_hp = 0 setget _shield_hp;
export(int) var maxhp = 100;
export(int) var combined_hp setget _set_combined_hp, _get_combined_hp;

export(int) var movement_speed = 200;
export(int) var melee_damage = 10;
export(int) var bullet_speed = 750;
export(int) var range_damage = 10;
export(int) var dash_speed_modification = 2;
export(float) var I_frame_time = 2;
export(float) var AttackTime = 0.5;
export(float) var dash_time = 2;
export(float) var dash_recover_time = 0.5;

func _shield_hp(new_shield_hp: int) -> void:
	shield_hp = new_shield_hp;
	emit_changed();
	
func _get_combined_hp() -> int:
	return hp + shield_hp;
	
func _set_combined_hp(_new_combined_hp: int) -> void:
	pass

func _hp(newHp: int) -> void:
	hp = newHp;
	emit_changed();
