using Godot;
using System.Collections.Generic;

public partial class Lights : Node3D
{
	Rule[] rule;
	private OmniLight3D spotlight;
	[Export] private float energy = 1;
	[Export] private float move_on_time = 1;
	[Export] private float move_on_max_time = 10f;
	private Area3D area;
	private Dictionary<CharacterBody3D, Vector3> movable_obj;
	private bool on_by_move = false;
	private SceneTreeTimer on_by_move_timer = null;
	public override void _Ready()
	{
		movable_obj = new Dictionary<CharacterBody3D, Vector3>();
		rule = new Rule[2];
		rule[0] = GetNode<Rule>("Rule");
		rule[1] = GetNode<Rule>("Rule2");
		spotlight = GetNode<OmniLight3D>("LightNode");
		area = GetNode<Area3D>("Area3D");
		rule[0].rule_name = "Lights";
		rule[1].rule_name = "Lights";
		rule[1].notif_prefix = "Movement Detection";
		rule[1].type = Rule.Type.CustomOff;
		rule[0].IsOn = (GameManager ctx) => {
			bool ret = false;
			if (ctx.time.T >= new mTime(18,30).T) {
				ret = true;
			}
			if (ctx.time.T >= new mTime(22,30).T) {
				ret = false;
			}
			return ret;
		};
		rule[1].IsOn = MovementDetection;

		rule[0].On += () => spotlight.LightEnergy = energy;
		rule[0].Off += () => {
			if (on_by_move)
				return;
			spotlight.LightEnergy = 0;
		};
		rule[1].On += () => {
			spotlight.LightEnergy = energy;
			on_by_move = true;
			if (on_by_move_timer == null) {
				on_by_move_timer = GetTree().CreateTimer(move_on_time);
				on_by_move_timer.Timeout += () => rule[1].OEmitSignal(false);
			}
			else {
				double tl = on_by_move_timer.TimeLeft + move_on_time;
				if (tl > move_on_max_time)
					tl = move_on_max_time;
				on_by_move_timer.TimeLeft = tl;
			}
		};
		rule[1].Off += () => {
			spotlight.LightEnergy = 0;
			on_by_move = false;
			on_by_move_timer = null;
		};
	}
    public bool MovementDetection(GameManager _ctx) {
		var nodes = area.GetOverlappingBodies();
		bool ret = false;
		foreach (var i in nodes) {
			if (i is CharacterBody3D) {
				var body = i as CharacterBody3D;
				if (movable_obj.TryGetValue(body, out Vector3 value)) {
					if (value != body.Position) {
						ret = true;
					}
				}
				movable_obj[body] = body.Position;
			}
		}
		return ret;
	}
}
