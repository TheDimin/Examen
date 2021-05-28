using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerSnap : MonoBehaviour
{
    [SerializeField] private float distance = 50;
    private Transform PcT;

    private void Awake()
    {
        PcT = Transform.FindObjectOfType<PlayerController>().transform;
        if (PcT == null)
            enabled = false;
    }

    private void FixedUpdate()
    {
        transform.position = PcT.position + Vector3.back * distance;
    }
}
