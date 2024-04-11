using Godot;
using System;

public partial class TimeTracker : Label
{
    private GameManager manager;

    public override void _Ready()
    {
        manager = GetTree().Root.GetNode<GameManager>("Game2");
    }
    
    public override void _Process(double delta)
    { 
        float seconds = (float)manager.WorldBorder.countDown.TimeLeft % 60;

        if (manager.WorldBorder.countDown.TimeLeft > 59.99)
        {
            float minutes = (int)(manager.WorldBorder.countDown.TimeLeft / 60) % 60;

            if (seconds < 10)
            {
                Text = $"{minutes}:0{(int)seconds}";
            }
            else
            {
                Text = $"{minutes}:{(int)seconds}";
            }
        }
        else
        {
            Text = $"{seconds:F}";
        }
    }
}
