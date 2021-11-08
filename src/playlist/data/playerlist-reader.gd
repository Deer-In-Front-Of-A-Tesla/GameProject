class_name PlaylistReader
extends Resource;

export(float) var current_beat_strength setget _is_on_beat_changed;

func _is_on_beat_changed(v: float) -> void:
	current_beat_strength = v;
	emit_changed();
