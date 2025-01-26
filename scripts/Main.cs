using Godot;

namespace Bubblebound.Scripts;

public partial class Main : Node2D
{

    [Export] private PackedScene _enemyScene;

    [Export] private Player _player;

    [Export] private TileMapLayer _tileMapLayer;

    [Export] private Timer _timer;

    [Export] public int EnemyCount { get; private set; }

    public override void _Ready()
    {
        _timer.Timeout += () => AddChild(_enemyScene.Instantiate());
        var camera = GetViewport().GetCamera2D();
        var usedRect = _tileMapLayer.GetUsedRect();
        var tileSize = _tileMapLayer.TileSet.TileSize;
        camera.LimitLeft = usedRect.Position.X * tileSize.X;
        camera.LimitTop = usedRect.Position.Y * tileSize.Y;
        camera.LimitRight = usedRect.End.X * tileSize.X;
        camera.LimitBottom = usedRect.End.Y * tileSize.Y;
    }

}