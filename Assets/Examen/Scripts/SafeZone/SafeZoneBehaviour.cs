using System.Collections;
using System.Collections.Generic;
using Examen.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SafeZoneEvent : UnityEvent<SafeZoneBehaviour> { }

[RequireComponent(typeof(BoxCollider2D), typeof(BoxCollider2D))]
public class SafeZoneBehaviour : MonoBehaviour
{
    private new BoxCollider2D collider;
    public SafeZoneEvent OnTriggerEnter = new SafeZoneEvent();

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnValidate()
    {
        if (collider == null)
            collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter.Invoke(this);

        if (SafeZoneManager.Instance.LastBestSafeZone == this)
        {
            SafeZoneManager.Instance.OnSafeZoneReached.Invoke(this);
        }
    }
}
