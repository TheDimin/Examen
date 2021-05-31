using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindDrawer : Drawer
{
    public WindDrawer() : base()
    {
        WindManager.Instance.OnDirectionChange.AddListener(UpdateDirection);
        WindManager.Instance.OnSpeedChange.AddListener(UpdateSpeed);
    }

    protected override string Path { get; set; } = "Wind";
    public override void Draw()
    {
        UpdateDirection(WindManager.Instance.GetWindDirection());
        UpdateSpeed(WindManager.Instance.GetWindSpeed());
    }

    private void UpdateDirection(int dir)
    {
        if (Window == null) return;

        Transform.Find("WindDirectionN").GetComponent<TextMeshProUGUI>().text = dir.ToString();
        Transform.Find("WindDirection").GetComponent<TextMeshProUGUI>().text = dir.GetDirection();
        Transform.Find("Image").localRotation= Quaternion.Euler(0, 0, (float)-(dir - 44));
    }

    private void UpdateSpeed(int speed)
    {
        Transform.Find("WindSpeed").GetComponent<TextMeshProUGUI>().text = speed.ToString();
    }

    public override void Dispose()
    {
        base.Dispose();
        WindManager.Instance.OnDirectionChange.RemoveListener(UpdateDirection);
        WindManager.Instance.OnSpeedChange.RemoveListener(UpdateSpeed);
    }
}
