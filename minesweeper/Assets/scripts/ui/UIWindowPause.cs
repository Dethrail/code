using UnityEngine;
using UnityEngine.UI;

public class UIWindowPause : UIWindow {
    [SerializeField] private Button _restart;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _menu;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        _restart.onClick.AddListener(OnRestartClick);
        _continue.onClick.AddListener(OnContinueCick);
        _menu.onClick.AddListener(OnMenuClick);
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        Game.Ui.hud.Show();
    }

    private void OnMenuClick()
    {
        Hide();
        Game.Ui.main.Show();
    }

    private void OnContinueCick()
    {
        Hide();
        Game.Ui.hud.Show();
    }

    private void OnRestartClick()
    {
        Hide();
        Game.Ui.hud.Show();
        Game.Instance.StartGame((int)Game.Ui.main.MinesCountSlider.value);
    }
}
