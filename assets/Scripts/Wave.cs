using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using FireWARks.assets.Scripts;

public partial class Wave : Node
{
	private int numOfBullet = 4;
	private int spread = 90;

	public int offset = 0;
	public int speed = 40;
	public int wait;
	private Player owner;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnBullets();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GetChildCount() < 1)
		{
			QueueFree();
		}
	}
	public void PopulateBullets(){

	}
	//checks num of bullets and spawns numofbullets at player location
	public void SpawnBullets(){
		//if multiple bullets loop for every bullet and instantiate setting variables and adding to scene
		if(numOfBullet > 1)
		{
			for (int i = 0; i < numOfBullet; i++)
			{
				PackedScene pattern = GD.Load<PackedScene>("res://Bullet.tscn");
				var instance = pattern.Instantiate();
				instance.Set("position", Get("position"));
				instance.Set("rotation", (-(Math.PI*spread/180)/2) + (i)*(Math.PI*(spread/(numOfBullet-1))/180) + (offset * Math.PI/180));
				instance.Set("delay", wait);
				instance.Set("speed", new Vector2(speed, 0));
				AddChild(instance);
			}
		}
		//if one bullet instantiate 1 bullet
		else
		{
			PackedScene pattern = GD.Load<PackedScene>("res://Bullet.tscn");
			var instance = pattern.Instantiate();
			instance.Set("position", Get("position"));
			instance.Set("delay", wait);
			instance.Set("speed", new Vector2(speed, 0));			
			AddChild(instance);
		}
	}
}
