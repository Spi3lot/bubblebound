using System;

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

    public override void _EnterTree()
    {
        Singleton = this;
        int maxMana = Mana;
        _manaTimer.Timeout += () => Mana = Mathf.Min(maxMana, Mana + 1);
    }

    public override void _Process(double delta)
    {
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

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) GetTree().ChangeSceneToPacked(_overScene);
    } 

    private bool IsShooting() => Input.IsActionPressed("shoot") && Mana > 0;

    private static string GetAttackDirection(Vector2 direction)
    {
        float radians = direction.Angle();
        float degrees = Mathf.RadToDeg(radians);
        degrees = Mathf.PosMod(degrees, 360);
        
        return (degrees) switch
        {
            >= 315 + 22.5f or < 0 + 22.5f => "right",
            >= 45 - 22.5f and < 45 + 22.5f => "right_down",
            >= 90 - 22.5f and < 90 + 22.5f => "down",
            >= 135 - 22.5f and < 135 + 22.5f => "left_down",
            >= 180 - 22.5f and < 180 + 22.5f => "left",
            >= 225 - 22.5f and < 225 + 22.5f => "left_up",
            >= 270 - 22.5f and < 270 + 22.5f => "up",
            >= 315 - 22.5f and < 315 + 22.5f => "right_up",
            float.NaN => throw new InvalidOperationException("Degrees can't be float.NaN")
        };
    }

}