using System;
using System.Diagnostics;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	private CollisionShape2D _collider;
	private int health;
	private HBoxContainer healthBar;
	private float _speed;
	private float _slowedSpeed;
	private Vector2 _direction;
	private Vector2 _aimDirection;
	private float _targetRotation;
	private float _rotationSpeed;

	
	[Export]
	private int player_id = 0; //Player ID is what makes the different players have separate controls

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collider = GetNode<CollisionShape2D>("%Collider");
		healthBar = GetNode<HBoxContainer>("%Lives");
		health = 2;
		_speed = 100;
		_slowedSpeed = 50;
		_direction = new Vector2(0, 0);
		_aimDirection = new Vector2(0, 0);
		_targetRotation = 0;
		_rotationSpeed = 1f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// We use string concatination to splice in the player ID for the input system
		// The controls will be uniform ACTION_{player_id}, player ID starts from 0 and goes up to 3
		if (Input.IsActionPressed($"Up_{player_id}"))
		{
			//Translate(new Vector2(0.0f, -1.0f));
			//Debug.Print($"Up_${player_id}");
		}
		
		if (Input.IsActionPressed($"Left_{player_id}"))
		{
			//Translate(new Vector2(-1.0f, 0.0f));
			//Debug.Print($"Left_${player_id}");
		}
		
		if (Input.IsActionPressed($"Down_{player_id}"))
		{
			//Translate(new Vector2(0.0f, 1.0f));
			//Debug.Print($"Down_${player_id}");
		}
		
		if (Input.IsActionPressed($"Right_{player_id}"))
		{
			//Translate(new Vector2(1.0f, 0.0f));
			//Debug.Print($"Right_${player_id}");
		}

		if (GetOverlappingAreas().Count != 0)
		{
			Debug.Print(GetOverlappingAreas().ToString());
		}

		// We use string concatination to splice in the player ID for the input system
		// The controls will have a naming convetion of Action_{player_id}, player ID starts from 0 and goes up to 3
		// Players 1 and 2 can will have keyboard control backups for testing (WASD and arrow keys respectively)
		
		_direction = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized();
		_aimDirection = Input.GetVector("AimLeft", "AimRight", "AimUp", "AimDown").Normalized();

		if (Input.IsActionPressed($"Shoot_L_{player_id}"))
		{
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position); 
			Debug.Print($"Shoot Left P{player_id}");
		}
		if (Input.IsActionPressed($"Shoot_R_{player_id}")){
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position);
			Debug.Print($"Shoot Right P{player_id}");
		}

		if (_aimDirection != Vector2.Zero)
		{
			if (_aimDirection.Y > 0)
			{
				_targetRotation = MathF.Acos(_aimDirection.X) * (float)delta;
			}
			else
			{
				_targetRotation = MathF.PI * 2 - MathF.Acos(_aimDirection.X) * (float)delta;
			}
		}
		Debug.Print("{0}", Rotation - _targetRotation);
		if (Mathf.Abs(Rotation - _targetRotation) <= _rotationSpeed)
		{
			Rotation = _targetRotation;
		}
		else if (MathF.Abs(Rotation - _targetRotation) > MathF.PI)
		{
			Rotation += _rotationSpeed * (Rotation - _targetRotation) / MathF.Abs(Rotation - _targetRotation);
		}
		else {
			Rotation += _rotationSpeed * -(Rotation - _targetRotation) / MathF.Abs(Rotation - _targetRotation);
		}

		if(Rotation > MathF.PI * 2) {
			Rotation -= MathF.PI * 2;
		}
		else if (Rotation < 0) {
			Rotation += MathF.PI * 2;
		}

		if (Input.IsActionPressed("Slow"))
		{
			Translate(_direction * _slowedSpeed * (float)delta);
		}
		else
		{
			Translate(_direction * _speed * (float)delta);
		}


		// Change Health bar display
		/*for (int i = 2; i >= 0; i--)
		{
			if (health >= i)
			{
				healthBar.GetChild<Texture2D>(i).Set("Visible", true);
			}
			else
			{
				healthBar.GetChild<Texture2D>(i).Set("Visible", false);
			}
		}*/
	}

	public void DamagePlayer(int amount)
	{
		health -= amount;
	}
}
