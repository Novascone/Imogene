extends Node3D

var speed = 5


# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var forward = $cameraRig.transfrom.basis.z.normalized() * speed

	if Input.is_action_pressed("LeftMouse"):
		$cameraRig.transform.origin += forward




