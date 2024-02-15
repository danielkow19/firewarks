using Godot;
using System;
using System.Linq;
using FireWARks.assets.Scripts;
using Godot.Collections;
using System.Diagnostics;

public partial class Resource : Area2D
{
	[Export]
	private double lifetime = 10;
	[Export]
	private string[] tags;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lifetime -= delta;
		if(lifetime <= 0)
		{
			this.QueueFree();
		}
		CheckCollisions();
	}

	private void CheckCollisions(){
		Array<Area2D> collisions = GetOverlappingAreas();
		if (collisions.Count != 0)
		{
			foreach (Area2D area in collisions)
			{
				if (area is not Bullet)
				{
					if (area is Player player)
					{
						player.RewardEnergy(15);
						lifetime = 0;
						QueueFree();

					}	
				}
			}
		}
	}
}
