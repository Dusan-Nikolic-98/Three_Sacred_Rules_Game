using UnityEngine;

public class GhostTargetManager : MonoBehaviour
{

    public static GhostTargetManager Instance;
    public Transform currTarget;
    private bool ghostsAfraid = false;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            currTarget = GameObject.FindWithTag("Player").transform;
        }
        else Destroy(gameObject);
    }
    public void SetTarget(Transform target) 
    {
        currTarget = target;
    }

    public Transform GetTarget() 
    { 
        return currTarget;
    }

    public void SetGhostsAfraid(bool afraid)
    {
        ghostsAfraid = afraid;
    }
    public bool AreGhostsAfraid()
    {
        return ghostsAfraid;
    }

}
