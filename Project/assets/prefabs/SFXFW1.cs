using Godot;
using System;

public partial class SFXFW1 : AudioStreamPlayer
{
	private RandomNumberGenerator rng;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
		this.PitchScale = (this.PitchScale + rng.RandfRange(-.1f,.1f));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
