using Godot;
using System;
using System.Linq;
using FireWARks.assets.Scripts;
using Godot.Collections;
using System.Diagnostics;

public enum PowerUpType
{
	Refill,
	Barrier,
	Orbit_Ring,
	Bullet_Speed,
	Supporting_Fire,
	Mobile_Attacker,
	Slowdown,
	Smoke_Bomb,
	Camo,
}

public partial class Resource : Area2D
{
	[Export]
	private double lifetime = 10;
	[Export]
	private string[] tags;

	public PowerUpType type;

	private PackedScene popup;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Resource Spawner determines power-up type  instead of this class
		popup = GD.Load<PackedScene>("res://assets/prefabs/MessagePopup.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lifetime -= delta;
		if(lifetime <= 0)
		{
			QueueFree();
		}
		CheckCollisions();
	}

	private void CheckCollisions()
	{
		Array<Area2D> collisions = GetOverlappingAreas();
		if (collisions.Count != 0)
		{
			foreach (Area2D area in collisions)
			{
				if (area is not Bullet)
				{
					if (area is Player player)
					{
						// Fire Popup to screen
						MessagePopup p = popup.Instantiate<MessagePopup>();
						GetTree().Root.AddChild(p);
						p.Popup(ToString(), GlobalPosition);
						
						player.ResourceCollected(type);
						lifetime = 0;
						QueueFree();
					}	
				}
			}
		}
	}

	public override string ToString()
	{
		string output = type.ToString();
		output = output.Replace("_", " ");
		
		return output + " PowerUp";
	}
}
