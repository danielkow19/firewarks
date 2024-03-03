using Godot;
using System;

public partial class PlayerInfo : Node
{
	// Private fields
	private int _id;
	private Color _color;
	private PackedScene _leftPattern;
	private PackedScene _rightPattern;
	private float _x;
	private float _y;

	// Getters
	public int PlayerID { get {  return _id; } }
	public Color Color { get { return _color; } }
	public PackedScene LeftPattern { get { return _leftPattern; } }
	public PackedScene RightPattern { get { return _rightPattern; } }
	public float X { get { return _x; } }
	public float Y { get { return _y; } }
	/// <summary>
	/// This class just holds player information and has no actual functions
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="leftPattern"></param>
	/// <param name="rightPattern"></param>
	/// <param name="color"></param>
	public PlayerInfo(int playerId, PackedScene leftPattern, PackedScene rightPattern, Color color, float x, float y)
	{
		_id = playerId;
		_color = color;
		_leftPattern = leftPattern;
		_rightPattern = rightPattern;
		_x = x;
		_y = y;
	}
}
