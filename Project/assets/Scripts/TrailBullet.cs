using Godot;
using System;
using System.Linq;
using FireWARks.assets.Scripts;
using Godot.Collections;
using System.Diagnostics;

public partial class TrailBullet : Area2D
{
	private double lifetime = 10;
	[Export]
	public Player owner;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{			
		//owner = (Player) this.GetParent();
		if(owner != null){			
			switch (owner.player_id)
			{
				case 0:
					Modulate = Colors.Aquamarine;
					break;
				case 1:
					Modulate = Colors.RebeccaPurple;
					break;
				case 2:
					Modulate = Colors.Firebrick;
					break;
				case 3:
					Modulate = Colors.Lime;
					break;
			}
		}
		
		
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

	//checks collision for the bullets if nonplayer stops bullet, if player checks player and dmgs if not owner
	private void CheckCollisions()
	{
		Array<Area2D> collisions = GetOverlappingAreas();
		if (collisions.Count != 0)
		{
			foreach (Area2D area in collisions)
			{
				if (area is not Resource)
				{
					if (area is Player player)
					{
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
