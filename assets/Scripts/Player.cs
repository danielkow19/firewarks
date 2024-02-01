using System;
using System.Diagnostics;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	private CollisionShape2D _collider;
	private float _speed;
	private float _slowedSpeed;
	private Vector2 _direction;
	private Vector2 _aimDirection;
	private float _targetRotation;
	private float _rotationSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collider = GetNode<CollisionShape2D>("%Collider");
		_speed = 6;
		_slowedSpeed = 3;
		_direction = new Vector2(0, 0);
		_aimDirection = new Vector2(0, 0);
		_targetRotation = 0;
		_rotationSpeed = 0.1f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if (Input.IsActionPressed("Up"))
		// {
		// 	Translate(new Vector2(0.0f, -1.0f));
		// }

		// if (Input.IsActionPressed("Left"))
		// {
		// 	Translate(new Vector2(-1.0f, 0.0f));
		// }

		// if (Input.IsActionPressed("Down"))
		// {
		// 	Translate(new Vector2(0.0f, 1.0f));
		// }

		// if (Input.IsActionPressed("Right"))
		// {
		// 	Translate(new Vector2(1.0f, 0.0f));
		// }

		if (GetOverlappingAreas().Count != 0)
		{
			Debug.Print(GetOverlappingAreas().ToString());
		}

		_direction = Input.GetVector("Left", "Right", "Up", "Down").Normalized();
		_aimDirection = Input.GetVector("AimLeft", "AimRight", "AimUp", "AimDown").Normalized();

		if (_aimDirection != Vector2.Zero)
		{
			if (_aimDirection.Y > 0)
			{
				_targetRotation = MathF.Acos(_aimDirection.X);
			}
			else
			{
				_targetRotation = MathF.PI * 2 - MathF.Acos(_aimDirection.X);
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
			Translate(_direction * _slowedSpeed);
		}
		else
		{
			Translate(_direction * _speed);
		}

	}
}
