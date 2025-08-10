using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GhostPlateTrigger : MonoBehaviour
{

    public GameObject blockerTile; //tile koji treba da pukne
    private HashSet<GameObject> ghostsOnPlate = new HashSet<GameObject>();
    public int requiredGhostCount = 1;
    private bool triggered = false;

    // za svetlucanje
    public GameObject tileToGlow;
    private SpriteRenderer glowRenderer;
    private Coroutine glowCoroutine;

    private void Start()
    {
        if (tileToGlow != null) 
        { 
            glowRenderer = tileToGlow.GetComponent<SpriteRenderer>();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Ghost")) 
        {
            ghostsOnPlate.Add(other.gameObject);
            CheckGhostCount();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            ghostsOnPlate.Remove(other.gameObject);
        }
    }

    private void CheckGhostCount()
    {
        if(ghostsOnPlate.Count >= requiredGhostCount)
        {

            triggered = true;
            StopGlow();
            if(blockerTile != null)
            {
                Destroy(blockerTile);
            }
        }
    }

    public void StartGlow()
    {
        if(glowRenderer != null && glowCoroutine == null)
        {
            glowCoroutine = StartCoroutine(GlowEffect());
        }
    }
    private void StopGlow()
    {
        if(glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
            if(glowRenderer != null)
            {
                glowRenderer.color = Color.white;
            }
        }
    }


    private IEnumerator GlowEffect()
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime * 2f;
            float alpha = (Mathf.Sin(t) + 1f) / 2f * 0.5f + 0.5f;
            if (glowRenderer != null)
            {
                glowRenderer.color = new Color(1f, 1f, 0f, alpha);
            }
            yield return null;
        }
    }

}
