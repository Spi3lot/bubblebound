using Bubblegum.Scripts;
using Godot;

public partial class Bubble : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += Hit;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void Hit(Node2D node)
    {
        if (node is not Enemy enemy) return;
        GD.Print("Hit " + enemy);
    }
}