using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerSnap : MonoBehaviour
{
    [SerializeField] private float distance = 50;
    private Transform PcT;
    private void Start()
    {
        PcT = GameManager.Instance.GetPlayerController().transform;
    }

    private void FixedUpdate()
    {
        transform.position = PcT.position + Vector3.back * distance;
    }
}
