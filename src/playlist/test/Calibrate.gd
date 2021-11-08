extends Button

export (Resource) var _calibration_scene;

func _pressed():
	Navigator.navigate(_calibration_scene);
