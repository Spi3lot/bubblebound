using Godot;

namespace Bubblegum.Scripts;

public partial class Enemy : AnimatedCharacter
{
    public override void _PhysicsProcess(double delta)
    {
        var direction = Player.Singleton.Position - Position;
        UpdateVelocity(direction * MovementSpeed);
        MoveAndSlide();
        PlayAnimation();
    }
}