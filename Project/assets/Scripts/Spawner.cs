using Godot;
using System;

public partial class Spawner : Node2D
{
	private double timer = 0;
	private RandomNumberGenerator rng;
	PackedScene resource = GD.Load<PackedScene>("res://assets/prefabs/Resource.tscn");
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
			var instance = resource.Instantiate();
			instance.Set("position", new Vector2(rng.RandfRange(-500,500),rng.RandfRange(-500,500)));
			Array values = Enum.GetValues(typeof(PowerUpType));
			(instance as Resource).type = (PowerUpType)values.GetValue(rng.RandiRange(0, 7));
			AddChild(instance);

			GD.Print(("Spawned a " + (instance as Resource).ToString()));
		}
	}
}
