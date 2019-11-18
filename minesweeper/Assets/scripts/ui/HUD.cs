using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIWindow
{
    [SerializeField] private Button _pause;

    [SerializeField] private Button _restart;
    [SerializeField] private Button _menu;
    [SerializeField] private GameObject _winState;
    [SerializeField] private GameObject _loseState;

    [SerializeField] private Text _safeZonesLeft; 

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _pause.onClick.AddListener(OnPauseClick);
        _restart.onClick.AddListener(OnRestartClick);
        _menu.onClick.AddListener(OnMenuClick);
    }

    private void OnMenuClick()
    {
        Hide();
        cleanUp();
        Game.Ui.main.Show();
    }

    private void OnRestartClick()
    {
        cleanUp();
        //Game.ui.hud.Show();
        Game.Instance.StartGame((int) Game.Ui.main.MinesCountSlider.value);
    }

    private void OnPauseClick()
    {
        Hide();
        Game.Ui.pause.Show();
    }

    public void SetVictory()
    {
        cleanUp();

        _menu.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
        _winState.gameObject.SetActive(true);
    }

    public void SetDefeat()
    {
        cleanUp();

        _menu.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
        _loseState.gameObject.SetActive(true);
    }

    private void cleanUp()
    {
        _restart.gameObject.SetActive(false);
        _menu.gameObject.SetActive(false);
        _winState.SetActive(false);
        _loseState.SetActive(false);
    }

    private void Update()
    {
        _safeZonesLeft.text = "Safe zones left: " + Game.Instance.SafeZonesLeft;
    }
}