using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    CELL1,
    CELL2, 
    CELL3, 
    //CELL4
}

public class MovingInfection : MonoBehaviour
{
    // [SerializeField]
    public CellType cellType;
    private WaypointPath path;
    public PathContainer pathContainer;

    [SerializeField]
    private float speed;

    private int targetWaypointIndex;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float elapsedTime;

    void Awake()
    {
        pathContainer = FindObjectOfType<PathContainer>();
        path = pathContainer.GetWaypointPath(cellType);
        if (path != null)
        {
            Debug.Log($"Enemy of type {cellType} selected path: {path.name}");
        }
        else
        {
            Debug.LogWarning($"Enemy of type {cellType} could not find a valid path.");
        }
        TargetNextWaypoint();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);
        // transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1.0f)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = path.GetWayPoint(targetWaypointIndex);
        targetWaypointIndex = path.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = path.GetWayPoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);

        timeToWaypoint = distanceToWaypoint / speed;
    }
}
