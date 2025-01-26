using Godot;

namespace Bubblebound.Scripts;

public partial class Enemy : Area2D
{

    [Export] protected float MovementSpeed = 200;

    [Export] public AnimatedSprite2D AnimatedSprite { get; private set; }

    public bool Trapped { get; set; }

    public override void _Ready()
    {
        BodyEntered += IncrementPlayerCollidingEnemyCount;
        BodyExited += DecrementPlayerCollidingEnemyCount;
        var camera = GetViewport().GetCamera2D();

        Position = new Vector2(
            GD.RandRange(camera.LimitLeft, camera.LimitRight),
            GD.RandRange(camera.LimitTop, camera.LimitBottom)
        );
    }

    public override void _Process(double delta)
    {
        if (Trapped || Player.Singleton.Health <= 0) return;
        var direction = Player.Singleton.Position - Position;
        Position += (float) delta * MovementSpeed * direction.Normalized();
    }

    public void Trap(Bubble bubble)
    {
        if (Trapped) return;
        Trapped = true;
        BodyExited -= DecrementPlayerCollidingEnemyCount;
        BodyEntered -= IncrementPlayerCollidingEnemyCount;

        BodyEntered += body =>
        {
            if (body is not Player) return;
            QueueFree();
            bubble.QueueFree();
        };
    }

    private static void IncrementPlayerCollidingEnemyCount(Node body)
    {
        if (body is Player player) player.CollidingEnemyCount++;
    }

    private static void DecrementPlayerCollidingEnemyCount(Node body)
    {
        if (body is Player player) player.CollidingEnemyCount--;
    }

}