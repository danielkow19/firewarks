using FireWARks.assets.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

public partial class player_settings : Node
{
    // Scene information
    private string currentScene;
	private string lobbyScene = "res://player_select.tscn";
	private string mapName;

	

    private Godot.Collections.Array<PlayerInfo> _players = new Array<PlayerInfo>();
	public Godot.Collections.Array<PlayerInfo> PlayerInfos { get { return _players; } }
	public string MapName { get { return mapName; } }


	public int numPlayers;
	private int deathCount = 0;
	private int[] playerPositions = new int[4];
	public int[] PlayerPositions { get { return playerPositions; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        SceneManager scene = (SceneManager)GetNode("/root/SceneManager");
        currentScene = scene.currentScene.SceneFilePath;
        // Clear the list when entering lobby (No Ghost players)
        if (currentScene == lobbyScene) { _players.Clear(); }
        //Connect(TreeEntered, Player_settings_TreeEntered);
        mapName = "boxes";
    }

    public void CheckScene()
    {

    }

    public void AddPlayerInfo(int playerID, PackedScene leftPattern, PackedScene rightPattern, Color color, Vector2 position)
	{
		// Create a PlayerInfo and push it to the array
		PlayerInfo player = new PlayerInfo(playerID, leftPattern, rightPattern, color, position);

		// Make sure only there's only 1 player per ID
		int index = GetPlayerInfoIndexFromID(player.PlayerID);
		// If the index was changed, remove the previous player
		if(index >-1) {
			_players.RemoveAt(index);
			GD.Print("Player Removed, and replaced");
		}
		_players.Add(player);
	}

	public int GetPlayerInfoIndexFromID(int playerID) {
		for(int i = 0; i < _players.Count; i++) {
			if(_players[i].PlayerID == playerID) return i;
		}
		return -1;
	}

	public void RemovePlayerInfoAt(int index) {
		_players.RemoveAt(index);
	}

	public void Clear(){
		_players.Clear();
	}

	// Updates the stored map name. Using this to prevent accidentally changing the name.
	public void SaveMap(string name)
	{
		mapName = name;
	}

	public void playerDeath(int playerID) {
		Debug.Print($"{numPlayers - deathCount}");
		playerPositions[playerID] = numPlayers - deathCount;
		Debug.Print($"{playerPositions[0]}, {playerPositions[1]}, {playerPositions[2]}, {playerPositions[3]}");
		deathCount++;
	}

	public void ResetDeaths() {
		deathCount = 0;
	}
}
