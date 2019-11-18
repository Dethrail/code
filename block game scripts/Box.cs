using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private const float RecordTime = 3f;
    private BoxState _initialState;
    private Rigidbody _rb;

    private List<BoxState> _boxStates = new List<BoxState>();
    private bool _record;
    private float _recordTimer;
    private float _restoreTimer;
    private float _timer;
    private float _lerpValue;
    private bool _restorePath;
    private bool _approach;
    private int _currentIndex;

    private Quaternion _nextRotation;
    private Vector3 _nextPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StoreInitialState()
    {
        _initialState = new BoxState(transform);
    }

    public void Freeze()
    {
        StopBoxFloating();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _restorePath = true;
        _restoreTimer = -0.15f;
        _currentIndex++;
    }

    public void StopBoxFloating()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void StartRecordingPath()
    {
        _record = true;
        _boxStates.Clear();
        _boxStates.Add(_initialState);
    }

    private void Update()
    {
        if (_record)
        {
            RecordPath();
        }

        if (_restorePath)
        {
            RestorePath();
        }
    }

    private void RestorePath()
    {
        if (_restoreTimer <= RecordTime)
        {
            if (_timer >= 1 / 30f - (_currentIndex/3f))
            {
                if (_currentIndex >= 0)
                {
                    if (_currentIndex >= _boxStates.Count)
                    {
                        _currentIndex = _boxStates.Count - 1;
                    }

                    _nextPosition = _boxStates[_currentIndex].position;
                    _nextRotation = _boxStates[_currentIndex].rotation;
                }

                _approach = true;
                _lerpValue = 0;
                _timer = 0;
                _currentIndex--;
            }

            _restoreTimer += Time.deltaTime;
            _timer += Time.deltaTime;
            _lerpValue += Time.deltaTime;

            if (_approach)
            {
                transform.position = Vector3.Lerp(transform.position, _nextPosition, _lerpValue);
                transform.rotation = Quaternion.Slerp(transform.rotation, _nextRotation, _lerpValue);
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.isKinematic = false;
            _restoreTimer = 0;
            _restorePath = false;
            transform.position = _boxStates[0].position;
            transform.rotation = _boxStates[0].rotation;
        }
    }

    private void RecordPath()
    {
        if (_recordTimer <= RecordTime)
        {
            if (_timer >= 1 / 10f)
            {
                _timer = 0;
                _boxStates.Add(new BoxState(transform));
                _currentIndex++;
            }

            _recordTimer += Time.deltaTime;
            _timer += Time.deltaTime;
        }
        else
        {
            _recordTimer = 0;
            _currentIndex--;
            _record = false;
            StopBoxFloating();
        }
    }
}