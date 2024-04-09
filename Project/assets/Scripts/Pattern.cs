using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using FireWARks.assets.Scripts;

public partial class Pattern : Node2D
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
	private float timeAlive;
	private int lastFire;
	[Export]
	public bool fireAndForget = false;
	[Export]
	public bool playerLocked = false;
	[Export]
	private float initialCost = 10;
	[Export]
	private float costPerSecond = 5;
	[Export]
	private float recharge = 2;
	[Export]
	private bool randomness = false;
	private bool drained = false;
	private float coolDown;
	private string passer;
	private RandomNumberGenerator rng;
	public Pattern(){
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
		AddChild(sfx.Instantiate());
		PopulateWaves();
		if(fireAndForget)
		{
			Release();
			SpawnWaves();
		}
		owner.DrainEnergy(initialCost, .35f);
		timeAlive = 0;
		lastFire = 0;
		coolDown = 0;
		TopLevel = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//spawn a wave/waves every x seconds while not released
		//if end of waves start from beggining		
		if(!fireAndForget && !drained)
		{
			timeAlive += (float)delta;
			if(!released && (float)owner.Get("energy") >= (costPerSecond * delta))
			{
				owner.DrainEnergy(costPerSecond*(float)delta, (float)delta);
				if(timeAlive > coolDown)
				{			
					if((float)owner.Get("energy") >= costPerSecond * delta)
					{
						SpawnWave(lastFire % waves);
						lastFire += 1;
						coolDown += (float)delay;
					}
					
				}
				
				
			}
			if((float)owner.Get("energy") < costPerSecond/4)
			{
				drained = true;
			}
		}

		
		//if attached follow player to continue to spawn from...
		if(!released)
		{
			Set("position", owner.Get("position"));
			Set("rotation", owner.Get("rotation"));
		}
		else
		{
			if(GetChildCount() < 2)
			{
				QueueFree();
			}
		}
	}

	public void Release()
	{
		released = true;
	}
	//instantiates each wave setting variables
	public void SpawnWaves()
	{
		for (int i = 0; i < waves; i++)
		{		
			//! dettach waves as siblings unless they have bool
			var instance = pattern.Instantiate();
			instance.Set("owner", owner);
			instance.Set("passer", passer);
			instance.Set("wait", i*delay);
			instance.Set("numOfBullet", bulletPerWave[i]);
			if(randomness){
				instance.Set("spread", spreadPerWave[i] * rng.RandfRange(.9f,1.1f));
				instance.Set("offset",offsetPerWave[i] * rng.RandfRange(.9f,1.1f));
				instance.Set("spin",spinPerWave[i] * rng.RandfRange(.9f,1.1f));
				instance.Set("spinAccel",spinAccelPerWave[i] * rng.RandfRange(.9f,1.1f));
				instance.Set("swirlMod", swirlMod[i] * rng.RandfRange(.9f,1.1f));
				if(passer == "BulletSpeed")
				{
					instance.Set("speed", (speedPerWave[i] * 1.5)* rng.RandfRange(.9f,1.1f));
				}
				else{
					instance.Set("speed", speedPerWave[i] * rng.RandfRange(.9f,1.1f));
				}
			}
			else{
				instance.Set("spread", spreadPerWave[i]);
				instance.Set("offset",offsetPerWave[i]);
				instance.Set("spin",spinPerWave[i]);
				instance.Set("spinAccel",spinAccelPerWave[i]);
				instance.Set("swirlMod", swirlMod[i]);
				if(passer == "BulletSpeed")
				{
					instance.Set("speed", speedPerWave[i] * 1.5);
				}
				else{
					instance.Set("speed", speedPerWave[i]);
				}
			}
			
								
			instance.Set("swirl", swirl);

			if(passer == "RFW")
			{
			GD.Print("here");
			instance.Set("position", new Vector2(rng.RandfRange(-500,500),rng.RandfRange(-500,500)));
			AddSibling(instance);
			
			}		
			else if(!playerLocked)
			{
				instance.Set("position", owner.Get("position"));
				instance.Set("rotation", owner.Get("rotation"));
				AddSibling(instance);
			}
			else
			{
				RemoteTransform2D remote = new RemoteTransform2D();
				AddChild(instance);
				AddSibling(remote);
				remote.RemotePath = remote.GetPathTo(instance);
				remote.UpdatePosition = true;
				remote.UpdateRotation = false;
				remote.UpdateScale = false;
			}
		}
	}

	public void SpawnWave(int waveToSpawn)
	{
		var instance = pattern.Instantiate();
		instance.Set("owner", owner);
		instance.Set("passer", passer);
		instance.Set("wait", 0);
		if(randomness){
				instance.Set("spread", spreadPerWave[waveToSpawn] + rng.RandfRange(-10,10));
				instance.Set("offset",offsetPerWave[waveToSpawn] + rng.RandfRange(-5,5));
				instance.Set("spin",spinPerWave[waveToSpawn] + rng.RandfRange(-5,5));
				instance.Set("spinAccel",spinAccelPerWave[waveToSpawn] + rng.RandfRange(-5,5));
				instance.Set("swirlMod", swirlMod[waveToSpawn] + rng.RandfRange(-5,5));
				if(passer == "BulletSpeed")
				{
					instance.Set("speed", (speedPerWave[waveToSpawn] * 1.5)+ rng.RandfRange(-50,50));
				}
				else{
					instance.Set("speed", speedPerWave[waveToSpawn] + rng.RandfRange(-50,50));
				}	
			}
			else{
				instance.Set("spread", spreadPerWave[waveToSpawn]);
				instance.Set("offset",offsetPerWave[waveToSpawn]);
				instance.Set("spin",spinPerWave[waveToSpawn]);
				instance.Set("spinAccel",spinAccelPerWave[waveToSpawn]);
				instance.Set("swirlMod", swirlMod[waveToSpawn]);
				if(passer == "BulletSpeed")
				{
					instance.Set("speed", speedPerWave[waveToSpawn] * 1.5);
				}
				else{
					instance.Set("speed", speedPerWave[waveToSpawn]);
				}		
			}


		instance.Set("numOfBullet", bulletPerWave[waveToSpawn]);
		
		instance.Set("swirl", swirl);

		if(passer == "RFW")
		{
			GD.Print("here");
			instance.Set("position", new Vector2(rng.RandfRange(-500,500),rng.RandfRange(-500,500)));
			AddSibling(instance);
		}
		else if(!playerLocked)
		{
			instance.Set("position", owner.Get("position"));
			instance.Set("rotation", owner.Get("rotation"));
			AddSibling(instance);
		}
		else
		{
			RemoteTransform2D remote = new RemoteTransform2D();
			AddChild(instance);
			AddChild(remote);
			remote.RemotePath = remote.GetPathTo(instance);
			remote.UpdatePosition = true;
			remote.UpdateRotation = false;
			remote.UpdateScale = false;
		}
		
	}
	//checks arrays for wave values before spawning waves, setting unfilled values to a default
	public void PopulateWaves()
	{
		if(bulletPerWave.Length < waves)
		{
			int[] waveUpdate = new int[waves];
			for (int i = 0; i < waveUpdate.Length; i++)
			{
				if(i < bulletPerWave.Length)
				{
					waveUpdate[i] = bulletPerWave[i];
				}
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%bulletPerWave.Length];
				}
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%spreadPerWave.Length];
				}
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%speedPerWave.Length];
				}
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%offsetPerWave.Length];
				}
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%spinPerWave.Length];
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%spinAccelPerWave.Length];
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
				else
				{
					if(i == 0)
					{
						waveUpdate[i]= 40;
					}
					waveUpdate[i]= waveUpdate[i%swirlMod.Length];
				}
			}
			swirlMod = waveUpdate;
		}
		//initial bullet rotation not wave rotation needs to be added here
	}
}
