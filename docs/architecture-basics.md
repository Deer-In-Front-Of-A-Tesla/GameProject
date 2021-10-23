# Architecture Basics

## Module Structure

This project is layed out in a module based system where the game main runner starts at [game module](./src\game\scenes\main.tscn)

Each module has its a directory of subfolders declaring the scenes used, scripts, and internal data, and assets being used for a module.

Here is the structure:

```
.
└── ./my-module
    ├── ./assets
    ├── ./data
    ├── ./scenes
    └── ./scripts
```

Here's an example use case for a player controller:

```
.
└── ./player
    ├── ./assets
    │   ├── sprite_sheet.png
    │   └── player.tres
    ├── ./data
    │   └── player-data.gd
    ├── ./scenes
    │   └── player.tscn
    └── ./scripts
        └── player-controller.tscn
```

## Data Management

All our data should live within Resource that can be shared around. Instead of saving data within a massive global singleton, we can have each of our modules import only what the need via a custom Resource. 

### Why?
This reduces module interdependency(coupling) and we can give good read/write permissions for such structures

### Item Example

```javascript
class_name GameItem
extends Resource;

export(String) var name;
export(String) var item_id;
export(String, MULTILINE) var description;
export(String, MULTILINE) var lore;
export(String, MULTILINE) var stat_description;

export(int) var hp_cum_mod;
export(int) var hp_mult_mod;
```

## Inverse Your Dependencies!

No one likes seeing random objects being accessed everywhere! Here's a simple pattern for dependency inversion in godot

```javascript
// inside player-controller.gd
extends KinematicBody

export(Resource) var _local_player = _local_player as PlayerEntity; // packs of data to use in the cntrllr
export(Resource) var _camera_settings = _camera_settings as CameraSettings;
export(NodePath) onready var _camera_pivot = get_node(_camera_pivot) as Node; // nodes to be used
export(NodePath) onready var _camera = get_node(_camera) as Camera;
```