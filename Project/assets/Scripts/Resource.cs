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
	private AnimationPlayer animator;
	private bool fullyTransitioned;
	private bool startedIdle;
	private bool noCollision;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hopefully stops the frame one visibility bug on the animations
		GetChild<Sprite2D>(1).Visible = false;
		
		// Resource Spawner determines power-up type  instead of this class
		popup = GD.Load<PackedScene>("res://assets/prefabs/MessagePopup.tscn");
		
		animator = GetChild<AnimationPlayer>(2);
		
		// Play the spawn animation immediately
		animator.Play("PowerUp Spawn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lifetime -= delta;
		
		// destroy after animation finishes playing
		if(lifetime <= 0 && fullyTransitioned && !animator.IsPlaying())
		{
			QueueFree();
		}

		// Condition to start the despawn animation
		if (lifetime <= 1 && !animator.IsPlaying())
		{
			animator.Play("PowerUp Despawn");
			fullyTransitioned = true;
		}
		else if (!animator.IsPlaying() && startedIdle)
		{
			// Idle animation if nothing else is playing
			animator.Play("Idle");
		}
		else if (!animator.IsPlaying() && !startedIdle)
		{
			animator.Play("BeginIdle");
			startedIdle = true;
		}
		
		CheckCollisions();
	}

	private void CheckCollisions()
	{
		// Early out, final animation is playing
		if (noCollision)
		{
			return;
		}
		
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
						
						// Switch to grab animation no matter what
						animator.Stop();
						animator.Play("Grabbed");
						
						// Will allow it to be dequeued when animation finishes
						lifetime = 0;
						noCollision = true;
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
