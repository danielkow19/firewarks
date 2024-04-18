using Godot;
using System;
using System.Linq;
using FireWARks.assets.Scripts;
using Godot.Collections;
using System.Diagnostics;

public partial class BulletPreview : Area2D
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
	public PlayerAttackPreview owner;
	private string passer;
	private string[] tags;
	private bool swirl = false;
	public int swirlMod;
	private Line2D trail;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//grabs parent and sets colors to match
		var parent = this.GetParent();
		owner = (PlayerAttackPreview) parent.Get("owner");
		trail = GetNode<Line2D>("%Trail");
		trail.Set("baseCd", 2/speed.X);
		if(owner != null){
			Modulate = owner.Modulate;
			trail.Modulate = owner.Modulate;
		}
		if(passer == "Camo"){
			Modulate = new Color(Modulate.R,Modulate.G,Modulate.B, .15f);
			trail.Modulate = new Color(Modulate.R,Modulate.G,Modulate.B, .15f);
		}
		// Sets the opacity of the trail 0 - 255
		//trail.Modulate = trail.Modulate with { A8 = 255 };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

	//checks collision for the bullets if nonplayer stops bullet, if player checks player and dmgs if not owner
	private void CheckCollisions()
	{
		Array<Area2D> collisions = GetOverlappingAreas();
		if (collisions.Count != 0)
		{
			//check each collision make sure its a player
			foreach (Area2D area in collisions)
			{
				if (area is not Resource)
				{
						//lifetime = 0;
						//QueueFree();
				}
			}
		}
	}
}