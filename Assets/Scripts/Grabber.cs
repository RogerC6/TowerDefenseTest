using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedGameObject;
    private Vector3 originalWorldPos;
    public float minDistanceBetweenTowers = 1.0f;

    private LineRenderer minDistanceCircle;
    public Material material;
    public Color colorMinDistanceCircle;
    public Color colorCannotDropMinDistanceCircle;

    private void Start()
    {
        minDistanceCircle = gameObject.AddComponent<LineRenderer>();
        minDistanceCircle.startWidth = 0.5f;
        minDistanceCircle.endWidth = 0.5f;
        minDistanceCircle.loop = true;
        minDistanceCircle.positionCount = 50;
        minDistanceCircle.material = material;
        minDistanceCircle.startColor = colorMinDistanceCircle;
        minDistanceCircle.endColor = colorMinDistanceCircle;
        minDistanceCircle.useWorldSpace = true;
        minDistanceCircle.enabled = false;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (selectedGameObject == null)
            {
                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("grab"))
                    {
                        return;
                    }

                    selectedGameObject = hit.collider.gameObject;
                    originalWorldPos = selectedGameObject.transform.position;
                    Cursor.visible = false;

                    DrawCircleAroundObject(selectedGameObject.transform.position, minDistanceBetweenTowers);
                    minDistanceCircle.enabled = true;
                }
            }
            else
            {
                if (hit.collider.CompareTag("board"))
                {
                    Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedGameObject.transform.position).z);
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                    Vector3 dropPosition = new Vector3(worldPosition.x, 1.25f, worldPosition.z);

                    if (!IsPositionOccupied(dropPosition))
                    {
                        selectedGameObject.transform.position = dropPosition;
                        selectedGameObject = null;
                        Cursor.visible = true;
                        minDistanceCircle.enabled = false;
                    }
                    else
                    {
                        selectedGameObject.transform.position = originalWorldPos;
                    }

                }
                else
                {
                    selectedGameObject.transform.position = new Vector3(originalWorldPos.x, 0.0f, originalWorldPos.z);
                    selectedGameObject = null;
                    Cursor.visible = true;
                    minDistanceCircle.enabled = false;
                }
            }
        }

        if (selectedGameObject != null) 
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedGameObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedGameObject.transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);

            DrawCircleAroundObject(selectedGameObject.transform.position, minDistanceBetweenTowers);

            if (IsPositionOccupied(selectedGameObject.transform.position))
            {
                minDistanceCircle.material.color = colorCannotDropMinDistanceCircle;
            }
            else
            {
                minDistanceCircle.material.color = colorMinDistanceCircle;
            }

            if (Input.GetMouseButtonDown(1))
            {
                selectedGameObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedGameObject.transform.rotation.eulerAngles.x,
                    selectedGameObject.transform.rotation.eulerAngles.y + 90.0f,
                    selectedGameObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenTowers);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("grab") && collider.gameObject != selectedGameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void DrawCircleAroundObject(Vector3 position, float radius)
    {
        int points = minDistanceCircle.positionCount;
        float angleStep = 360.0f / points;
        for (int i = 0; i < points; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pointPosition = new Vector3(
                position.x + Mathf.Cos(angle) * radius,
                position.y,
                position.z + Mathf.Sin(angle) * radius
            );
            minDistanceCircle.SetPosition(i, pointPosition);
        }
    }
}
