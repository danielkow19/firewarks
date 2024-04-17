using Godot;
using System;

public partial class PlayerInfo : Node
{
	// Private fields
	private int _id;
	private Color _color;
	private PackedScene _leftPattern;
	private PackedScene _rightPattern;
	private Vector2 _position;

	// Getters
	public int PlayerID { get {  return _id; } }
	public Color Color { get { return _color; } }
	public PackedScene LeftPattern { get { return _leftPattern; } }
	public PackedScene RightPattern { get { return _rightPattern; } }
	public float X { get { return _position.X; } }
	public float Y { get { return _position.Y; } }
	/// <summary>
	/// This class just holds player information and has no actual functions
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="leftPattern"></param>
	/// <param name="rightPattern"></param>
	/// <param name="color"></param>
	public PlayerInfo(int playerId, PackedScene leftPattern, PackedScene rightPattern, Color color, Vector2 position)
	{
		_id = playerId;
		_color = color;
		_leftPattern = leftPattern;
		_rightPattern = rightPattern;
		_position = position;
	}
	/// <summary>
	/// Default Constructorm AVOID USING!
	/// </summary>
	public PlayerInfo() {
		_id = -1;
		_leftPattern = GD.Load<PackedScene>("res://assets/prefabs/PatternCircleBurst.tscn");
		_rightPattern = GD.Load <PackedScene>("res://assets/prefabs/PatternFastSS.tscn");
		_position.X = 0;
		_position.Y = 0;
	}

}
