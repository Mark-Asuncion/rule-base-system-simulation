using Godot;
using System;

public partial class Roomba : CharacterBody3D
{
	enum State
	{
		Forward,
		Reverse
	}
	[Export] private float move_speed = 1f;
	[Export] private float rotation_amount = 0.05f;
	[Export] private float target_rotation = 45f;
	[Export] private float reverse_time = 1f;
	Rule rule;
	bool is_on = false;
	float amount_rotated = 0.0f;
	double hit_time = 0f;
	State state = State.Forward;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
		rule = GetNode<Rule>("Rule");
		rule.rule_name = "Roomba";
		rule.time_on = new mTime(8,3);
		rule.time_off = new mTime(9,0);

		rule.On += _on;
		rule.Off += _off;
    }
	public void _on()
	{
		is_on = true;
	} 
	public void _off()
	{
		is_on = false;
	}
    public override void _PhysicsProcess(double delta)
	{
		for (int i=0;i<GetSlideCollisionCount();i++) {
			var n = GetSlideCollision(i);
			var c = n.GetCollider(0);
			if (n.GetAngle() > 0.7 && n.GetAngle() < 2.5) {
				state = State.Reverse;
				break;
			}
		}
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (is_on) {
			float time_scale = GetTree().Root.GetChild<GameManager>(0).TimeScale;
			Vector3 d;
			switch (state)
			{
				case State.Forward:
					d = (Transform.Basis * new Vector3(0, 0, 1))
						.Normalized();
					velocity.Z = d.Z * move_speed * (time_scale / 2);
					velocity.X = d.X * move_speed * (time_scale / 2);
					break;
				case State.Reverse:
					hit_time += delta;
					bool do_rotate = false;
					if (hit_time >= reverse_time) {
						do_rotate = true;
					}
					else {
						d = (Transform.Basis * new Vector3(0, 0, -1))
							.Normalized();
						velocity.Z = d.Z * move_speed * (time_scale / 2);
						velocity.X = d.X * move_speed * (time_scale / 2);
					}
					if (do_rotate) {
						RotateY(rotation_amount);
						amount_rotated += rotation_amount;
						float rt = target_rotation * MathF.PI / 180f;
						if (amount_rotated >= rt) {
							amount_rotated = 0.0f;
							hit_time = 0;
							state = State.Forward;
						}
						velocity.Z = 0;
						velocity.X = 0;
					}
					break;
			}
		}
		else
		{
			velocity.Z = 0;
			velocity.X = 0;
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
