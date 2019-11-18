using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MinesweeperGrid
{
    private static Random _rnd;
    private readonly int _gridSize;
    private readonly int _minesCount;

    public bool IsInitialized { get; private set; }
    public bool[,] Field { get; private set; }
    public Cell[,] CellGrid { get; private set; }

    public MinesweeperGrid(int gridSizeParam, int minesCountParam, Cell[,] cellGridParam)
    {
        _gridSize = gridSizeParam;
        _minesCount = minesCountParam;
        CellGrid = cellGridParam;
    }

    public void SetUpFieldExcluding(Point2D point)
    {
        Field = new bool[_gridSize, _gridSize];

        int minesPlaced = 0;
        int i = 0;
        while (minesPlaced < _minesCount) {
            int x = i / _gridSize;
            int y = i % _gridSize;
            i++;
            if (x == point.x && y == point.y) {
                continue;
            }

            minesPlaced++;
            Field[x, y] = true;
        }

        Shuffle(point); // bogosort suffle
        IsInitialized = true;
    }

    // Bogosort with excluding x,y
    private void Shuffle(Point2D point)
    {
        if (_rnd == null) {
            _rnd = new Random();
        }

        for (int x1 = 0; x1 <= Field.GetUpperBound(0); x1++) {
            for (int y1 = 0; y1 <= Field.GetUpperBound(1); y1++) {
                int x2 = _rnd.Next(0, Field.GetUpperBound(0) - 1);
                int y2 = _rnd.Next(0, Field.GetUpperBound(1) - 1);

                if ((point.x == x1 && point.y == y1) || (point.x == x2 && point.y == y2)) {
                    continue;
                }

                Swap(x1, y1, x2, y2);
            }
        }
    }

    private void Swap(int x1, int y1, int x2, int y2)
    {
        bool temp = (bool) Field.GetValue(x1, y1);
        Field.SetValue((bool) Field.GetValue(x2, y2), x1, y1);
        Field.SetValue(temp, x2, y2);
    }

    public bool[,] GetMinedNeighbors(Point2D point)
    {
        bool[,] neighbors = new bool[3, 3];

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (point.x + x < 0 || point.x + x > Field.GetUpperBound(0)) { continue; }

                if (point.y + y < 0 || point.y + y > Field.GetUpperBound(1)) { continue; }

                neighbors[x + 1, y + 1] = Field[point.x + x, point.y + y];
            }
        }

        return neighbors;
    }

    public Cell[,] GetNeighbors(Point2D point)
    {
        Cell[,] neighbors = new Cell[3, 3];

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (point.x + x < 0 || point.x + x > Field.GetUpperBound(0)) { continue; }

                if (point.y + y < 0 || point.y + y > Field.GetUpperBound(1)) { continue; }

                neighbors[x + 1, y + 1] = CellGrid[point.x + x, point.y + y];
            }
        }

        return neighbors;
    }

    private List<Point2D> _watchedCells;

    public void RevealWave(Point2D point)
    {
        _watchedCells = new List<Point2D>();

        WatchCells(GetNeighbors(point));
    }

    private void WatchCells(Cell[,] cells)
    {
        foreach (Cell cell in cells) {
            // possible when close to border
            if (cell == null) {
                continue;
            }

            if (_watchedCells.Contains(cell.Position)) {
                continue;
            }

            _watchedCells.Add(cell.Position);

            if (cell.CellHeigh == 0) {
                cell.Reveal();
                WatchCells(GetNeighbors(cell.Position));
            }
            else {
                if (!cell.HasMine) {
                    cell.Reveal();
                }
            }
        }
    }
}