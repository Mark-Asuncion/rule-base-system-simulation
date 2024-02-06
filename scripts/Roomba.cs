using Godot;
using System;

public partial class Roomba : CharacterBody3D
{
	[Export] private float move_speed = 1f;
	[Export] private float rotation_amount = 0.05f;
	[Export] private float target_rotation = 0.785398f; // radians
	[Export] private float reverse_time = 1f;
	Rule rule;
	bool is_on = false;
	bool is_hit = false;
	bool do_rotate = false;
	float amount_rotated = 0.0f;
	double hit_time = 0f;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
		rule = GetNode<Rule>("Rule");
		rule.rule_name = "Roomba";
		rule.time_on = new mTime(12,3);
		rule.time_off = new mTime(15,0);

		rule.On += _on;
		rule.Off += _off;
		is_on = true;
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
			GD.Print("angle: ", n.GetAngle());
			if (n.GetAngle() > 0.7) {
				is_hit = true;
				break;
			}
		}
		if (is_hit) {
			hit_time += delta;
		}
		float time_scale = GetTree().Root.GetChild<GameManager>(0).TimeScale;
		if (hit_time >= reverse_time) {
			hit_time = 0;
			is_hit = false;
			do_rotate = true;
		}

		Vector3 velocity = Velocity;
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (is_on) {
			float forward = (is_hit)? -1:1;
			Vector3 d = (Transform.Basis * new Vector3(0,0,forward)).Normalized();
			velocity.Z = d.Z * move_speed * (time_scale / 2);
			velocity.X = d.X * move_speed * (time_scale / 2);
		}
		else {
			velocity.Z = 0;
			velocity.X = 0;
		}

		if (do_rotate) {
			RotateY(rotation_amount);
			amount_rotated += rotation_amount;
			if (amount_rotated >= target_rotation) {
				amount_rotated = 0.0f;
				do_rotate = false;
			}
			velocity.Z = 0;
			velocity.X = 0;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	public float Lerp(float origin, float target, float amount)
	{
		return origin + (target - origin) * amount;
	}
}
