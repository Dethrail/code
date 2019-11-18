using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    #region references

    [SerializeField] private WindowManager _windowManager;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform _container;

    #endregion

    #region static

    public static WindowManager Ui { get { return Instance._windowManager; } }
    public static Game Instance { get; private set; }

    #endregion

    private void Awake()
    {
        #region Singleton

        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        #endregion

        Ui.Init(_camera, _canvas);
        Ui.main.Show();
    }

    #region gameLogic

    private MinesweeperGrid _minesweeper;
    public int SafeZonesLeft;

    public void StartGame(int minesCount)
    {
        foreach (Transform cell in _container) {
            Destroy(cell.gameObject);
        }

        Cell[,] cells = new Cell[10, 10];
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                Cell c = GameObject.Instantiate(_cellPrefab, _container, false);
                c.Init(i, j);
                c.Button.onClick.AddListener(OnCellClick);
                cells[i, j] = c;
            }
        }

        _minesweeper = new MinesweeperGrid(10, minesCount, cells);
        SafeZonesLeft = 100;
    }

    private void InitializeBoard(Cell initCell)
    {
        _minesweeper.SetUpFieldExcluding(initCell.Position);

        foreach (Cell cell in _minesweeper.CellGrid) {
            cell.Rearm(_minesweeper.Field[cell.Position.x, cell.Position.y]);
        }

        foreach (Cell cell in _minesweeper.CellGrid) {
            cell.CalculateHeigh(_minesweeper.GetMinedNeighbors(cell.Position));
        }
        SafeZonesLeft = _minesweeper.CellGrid.Cast<Cell>().Count(x => !x.IsReveald && !x.HasMine);
    }

    private void ProcessMinedCell(Cell currentCell)
    {
        currentCell.CellColor.color = Color.red;
        Ui.hud.SetDefeat();
        foreach (Cell cell in _minesweeper.CellGrid)
        {
            if (cell.HasMine)
            {
                cell.CellColor.color = Color.red;
            }
        }
    }

    private void OnCellClick()
    {
        Cell currentCell = EventSystem.current.currentSelectedGameObject.GetComponent<Cell>();

        if (!_minesweeper.IsInitialized)
        {
            InitializeBoard(currentCell);
        }

        if (currentCell.HasMine)
        {
            ProcessMinedCell(currentCell);
            return;
        }

        _minesweeper.RevealWave(currentCell.Position);

        SafeZonesLeft = _minesweeper.CellGrid.Cast<Cell>().Count(x => !x.IsReveald && !x.HasMine);

        if (SafeZonesLeft == 0) {
            Ui.hud.SetVictory();
        }
    }

    #endregion
}