extends Node3D
# kinda have to use GDScript. C# wrapper for PhantomCamera is hell to use

@onready var phantomCamera : PhantomCamera3D = $PhantomCamera3D
@onready var sky3D : Sky3D = $Sky3D
@onready var player : CharacterBody3D = $Player_2


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass



	
