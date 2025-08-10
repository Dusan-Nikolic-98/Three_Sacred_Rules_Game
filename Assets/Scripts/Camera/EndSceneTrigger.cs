using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndSceneTrigger : MonoBehaviour
{

    //player deo
    public GameObject player;
    public MonoBehaviour[] playerControlScripts;
    public GameObject ghostPrefab;

    //bad guy
    public GameObject badGuyPrefab;
    public Transform badGuySpawnPoint;
    public Transform badGuyTargetPoint;
    public float badGuyMoveSpeed = 2f;

    //dijalog
    public Image[] dialogBubbles;
    public float dialogDisplayTime = 6.0f;

    //fejd
    public Image fadeImage;
    public float fadeSpeed = 0.5f;

    public Image replayPromptImage;

    private bool triggered = false;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerControlScripts = new MonoBehaviour[2];
        playerControlScripts[0] = player.GetComponent<PlayerMovement>();
        playerControlScripts[1] = player.GetComponent<PlayerAbillities>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PlayEndScene());
        }
    }

    private IEnumerator PlayEndScene()
    {
        foreach (var script in playerControlScripts) 
        {
            script.enabled = false;
        }

        yield return new WaitForSeconds(1f);
        //hodanje
        GameObject badGuy = Instantiate(badGuyPrefab, badGuySpawnPoint.position, Quaternion.identity);

        SpriteRenderer sr = badGuy.GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            sr.flipX = !sr.flipX;
        }

        while(Vector3.Distance(badGuy.transform.position, badGuyTargetPoint.position) > 0.05f)
        {
            badGuy.transform.position = Vector3.MoveTowards(
                    badGuy.transform.position,
                    badGuyTargetPoint.position,
                    badGuyMoveSpeed * Time.deltaTime
                );
            yield return null;
        }

        //dijalog deo
        
        for(int i = 0; i < dialogBubbles.Length; i++)
        {
            dialogBubbles[i].gameObject.SetActive(true);
            float waitTime = dialogDisplayTime;
            if (i == 1)
                waitTime += 3.0f;
            if (i == 2)
                waitTime += 1.0f;
            yield return new WaitForSeconds(waitTime);
            dialogBubbles[i].gameObject.SetActive(false);
        }


        //player papa
        Vector3 playerPos = player.transform.position;
        Destroy(player);
        GameObject ghost = Instantiate(ghostPrefab, playerPos, Quaternion.identity);

        float fadeAlpha = 0f;
        
        while(fadeAlpha < 1f)
        {
            ghost.transform.position += Vector3.up * 1f * Time.deltaTime;
            fadeAlpha += fadeSpeed * Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, fadeAlpha);
            yield return null;

        }
        yield return new WaitForSeconds(3.0f);
        //sta posle fade to blacka
        replayPromptImage.gameObject.SetActive(true);

        bool choiceMade = false;
        while (!choiceMade) 
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                choiceMade = true;
            }
            if (Input.GetKeyDown(KeyCode.N)) 
            { 


                Application.Quit();
            }
            yield return null;
        }


    }


}
