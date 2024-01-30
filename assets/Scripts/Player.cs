using Godot;

namespace FireWARks.assets.Scripts;

public partial class Player : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		//if (Input.IsActionPressed("Up"))
		//{
			//Translate(new Vector2(0.0f, -1.0f));
		//}
		//
		//if (Input.IsActionPressed("Left"))
		//{
			//Translate(new Vector2(-1.0f, 0.0f));
		//}
		//
		//if (Input.IsActionPressed("Down"))
		//{
			//Translate(new Vector2(0.0f, 1.0f));
		//}
		//
		//if (Input.IsActionPressed("Right"))
		//{
			//Translate(new Vector2(1.0f, 0.0f));
		//}
		
		Translate(Input.GetVector("Left", "Right", "Up", "Down"));
	}
}
