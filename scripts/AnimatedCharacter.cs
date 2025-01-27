using System;

using Godot;

namespace Bubblebound.Scripts;

public partial class AnimatedCharacter : CharacterBody2D
{

    protected Vector2 FacingDirection = Vector2.Down;

    [Export]
    protected float MovementSpeed = 200;

    [Export]
    public AnimatedSprite2D AnimatedSprite { get; private set; }

    protected void UpdateVelocity(Vector2 velocity)
    {
        Velocity = velocity;
        if (!velocity.IsZeroApprox()) FacingDirection = velocity;
    }

    protected void PlayAnimation()
    {
        var velocity = Velocity;

        string animation = velocity.IsZeroApprox()
            ? $"idle_{GetAnimationDirection(FacingDirection)}"
            : $"move_{GetAnimationDirection(velocity)}";

        AnimatedSprite.Play(animation);
    }

    private static string GetAnimationDirection(Vector2 velocity)
    {
        return (velocity.X, velocity.Y) switch
        {
            (< 0, >= 0) => "left",
            (> 0, <= 0) => "right",
            (<= 0, < 0) => "up",
            (>= 0, > 0) => "down",
            _ => throw new ArgumentOutOfRangeException(nameof(velocity), velocity, null) // NaN
        };
    }

}