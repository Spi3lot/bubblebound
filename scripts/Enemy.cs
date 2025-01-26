using Godot;

namespace Bubblebound.Scripts;

public partial class Enemy : Area2D
{

    private bool _playerColliding;

    private Bubble _bubble;

    [Export] public float MovementSpeed { get; private set; }

    [Export] public AnimatedSprite2D AnimatedSprite { get; private set; }

    [Export] private Timer _breakoutTimer;

    public bool Trapped { get; private set; }

    public override void _Ready()
    {
        _breakoutTimer.Timeout += () =>
        {
            _bubble.QueueFree();
            _bubble = null;
            Trapped = false;
        };

        BodyEntered += body => { if (body is Player) _playerColliding = true; };
        BodyExited += body => { if (body is Player) _playerColliding = false; };
        var camera = GetViewport().GetCamera2D();

        Position = new Vector2(
            GD.RandRange(camera.LimitLeft, camera.LimitRight),
            GD.RandRange(camera.LimitTop, camera.LimitBottom)
        );
    }

    public override void _Process(double delta)
    {
        if (_playerColliding)
        {
            if (Trapped) Pop();
            else Player.Singleton.TakeDamage(delta);
        }

        if (Trapped) return;
        var direction = Player.Singleton.Position - Position;
        Position += (float) delta * MovementSpeed * direction.Normalized();
    }

    public void Trap(Bubble bubble)
    {
        if (Trapped) return;
        Trapped = true;
        _bubble = bubble;
        _breakoutTimer.Start();
    }

    private void Pop()
    {
        _bubble.QueueFree();
        QueueFree();
    }

}