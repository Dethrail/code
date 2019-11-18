using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Text _cellText = null;
    [SerializeField] private Image _cellColor = null;
    [SerializeField] private Button _button = null;

    [SerializeField] private Point2D _position;
    [SerializeField] private bool _isReveald;
    [SerializeField] private bool _hasMine;
    [SerializeField] private int _cellHeigh;

    private MinesweeperGrid _gridReference = null;

    public Point2D Position { get { return _position; } private set { _position = value; } }
    public Button Button { get { return _button; } private set { _button = value; } }
    public Image CellColor { get { return _cellColor; } }
    public Text CellText { get { return _cellText; } private set { _cellText = value; } }
    public bool IsReveald { get { return _isReveald; } private set { _isReveald = value; } }
    public bool HasMine { get { return _hasMine; } private set { _hasMine = value; } }
    public int CellHeigh { get { return _cellHeigh; } private set { _cellHeigh = value; } }

    private Color[] _colors = new Color[] {
        new Color(1.0f, 0.8f, 0.8f, 1), // 0
        new Color(1.0f, 0.7f, 0.7f, 1), // 1
        new Color(1.0f, 0.6f, 0.6f, 1), // 2
        new Color(1.0f, 0.5f, 0.5f, 1), // 3
        new Color(1.0f, 0.4f, 0.4f, 1), // 4
        new Color(1.0f, 0.3f, 0.3f, 1), // 5
        new Color(1.0f, 0.25f, 0.25f, 1), // 6
        new Color(1.0f, 0.2f, 0.2f, 1), // 7
        new Color(1.0f, 0.15f, 0.15f, 1), // 8
        new Color(1.0f, 0f, 0f, 1), // 9 // impossible??
    };

    public void Init(int x, int y)
    {
        Position = new Point2D(x, y);
        CellHeigh = 0;

        IsReveald = false;
    }

    public void Rearm(bool hasMineParam)
    {
        HasMine = hasMineParam;
    }

    public void CalculateHeigh(bool[,] getNeighbors)
    {
        foreach (var neighbor in getNeighbors) {
            if (neighbor) {
                CellHeigh++;
            }
        }
    }

    public void Reveal()
    {
        Button.interactable = false;
        var cb = new ColorBlock();
        cb.colorMultiplier = 1;
        cb.fadeDuration = 0.1f;
        cb.disabledColor = _colors[CellHeigh];
        Button.colors = cb;
        IsReveald = true;
        
        if (CellHeigh != 0) {
            CellText.text = "" + CellHeigh;
        }
    }
}