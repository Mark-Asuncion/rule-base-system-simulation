using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export] private float move_speed = 5.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	[Export] private Node3D cam_rot_orig;
	[Export] private float cam_speed = 0.01f;
	[Export] private float min_x_rotation = 45;

    private float ToRadians(float v)
	{
		return v * MathF.PI / 180f;
	}
    public override void _Ready()
    {
        base._Ready();
		cam_rot_orig = GetNode<Node3D>("Node3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
			Input.MouseMode = Input.MouseModeEnum.Captured;
		else if (@event.IsActionPressed("ui_cancel"))
			Input.MouseMode = Input.MouseModeEnum.Visible;

		if (Input.MouseMode != Input.MouseModeEnum.Captured)
			return;
		if (@event is InputEventMouseMotion) {
			var @mouse_motion = (InputEventMouseMotion) @event;
			RotateY(-@mouse_motion.Relative.X * cam_speed);
			var nx = new Vector2(cam_rot_orig.Rotation.X + (-@mouse_motion.Relative.Y * cam_speed),0);
			float min_rot = ToRadians(min_x_rotation);
			nx = nx.Clamp(new Vector2(-min_rot, 0), new Vector2(min_rot, 0));
			cam_rot_orig.Rotation = new Vector3(nx.X, 0, 0);
			// GD.Print(cam_rot_orig.Rotation);
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * move_speed;
			velocity.Z = direction.Z * move_speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, move_speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, move_speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
