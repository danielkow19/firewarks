using Godot;
using System;
using System.Diagnostics;

public partial class SelectMenu : Control
{
	private Node2D cursor;
	private AnimatedSprite2D displaySprite;
	private ColorRect color;
	private Label attack1;
	private Label attack2;
	private VideoStreamPlayer attackDisplay;
	private TextureRect cursorTexture;
	private int currentPos;
	private int prevPos;
	private VideoStream circleBurst = GD.Load<VideoStream>("res://assets/videos/circleburst.ogv");
	private VideoStream spreadShot = GD.Load<VideoStream>("res://assets/videos/spreadshot.ogv");
	private VideoStream fastSS = GD.Load<VideoStream>("res://assets/videos/fastss.ogv");
	private VideoStream knot = GD.Load<VideoStream>("res://assets/videos/knot.ogv");
	private VideoStream swirl = GD.Load<VideoStream>("res://assets/videos/swirl.ogv");
	private VideoStream weave = GD.Load<VideoStream>("res://assets/videos/weave.ogv");
	private VideoStream willow = GD.Load<VideoStream>("res://assets/videos/willow.ogv");
	private VideoStream newStream;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor = GetNode<Node2D>("ColorRect/Cursor");
		displaySprite = GetNode<AnimatedSprite2D>("ColorRect/FlyingSprite");
		color = GetNode<ColorRect>("ColorRect/Color");
		attack1 = GetNode<Label>("ColorRect/Attack1Label");
		attack2 = GetNode<Label>("ColorRect/Attack2Label");
		attackDisplay = GetNode<VideoStreamPlayer>("ColorRect/AttackDisplayer");
		cursorTexture = GetNode<TextureRect>("ColorRect/Cursor/CursorTexture");
		prevPos = (int)cursor.Get("positionIndex");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cursorTexture.Modulate = displaySprite.Modulate;
		currentPos = (int)cursor.Get("positionIndex");
		//if(((currentPos < 2 && prevPos >= 2) || (currentPos > 1 && currentPos < 4 && (prevPos >= 4 || prevPos <= 1)) || (currentPos > 3 && prevPos <= 3)) || Input.IsActionPressed("UI_Click_0")) {
			switch(currentPos) {
			case 0:
			case 1:
				displaySprite.Visible = true;
				attackDisplay.Visible = false;
				break;
			case 2:
			case 3:
				displaySprite.Visible = false;
				attackDisplay.Visible = true;
				switch(attack1.Text) {
					case "Circle Burst":
						newStream = circleBurst;
						break;
					case "Spreadshot":
						newStream = spreadShot;
						break;
					case "Fast Spreadshot":
						newStream = fastSS;
						break;
					case "Knot":
						newStream = knot;
						break;
					case "Swirl":
						newStream = swirl;
						break;
					case "Weave":
						newStream = weave;
						break;
					case "Willow":
						newStream = willow;
						break;
					default:
						newStream = circleBurst;
						break;
				}
				if(attackDisplay.Stream != newStream) {
					attackDisplay.Stream = newStream;
					attackDisplay.Play();
				}
				break;
			case 4:
			case 5:
			case 6:
				displaySprite.Visible = false;
				attackDisplay.Visible = true;
				switch(attack2.Text) {
					case "Circle Burst":
						newStream = circleBurst;
						break;
					case "Spreadshot":
						newStream = spreadShot;
						break;
					case "Fast Spreadshot":
						newStream = fastSS;
						break;
					case "Knot":
						newStream = knot;
						break;
					case "Swirl":
						newStream = swirl;
						break;
					case "Weave":
						newStream = weave;
						break;
					case "Willow":
						newStream = willow;
						break;
					default:
						newStream = spreadShot;
						break;
				}
				if(attackDisplay.Stream != newStream) {
					attackDisplay.Stream = newStream;
					attackDisplay.Play();
				}
				break;
			}
		//}
		//prevPos = currentPos;
	}
}
