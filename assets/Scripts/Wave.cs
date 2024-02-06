using Godot;
using System;
using System.Diagnostics;

public partial class Wave : Node
{
	private int numOfBullet;
	private int spread;

	public int player_id;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var parent = this.GetParent();
		player_id = (int)parent.Get("player_id");
		
		Debug.Print($"{player_id}, wave");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
