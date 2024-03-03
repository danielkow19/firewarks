using FireWARks.assets.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Linq.Expressions;

public partial class player_settings : Node
{
	private Godot.Collections.Array<PlayerInfo> _players = new Array<PlayerInfo>();
	public Godot.Collections.Array<PlayerInfo> PlayerInfos { get { return _players; } }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void AddPlayerInfo(int playerID, PackedScene leftPattern, PackedScene rightPattern, Color color, float x, float y)
	{
		
		if(_players.Count != 0)
		{
            int index = -1;
			// Loop through the players and make sure we only have
			// 1 player with a given ID at a time
            for (int i =0; i < _players.Count; i++)
			{
				if (_players[i].PlayerID == playerID)
				{
					index = i;
				}
			}
			if (index >= 0) { _players.RemoveAt(index); }
		}

		// Create a PlayerInfo and push it to the array
		PlayerInfo player = new PlayerInfo(playerID, leftPattern, rightPattern, color, x, y);
		_players.Add(player);
	}
}
