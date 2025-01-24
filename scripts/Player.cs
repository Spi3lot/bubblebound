using Godot;

namespace Bubblegum.Scripts;

public partial class Player : AnimatedCharacter
{
        
    [Export] private PackedScene _bubbleScene;
    
    public static Player Singleton { get; private set; }

    public override void _EnterTree()
    {
        Singleton = this;
    }

    public override void _Process(double delta)
    {
        if (!Input.IsActionPressed("shoot")) return;
        var bubble = _bubbleScene.Instantiate<Bubble>();
        bubble.Position = new Vector2(100, -100);
        AddChild(bubble);
    }

    public override void _PhysicsProcess(double delta)
    {
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
}