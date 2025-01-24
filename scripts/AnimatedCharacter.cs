using System;
using Godot;

namespace Bubblegum.Scripts;

public partial class AnimatedCharacter : CharacterBody2D
{
    private Vector2 _lastNonZeroVelocity = Vector2.Zero;

    [Export] private AnimatedSprite2D _sprite;

    [Export] protected float MovementSpeed = 200;

    protected void UpdateVelocity(Vector2 velocity)
    {
        Velocity = velocity;
        if (!velocity.IsZeroApprox()) _lastNonZeroVelocity = velocity;
    }

    protected void PlayAnimation()
    {
        var velocity = Velocity;

        string animation = velocity.IsZeroApprox()
            ? $"idle_{GetAnimationDirection(_lastNonZeroVelocity)}"
            : $"move_{GetAnimationDirection(velocity)}";

        _sprite.Play(animation);
    }

    private static string GetAnimationDirection(Vector2 velocity)
    {
        return (velocity.X, velocity.Y) switch
        {
            (0, < 0) => "up",
            (0, >= 0) => "down",
            (< 0, not float.NaN) => "left",
            (> 0, not float.NaN) => "right",
            _ => throw new ArgumentOutOfRangeException(nameof(velocity), velocity, null) // NaN
        };
    }
}