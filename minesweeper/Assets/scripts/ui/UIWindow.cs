using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIWindow : MonoBehaviour
{
    [SerializeField] protected GameObject _content = null;
    [SerializeField] protected Button _backButton = null;
    [SerializeField] protected Button _backgroundButton = null;

    public GameObject content { get { return _content; } }
    protected UIWindow _callerWindow;

    public virtual bool isPopup { get { return false; } }

    public bool isVisible { get { return content.activeInHierarchy; } }
    public UIWindow callerWindow { get { return _callerWindow; } }

    public void Initialize()
    {
        if (_backButton != null) {
            _backButton.onClick.AddListener(OnBackClick);
        }

        if (_backgroundButton != null) {
            _backgroundButton.onClick.AddListener(OnBackClick);
        }

        OnInitialize();
    }

    public virtual void Show(UIWindow window = null)
    {
        if (isVisible) {
            return;
        }

        if (window != null) {
            _callerWindow = window;
        }
        OnShowing();
        content.SetActive(true);
        OnShown();
    }

    public virtual void Hide(bool showCallerWindow = true)
    {
        OnHiding();
        content.SetActive(false);
        OnHid();
        if (_callerWindow != null && showCallerWindow) {
            _callerWindow.Show();
            _callerWindow = null;
        }
    }

    protected virtual void OnBackClick()
    {
        Hide();
    }

    protected virtual void OnShowing()
    {
        //showBackground();
        //TODO: 
        //Game.Analytics.OpenWindow(GetType().ToString());
    }

    protected virtual void OnInitialize() { }
    protected virtual void OnShown() { }
    protected virtual void OnHiding() { }

    protected virtual void OnHid()
    {
    }

    protected virtual void Update() { }
    
    /// <summary>
    /// in = true, out = false
    /// </summary>
    /// <param name="inOut"></param>
    /// <returns></returns>
    public IEnumerator FadeInOut(bool inOut)
    {
        float t = 0;
        CanvasGroup cGroup = content.GetComponent<CanvasGroup>();
        if (cGroup == null) {
            yield break;
        }
        if (inOut) {
            Show();
            while (t < 1) {
                t += Time.deltaTime * 2;
                cGroup.alpha = t;
                yield return null;
            }

            cGroup.alpha = 1;
        }
        else {
            while (t > 0) {
                t -= Time.deltaTime * 2;
                cGroup.alpha = t;
                yield return null;
            }

            cGroup.alpha = 0;
            Hide();
        }

        yield break;
    }
}