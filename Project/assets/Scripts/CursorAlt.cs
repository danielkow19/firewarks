using Godot;
using System;
using System.Numerics;

public partial class CursorAlt : Node2D
{
	public int playerNum;
	private int positionIndex;
	private Godot.Vector2[] positions = {new Godot.Vector2(510, 100), new Godot.Vector2(850, 100), new Godot.Vector2(510, 200), new Godot.Vector2(855, 200), new Godot.Vector2(510, 300), new Godot.Vector2(850, 300), new Godot.Vector2(565, 400)};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		positionIndex = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Cursor movement Logic
		if(Input.IsActionJustPressed($"Up_{playerNum}")) {
			positionIndex -= 2;
		} else if (Input.IsActionJustPressed($"Down_{playerNum}")) {
			positionIndex += 2;
		} else if (Input.IsActionJustPressed($"Left_{playerNum}")) {
			positionIndex -= 1;
		} else if (Input.IsActionJustPressed($"Right_{playerNum}")) {
			positionIndex += 1;
		}

		if(positionIndex > 6) {
			positionIndex = 6;
		} else if (positionIndex < 0) {
			positionIndex = 0;
		}

		Position = positions[positionIndex];

		// Cursor click logic
		if(Input.IsActionJustPressed($"UI_Click_{playerNum}"))
		{
			switch(positionIndex)
			{
				case 0:
					{
						break;
					}
				case 1:
					{
						break;
					}
				case 2:
					{
						break;
					}
				case 3:
					{
						break;
					}
				case 4:
					{
						break;
					}
				case 5:
					{
						break;
					}
				case 6:
					{
						break;
					}
			}
		}
	}
}
