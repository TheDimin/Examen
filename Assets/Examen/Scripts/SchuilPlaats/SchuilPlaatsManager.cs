using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public static class SchuilPlaatsManager
{

    private static SchuilPlaatsBehaviour[] schuilplaatsen;
    private static Vector2[] locs;
    public static SchuilPlaatsEvent OnBestSafeZoneFound = new SchuilPlaatsEvent();
    public static SchuilPlaatsEvent OnSafeZoneReached = new SchuilPlaatsEvent();
    public static SchuilPlaatsBehaviour LastBestSchuilPlaats { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Awake()
    {
        GenerateSchuilplaatsen();
    }

    private static void GenerateSchuilplaatsen()
    {
        Debug.Log("Generate schuilplaatsen");
        schuilplaatsen = Transform.FindObjectsOfType<SchuilPlaatsBehaviour>();
        locs = schuilplaatsen.Select(behaviour => (Vector2)behaviour.transform.position).ToArray();
    }

    public static SchuilPlaatsBehaviour FindBestSaveZone(Vector2 playerLocation)
    {
        Debug.Log("FindBestSaveZone");
        EventLocation eventLocation = EmergencyManager.currentEvent;
        SchuilPlaatsBehaviour closestSchuilPlaats = null;
        float closestDistance = -1;
        foreach (var schuilPlaats in schuilplaatsen)
        {
            float mag = ((Vector2)schuilPlaats.transform.position - playerLocation).magnitude;
            if (closestSchuilPlaats == null || (mag < closestDistance && (eventLocation.transform.position - schuilPlaats.transform.position).magnitude > 100))
            {
                closestSchuilPlaats = schuilPlaats;
                closestDistance = mag;
            }
        }

        if (closestSchuilPlaats == null)
            throw new Exception("Failed to find safezone");

        if (closestSchuilPlaats == LastBestSchuilPlaats) return closestSchuilPlaats;

        LastBestSchuilPlaats = closestSchuilPlaats;
        OnBestSafeZoneFound.Invoke(LastBestSchuilPlaats);

        return LastBestSchuilPlaats;
    }

    public static void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (schuilplaatsen == null) return;

        foreach (var t in schuilplaatsen)
        {
            if (t == null) return;
            Transform scp = t.transform;
            Gizmos.DrawCube(scp.position, scp.GetComponent<BoxCollider2D>().size * scp.localScale);
        }
    }
}