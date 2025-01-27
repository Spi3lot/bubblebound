using Godot;

namespace Bubblebound.Scripts;

public partial class Gui : CanvasLayer
{

    [Export]
    private ProgressBar _manaBar;

    [Export]
    private ProgressBar _healthBar;

    public override void _Ready()
    {
        _healthBar.MaxValue = Player.Singleton.Health;
        _manaBar.MaxValue = Player.Singleton.Mana;
    }

    public override void _Process(double delta)
    {
        _healthBar.Value = Player.Singleton.Health;
        _manaBar.Value = Player.Singleton.Mana;
    }

}