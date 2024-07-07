using UnityEngine;

public class FireplaceController : MonoBehaviour
{
    public SpriteRenderer fireplaceSpriteRenderer;
    public Sprite[] fireplaceSprites;
    public ParticleSystem fireParticles;
    public float activationRange = 2f;

    private const int UNLIT = 0;
    private const int LIT = 1;
    private bool isLit = false;
    private Transform player;

    public float faithIncreaseAmount = 10f;
    private BoxCollider2D fireplaceCollider;
    // Start is called before the first frame update
    private void Start()
    {
        fireplaceSpriteRenderer.sprite = fireplaceSprites[UNLIT];
        fireParticles.Stop();
        player = GameObject.FindGameObjectWithTag("Player").transform;  
        fireplaceCollider = GetComponent<BoxCollider2D>();
        fireplaceCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLit && other.CompareTag("NPC"))
        {
            NPCController npc = other.GetComponent<NPCController>();
            if (npc != null && npc.isDragging)
            {
                npc.IncreaseFaith(faithIncreaseAmount);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isLit && Input.GetKeyDown(KeyCode.F) && IsPlayerInRange())
        {
            LightFire();
        }
    }

    bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= activationRange;
    }

    void LightFire()
    {
        isLit = true;
        fireplaceSpriteRenderer.sprite = fireplaceSprites[LIT];
        fireParticles.Play();
    }
}
