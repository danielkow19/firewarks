using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using FireWARks.assets.Scripts;

public partial class Pattern : Node
{
	private Player owner;
	[Export]
	public int waves = 3;
	[Export]
	public double delay = 1;
	[Export]
	public int[] bulletPerWave = {3,4,3};
	[Export]
	public int[] spreadPerWave = {80,80,80};
	[Export]
	public int[] speedPerWave = {40,40,40};

	[Export]
	public int[] offsetPerWave = {0,0,0};
	PackedScene pattern = GD.Load<PackedScene>("res://Wave.tscn");
	public Pattern(){
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PopulateWaves();
		SpawnWaves();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GetChildCount() < 2)
		{
			QueueFree();
		}
	}
	//instantiates each wave setting variables
	public void SpawnWaves(){
		for (int i = 0; i < waves; i++)
		{		
			
			var instance = pattern.Instantiate();
			instance.Set("owner", owner);
			instance.Set("wait", i*delay);
			instance.Set("numOfBullet", bulletPerWave[i]);
			instance.Set("spread", spreadPerWave[i]);
			instance.Set("speed", speedPerWave[i]);
			instance.Set("offset",offsetPerWave[i]);
			AddChild(instance);
		}
	}
	//checks arrays for wave values before spawning waves, setting unfilled values to a default
	public void PopulateWaves(){
		if(bulletPerWave.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < bulletPerWave.Length)
				{
					waveUpdate[i] = bulletPerWave[i];
				}
				else{waveUpdate[i]= 1;}
			}
			bulletPerWave = waveUpdate;
		}
		if(spreadPerWave.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < spreadPerWave.Length)
				{
					waveUpdate[i] = spreadPerWave[i];
				}
				else{waveUpdate[i]= 80;}
			}
			spreadPerWave = waveUpdate;
		}
		if(speedPerWave.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < speedPerWave.Length)
				{
					waveUpdate[i] = speedPerWave[i];
				}
				else{waveUpdate[i]= 40;}
			}
			speedPerWave = waveUpdate;
		}
		if(offsetPerWave.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < offsetPerWave.Length)
				{
					waveUpdate[i] = offsetPerWave[i];
				}
				else{waveUpdate[i]= 40;}
			}
			offsetPerWave = waveUpdate;
		}
	}
}
