using Godot;
using System;
using System.Diagnostics;

public partial class SelectMenu : Control
{
	public int[] attackIndices = new int[2];
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
	private Area2D attackPreview;
	private int attackIndex1 = 0;
	private int attackIndex2 = 1;
	private PlayerAttackPreview previewScript;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor = GetNode<Node2D>("ColorRect/Cursor");
		displaySprite = GetNode<AnimatedSprite2D>("ColorRect/FlyingSprite");
		color = GetNode<ColorRect>("ColorRect/Color");
		attack1 = GetNode<Label>("ColorRect/Attack1Label");
		attack2 = GetNode<Label>("ColorRect/Attack2Label");
		attackDisplay = GetNode<VideoStreamPlayer>("ColorRect/AttackDisplayer");
		cursorTexture = GetNode<TextureRect>("ColorRect/Cursor/TextureRect");
		prevPos = (int)cursor.Get("positionIndex");
		GetNode<ColorSwap>("ColorRect").sprite = displaySprite;
		attackPreview = GetNode<Area2D>("ColorRect/PlayerAttackPreview");
		previewScript = attackPreview as PlayerAttackPreview;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentPos = (int)cursor.Get("positionIndex");
		//if(((currentPos < 2 && prevPos >= 2) || (currentPos > 1 && currentPos < 4 && (prevPos >= 4 || prevPos <= 1)) || (currentPos > 3 && prevPos <= 3)) || Input.IsActionPressed("UI_Click_0")) {
			switch(currentPos) {
			case 0:
				displaySprite.Visible = true;
				attackDisplay.Visible = false;
				attackPreview.Visible = false;
				previewScript.ReleaseCurrentPattern();
				break;
			case 1:
				displaySprite.Visible = false;
				//attackDisplay.Visible = true;
				attackPreview.Visible = true;
				switch(attack1.Text) {
					case "Circle Burst":
						if(attackIndex1 != 0 || currentPos != prevPos) {
							attackPreview.Set("index", 0);
							attackIndex1 = 0;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = circleBurst;
						break;
					case "Spreadshot":
						if(attackIndex1 != 1 || currentPos != prevPos) {
							attackPreview.Set("index", 1);
							attackIndex1 = 1;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = spreadShot;
						break;
					case "Fast Spreadshot":
						if(attackIndex1 != 2 || currentPos != prevPos) {
							attackPreview.Set("index", 2);
							attackIndex1 = 2;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = fastSS;
						break;
					case "Knot":
						if(attackIndex1 != 3 || currentPos != prevPos) {
							attackPreview.Set("index", 3);
							attackIndex1 = 3;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = knot;
						break;
					case "Swirl":
						if(attackIndex1 != 4 || currentPos != prevPos) {
							attackPreview.Set("index", 4);
							attackIndex1 = 4;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = swirl;
						break;
					case "Weave":
						if(attackIndex1 != 5 || currentPos != prevPos) {
							attackPreview.Set("index", 5);
							attackIndex1 = 5;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = weave;
						break;
					case "Willow":
						if(attackIndex1 != 6 || currentPos != prevPos) {
							attackPreview.Set("index", 6);
							attackIndex1 = 6;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(180,250));
						}
						newStream = willow;
						break;
					default:
						if(attackIndex1 != 0 || currentPos != prevPos) {
							attackPreview.Set("index", 0);
							attackIndex1 = 0;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = circleBurst;
						break;
				}
				if(attackDisplay.Stream != newStream) {
					attackDisplay.Stream = newStream;
					attackDisplay.Play();
				}
				break;
			case 2:
			case 3:
				displaySprite.Visible = false;
				//attackDisplay.Visible = true;
				attackPreview.Visible = true;
				switch(attack2.Text) {
					case "Circle Burst":
						if(attackIndex1 != 0 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 0);
							attackIndex1 = 0;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = circleBurst;
						break;
					case "Spreadshot":
						if(attackIndex1 != 1 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 1);
							attackIndex1 = 1;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = spreadShot;
						break;
					case "Fast Spreadshot":
						if(attackIndex1 != 2 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 2);
							attackIndex1 = 2;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = fastSS;
						break;
					case "Knot":
						if(attackIndex1 != 3 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 3);
							attackIndex1 = 3;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = knot;
						break;
					case "Swirl":
						if(attackIndex1 != 4 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 4);
							attackIndex1 = 4;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(250,250));
						}
						newStream = swirl;
						break;
					case "Weave":
						if(attackIndex1 != 5 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 5);
							attackIndex1 = 5;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = weave;
						break;
					case "Willow":
						if(attackIndex1 != 6 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 6);
							attackIndex1 = 6;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(180,250));
						}
						newStream = willow;
						break;
					default:
						if(attackIndex1 != 1 || currentPos != prevPos && currentPos != 3 && prevPos != 3) {
							attackPreview.Set("index", 1);
							attackIndex1 = 1;
							attackPreview.Set("startFiring", true);
							attackPreview.Set("position", new Vector2(120,250));
						}
						newStream = circleBurst;
						break;
				}
				if(attackDisplay.Stream != newStream) {
					attackDisplay.Stream = newStream;
					attackDisplay.Play();
				}
				break;
			}
		//}
		prevPos = currentPos;
	}
}
