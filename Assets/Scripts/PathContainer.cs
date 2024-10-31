using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathContainer : MonoBehaviour
{
    public WaypointPath[] cell1Paths;
    public WaypointPath[] cell2Paths;
    public WaypointPath[] cell3Paths;
    public WaypointPath GetWaypointPath(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.CELL1:
                return cell1Paths[Random.Range(0, cell1Paths.Length)];
            case CellType.CELL2:
                return cell2Paths[Random.Range(0, cell2Paths.Length)];
            case CellType.CELL3:
                return cell3Paths[Random.Range(0, cell3Paths.Length)];
            default:
                return null;
        }
    }
}
