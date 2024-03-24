using Godot;
using System;
using System.Diagnostics;

public partial class SelectMenu : Control
{
	private Node2D cursor;
	private TextureRect display;
	private ColorRect color;
	private Label attack1;
	private Label attack2;
	private int currentPos;
	private int prevPos;
	private CompressedTexture2D playerTexture = GD.Load<CompressedTexture2D>("res://assets/sprites/phoenix/Phoenix Sketch D.png");
	private AnimatedTexture circleBurst = GD.Load<AnimatedTexture>("res://assets/gifs/circleburst.gif");
	private AnimatedTexture spreadShot = GD.Load<AnimatedTexture>("res://assets/gifs/spreadshot.gif");
	private AnimatedTexture fastSS = GD.Load<AnimatedTexture>("res://assets/gifs/fastss.gif");
	private AnimatedTexture knot = GD.Load<AnimatedTexture>("res://assets/gifs/knot.gif");
	private AnimatedTexture swirl = GD.Load<AnimatedTexture>("res://assets/gifs/swirl.gif");
	private AnimatedTexture weave = GD.Load<AnimatedTexture>("res://assets/gifs/weave.gif");
	private AnimatedTexture willow = GD.Load<AnimatedTexture>("res://assets/gifs/willow.gif");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor = GetNode<Node2D>("ColorRect/Cursor");
		display = GetNode<TextureRect>("ColorRect/DisplayTexture");
		color = GetNode<ColorRect>("ColorRect/Color");
		attack1 = GetNode<Label>("ColorRect/Attack1Label");
		attack2 = GetNode<Label>("ColorRect/Attack2Label");
		prevPos = (int)cursor.Get("positionIndex");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentPos = (int)cursor.Get("positionIndex");
		//if((currentPos < 2 && prevPos >= 2) || (currentPos > 1 && currentPos < 4 && (prevPos >= 4 || prevPos <= 1)) || (currentPos > 3 && prevPos <= 3)) {
			switch(currentPos) {
			case 0:
			case 1:
				display.Texture = playerTexture;
				display.Modulate = color.Color;
				display.Scale = new Vector2(0.3f, 0.3f);
				break;
			case 2:
			case 3:
				display.Modulate = Colors.White;
				display.Scale = new Vector2(0.74f,0.74f);
				switch(attack1.Text) {
					case "Circle Burst":
						display.Texture = circleBurst;
						break;
					case "Spreadshot":
						display.Texture = spreadShot;
						break;
					case "Fast Spreadshot":
						display.Texture = fastSS;
						break;
					case "Knot":
						display.Texture = knot;
						break;
					case "Swirl":
						display.Texture = swirl;
						break;
					case "Weave":
						display.Texture = weave;
						break;
					case "Willow":
						display.Texture = willow;
						break;
					default:
						display.Texture = circleBurst;
						break;
				}
				break;
			case 4:
			case 5:
			case 6:
				display.Modulate = Colors.White;
				display.Scale = new Vector2(0.74f,0.74f);
				switch(attack2.Text) {
					case "Circle Burst":
						display.Texture = circleBurst;
						break;
					case "Spreadshot":
						display.Texture = spreadShot;
						break;
					case "Fast Spreadshot":
						display.Texture = fastSS;
						break;
					case "Knot":
						display.Texture = knot;
						break;
					case "Swirl":
						display.Texture = swirl;
						break;
					case "Weave":
						display.Texture = weave;
						break;
					case "Willow":
						display.Texture = willow;
						break;
					default:
						display.Texture = spreadShot;
						break;
				}
				break;
		}
		//}
		//prevPos = currentPos;
	}
}
