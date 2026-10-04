using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    public Vector3 Velocity { get; private set; }

    Rigidbody body;
    int currentWaypointIndex;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0)
        {
            Velocity = Vector3.zero;
            return;
        }

        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = 0;
        }

        Transform target = waypoints[currentWaypointIndex];

        if (target == null)
        {
            Velocity = Vector3.zero;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(
            body.position,
            target.position,
            speed * Time.fixedDeltaTime
        );

        Velocity = (nextPosition - body.position) / Time.fixedDeltaTime;
        body.MovePosition(nextPosition);

        if ((nextPosition - target.position).sqrMagnitude < 0.0001f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
