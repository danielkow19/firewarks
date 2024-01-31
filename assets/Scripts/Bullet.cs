using Godot;
using System;

public partial class Bullet : Area2D
{
	private Vector2 velocity = new Vector2(20,40);
	private Vector2 acceleration = new Vector2(0,1);
	private double timer;
	private string[] tags;

	//constructor
	public Bullet(){

	}
	public Bullet(Vector2 position, Vector2 direction){

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Translate(velocity * (float)delta);
		LookAt(velocity);
		timer += delta;
		//CalculateVectors(1);

	}
	// Called in process. Updates velocity and acceleration based on given pattern.
	private void CalculateVectors(int pattern)
	{
		velocity += acceleration;
		//acceleration = pattern(timer);	 	
	}
}
