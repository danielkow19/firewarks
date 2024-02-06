using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

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
	private Vector2 _rightStickInput;
	private float _targetRotation;
	private float _rotationSpeed;
	
	// Energy variables
	private Timer freeze;
	private float energy;

	private double _leftCooldown;
	private double _rightCooldown;
	private const double COOLDOWN_MAX = 2.0f;
	
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
		_direction = Vector2.Zero;
		_rightStickInput = Vector2.Zero;
		_aimDirection = Vector2.Right;
		_targetRotation = 0;
		_rotationSpeed = 3f;

		energy = 1000;
		freeze = GetNode<Timer>("%Freeze");
		freeze.OneShot = true;
		freeze.WaitTime = 5;
		freeze.Start();
		_leftCooldown = COOLDOWN_MAX;
		_rightCooldown = COOLDOWN_MAX;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// We use string concatenation to splice in the player ID for the input system
		// Update the Bullet Cooldowns, add the delta time to the current value
		_leftCooldown += delta;
		_rightCooldown += delta;
		
		
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

		// We use string concatenation to splice in the player ID for the input system
		// The controls will have a naming convention of Action_{player_id}, player ID starts from 0 and goes up to 3
		// Players 1 and 2 can will have keyboard control backups for testing (WASD and arrow keys respectively)
		
		_direction = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized();
		_rightStickInput = Input.GetVector($"AimLeft_{player_id}", $"AimRight_{player_id}", $"AimUp_{player_id}", $"AimDown_{player_id}").Normalized();

		if (Input.IsActionPressed($"Shoot_L_{player_id}") && _leftCooldown >= COOLDOWN_MAX)
		{
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position); 
			DrainEnergy(100, .15f);
			Debug.Print($"Shoot Left P{player_id}");
			_leftCooldown = 0.0;
		}
		else if (Input.IsActionPressed($"Shoot_L_{player_id}")){
			Debug.Print($"P{player_id} Left on Cooldown");
		}
		if (Input.IsActionPressed($"Shoot_R_{player_id}") && _rightCooldown >= COOLDOWN_MAX){
			PackedScene pattern = GD.Load<PackedScene>("res://Pattern1.tscn");
			var instance = pattern.Instantiate();			
			AddSibling(instance);
			instance.Set("position", Position);
			Debug.Print($"Shoot Right P{player_id}");
			_rightCooldown = 0.0;
		}
		else if (Input.IsActionPressed($"Shoot_R_{player_id}")){
			Debug.Print($"P{player_id} Right on Cooldown");
		}

		if (_rightStickInput != Vector2.Zero)
		{
			_aimDirection = _rightStickInput;
			if (_aimDirection.Y > 0)
			{
				_targetRotation = MathF.Acos(_aimDirection.X);
			}
			else
			{
				_targetRotation = MathF.PI * 2 - MathF.Acos(_aimDirection.X);
			}
		} else {
			_targetRotation = Rotation;
		}
		Debug.Print("{0}", Rotation - _targetRotation);
		if (Mathf.Abs(Rotation - _targetRotation) <= _rotationSpeed * (float)delta)
		{
			Rotation = _targetRotation;
		}
		else if (MathF.Abs(Rotation - _targetRotation) > MathF.PI)
		{
			Rotation += _rotationSpeed * (float)delta * (Rotation - _targetRotation) / MathF.Abs(Rotation - _targetRotation);
		}
		else {
			Rotation += _rotationSpeed * (float)delta * -(Rotation - _targetRotation) / MathF.Abs(Rotation - _targetRotation);
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

		Debug.Print("Time:" + freeze.TimeLeft.ToString());
		Debug.Print("Energy: " + energy);

		if (freeze.TimeLeft == 0)
		{
			energy += 100 * (float)delta;

			if (energy > 1000)
			{
				energy = 1000;
			}
		}


		freeze.GetParent<ProgressBar>().Value = energy;

		// Can't Hide and Show the objects unless I have access to the node
		Array<Node> lives = healthBar.GetChildren();
		
		// Change Health bar display
		for (int i = 2; i >= 0; i--)
		{
			lives[i].Set("visible", health >= i);
		}
	}

	public void DamagePlayer(int amount)
	{
		health -= amount;
	}

	public void DrainEnergy(int amount, float delayTime = 0)
	{
		energy -= amount;
		if (energy < 0)
		{
			// Potential increase in delay if the energy goes negative
			energy = 0;
		}


		if (delayTime > 0)
		{
			freeze.WaitTime = freeze.TimeLeft + delayTime;
			freeze.Start();
		}
	}
}
