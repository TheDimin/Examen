using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneLocationDrawer : Drawer
{
    private SchuilPlaatsBehaviour targetSchuilPlaats = null;
    private Transform SafeZoneTransform;
    private Camera cam;
    private float size;

    public SafeZoneLocationDrawer() : base()
    {
        OnBestSafeZoneChange(SchuilPlaatsManager.LastBestSchuilPlaats);
        SchuilPlaatsManager.OnBestSafeZoneFound.AddListener(OnBestSafeZoneChange);
        cam = Camera.main;
    }

    protected override string Path { get; set; } = "SchuilPlaatsLocation";
    public override void Draw()
    {
        size = Window.GetComponent<RectTransform>().sizeDelta.x;
        if (targetSchuilPlaats == null)
            Window.SetActive(false);
    }

    public override void Tick()
    {
        if (targetSchuilPlaats == null)
        {
            Debug.LogWarning("Attempted to draw SafeZone, but no valid safezone");
            return;
        }

        Vector3 screenUnclamped = cam.WorldToScreenPoint(SafeZoneTransform.position);
        Transform.position = new Vector3(Mathf.Clamp(screenUnclamped.x, size / 2, Screen.width - size / 2),
            Mathf.Clamp(screenUnclamped.y, size / 2, Screen.height - size / 2));
    }

    private void OnBestSafeZoneChange(SchuilPlaatsBehaviour newSafeZone)
    {
        if (newSafeZone == null)
        {
            Debug.LogError("New safe zone null");
            return;
        }
        Debug.Log("FoundSafeZone");

        targetSchuilPlaats = newSafeZone;
        SafeZoneTransform = targetSchuilPlaats?.transform;
        Window?.SetActive(targetSchuilPlaats != null);
    }

    public override void Dispose()
    {
        base.Dispose();
        SchuilPlaatsManager.OnBestSafeZoneFound.RemoveListener(OnBestSafeZoneChange);
    }
}
