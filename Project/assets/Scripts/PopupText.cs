using Godot;
using System;

public partial class PopupText : Node
{
	public async void DisplayText(string value, Vector2 position, bool bold = false)
	{
		Label output = new Label();
		output.GlobalPosition = position;
		output.Text = value;
		output.ZIndex = 5;
		output.LabelSettings = new LabelSettings();
		
		output.LabelSettings.FontColor = Colors.White;
		output.LabelSettings.Font = new SystemFont();
		output.LabelSettings.FontSize = 18;
		output.LabelSettings.OutlineColor = Colors.Black;
		output.LabelSettings.OutlineSize = bold ? 5 : 1; // Different if bold
		GD.Print(output.LabelSettings.Font);

		CallDeferred("add_child", output);
		
		GD.Print("1");
		await ToSignal(output, "ready");
		output.Position = new Vector2(output.Position.X - (output.Position.X / 2), output.Position.Y);
		GD.Print("3");
		
		var tween = GetTree().CreateTween();
		tween.SetParallel(true);
		tween.TweenProperty(output, "position:y", output.Position.Y - 24, 0.25).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(output, "position:y", output.Position.Y, 0.5).SetEase(Tween.EaseType.In).SetDelay(0.25);
		tween.TweenProperty(output, "scale", Vector2.Zero, 0.25).SetEase(Tween.EaseType.In).SetDelay(0.5);
		GD.Print("4");

		await ToSignal(tween, "tween_all_completed");
		GD.Print("5");

		output.QueueFree();
	}
}
