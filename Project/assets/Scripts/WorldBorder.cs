using Godot;
using System;
using FireWARks.assets.Scripts;

public partial class WorldBorder : Area2D
{
	private Timer countDown;
	private float scale;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scale = 1;
		countDown = new Timer();
		AddChild(countDown);
		countDown.OneShot = true;
		countDown.Start(300); // TODO: Make this a public variable that can be changed in the inspector. Currently is 5 minutes.
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (countDown.TimeLeft == 0 && Math.Abs(scale - .25f) > .01f)
		{
			scale -= .05f * (float)delta;

			if (scale < .25f)
			{
				scale = .25f;
			}
			
			Scale = new Vector2(scale, scale);
		}
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
