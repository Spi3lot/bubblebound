using Godot;

namespace Bubblebound.Scripts;

public partial class Player : AnimatedCharacter
{

    public static Player Singleton { get; private set; }
    
    [Export] private PackedScene _bubbleScene;

    [Export] private Timer _manaTimer;

    [Export] private PackedScene _overScene;

    [Export] public double Health { get; set; } = 1;

    [ExportGroup("Mana")]
    [Export] public int Mana { get; set; } = 100;

    public int CollidingEnemyCount { get; set; }

    public override void _EnterTree()
    {
        Singleton = this;
        int maxMana = Mana;
        _manaTimer.Timeout += () => Mana = Mathf.Min(maxMana, Mana + 1);
    }

    public override void _Process(double delta)
    {
        TakeDamage(delta * CollidingEnemyCount);
        if (!IsShooting()) return;
        Mana--;
        var bubble = _bubbleScene.Instantiate<Bubble>();
        var direction = GetLocalMousePosition().Normalized();
        bubble.Direction = direction;
        bubble.Position = Position + direction * 100;
        AddSibling(bubble);
        AnimatedSprite.Play($"attack_{GetAttackDirection(direction)}");
        FacingDirection = direction;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot")) return;

        var direction = Input.GetVector(
            "move_left",
            "move_right",
            "move_up",
            "move_down"
        );

        UpdateVelocity(direction * MovementSpeed);
        MoveAndSlide();
        PlayAnimation();
    }


    private void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health > 0) return;
        GetTree().ChangeSceneToPacked(_overScene);
    }

    private bool IsShooting() => Input.IsActionPressed("shoot") && Mana > 0;

    private static string GetAttackDirection(Vector2 direction)
    {
        return (direction.X, direction.Y) switch
        {
            (< 0, < 0) => "left_up",
            (< 0, 0) => "left",
            (< 0, > 0) => "left_down",
            (0, < 0) => "up",
            (0, > 0) => "down",
            (> 0, < 0) => "right_up",
            (> 0, 0) => "right",
            (> 0, > 0) => "right_down",
            _ => "congratulations"
        };
    }

}