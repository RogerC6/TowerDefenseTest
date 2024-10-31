using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform GetWayPoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;

        if (nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }
        return nextWaypointIndex;
    }

    private void OnDrawGizmos()
    {
        if (IsWayPointSelected())
        {
            DrawWaypointGizmos();
        }
    }

    public void DrawWaypointGizmos()
    {
        for (int waypointIndex = 0; waypointIndex < transform.childCount; waypointIndex++)
        {
            var waypoint = GetWayPoint(waypointIndex);

            Gizmos.color = new Color(102, 0, 204);
            Gizmos.DrawSphere(waypoint.position, 1.0f);

            int nextWaypointIndex = GetNextWaypointIndex(waypointIndex);
            var nextWaypoint = GetWayPoint(nextWaypointIndex);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(waypoint.position, nextWaypoint.position);
        }
    }

    private bool IsWayPointSelected()
    {
        if (Selection.transforms.Contains(transform))
        {
            return true;
        }

        foreach (Transform child in transform)
        {
            if (Selection.transforms.Contains(child))
            {
                return true;
            }
        }

        return false;
    }
}
