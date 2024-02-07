using Godot;
using System;
using System.Linq;
using FireWARks.assets.Scripts;
using Godot.Collections;
using System.Diagnostics;

public partial class Bullet : Area2D
{
	private Vector2 direction = new Vector2(0, 0);
	[Export]
	private Vector2 speed = new Vector2(40, 0);
	private Vector2 acceleration = new Vector2(0,1);
	[Export]
	private double lifetime = 10;
	[Export]
	public double delay = 0;
	private Player owner;
	private string[] tags;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		var parent = this.GetParent();
		owner = (Player) parent.Get("owner");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		delay -= delta;
		if(delay <= 0)
		{
			Translate(speed.Rotated(Rotation) * (float)delta);
			lifetime -= delta;
		}
		if(lifetime <= 0)
		{
			this.QueueFree();
		}
		CheckCollisions();
	}

	//checks collision for the bullets if nonplayer stops bullet, if player checks player and dmgs if not owner
	private void CheckCollisions(){
		Array<Area2D> collisions = GetOverlappingAreas();
		
		// Collision Checking, might move to new method if it gets more complicated
		if (collisions.Count != 0)
		{
			foreach (Area2D area in collisions)
			{
				if (area is not Bullet)
				{
					if (area is Player player)
					{
						//GD.Print(this.id);
						if(player != owner)
						{
							// Attempt to reward player the bullet came from
							owner.RewardEnergy(15);
							
							player.DamagePlayer(1);
							lifetime = 0;
							QueueFree();
						}
					}
					else
					{
						lifetime = 0;
						QueueFree();
					}		
				}
			}
		}
	}
}
