using Godot;
using System;

public partial class Pattern : Node
{
	//base class for a bullet pattern, returns function or path for x bullet in pattern at y time
	//creates UI warning for bullet paths etc
	private int waves;
	private Bullet[] bullets;

	public Pattern(){
		bullets = new Bullet[3];
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int i = 0; i < 3; i++){
			bullets[i] = new Bullet();
			if(i < 3){ 
				bullets[i].Rotate((i-1)* (float)Math.PI/6);
			}

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PopulateBullets(){

	}
}
