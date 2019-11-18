using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowMain : UIWindow
{
    [SerializeField] private Button _play;
    [SerializeField] private Slider _minesCountSlider;
    [SerializeField] private Text _minesCountText;

    public Slider MinesCountSlider { get { return _minesCountSlider; } }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _play.onClick.AddListener(OnPlayClick);
    }

    private void OnPlayClick()
    {
        Hide();
        Game.Ui.hud.Show();
        Game.Instance.StartGame((int) _minesCountSlider.value);
    }


    public void Update()
    {
        _minesCountText.text = "Mines count: " + (int) _minesCountSlider.value;
    }
}