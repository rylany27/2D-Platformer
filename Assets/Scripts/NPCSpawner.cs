using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public int numNPCs = 5;
    public float spawnWidth = 10f;
    public int minOrderInLayer = -7;
    public int maxOrderInLayer = -6;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < numNPCs; i++)
        {
            float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
            Vector3 spawnPosition = transform.position + new Vector3(randomX, 0f, 0f);

            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            npc.transform.SetParent(transform);

            SpriteRenderer spriteRenderer = npc.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = Random.Range(minOrderInLayer, maxOrderInLayer);
        }
    }
}
