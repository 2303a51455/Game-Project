using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    private Vector3 startPos;
    private float offset;
    private float speed;
    private int movementType;
    private PlayerController player;

    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, 2f * Mathf.PI);
        speed = Random.Range(0.5f, 1.5f); // Reduced speed for easier gameplay
        movementType = Random.Range(0, 3);
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // Animate visually: 3 patterns
        if (movementType == 0) // Bobbing up and down
        {
            transform.position = startPos + new Vector3(0, Mathf.Abs(Mathf.Sin(Time.time * speed + offset)) * 1f, 0);
        }
        else if (movementType == 1) // Sweeping X axis
        {
            transform.position = startPos + new Vector3(Mathf.Sin(Time.time * speed + offset) * 1f, 0, 0); // Reduced sweep distance
        }
        else // Sweeping Z axis
        {
            transform.position = startPos + new Vector3(0, 0, Mathf.Sin(Time.time * speed + offset) * 1f); // Reduced sweep distance
        }

        // Add a chaotic spin
        transform.Rotate(Vector3.up * 360f * Time.deltaTime);
        transform.Rotate(Vector3.right * 180f * Time.deltaTime);

        // Check distance purely by math instead of physics, to guarantee it works consistently!
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            // Player size is likely 1 Unit. Trigger if extremely close.
            if (dist < 0.8f) // Reduced trigger distance for easier gameplay
            {
                if (ThemesAndSounds.Instance != null)
                {
                    ThemesAndSounds.Instance.PlayHit();
                }
                player.GameOver();
                gameObject.SetActive(false); // Disable to avoid duplicate triggers
            }
        }
    }
}
