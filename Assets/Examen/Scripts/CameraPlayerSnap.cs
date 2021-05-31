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

public class CameraPlayerController : MonoBehaviour
{
    [SerializeField] private float size = 8;
    [SerializeField] private float speed = 2.5f;
    private Transform trans;

    private Vector2 input;

    private void Awake()
    {
        trans = transform;
        GetComponent<Camera>().orthographicSize = size;
    }

    void Update()
    {
        Vector2 rawInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = rawInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        trans.position += (Vector3)(input * (Time.fixedDeltaTime * speed));
    }
}