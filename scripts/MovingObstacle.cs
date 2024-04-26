using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class MovingObstacle : MonoBehaviour
{
    public float movementDistance = 2f; // Total distance the obstacle will move up and down
    public float movementSpeed = 1f; // Speed at which the obstacle moves

    private Vector3 initialPosition;
    private float direction = 1f; // 1 for moving up, -1 for moving down

    void Start()
    {
        // Store the initial position of the obstacle
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position of the obstacle
        float newY = initialPosition.y + Mathf.Sin(Time.time * movementSpeed) * movementDistance * direction;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}