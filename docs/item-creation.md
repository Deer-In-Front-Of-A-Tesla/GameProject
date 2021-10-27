# Item Creation

This is documentation on the current standing `ItemAPI` and will be a short introduction to creating items within this project.

## ItemAPI

There exists an `ItemAPI` class that contains 4 properties:

```md
# player data entry point for item modification
export(Resource) var _entity = _entity as mainplayer;

# return true/false when an item should be used. This is checked once each frame
func _should_use_item() -> bool:

#  entry for item processing
func _apply_item() -> void:
```

## Creating an item

Follow the normal procedure of creating an item using using an `Area2D` node with collisions, etc. Then add an empty `Node` which is where your logic will sit. Call it `ItemLogic` or something similar. 

extend the `ItemAPI` and include the `Area2D` node path to get collision info

```md
extends ItemAPI

export(NodePath) onready var _area2d = get_node(_area2d) as Area2D;

func _should_use_item() -> bool:
    pass

func _apply_item() -> void:
    pass
```

## Thats It!

If you need any more info just contact Emil