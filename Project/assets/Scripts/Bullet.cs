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
	private float spin = 0;
	private float spinAccel = 0;
	[Export]
	private double lifetime = 10;
	[Export]
	public double delay = 1;
	public Player owner;
	private string passer;
	private string[] tags;
	private bool swirl = false;
	public int swirlMod;
	private Line2D trail;
	private double grazeTimer;
	private bool grazeStart = false;
	private Color bulletColor; // Believe this is needed in case player is in the middle of other animation
	private const float grazeLength = 0.125f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//grabs parent and sets colors to match
		var parent = this.GetParent();
		owner = (Player) parent.Get("owner");
		trail = GetNode<Line2D>("%Trail");
		trail.Set("baseCd", 2/speed.X);
		if(owner != null){
			Modulate = owner.Modulate;
			trail.Modulate = owner.Modulate;
		}
		if(passer == "Camo"){
			Modulate = new Color(Modulate.R,Modulate.G,Modulate.B, .15f);
			trail.Modulate = owner.Modulate; // Don't double dip on transparency
		}

		bulletColor = Modulate;
		grazeTimer = 0;


		// Sets the opacity of the trail 0 - 255
		//trail.Modulate = trail.Modulate with { A8 = 255 };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		grazeTimer -= delta;
		if (grazeTimer < 0)
		{
			grazeTimer = 0;
		} 
		
		// Transition from bullet color to white
		if (grazeStart)
		{
			// alpha needs to be saved to maintain camo power-up
			float alpha = Modulate.A;
			Modulate = Colors.White.Lerp(bulletColor, (float)grazeTimer / grazeLength);
			Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, alpha);
			trail.Modulate = Modulate;
			
			
			if (grazeTimer == 0)
			{
				// Start latter loop, where white decreases
				grazeStart = false;
				grazeTimer = grazeLength;
			}
		}
		else // Transition from white to bullet color, or just stay bullet color
		{
			// alpha needs to be saved to maintain camo power-up
			float alpha = Modulate.A;
			Modulate = bulletColor.Lerp(Colors.White, (float)grazeTimer / grazeLength);
			Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, alpha);
			trail.Modulate = Modulate;
		}
		
		
		//increment timer
		delay -= delta;
		//when timer reaches zero start attack
		if(delay <= 0)
		{
			//swirl logic for sharp turns after slight moving away from player
			if (swirl && delay <= .5)
			{
				//GD.Print(speed);
				speed = speed.Rotated(((float)Math.PI/180) * swirlMod * (float)delta);
				if(delay < -1.5)
				{
					swirl = false;
				}							
				//GD.Print(speed);
			}
			//translate and rotate bullet by respective variables, adjust spin and life
			Translate(speed.Rotated(Rotation) * (float)delta);
			Rotate((float)(Math.PI*spin*delta/180));
			spin += (float)(spinAccel*delta);		
			lifetime -= delta;	
			//when life goes to 0 delete
			if(lifetime <= 0)
			{
				this.QueueFree();
			}		
			CheckCollisions();
		}
	}

	//checks collision for the bullets if non-player stops bullet, if player checks player and dmgs if not owner
	private void CheckCollisions()
	{
		Array<Area2D> collisions = GetOverlappingAreas();
		if (collisions.Count != 0)
		{
			//check each collision make sure it's a player
			foreach (Area2D area in collisions)
			{
				if (area is not Resource)
				{
					//if it's a different player reward owner, dmg enemy, delete bullet
					if (area is Player player)
					{
						if(player != owner && player.Damageable)
						{
							// Attempt to reward player the bullet came from
							owner.RewardEnergy(15);
							
							player.DamagePlayer(1);
							lifetime = 0;
							QueueFree();
						}
					}
					//if hit a cloud destroy
					else
					{
						lifetime = 0;
						QueueFree();
					}		
				}
			}
		}
	}

	public void StartGrazeAnim()
	{
		grazeStart = true;
		grazeTimer = grazeLength;
	}
}
