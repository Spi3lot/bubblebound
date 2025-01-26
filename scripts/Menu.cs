using Godot;

namespace Bubblebound.Scripts;

public partial class Menu : Control
{

    [ExportGroup("Bubbles")] [Export] private PackedScene _bubbleScene;

    [Export] private Timer _bubbleTimer;

    [Export] private PackedScene _gameScene;

    [Export] private Button _playButton;

    [Export] private Button _quitButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playButton.Pressed += () => GetTree().ChangeSceneToPacked(_gameScene);
        _quitButton.Pressed += () => GetTree().Quit();

        _bubbleTimer.Timeout += () =>
        {
            var bubble = _bubbleScene.Instantiate<Bubble>();
            var viewportRectSize = GetViewportRect().Size;
            bubble.Range = viewportRectSize.Y;
            bubble.Position = new Vector2(GD.Randf() * viewportRectSize.X, viewportRectSize.Y);
            bubble.Direction = Vector2.Up;
            AddChild(bubble);
        };
    }

}