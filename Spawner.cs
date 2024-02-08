using Godot;
using System;

public partial class Spawner : Node2D
{
	private double timer = 0;
	private RandomNumberGenerator rng;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer+=delta;
		if (timer>10){
			timer = 0;
			PackedScene resource = GD.Load<PackedScene>("res://Resource.tscn");
			var instance = resource.Instantiate();
			instance.Set("position", new Vector2(rng.RandfRange(-500,500),rng.RandfRange(-500,500)));
			AddChild(instance);
		}
	}
}
