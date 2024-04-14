using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class PlayerStats : Control
{
	private Label rankLabel;
	private AnimatedSprite2D playerSprite;
	private player_settings settings;
	public int playerNum = -1;
	private int position = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rankLabel = GetNode<Label>("Background/RankLabel");
		playerSprite = GetNode<AnimatedSprite2D>("Background/FlyingSprite");
		settings = (player_settings)GetNode("/root/PlayerSettings");
		for(int i = 0; i < 4; i++) {
			try {
			if(settings.PlayerInfos[i].PlayerID == playerNum) {
				playerSprite.Modulate = settings.PlayerInfos[i].Color;
			}
			} catch (Exception e) {

			}
		}
		position = settings.PlayerPositions[playerNum];
		SetRankLabel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetRankLabel() {
		switch(position) {
			case 1:
				rankLabel.Text = "1st";
				break;
			case 2:
				rankLabel.Text = "2nd";
				break;
			case 3:
				rankLabel.Text = "3rd";
				break;
			case 4:
				rankLabel.Text = "4th";
				break;
			default:
				Debug.Print($"Something Probably Went Wrong, Rank value of {position}");
				break;
		}
	}
}
