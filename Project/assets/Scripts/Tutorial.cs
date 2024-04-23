using Godot;
using System;

public partial class Tutorial : ColorRect
{

	private Button _tutorialButton;
	private bool _readTutorial;
	private int _playerCount;
	private int _hasViewedCount;
	private GameManager _gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Tutorial Button not being set as a reference properly, Try using root pathing
		_tutorialButton = GetNode<Button>("/root/Game2/Tutorial/TutorialScreen/TutorialButton");
		_tutorialButton.Pressed += tutorialButtonPressed;
		_readTutorial = false;
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		_playerCount = settings.PlayerInfos.Count;
		_hasViewedCount = 0;
		_gameManager = GetNode<GameManager>("/root/Game2");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_tutorialButton.GrabFocus();
		for(int i =0; i < _playerCount; i++)
		{
			if(Input.IsActionJustPressed("UI_Click_" +  i))
			{
				_hasViewedCount++;
			}
		}
		if(_hasViewedCount >= _playerCount)
		{
			_readTutorial = true;
		}
	}
	private void tutorialButtonPressed()
	{
		if (_readTutorial)
		{
            GetTree().Paused = false;
			_gameManager.viewedTutorial = true;
            this.Hide();
        }
	}
}
