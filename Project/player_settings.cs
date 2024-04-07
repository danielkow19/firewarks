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
	private string mapName;


	private Godot.Collections.Array<PlayerInfo> _players = new Array<PlayerInfo>();
	public Godot.Collections.Array<PlayerInfo> PlayerInfos { get { return _players; } }
	public string MapName { get { return mapName; } }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneManager scene = (SceneManager)GetNode("/root/SceneManager");
		currentScene = scene.currentScene.SceneFilePath;
		// Clear the list when entering lobby (No Ghost players)
		if(currentScene == lobbyScene) { _players.Clear(); }
		mapName = "boxes";
	}

	public void AddPlayerInfo(int playerID, PackedScene leftPattern, PackedScene rightPattern, Color color, Vector2 position)
	{
		// Create a PlayerInfo and push it to the array
		PlayerInfo player = new PlayerInfo(playerID, leftPattern, rightPattern, color, position);

		// Make sure only there's only 1 player per ID
		int index = -1;
		for(int i = 0; i < _players.Count; i++)
		{
			if (_players[i].PlayerID == player.PlayerID)
			{
				index = i; break;
			}
		}
		// If the index was changed, remove the previous player
		if(index >-1) {
			_players.RemoveAt(index);
			GD.Print("Player Removed, and replaced");
		}
		_players.Add(player);
	}

	public void Clear(){
		_players.Clear();
	}

}
