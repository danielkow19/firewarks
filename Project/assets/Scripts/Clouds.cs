using System;
using Godot;
using Godot.Collections;

namespace FireWARks.assets.Scripts;

public partial class Clouds : Area2D
{
	[Export(PropertyHint.Enum, "Up,Down,Left,Right,UpAndLeft,UpAndRight,DownAndLeft,DownAndRight")]
	public string[] directions = new string[0];
	[Export]
	public double[] speed = new double[0];
	[Export]
	public double[] rotations = new double[0];
	[Export]
	public double[] timers = new double[0];
	[Export]
	public bool moving = false;
	[Export]
	public bool boomerang = false;
	private int step = 0;
	private Vector2 start;
	private bool reverseTracking = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		start = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//this.Position = this.Position + (path[step].Normalized().X * speed[step]);
	}
	
	private void _on_area_entered(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds++;
		}
	}
	
	private void _on_area_exited(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds--;
		}
	}
}


