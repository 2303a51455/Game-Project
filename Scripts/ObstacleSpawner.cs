using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public PlayerController player;
    public int numberOfObstacles = 2;

    void Start()
    {
        if (player == null) player = FindObjectOfType<PlayerController>();

        if (player != null && player.right != null && player.left != null && player.up != null && player.down != null)
        {
            float minX = player.left.position.x + 2f;
            float maxX = player.right.position.x - 2f;
            float minZ = player.down.position.z + 2f;
            float maxZ = player.up.position.z - 2f;

            int spawned = 0;
            int attempts = 0;
            
            // Try to spawn precisely
            while (spawned < numberOfObstacles && attempts < 1000)
            {
                attempts++;
                Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));

                if (Vector3.Distance(randomPos, player.transform.position) < 6f) continue;

                // Smart raycast: drop a ray from 10 units up down to the ground.
                // If it hits something higher than Y=0.5, it's a wall.
                if (Physics.Raycast(new Vector3(randomPos.x, 10f, randomPos.z), Vector3.down, out RaycastHit hit, 20f))
                {
                    if (hit.point.y > 0.5f || hit.collider.name.Contains("Wall"))
                    {
                        continue; // skip this spot since it's on a wall
                    }
                }

                SpawnObstacle(randomPos);
                spawned++;
            }
        }
    }

    void SpawnObstacle(Vector3 pos)
    {
        GameObject obs = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obs.transform.position = pos;
        obs.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        // Danger color (bright orange/red works well in dark forest)
        Renderer r = obs.GetComponent<Renderer>();
        r.material.color = new Color(1f, 0.4f, 0f);

        obs.AddComponent<ObstacleBehavior>();
    }
}
