using System.Collections.Generic;
using System.Linq;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	private int maxPoints = 100;
	private Curve2D curve;
	private List<CollisionShape2D> tailSegments;
	private List<float> timeActive;
	private Player playerRef;

	private bool isVisible = true;
	private bool deleteFirstValue = true;

	private Area2D segment = new Area2D();
	private CollisionShape2D collisionShape = new CollisionShape2D();
	private Player parent;
	private Callable callable;
	private List<float> lengths;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = GetParent<Area2D>() as Player;
		curve = new Curve2D();
		tailSegments = new List<CollisionShape2D>();
		timeActive = new List<float>();
		lengths = new List<float>();
		Modulate = playerRef.Modulate;
		collisionShape.Shape = new SegmentShape2D();
		segment.CollisionLayer = 2;
		parent = GetParent<Area2D>() as Player;
		callable = new Callable(this, nameof(OnAreaEntered));
		
		for (int i = 0; i < maxPoints - 1; i++)
		{
			// Temp, doesn't need to be saved in class but will in scene
			Area2D newSegment = (Area2D)segment.Duplicate();
			CollisionShape2D newColShape = (CollisionShape2D)collisionShape.Duplicate();
			newSegment.AddChild(newColShape);	
			tailSegments.Add(newColShape);
			((SegmentShape2D)tailSegments[i].Shape).A = Position;
			((SegmentShape2D)tailSegments[i].Shape).B = Position;
			AddChild(newSegment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			newSegment.Connect("area_entered", callable);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Points = curve.GetBakedPoints(); // TODO: will this add points? no callable function yet

		// Add new point
		curve.AddPoint(new Vector2(parent.Position.X + 20 * (Mathf.Cos(parent.Rotation) * -1), 
			parent.Position.Y + 20 * (Mathf.Sin(parent.Rotation) * -1)));
		timeActive.Add(0); // new time tracker		
		if(Points.Length > 1){
			lengths.Add(Points.Last<Vector2>().DistanceTo(Points[Points.Length - 1]));
			Area2D newSegment = (Area2D)segment.Duplicate();
			CollisionShape2D newColShape = (CollisionShape2D)collisionShape.Duplicate();
			newSegment.AddChild(newColShape);	
			tailSegments.Add(newColShape);
			((SegmentShape2D)tailSegments[tailSegments.Count-1].Shape).A = Points[Points.Length - 1];
			((SegmentShape2D)tailSegments[tailSegments.Count-1].Shape).B = Points[Points.Length - 2];
			AddChild(newSegment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			newSegment.Connect("area_entered", callable);
		}
		
		// Fixes problem with player teleporting after first frame
		if (deleteFirstValue)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
			var deleteThis = tailSegments[0];
			tailSegments.RemoveAt(0);
			deleteThis.QueueFree();
			deleteFirstValue = false;
		}
		
		// Only start deleting if a line is drawn
		if (curve.PointCount <= 1) return;
		
		for (int i = 0; i < curve.PointCount; i++)
		{
			// last point doesn't need to be incremented
			if (i != curve.PointCount - 1)
			{
				timeActive[i] += (float)delta;
			}
		}

		#region DistanceCulling
		
		float goalLength = 150f / (parent.Health + 1);
		while(lengths.Sum() > goalLength)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
			lengths.RemoveAt(0);
			var deleteThis = tailSegments[0];
			tailSegments.RemoveAt(0);
			deleteThis.QueueFree();
		}
		
		#endregion
		
		// Time Culling
		float timeAllowed = 1.0f / (parent.Health + 1);
		while(timeActive[0] > timeAllowed)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
			lengths.RemoveAt(0);
			var deleteThis = tailSegments[0];
			tailSegments.RemoveAt(0);
			deleteThis.QueueFree();
		}
		
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
