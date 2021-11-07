extends Button

export (Resource) var _game_scene;

func _pressed():
	Navigator.navigate(_game_scene);
