using Godot;

namespace Bubblegum.Scripts;

public partial class Main : Node2D
{
    [Export] private Player _player;
    
    [Export] private PackedScene _enemyScene;

    public override void _Ready()
    {
        AddChild(_enemyScene.Instantiate());
    }
}