using Godot;
using System;

public partial class PlayerAttackPreview : Area2D
{
	private Node currentPattern;

	private PackedScene[] patterns;

	public int index = 0;

	public bool startFiring = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		patterns = new PackedScene[] {GD.Load<PackedScene>("res://assets/prefabs/PatternCircleBurstPreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternSpreadShotPreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternFastSSPreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternKnotPreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternSwirlPreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternWeavePreview.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternWillowPreview.tscn")};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(this.Visible && startFiring) FirePattern(patterns[index]);
	}

	private void FirePattern(PackedScene pToFire)
	{	
		var instance = pToFire.Instantiate();
		instance.Set("position", this.Position);
		instance.Set("rotation", this.Rotation);
		instance.Set("owner", this);
		ReleaseCurrentPattern();
		currentPattern = instance;
		AddChild(instance);
		startFiring = false;
	}

	public void ReleaseCurrentPattern() {
		if(currentPattern != null) {
			PatternPreview pattern = currentPattern as PatternPreview;
			pattern.FreeWaves();
			currentPattern.Free();
			currentPattern = null;
		}
	}
}
