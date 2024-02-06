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
	public int player_id;

	//private CollisionShape2D collider;
	//public Sprite2D sprite ;
	//private double second;
	private string[] tags;

	//constructor
	// public Bullet(){
	// 	//collider = new CollisionShape2D();
	// 	//collider.Shape = new CapsuleShape2D();
	// 	//collider.ApplyScale(new Vector2(3/4,3/4));
	// 	//collider.Rotate((float)Math.PI/2);
	// 	//sprite = new Sprite2D();
	// 	//sprite.Texture = GD.Load<Texture2D>("res://.godot/imported/quick bullet.png");
		
	// 	//sprite.Texture;
	// 	//LookAt(this.Position + direction);
	// 	//this.AddChild(collider);
	// 	//this.AddChild(sprite);
		
	// }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		//second = 0;
		var parent = this.GetParent();
		player_id = (int)parent.Get("player_id");
		Debug.Print($"{player_id}, bullet");
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
		//second += delta;
		//if(second >= 1)
		//{
		//	second = 0;
		//	CalculateVectors(1, delta);
		//}


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
						if(player.player_id != this.player_id)
						{					
							// Reward other player for hitting
							/*string playerName = "%Player_" + id;
							GetNode<Player>(playerName).RewardEnergy(15);*/
								
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
	// Called in process. Updates vectors based on given pattern.
	private void CalculateVectors(int pattern, double delta)
	{	 	
		//Rotate((float)Math.PI/45 * (float)delta);
		//speed.Y = Mathf.Sin(Position.X * 1/20) * 50;
	}
}
