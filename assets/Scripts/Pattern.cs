using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Pattern : Node
{

	public int player_id;
	//base class for a bullet pattern, returns function or path for x bullet in pattern at y time
	//creates UI warning for bullet paths etc
	[Export]
	public int waves = 3;
	[Export]
	public int[] bulletPerWave = {3,4,3};
	public Pattern(){
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
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
				else{waveUpdate[i]= 1;}
			}
			bulletPerWave = waveUpdate;
		}
		for (int i = 0; i < waves; i++)
		{
			
			PackedScene pattern = GD.Load<PackedScene>("res://Wave.tscn");
			var instance = pattern.Instantiate();
			//instance.Set("position", Get("position"));	
			instance.Set("player_id", player_id);
			instance.Set("wait", 2*i);
			instance.Set("numOfBullet", bulletPerWave[i]);
			AddChild(instance);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PopulateBullets(){

	}
}
