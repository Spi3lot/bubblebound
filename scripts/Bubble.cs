using Godot;

namespace Bubblebound.Scripts;

public partial class Bubble : Area2D
{

    private bool _trappedEnemy;

    private float _totalDistanceTraveled;

    [Export] public float Speed { get; set; } = 500;

    [Export] public float Range { get; set; } = 500;

    public Vector2 Direction { get; set; }

    public override void _Ready()
    {
        AreaEntered += Hit;
    }

    public override void _Process(double delta)
    {
        if (_trappedEnemy) return;
        float distanceTraveled = (float) delta * Speed;
        _totalDistanceTraveled += distanceTraveled;

        if (_totalDistanceTraveled > Range)
        {
            QueueFree();
            return;
        }

        Position += Direction * distanceTraveled;
    }

    private void Hit(Node2D node)
    {
        if (node is not Enemy enemy || enemy.Trapped) return;
        AreaEntered -= Hit;
        _trappedEnemy = true;
        Scale = new Vector2(10, 10);
        Position = enemy.Position;
        enemy.Trap(this);
    }

}