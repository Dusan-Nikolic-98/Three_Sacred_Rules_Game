using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private PlayerMovement playerMovement;
    public GameObject deathScrenUI;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ghost") || collision.collider.CompareTag("Void"))
        {
            ByeBye();
        }
    }

    private void ByeBye()
    {
        playerMovement.enabled = false;

        //da ne mogu ni duhici da ga mrdaju
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.angularVelocity = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (deathScrenUI != null) 
        {
            deathScrenUI.SetActive(true);
        }

        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {


        yield return new WaitForSecondsRealtime(4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

}
