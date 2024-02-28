using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Transactions;
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

	// Player sprite for death
	private Sprite2D mySprite;
	
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

	// Burst Variables
	private Area2D _burstArea;
	private Timer _burstTimer;
	private float _burstCD;
	
	// Particles
	private GpuParticles2D playerDamaged;

	// Trail Variables
	private Timer _trailTimer;
	private float _trailCD;

	// How long a single color of the invulnerability display lasts before flashing back to the other color
	private float _singleColorTime;
	// Double the value of _singleColorTime, the time a cycle including both colors takes
	private float _doubleColorTime;
	
	// Energy variables, also consider just accessing
	private Timer freeze;
	private float energy;
	private bool firing = false;

	private Timer _leftCooldown;
	private Timer _rightCooldown;
	private Timer _grazeCooldown;
	private const double LEFT_COOLDOWN_MAX = 2.0f;
	private const double RIGHT_COOLDOWN_MAX = 4.0f;

	// Interaction variable
	public int numClouds;
	
	[Export]
	public int player_id = 0; //Player ID is what makes the different players have separate controls
	[Export]
	public bool _isDead; // Whether or not the player is alive, referenced in class and by the game manager
	public bool _canMove; // Used within class and by game manager for pausing
	private bool _uiVisible;
	private float chargeTime = 0;
	private Node currentPattern;
	
	[Export]
	PackedScene patternLeft = null;
	[Export]
	PackedScene patternRight = null;

	PackedScene pattern = GD.Load<PackedScene>("res://assets/prefabs/Pattern1.tscn");
	PackedScene pattern1 = GD.Load<PackedScene>("res://assets/prefabs/Pattern2.tscn");
	PackedScene pattern2 = GD.Load<PackedScene>("res://assets/prefabs/Pattern3.tscn");
	PackedScene pattern3 = GD.Load<PackedScene>("res://assets/prefabs/Pattern4.tscn");
	PackedScene hitFX = GD.Load<PackedScene>("res://assets/prefabs/SFXHit.tscn");
	PackedScene trailBullet = GD.Load<PackedScene>("res://assets/prefabs/TrailBullet.tscn");

	public bool Damageable{ get => _damageable;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(patternLeft == null)
		{
			patternLeft = pattern1;
		}
		if(patternRight == null)
		{
			patternRight = pattern1;
		}
		// Fetch Children
		Hud = GetNode<Control>("%PlayerHUD");
		healthBar = Hud.GetNode<HBoxContainer>("%Lives");
		_collider = GetNode<CollisionShape2D>("%Collider");
		playerDamaged = GetNode<GpuParticles2D>("%PlayerDamaged");
		Sprite2D playerSprite = GetNode<Sprite2D>("%PlayerTexture");
		
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
		_initialColor = _playerSprite.SelfModulate;
		_alternateColor = new Color(_initialColor.R / 4, _initialColor.G / 4, _initialColor.B / 4, 255);
		_singleColorTime = 0.5f;
		_doubleColorTime = 1f;

		_burstArea = GetNode<Area2D>("BurstArea");
		_burstTimer = GetNode<Timer>("%BurstCD");
		_burstTimer.OneShot = true;
		_burstTimer.WaitTime = 0.1f;
		_burstTimer.Start();
		_burstCD = 0.5f;
		_burstArea.Monitoring = false;

		_trailTimer = GetNode<Timer>("%TrailCD");
		_trailTimer.OneShot = true;
		_trailTimer.WaitTime = 0.1f;
		_trailTimer.Start();
		_trailCD = 0.25f;

		// UI and Cool downs
		energy = 100;
		freeze = Hud.GetNode<Timer>("%Freeze");
		freeze.OneShot = true;
		freeze.WaitTime = .25f; // An initial, just so I know everything works correctly
		freeze.Start();
		_uiVisible = true;

		_leftCooldown = GetNode<Timer>("%LeftTimer");
		_leftCooldown.OneShot = true;
		_rightCooldown = GetNode<Timer>("%RightTimer");
		_rightCooldown.OneShot = true;
		_grazeCooldown = GetNode<Timer>("%GrazeCooldown");
		_grazeCooldown.OneShot = true;
		_leftCooldown.WaitTime = .1f;
		_rightCooldown.WaitTime = .1f;
		_grazeCooldown.WaitTime = .1f;
		_leftCooldown.Start();
		_rightCooldown.Start(); 
		_grazeCooldown.Start();

		_isDead = false;
		_canMove = true;
		numClouds = 0;

		// Idea for placement, UI may have something better
		/*if (player_id == 0)
		{
			Hud.Position = new Vector2(-250f, 500f);
		}*/
		
		// Can't Hide and Show the objects unless I have access to the node
		Array<Node> lives = healthBar.GetChildren();
		
		

		Color set;
		switch (player_id)
		{
			case 0:
				set = Colors.Aquamarine;
				break;
			case 1:
				set = Colors.RebeccaPurple;
				break;
			case 2:
				set = Colors.Firebrick;
				break;
			case 3:
				set = Colors.Lime;
				break;
			default:
				set = Colors.White;
				break;
		}

		// Change Phoenix color
		SelfModulate = set;
		playerSprite.Modulate = set;
		
		// Change lives color
		for (int i = 2; i >= 0; i--) // TODO: This should be counting upwards to a max lives value in order to support potential changing of the max lives number.
		{
			((TextureRect)lives[i]).Modulate = set;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//_burstArea.Monitoring = false;

		// We use string concatenation to splice in the player ID for the input system

		if (_isDead)
		{
			_canMove = false;
			healthBar.Hide();
			freeze.GetParent<ProgressBar>().Hide();
			_collider.Disabled = true;
			_playerSprite.Hide();
}
		if (!_canMove)
		{
			// Stops the player inputs from affecting the player object
			return;
		}

		// We use string concatenation to splice in the player ID for the input system
		// The controls will have a naming convention of Action_{player_id}, player ID starts from 0 and goes up to 3
		// Players 1 and 2 can will have keyboard control backups for testing (WASD and arrow keys respectively)
		
		_direction = Input.GetVector($"Left_{player_id}", $"Right_{player_id}", $"Up_{player_id}", $"Down_{player_id}").Normalized();
		_rightStickInput = Input.GetVector($"AimLeft_{player_id}", $"AimRight_{player_id}", $"AimUp_{player_id}", $"AimDown_{player_id}").Normalized();

		// Update cool down timers
		if (Input.IsActionJustPressed($"Shoot_L_{player_id}") && _leftCooldown.TimeLeft == 0)
		{
			if (energy >= 60 && !InCloud())
			{								
				
				DrainEnergy(60, .15f);
				FirePattern(patternLeft);
				firing = true;
				_leftCooldown.WaitTime = _leftCooldown.TimeLeft + LEFT_COOLDOWN_MAX;
				_leftCooldown.Start();
				chargeTime = 0;
				firing = false;
			}
		}
		else if (Input.IsActionJustPressed($"Shoot_L_{player_id}")){
			//Debug.Print($"P{player_id} Left on Cooldown");
		}
		if (Input.IsActionPressed($"Shoot_R_{player_id}") && _rightCooldown.TimeLeft == 0){
			if (energy >= 40 && !InCloud())
			{
				FirePattern(patternRight);
				DrainEnergy(40, .15f);
				firing = true;
				_rightCooldown.WaitTime = _rightCooldown.TimeLeft + RIGHT_COOLDOWN_MAX;
				_rightCooldown.Start();				
				chargeTime = 0;
				firing = false;
			}
		}
		else if (Input.IsActionJustPressed($"Shoot_R_{player_id}")){
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

		
		Translate(_direction * (Input.IsActionPressed($"Slow_{player_id}") ? _slowedSpeed : _speed) * ((InCloud() && _damageable) ? .25f : 1) * (float)delta);

		// Force player to stay in the world, will probably be changed
		if (Position.X < -960)
		{
			Translate(new Vector2((float)Math.Abs(Position.X + 960f), 0));
		}
		if (Position.X > 960)
		{
			Translate(new Vector2(-1 * (float)Math.Abs(Position.X - 960f), 0));
		}
		if (Position.Y < -540)
		{
			Translate(new Vector2(0, (float)Math.Abs(Position.Y + 540f)));
		}
		if (Position.Y > 540)
		{
			Translate(new Vector2(0, -1 * (float)Math.Abs(Position.Y - 540f)));
		}
		

		#region Energy
		if (InCloud())
		{
			// Energy drains while in cloud
			energy -= 10 * (float)delta;
			
			if (energy < 0)
			{
				energy = 0;
				DamagePlayer(1);
			}
}
		else if (freeze.TimeLeft == 0)
		{
			energy += 10 * (float)delta;

			if (energy > 100)
			{
				energy = 100;
			}
		}

		#region UI
			freeze.GetParent<ProgressBar>().Value = energy;
			
			// Can't Hide and Show the objects unless I have access to the node
			Array<Node> lives = healthBar.GetChildren();
		
			// Change Health bar display
			for (int i = 2; i >= 0; i--)
			{
				((TextureRect)lives[i]).Visible = (health >= i);
			}
		#endregion // UI
		#endregion // Energy, technically ended after freeze set

		// Invulnerability logic
		if(_invTime >= _invTimeMax) {
			_damageable = true;
		} else {
			_invTime += (float)delta;
		}
		if(_invTime % _doubleColorTime <= _singleColorTime && _invTime < _invTimeMax && _playerSprite.SelfModulate != _alternateColor) {
			_playerSprite.SelfModulate = _alternateColor;
		} else if(_invTime % _doubleColorTime > _singleColorTime || _invTime >= _invTimeMax) {
			_playerSprite.SelfModulate = _initialColor;
		}
		
		// Burst Logic
		if(Input.IsActionPressed($"Burst_{player_id}") && energy >= 50 && !InCloud() && _burstTimer.TimeLeft == 0) {
			_burstArea.Monitoring = true;
			DrainEnergy(50);
			_burstTimer.WaitTime = _burstCD;
			_burstTimer.Start();
		} else if(_burstTimer.TimeLeft < 0.1f) {
			_burstArea.Monitoring = false;
		}

		// Trails
		if(health == 1 && _trailTimer.TimeLeft == 0) {
			MakeTrail();
		} else if (health == 0 && _trailTimer.TimeLeft == 0) {
			MakeTrail(2f);
		}
	}

	public void DamagePlayer(int amount)
	{
		if(_damageable) {
			AddSibling(hitFX.Instantiate());
			health -= amount;
			_damageable = false;
			_invTime = 0;

			if (health < 0)
			{
				_isDead = true;
			}
			else
			{
				playerDamaged.Emitting = true;
			}
		}
	}

	public void DrainEnergy(float amount, float delayTime = 0)
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

	public bool HasEnergy()
	{
		// Because it works on float it isn't ==0
		return energy >=1;
	}
	
	private bool InCloud()
	{
		return numClouds > 0;
	}
	
	//takes in pattern and sets properties then spawns
	private void FirePattern(PackedScene pToFire)
	{	
		var instance = pToFire.Instantiate();
		instance.Set("position", this.Position);
		instance.Set("rotation", this.Rotation);
		instance.Set("owner", this);
		currentPattern = instance;
		AddSibling(instance);
	}

	private void MakeTrail(float lifetime = 1f) {
		_trailTimer.WaitTime = _trailCD;
		_trailTimer.Start();
		var instance = trailBullet.Instantiate();
		instance.Set("position", this.Position);
		instance.Set("lifetime", lifetime);
		instance.Set("owner", this);
		AddSibling(instance);
	}
	
	// This is an event/signal function
	private void _on_graze(Area2D area)
	{
		if (_grazeCooldown.TimeLeft > 0) return;
		if (area is not Bullet bullet) return;
		if (bullet.owner != this)
		{
			_grazeCooldown.Start(1.25f);
			RewardEnergy(10);
		}
	}

	// Signal for burst area being entered
	public void _on_burst_area_entered(Area2D area) {
		if (area is not Bullet bullet || area.Visible == false) return;
		if(bullet.owner != this) {
			bullet.QueueFree();
		}
	}

	public void ToggleHUD()
	{
		if (_uiVisible)
		{
			healthBar.Hide();
			freeze.GetParent<ProgressBar>().Hide();
			// Hiding Sprite for Now, should be handled else where in future
			_playerSprite.Hide();
		}
		else
		{
			healthBar.Show();
			freeze.GetParent<ProgressBar>().Show();
			_playerSprite.Show();
		}
		_uiVisible = !_uiVisible;
	}
}
