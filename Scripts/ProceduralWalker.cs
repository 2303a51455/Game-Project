using UnityEngine;

// Removed since the user provided a real 3D model. 
// This gracefully removes itself just in case it was saved to a prefab.
public class ProceduralWalker : MonoBehaviour
{
    void Awake()
    {
        Destroy(this);
    }
}
