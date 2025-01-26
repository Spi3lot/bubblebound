using Godot;

namespace Bubblebound.Scripts;

public partial class Over : Control
{

    private static readonly PackedScene GameScene = ResourceLoader.Load<PackedScene>("res://scenes/game.tscn");

    private static readonly PackedScene MenuScene = ResourceLoader.Load<PackedScene>("res://scenes/menu.tscn");

    [Export] private Button _menuButton;

    [Export] private Button _quitButton;

    [Export] private Button _retryButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _retryButton.Pressed += () => GetTree().ChangeSceneToPacked(GameScene);
        _menuButton.Pressed += () => GetTree().ChangeSceneToPacked(MenuScene);
        _quitButton.Pressed += () => GetTree().Quit();
    }

}