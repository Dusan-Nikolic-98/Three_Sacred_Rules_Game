using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level3Trigger : MonoBehaviour
{

    public Text timerText;
    private bool triggered = false;
    private float surviveTime = 40f;
    public GameObject timer_obj;
    public GameObject tileToDestroy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (collision.CompareTag("Player"))
        {
            triggered = true;
            GhostSpawner.Instance.DestroyAllGhosts();
            GhostSpawner.Instance.StartLevel3SpawnSequence();


            if(timerText != null)
            {
                StartCoroutine(TimerFun());
            }
        }
    }

    private IEnumerator TimerFun()
    {
        float timeLeft = surviveTime;
        if(timer_obj != null)
        {
            timer_obj.SetActive(true);
        }
        
        while(timeLeft > 0)
        {
            if(timerText != null)
            {
                timerText.text = "¡SURVIVE! " + Mathf.Ceil(timeLeft) + "s";

                yield return null;
                timeLeft -= Time.deltaTime;
            }
        }

        // sta kad prodje vreme
        GhostSpawner.Instance.DestroyAllGhosts();
        if (timer_obj != null)
        {
            timer_obj.SetActive(false);
        }
        Destroy(tileToDestroy);


    }

    void Start()
    {
        
    }



    void Update()
    {
        
    }
}
