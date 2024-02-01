using System;
using System.Diagnostics;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	private CollisionShape2D _collider;
	
	[Export]
	private int player_id = 0; //Player ID is what makes the different players have separate controls

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collider = GetNode<CollisionShape2D>("%Collider");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// We use string concatination to splice in the player ID for the input system
		// The controls will be uniform ACTION_{player_id}, player ID starts from 0 and goes up to 3
		if (Input.IsActionPressed($"Up_{player_id}"))
		{
			//Translate(new Vector2(0.0f, -1.0f));
			Debug.Print($"Up_${player_id}");
		}
		
		if (Input.IsActionPressed($"Left_{player_id}"))
		{
			//Translate(new Vector2(-1.0f, 0.0f));
			Debug.Print($"Left_${player_id}");
		}
		
		if (Input.IsActionPressed($"Down_{player_id}"))
		{
			//Translate(new Vector2(0.0f, 1.0f));
			Debug.Print($"Down_${player_id}");
		}
		
		if (Input.IsActionPressed($"Right_{player_id}"))
		{
			//Translate(new Vector2(1.0f, 0.0f));
			Debug.Print($"Right_${player_id}");
		}

		if (GetOverlappingAreas().Count != 0)
		{
			Debug.Print(GetOverlappingAreas().ToString());
		}

        // We use string concatination to splice in the player ID for the input system
        // The controls will have a naming convetion of Action_{player_id}, player ID starts from 0 and goes up to 3
		// Players 1 and 2 can will have keyboard control backups for testing (WASD and arrow keys respectively)

        Vector2 movement = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized() * (float)delta * 100f;
		if (Input.IsKeyPressed(Key.Space))
		{
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position);
			 
		}

		if (Input.IsKeyPressed(Key.Space))
		{
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position);
			 
		}

		Vector2 movement = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized() * (float)delta * 100f;
		Translate(movement);
	}
}
