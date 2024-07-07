using UnityEngine;
using System.Linq;

public class PlayerDragController : MonoBehaviour
{
    private NPCController currentlyDraggedNPC;
    private Vector3 offset;
    private Camera mainCamera;
    public float dragRadius = 0.5f; // Adjust this value to change the "click area" around NPCs

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }

        if (currentlyDraggedNPC != null)
        {
            DragNPC();
        }
    }

    void TryStartDrag()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        NPCController closestNPC = FindClosestNPC(mousePosition);

        if (closestNPC != null)
        {
            currentlyDraggedNPC = closestNPC;
            offset = currentlyDraggedNPC.transform.position - (Vector3)mousePosition;
            offset.z = 0;
            currentlyDraggedNPC.OnDragStart();
        }
    }

    NPCController FindClosestNPC(Vector2 position)
    {
        return FindObjectsOfType<NPCController>()
            .Where(npc => Vector2.Distance(npc.transform.position, position) <= dragRadius)
            .OrderBy(npc => Vector2.Distance(npc.transform.position, position))
            .FirstOrDefault();
    }

    void EndDrag()
    {
        if (currentlyDraggedNPC != null)
        {
            currentlyDraggedNPC.OnDragEnd();
            currentlyDraggedNPC = null;
        }
    }

    void DragNPC()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPosition.z = currentlyDraggedNPC.transform.position.z;
        newPosition.y = Mathf.Max(newPosition.y, currentlyDraggedNPC.groundLevel);
        currentlyDraggedNPC.transform.position = newPosition;
    }
}