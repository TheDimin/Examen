using System.Collections;
using System.Collections.Generic;
using Examen.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SchuilPlaatsEvent : UnityEvent<SchuilPlaatsBehaviour> { }

[RequireComponent(typeof(BoxCollider2D), typeof(BoxCollider2D))]
public class SchuilPlaatsBehaviour : MonoBehaviour
{
    private new BoxCollider2D collider;
    public SchuilPlaatsEvent OnTriggerEnter = new SchuilPlaatsEvent();

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
        if (SchuilPlaatsManager.Instance.LastBestSchuilPlaats == this)
        {
            SchuilPlaatsManager.Instance.OnSafeZoneReached.Invoke(this);
        }
    }
}
