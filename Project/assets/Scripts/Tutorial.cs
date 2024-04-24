using Godot;
using Godot.Collections;
using System;

public partial class Tutorial : ColorRect
{

	private Button _tutorialButton;
	private bool _readTutorial;
	private int _playerCount;
	private int _hasViewedCount;
	private GameManager _gameManager;
	private player_settings settings;

    // Player Confirmation Feathers
    private TextureRect _p1_Confirm;
	private TextureRect _p2_Confirm;
	private TextureRect _p3_Confirm;
	private TextureRect _p4_Confirm;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tutorialButton = GetNode<Button>("/root/Game2/Tutorial/TutorialScreen/TutorialButton");
		_tutorialButton.Pressed += tutorialButtonPressed;
		_readTutorial = false;

		// Connect confirmation feathers
		_p1_Confirm = GetNode<TextureRect>("/root/Game2/Tutorial/TutorialScreen/TutorialButton/HBoxContainer/P1_Confirm");
		_p2_Confirm = GetNode<TextureRect>("/root/Game2/Tutorial/TutorialScreen/TutorialButton/HBoxContainer/P2_Confirm");
        _p3_Confirm = GetNode<TextureRect>("/root/Game2/Tutorial/TutorialScreen/TutorialButton/HBoxContainer/P3_Confirm");
        _p4_Confirm = GetNode<TextureRect>("/root/Game2/Tutorial/TutorialScreen/TutorialButton/HBoxContainer/P4_Confirm");


        settings = (player_settings)GetNode("/root/PlayerSettings");
		_playerCount = settings.PlayerInfos.Count;
		assignColors(_playerCount);



		_hasViewedCount = 0;
		_gameManager = GetNode<GameManager>("/root/Game2");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Only process if the tutorial is visible
		if(this.Visible) { 
			_tutorialButton.GrabFocus();
            // Show the feather for each player that just pressed continue
            for (int i = 0; i < _playerCount; i++)
            {
                if (Input.IsActionJustPressed("UI_Click_" + i))
                {
                    if (i == 0)
                    {
                        _p1_Confirm.Show();
                    }
                    else if (i == 1)
                    {
                        _p2_Confirm.Show();
                    }
                    else if (i == 2)
                    {
                        _p3_Confirm.Show();
                    }
                    else if (i == 3)
                    {
                        _p4_Confirm.Show();
                    }
                }
            }
            _readTutorial = allConfirmed();
            if (settings.tutorialViewed)
            {
                _readTutorial = true;
                tutorialButtonPressed();
            }
        }
		
	}
	private void tutorialButtonPressed()
	{
		if (_readTutorial)
		{
            GetTree().Paused = false;
			_gameManager.viewedTutorial = true;
			settings.tutorialViewed = true;
            this.Hide();
        }
	}

	private void assignColors(int count)
	{
		if (count > 0)
		{
            _p1_Confirm.Modulate = settings.PlayerInfos[settings.GetPlayerInfoIndexFromID(0)].Color;
        }
		if(count > 1)
		{
            _p2_Confirm.Modulate = settings.PlayerInfos[settings.GetPlayerInfoIndexFromID(1)].Color;
        }
		if(count > 2)
		{
            _p3_Confirm.Modulate = settings.PlayerInfos[settings.GetPlayerInfoIndexFromID(2)].Color;
        }
		if( count > 3)
		{
            _p4_Confirm.Modulate = settings.PlayerInfos[settings.GetPlayerInfoIndexFromID(3)].Color;
        }
	}
	private bool allConfirmed()
	{
		switch(_playerCount)
		{
			case 1:
				{
					if(_p1_Confirm.Visible)
					{
						return true;
					}
					return false;
				}
			case 2:
				{
					if(_p1_Confirm.Visible && _p2_Confirm.Visible)
					{
						return true;
					}
					return false;
				}
			case 3:
				{
					if(_p1_Confirm.Visible && _p2_Confirm.Visible && _p3_Confirm.Visible)
					{
						return true;
					}
					return false;
				}
			case 4:
				{
					if(_p1_Confirm.Visible && _p2_Confirm.Visible && _p3_Confirm.Visible && _p4_Confirm.Visible)
					{
						return true;
					}
					return false;
				}
			default:
				{
					return false;
				}
		}
	}
}
