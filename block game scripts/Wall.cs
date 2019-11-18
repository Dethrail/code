using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private const float DelayBeforeNewFigure = 2f;
    public List<WallSeed> Seeds;
    public List<Box> Boxes;
    public BoxCollider gridStopperCollider;
    public Rigidbody epicenter;
    private readonly List<Figure> _figures = new List<Figure>();
    private readonly List<Figure> _figuresForCleanUp = new List<Figure>();

    private Vector3 _storedAnchorPosition;
    private List<WallSeed> _storedSeeds;

    private void Awake()
    {
        _storedSeeds = new List<WallSeed>(Seeds);
    }

    private void Start()
    {
        Boxes = new List<Box>(GetComponentsInChildren<WallBox>());
        foreach (Box box in Boxes)
        {
            box.StoreInitialState();
        }
    }

    private void CutMatchedFigures()
    {
        for (var i = _figures.Count - 1; i >= 0; i--)
        {
            if (_figures[i].inGrid)
            {
                _figuresForCleanUp.Add(_figures[i]);
                _figures.Remove(_figures[i]);
            }
        }
        for (var i = _storedSeeds.Count - 1; i >= 0; i--)
        {
            if (_storedSeeds[i].TargetFigure.inGrid)
            {
                _storedSeeds.Remove(_storedSeeds[i]);
            }
        }
        Seeds = new List<WallSeed>(_storedSeeds);
    }

    private void DissolveUnmatchedBoxes()
    {
        for (var i = _figures.Count - 1; i >= 0; i--)
        {
            DissolveTimer[] dissolveTimers = _figures[i].transform.GetComponentsInChildren<DissolveTimer>();
            foreach (DissolveTimer dissolveTimer in dissolveTimers)
            {
                // dissolve timer will destroy box, detach it from figure
                dissolveTimer.enabled = true;
                dissolveTimer.transform.parent = null;
            }

            Destroy(_figures[i].gameObject);
        }
    }

    private void FreezeBoxes()
    {
        foreach (Box box in Boxes)
        {
            box.Freeze();
        }
    }

    public IEnumerator Reset()
    {
        CutMatchedFigures();
        DissolveUnmatchedBoxes();

        _figures.Clear();
        FreezeBoxes();

        yield return new WaitForSeconds(DelayBeforeNewFigure);

        CreateFigures(_storedAnchorPosition);
    }

    private bool HasSeed()
    {
        return Seeds.Count != 0;
    }

    private Figure CreateFigure(Vector3 anchorPosition)
    {
        if (!HasSeed())
        {
            return null;
        }


        var seed = Seeds[0];
        Seeds.Remove(seed);
        Figure figure = FigureFactory.CreateFigure(seed.type, seed, anchorPosition.z);
        return figure;
    }

    public void CreateFigures(Vector3 anchorPosition)
    {
        _storedAnchorPosition = anchorPosition;
        if (_figures.Count != 0)
        {
            return;
        }

        while (HasSeed())
        {
            _figures.Add(CreateFigure(anchorPosition));
        }
    }

    public void Clear()
    {
        for (int i = Seeds.Count - 1; i >= 0; --i)
        {
            Destroy(Seeds[i].gameObject);
            Seeds.Remove(Seeds[i]);
        }

        for (int i = _figures.Count - 1; i >= 0; --i)
        {
            Destroy(_figures[i].gameObject);
            _figures.Remove(_figures[i]);
        }

        for (int i = _figuresForCleanUp.Count - 1; i >= 0; --i)
        {
            Destroy(_figuresForCleanUp[i].gameObject);
            _figuresForCleanUp.Remove(_figuresForCleanUp[i]);
        }
    }

    public bool IsFilled()
    {
        return _figures.Count((f) => f.inGrid) == _figures.Count;
    }

    public void StartRecord()
    {
        foreach (Box box in Boxes)
        {
            box.StartRecordingPath();
        }

        // assign wall box to mathed figure boxes and record theirs path
        for (var i = _storedSeeds.Count - 1; i >= 0; i--)
        {
            if (_storedSeeds[i].TargetFigure.inGrid)
            {
                foreach (Box box in _storedSeeds[i].TargetFigure.Boxes)
                {
                    WallBox wb = box.gameObject.AddComponent<WallBox>();
                    wb.StoreInitialState();
                    wb.StartRecordingPath();
                    Boxes.Add(wb);
                }
            }
        }
    }
}