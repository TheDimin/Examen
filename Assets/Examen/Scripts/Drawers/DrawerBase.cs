using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;


public interface IChildDrawer
{
    Drawer ActiveChild { get; set; }
}
public abstract class Drawer : IDisposable
{
    protected abstract string Path { get; set; }
    public GameObject Window { get; private set; }

    public List<Drawer> Childs { get; } = new List<Drawer>();

    public Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = Window.transform;
            return _transform;
        }
    }
    private Transform _transform;

    protected Drawer()
    {

    }

    ~Drawer()
    {
        Dispose();
    }

    public abstract void Draw();

    public virtual bool Back()
    {
        return false;
    }
    //TODO way to prevent boxing ?

    public GameObject LoadWindow()
    {
        if (Window != null)
            return Window;

        Window = Object.Instantiate(Resources.Load<GameObject>(Path));

        return Window;
    }
    public GameObject LoadWindow(Transform parent)
    {
        if (Window != null)
            return Window;

        Window = Object.Instantiate(Resources.Load<GameObject>(Path), parent);

        return Window;
    }

    public TDrawer LoadChild<TDrawer>(Transform transform, string path = "", bool manualDraw = false) where TDrawer : Drawer, new()
    {
        TDrawer d = new TDrawer();
        if (path != "")
            d.Path = path;
        d.LoadWindow(transform);

        if (!manualDraw)
            d.Draw();

        Childs.Add(d);
        return d;
    }

    public void SetActive(bool active)
    {
        Window.SetActive(active);
    }

    public void SetSize(Vector2 size)
    {
        Window.GetComponent<RectTransform>().sizeDelta = size;
    }

    public void SetLocation(Vector2 loc)
    {
        Window.transform.localPosition = loc;
    }

    public Vector2 GetLocation()
    {
        return Window.transform.localPosition;
    }

    public void AddLocation(Vector2 loc)
    {
        Vector2 old = Window.transform.localPosition;

        Window.transform.localPosition = old + loc;
    }

    public virtual void Dispose()
    {
        foreach (var child in Childs)
            child.Dispose();
        Childs.Clear();

        GameObject.Destroy(Window);
    }

    public virtual void Tick()
    {
        foreach (var child in Childs)
        {
            child.Tick();
        }
    }

}

public class WindowItemDrawer : Drawer
{
    public WindowItemDrawer() : base()
    {
    }


    protected override string Path { get; set; } = "DraggedItem";
    public override void Draw()
    {
    }

    public override void Tick()
    {
        base.Tick();
        Window.transform.position = Input.mousePosition;
    }
}

public class ButtonDrawer : Drawer
{
    public delegate void OnAppClicked();
    public event OnAppClicked onAppClicked;

    private string buttonPath = "";
    private string DisplayName = "NO NAME";


    public ButtonDrawer() : base()
    {
    }

    protected override string Path { get; set; } = "NONE";

    public void SetPath(string path)
    {
        Path = path;
    }
    public void SetDisplayName(string name)
    {
        DisplayName = name;
        if (Window != null)
        {
            (buttonPath != null ? (Window.transform.Find(buttonPath)) : (Window.transform)).GetChild(0).GetComponent<TextMeshProUGUI>().text = DisplayName;
        }
    }

    public void SetButtonPath(string path)
    {
        buttonPath = path;
    }

    public override void Draw()
    {
        if (buttonPath != "")
        {
            Window.transform.Find(buttonPath).GetChild(0).GetComponent<TextMeshProUGUI>().text = DisplayName;
            Window.transform.Find(buttonPath).GetComponent<Button>().onClick.AddListener(() => onAppClicked?.Invoke());
            return;
        }
        Window.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DisplayName;
        Window.GetComponent<Button>().onClick.AddListener(() => onAppClicked?.Invoke());
    }
}