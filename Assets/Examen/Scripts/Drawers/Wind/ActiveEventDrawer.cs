using System;
using System.Collections;
using System.Collections.Generic;
using Examen.Managers;
using UnityEngine;
using UnityEngine.UI;

public class ActiveEventDrawer : Drawer
{
    private EventLocation eventLocation;
    private Transform evenTransform;
    private Camera cam;
    private float size;

    protected override string Path { get; set; } = "Emergency";

    public ActiveEventDrawer() : base()
    {
        EmergencyManager.Instance.onEventActivated.AddListener(OnBestSafeZoneFound);
        cam = Camera.main;
        //  OnBestSafeZoneFound(EmergencyManager.currentEvent);
    }

    private void OnBestSafeZoneFound(EventLocation arg0)
    {
        if (arg0 == null) throw new ArgumentException("Event location is not valid");
        eventLocation = arg0;
        evenTransform = eventLocation.transform;
    }

    public override void Draw()
    {
        size = Window.GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void Tick()
    {
        if (eventLocation == null)
        {
            Debug.LogWarning("Attempted to draw SafeZone, but no valid safezone");
            return;
        }

        Vector3 screenUnclamped = cam.WorldToScreenPoint(evenTransform.position);
        Transform.position = new Vector3(Mathf.Clamp(screenUnclamped.x, size / 2, Screen.width - size / 2),
          Mathf.Clamp(screenUnclamped.y, size / 2, Screen.height - size / 2));
    }


    public override void Dispose()
    {
        EmergencyManager.Instance.onEventActivated.RemoveListener(OnBestSafeZoneFound);
    }
}
