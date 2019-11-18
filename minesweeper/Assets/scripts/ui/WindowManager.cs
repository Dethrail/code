using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Window Manager holds all UI windows.
/// </summary>
public class WindowManager : MonoBehaviour
{
    private Camera _camera;
    private Canvas _canvas;
    private bool _boundsIsCached;


    public Canvas Canvas { get { return _canvas; } }
    public Camera Camera { get { return _camera; } }


    public bool UIIsLinked { get; private set; }

    #region CachedWindows

    private static Dictionary<Type, UIWindow> _cachedWindows;

    public UIWindowMain main { get; private set; }
    public HUD hud { get; private set; }
    public UIWindowPause pause { get; private set; }

    private void CacheWindowInstances()
    {
        main = GetWindow<UIWindowMain>();
        hud = GetWindow<HUD>();
        pause = GetWindow<UIWindowPause>();
    }

    private static T GetWindow<T>() where T : UIWindow
    {
        var t = typeof(T);
        UIWindow window;
        if (_cachedWindows.TryGetValue(t, out window)) {
            return window as T;
        }

        Debug.LogWarning("Sadly, window not found: " + t);
        return null;
    }

    public void LinkGameUi()
    {
        if (UIIsLinked) {
            return;
        }

        UIIsLinked = true;
        var controllersTransform = new GameObject("ui controllers").transform;
        UIWindow[] windows = _camera.GetComponentsInChildren<UIWindow>(true);

        foreach (UIWindow uiWindow in windows) {
            uiWindow.transform.SetParent(_canvas.transform, false);
            uiWindow.gameObject.SetActive(true);
        }

        CacheWindows();
        CacheWindowInstances();
        InitializeWindows();

        foreach (UIWindow uiWindow in windows) {
            uiWindow.transform.GetChild(0).name = uiWindow.transform.name;
            uiWindow.transform.GetChild(0).SetParent(_canvas.transform, false);
            uiWindow.transform.SetParent(controllersTransform, false);
        }
    }

    private void CacheWindow(Transform tr)
    {
        var window = tr.GetComponent<UIWindow>();
        if (window == null || !window.enabled) {
            return;
        }

        Type t = window.GetType();
        if (_cachedWindows.ContainsKey(t)) {
            Debug.LogError("There can be only one " + t + " window key!");
        }
        else if (_cachedWindows.ContainsValue(window)) {
            Debug.LogError("There can be only one " + t + " window!");
        }
        else {
            _cachedWindows[t] = window;
        }
    }

    private void CacheWindows()
    {
        _cachedWindows = new Dictionary<Type, UIWindow>(_canvas.transform.childCount); //  + _popupsParent.childCount
        for (int i = 0; i < _canvas.transform.childCount; i++) {
            CacheWindow(_canvas.transform.GetChild(i));
        }
    }

    #endregion CachedWindows

    public void Init(Camera cameraParam, Canvas canvas)
    {
        _camera = cameraParam;
        _canvas = canvas;
        LinkGameUi();
    }

    private void InitializeWindows()
    {
        foreach (UIWindow w in _cachedWindows.Values) {
            w.Initialize();
            w.content.SetActive(false);
        }
    }
}