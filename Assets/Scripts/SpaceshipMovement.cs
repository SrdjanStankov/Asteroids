﻿using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode backwards;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    private SpaceshipAttribute attribute;

    public KeyCode Backwards { get => backwards; set => backwards = value; }
    public KeyCode Forward { get => forward; set => forward = value; }
    public KeyCode Left { get => left; set => left = value; }
    public KeyCode Right { get => right; set => right = value; }

    private void Start()
    {
        attribute = GetComponent<SpaceshipAttribute>();
    }

    private void Update()
    {
        int translation = Input.GetKey(Forward) ? 1 : Input.GetKey(Backwards) ? -1 : 0;
        int rotation = Input.GetKey(Left) ? 1 : Input.GetKey(Right) ? -1 : 0;

        transform.Rotate(0, 0, rotation * Time.deltaTime * attribute.RotationSpeed);
        transform.Translate(0, translation * Time.deltaTime * attribute.FlightSpeed, 0);
    }
}
