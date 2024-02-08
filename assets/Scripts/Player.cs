using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	
	private Control Hud;
	
	// Consider just accessing instead of saving
	private CollisionShape2D _collider;
	private int health;
	private HBoxContainer healthBar;
	
	// Movement and Aiming Variables
	private float _speed;
	private float _slowedSpeed;
	private Vector2 _direction;
	private Vector2 _aimDirection;
	private Vector2 _rightStickInput;
	private float _targetRotation;
	private float _rotationSpeed;

	// Invincibility frames variables
	private bool _damageable;
	private float _invTimeMax;
	private float _invTime;
	private Sprite2D _playerSprite;
	private Color _initialColor;
	private Color _alternateColor;

	// How long a single color of the invulnerability display lasts before flashing back to the other color
	private float _singleColorTime;
	// Double the value of _singleColorTime, the time a cycle including both colors takes
	private float _doubleColorTime;
	
	// Energy variables, also consider just accessing
	private Timer freeze;
	private float energy;

	private double _leftCooldown;
	private double _rightCooldown;
	private const double COOLDOWN_MAX = 2.0f;
	
	[Export]
	public int player_id = 0; //Player ID is what makes the different players have separate controls

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hud = GetNode<Control>("%PlayerHUD");
		healthBar = Hud.GetNode<HBoxContainer>("%Lives");
		_collider = GetNode<CollisionShape2D>("%Collider");

		
		health = 2;
		_speed = 200;
		_slowedSpeed = _speed / 2;
		_direction = Vector2.Zero;
		_rightStickInput = Vector2.Zero;
		_aimDirection = Vector2.Right;
		_targetRotation = 0;
		_rotationSpeed = 3f;

		_damageable = true;
		_invTimeMax = 3;
		_invTime = _invTimeMax;
		_playerSprite = this.GetChild<Sprite2D>(0);
		_initialColor = _playerSprite.Modulate;
		_alternateColor = new Color(_initialColor.R / 4, _initialColor.G / 4, _initialColor.B / 4, 256);
		_singleColorTime = 0.5f;
		_doubleColorTime = 1f;

		energy = 100;
		freeze = Hud.GetNode<Timer>("%Freeze");
		freeze.OneShot = true;
		freeze.WaitTime = .25f; // An initial, just so I know everything works correctly
		freeze.Start();
		_leftCooldown = COOLDOWN_MAX;
		_rightCooldown = COOLDOWN_MAX;

		// Idea for placement, UI may have something bettter
		/*if (player_id == 0)
		{
			Hud.Position = new Vector2(-250f, 500f);
		}*/
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

		// If something extra is done with collisions in this class
		if (GetOverlappingAreas().Count != 0)
		{
			//Debug.Print(GetOverlappingAreas().ToString());
		}

		// We use string concatenation to splice in the player ID for the input system
		// The controls will have a naming convention of Action_{player_id}, player ID starts from 0 and goes up to 3
		// Players 1 and 2 can will have keyboard control backups for testing (WASD and arrow keys respectively)
		
		_direction = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized();
		_rightStickInput = Input.GetVector($"AimLeft_{player_id}", $"AimRight_{player_id}", $"AimUp_{player_id}", $"AimDown_{player_id}").Normalized();

		if (Input.IsActionPressed($"Shoot_L_{player_id}") && _leftCooldown >= COOLDOWN_MAX)
		{
			if (energy >= 60)
			{
				FirePattern("res://Pattern1.tscn");
				DrainEnergy(60, .15f);
				//Debug.Print($"Shoot Left P{player_id}");
				_leftCooldown = 0.0;
			}
		}
		else if (Input.IsActionPressed($"Shoot_L_{player_id}")){
			//Debug.Print($"P{player_id} Left on Cooldown");
		}
		if (Input.IsActionPressed($"Shoot_R_{player_id}") && _rightCooldown >= COOLDOWN_MAX){
			if (energy >= 40)
			{
				FirePattern("res://Pattern1.tscn");
				//Debug.Print($"{player_id}");
				//Debug.Print($"Shoot Right P{player_id}");
				_rightCooldown = 0.0;
			}
		}
		else if (Input.IsActionPressed($"Shoot_R_{player_id}")){
			//Debug.Print($"P{player_id} Right on Cooldown");
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
		//Debug.Print("{0}", Rotation - _targetRotation);
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

		if (Input.IsActionPressed($"Slow_{player_id}"))
		{
			Translate(_direction * _slowedSpeed * (float)delta);
		}
		else
		{
			Translate(_direction * _speed * (float)delta);
		}

		if (freeze.TimeLeft == 0)
		{
			energy += 10 * (float)delta;

			if (energy > 100)
			{
				energy = 100;
			}
		}

		// Temp solution so this works for player one

			//Debug.Print("Time:" + freeze.TimeLeft.ToString());
			//Debug.Print("Energy: " + energy);
			freeze.GetParent<ProgressBar>().Value = energy;
			
			// Can't Hide and Show the objects unless I have access to the node
			Array<Node> lives = healthBar.GetChildren();
		
			// Change Health bar display
			for (int i = 2; i >= 0; i--)
			{
				lives[i].Set("visible", health >= i);
			}

		// Invulnerability logic
		if(_invTime >= _invTimeMax) {
			_damageable = true;
		} else {
			_invTime += (float)delta;
		}
		if(_invTime % _doubleColorTime <= _singleColorTime && _invTime < _invTimeMax && _playerSprite.Modulate != _alternateColor) {
			_playerSprite.Modulate = _alternateColor;
		} else if(_invTime % _doubleColorTime > _singleColorTime || _invTime >= _invTimeMax) {
			_playerSprite.Modulate = _initialColor;
		}
		
	}

	public void DamagePlayer(int amount)
	{
		if(_damageable) {
			health -= amount;
			_damageable = false;
			_invTime = 0;
		}
	}

	public void DrainEnergy(int amount, float delayTime = 0)
	{
		energy -= amount;
		if (energy < 0)
		{
			// Potential increase in delay if the energy goes negative
			energy = 0;
		}


		// Add the extra delay to the timer
		if (!(delayTime > 0)) return;
		freeze.WaitTime = freeze.TimeLeft + delayTime;
		freeze.Start();
	}
	
	public void RewardEnergy(int amount)
	{
		energy += amount;

		if (energy > 100)
		{
			energy = 100;
		}
	}
	//takes in pattern and sets properties then spawns
	private void FirePattern(string sceneToFire){
		PackedScene pattern = GD.Load<PackedScene>(sceneToFire);
		var instance = pattern.Instantiate();
		instance.Set("position", this.Position);
		instance.Set("rotation", this.Rotation);
		instance.Set("owner", this);
		AddSibling(instance);
	}
}
