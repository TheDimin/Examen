using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LocationEvent : UnityEvent<EventLocation> { }

public static class EmergencyManager
{
    private static EventLocation[] eventLocations;

    public static LocationEvent onEventActivated = new LocationEvent();
    public static EventLocation currentEvent { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Awake()
    {
        GetEmergencyLocations();
        currentEvent = null;
        onEventActivated = new LocationEvent();
        GetRandomEventLocation();
    }

    private static void GetEmergencyLocations()
    {
        eventLocations = Transform.FindObjectsOfType<EventLocation>();
    }

    public static void GetRandomEventLocation()
    {
        currentEvent = eventLocations[Random.Range(0, eventLocations.Length)];
        onEventActivated.Invoke(currentEvent);
    }

}



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerController pc;
    private HudDrawer hud;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Already instance found");
            return;
        }

        Instance = this;
        

        pc = Transform.FindObjectOfType<PlayerController>();
        //  SchuilPlaatsManager.Awake();
        //    NavManager.StartPoint = transform.position;
        //  NavManager.Awake();

    }

    private void OnDestroy()
    {
        Instance = null;
        hud.Dispose();
    }

    private void Start()
    {
        EmergencyManager.Awake();
        SchuilPlaatsManager.Awake();

        hud = new HudDrawer();
        hud.LoadWindow();
        hud.Draw();

        WindManager.SetWindDirection(Random.Range(0, 365));
        WindManager.SetWindSpeed(Random.Range(0, 100));

        EmergencyManager.GetRandomEventLocation();
    }


    private void FixedUpdate()
    {
        SchuilPlaatsManager.FindBestSaveZone(pc.transform.position);
        hud.Tick();
    }

    public PlayerController GetPlayerController()
    {
        return pc;
    }

    private void OnDrawGizmos()
    {
        //   NavManager.StartPoint = transform.position;
        //   NavManager.OnDrawGizmos();

        SchuilPlaatsManager.OnDrawGizmos();
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

    }


#endif
}
