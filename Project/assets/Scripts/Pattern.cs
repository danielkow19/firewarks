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
	public int[] bulletPerWave = {1};
	[Export]
	public int[] spreadPerWave = {0};
	[Export]
	public int[] speedPerWave = {0};

	[Export]
	public int[] offsetPerWave = {0};
	[Export]
	public float[] spinPerWave = {0};
	[Export]
	public float[] spinAccelPerWave = {0,0,0};
	[Export]
	public bool swirl = false;
	[Export]
	public int[] swirlMod = {1};
	PackedScene pattern = GD.Load<PackedScene>("res://assets/prefabs/Wave.tscn");
	PackedScene sfx = GD.Load<PackedScene>("res://assets/prefabs/SFXFW1.tscn");
	bool released = false;
	public Pattern(){
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddChild(sfx.Instantiate());
		PopulateWaves();
		SpawnWaves();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//spawn a wave/waves every x seconds while not released
		//if end of waves start from beggining
		if(GetChildCount() < 3)
		{
			QueueFree();
		}
		//if attached follow player to continue to spawn from... 
		//need to add pattern bool to see if bullets should also follow player
		if(!released){
			Set("position", owner.Get("position"));
		}
	}

	public void Release(){
		released = true;
	}
	//instantiates each wave setting variables
	public void SpawnWaves(){
		for (int i = 0; i < waves; i++)
		{		
			//! dettach waves as siblings unless they have bool
			var instance = pattern.Instantiate();
			instance.Set("owner", owner);
			instance.Set("wait", i*delay);
			instance.Set("numOfBullet", bulletPerWave[i]);
			instance.Set("spread", spreadPerWave[i]);
			instance.Set("speed", speedPerWave[i]);
			instance.Set("offset",offsetPerWave[i]);
			instance.Set("spin",spinPerWave[i]);
			instance.Set("spinAccel",spinAccelPerWave[i]);
			instance.Set("swirl", swirl);
			instance.Set("swirlMod", swirlMod[i]);
			if(true){
			AddChild(instance);
			}
			else{AddSibling(instance);}
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
				else{if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];}
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
				else{if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];}
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
				else{if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];}
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
				else{if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];;}
			}
			offsetPerWave = waveUpdate;
		}
		if(spinPerWave.Length < waves)
		{
			float[] waveUpdate = new float[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < spinPerWave.Length)
				{
					waveUpdate[i] = spinPerWave[i];
				}
				else{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];
					}
			}
			spinPerWave = waveUpdate;
		}
		if(spinAccelPerWave.Length < waves)
		{
			float[] waveUpdate = new float[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < spinAccelPerWave.Length)
				{
					waveUpdate[i] = spinAccelPerWave[i];
				}
				else{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];
					}
			}
			spinAccelPerWave = waveUpdate;
		}
		if(swirlMod.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < swirlMod.Length)
				{
					waveUpdate[i] = swirlMod[i];
				}
				else{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i-1];
					}
			}
			swirlMod = waveUpdate;
		}
		//initial bullet rotation not wave rotation needs to be added here
	}
}
