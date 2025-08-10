using UnityEngine;

public class SignTrigger : MonoBehaviour
{

    public GameObject instructionUI;
    public GhostPlateTrigger plateTrigger;
    public float displayTime = 4f;

    private bool hasTriggered = false;

    private void Start()
    {
        if(instructionUI != null)
        {
            instructionUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            instructionUI.SetActive(true);
            plateTrigger.StartGlow();
            Invoke(nameof(HideInstructions), displayTime); //valjda hide nakond delay
        }
    }

    private void HideInstructions()
    {
        instructionUI.SetActive(false);

        if (GhostSpawner.Instance != null) 
        {
            GhostSpawner.Instance.SpawnGhostsFixed();
        }
    }

}
