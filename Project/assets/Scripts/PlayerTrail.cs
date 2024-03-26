using System.Collections.Generic;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	//private int maxPoints = 100;
	private Curve2D curve;
	private List<CollisionShape2D> tailSegments;
	private List<float> timeActive;
	private Player playerRef;

	private bool isVisible = true;
	private bool deleteFirstValue = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = GetParent<Area2D>() as Player;
		curve = new Curve2D();
		tailSegments = new List<CollisionShape2D>();
		timeActive = new List<float>();
		Modulate = playerRef.Modulate;
		
		/*for (int i = 0; i < maxPoints - 1; i++)
		{
			// Temp, doesn't need to be saved in class but will in scene
			Area2D segment = new Area2D();
			CollisionShape2D collisionShape = new CollisionShape2D();
			collisionShape.Shape = new SegmentShape2D();
			segment.AddChild(collisionShape);
			segment.CollisionLayer = 2;
				
			tailSegments.Add(collisionShape);
			((SegmentShape2D)tailSegments[i].Shape).A = Position;
			((SegmentShape2D)tailSegments[i].Shape).B = Position;
			
			// Set up signals for the area2Ds so they don't need to be saved
			AddChild(segment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			Callable callable = new Callable(this, nameof(OnAreaEntered));
			segment.Connect("area_entered", callable);
		}*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		/*Player parent = GetParent<Area2D>() as Player;
		foreach (var segment in tailSegments)		
		{
			RemoveChild(segment);
			segment.QueueFree();
		}

		// Add new point
		curve.AddPoint(new Vector2(parent.Position.X + 20 * (Mathf.Cos(parent.Rotation) * -1), 
			parent.Position.Y + 20 * (Mathf.Sin(parent.Rotation) * -1)));
		timeActive.Add(0); // new time tracker

		// Fixes problem with player teleporting after first frame
		if (deleteFirstValue)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
			deleteFirstValue = false;
		}
		
		// Only start deleting if a line is drawn
		if (curve.PointCount <= 1) return;
		
		
		float totalLength = 0;
		
		for (int i = 0; i < curve.PointCount; i++)
		{
			// last point doesn't need to be incremented
			if (i != curve.PointCount - 1)
			{
				timeActive[i] += (float)delta;
			}

			float saveDist = 0;
			
			// Get the length so far; checking first point would crash
			if (i != 0 && i != curve.PointCount - 1)
			{
				saveDist = Points[i].DistanceTo(Points[i - 1]);
			}
			else if (i == curve.PointCount - 1)
			{
				// This block is for the newest point as it isn't part of curve
				saveDist = new Vector2(parent.Position.X + 20 * (Mathf.Cos(parent.Rotation) * -1),
					parent.Position.Y + 20 * (Mathf.Sin(parent.Rotation) * -1)).DistanceTo(Points[i - 1]);
			}

			// Due to player spawning there will occasionally be random large values at the beginning
			if (saveDist < 10)
			{
				totalLength += saveDist;
			}
		}

		#region DistanceCulling
		
		float goalLength = 150f / (parent.Health + 1);
		float startCutting = totalLength - goalLength;

		// Line is too long start removing points at the beginning
		if (startCutting > 0)
		{
			float activeLength = 0;
			int cutIndex = -1;

			for (int i = 1; i < Points.Length; i++)
			{
				activeLength += Points[i].DistanceTo(Points[i - 1]);

				if (activeLength > startCutting)
				{
					cutIndex = i;
					break;
				}
			}
			
			// It was most recent point that put it over, only remove one point
			if (cutIndex == -1)
			{
				cutIndex = 1;
			}

			int count = 0;
			while (count < cutIndex)
			{
				curve.RemovePoint(0);
				timeActive.RemoveAt(0);
				count++;
			}
		}
		#endregion
		
		// Time Culling
		float timeAllowed = 1.0f / (parent.Health + 1);
		while (timeActive[0] > timeAllowed)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
		}
		
		// Save Collision segments that should accurately represent the curve
		tailSegments.Clear();
		Points = curve.GetBakedPoints(); // TODO: will this add points? no callable function yet
		for (int i = 0; i < Points.Length - 1; i++)
		{
			// Temp, doesn't need to be saved in class but will in scene
			Area2D segment = new Area2D();
			CollisionShape2D collisionShape = new CollisionShape2D();
			collisionShape.Shape = new SegmentShape2D();
			segment.AddChild(collisionShape);
			segment.CollisionLayer = 2;
				
			tailSegments.Add(collisionShape);
			((SegmentShape2D)tailSegments[i].Shape).A = Points[i];
			((SegmentShape2D)tailSegments[i].Shape).B = Points[i + 1];
			AddChild(segment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			Callable callable = new Callable(this, nameof(OnAreaEntered));
			segment.Connect("area_entered", callable);
		}*/
	}
	
	private void OnAreaEntered(Area2D area)
	{
		// Check if the area that entered is a player
		if (area is Player player && player != playerRef)
		{
			player.DamagePlayer(1);
		}
	}

	public void ToggleTrail()
	{
		if (isVisible)
		{
			this.Hide();
		}
		else
		{
			this.Show();
		}
		isVisible = !isVisible;
	}
}
