using Godot;
using System;
using System.Diagnostics;

public partial class Pattern : Node
{

	public int player_id;
	//base class for a bullet pattern, returns function or path for x bullet in pattern at y time
	//creates UI warning for bullet paths etc
	private int waves;
	private Bullet[] bullets;

	public Pattern(){
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Debug.Print($"{player_id} Pattern");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PopulateBullets(){

	}
}
