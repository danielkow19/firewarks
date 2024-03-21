using FireWARks.assets.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Linq.Expressions;

public partial class player_settings : Node
{
	// Scene information
	private string currentScene;
	private string lobbyScene = "res://assets/prefabs/PlayerNumbers.tscn";


    private Godot.Collections.Array<PlayerInfo> _players = new Array<PlayerInfo>();
	public Godot.Collections.Array<PlayerInfo> PlayerInfos { get { return _players; } }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentScene = GetTree().CurrentScene.SceneFilePath;
		// Clear the list when entering lobby (No Ghost players)
		if(currentScene == lobbyScene) { _players.Clear(); }
    }

	public void AddPlayerInfo(int playerID, PackedScene leftPattern, PackedScene rightPattern, Color color, float x, float y)
	{

		// Create a PlayerInfo and push it to the array
		PlayerInfo player = new PlayerInfo(playerID, leftPattern, rightPattern, color, x, y);
		_players.Add(player);
	}

	public void Clear(){
		_players.Clear();
	}

}
