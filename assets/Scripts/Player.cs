using System;
using System.Diagnostics;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	private CollisionShape2D _collider;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collider = GetNode<CollisionShape2D>("%Collider");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.W))
		{
			Translate(new Vector2(0.0f, -1.0f));
		}
		
		if (Input.IsKeyPressed(Key.A))
		{
			Translate(new Vector2(-1.0f, 0.0f));
		}
		
		if (Input.IsKeyPressed(Key.S))
		{
			Translate(new Vector2(0.0f, 1.0f));
		}
		
		if (Input.IsKeyPressed(Key.D))
		{
			Translate(new Vector2(1.0f, 0.0f));
		}

		if (GetOverlappingAreas().Count != 0)
		{
			Debug.Print(GetOverlappingAreas().ToString());
		}
	}
}
