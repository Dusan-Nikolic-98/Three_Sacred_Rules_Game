using System.Collections;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public GameObject levelTransitionUI;
    public Transform nextLevelStartPoint; //za igraca
    public Camera mainCamera;
    public Transform newCameraPosition;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TransitionToNextLevel(collision.gameObject));
        }
    }

    private IEnumerator TransitionToNextLevel(GameObject player)
    {
        Time.timeScale = 0f;
        if(levelTransitionUI != null)
        {
            levelTransitionUI.SetActive(true);
        }

        //duhovi duhnite
        if (GhostSpawner.Instance != null)
        {
            GhostSpawner.Instance.DestroyAllGhosts();
        }

        //pomeranje

        if (player != null && nextLevelStartPoint != null)
        {
            player.transform.position = nextLevelStartPoint.position;

        }
        if (mainCamera != null) 
        {
            mainCamera.transform.position = newCameraPosition.position;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        if (levelTransitionUI != null)
            levelTransitionUI.SetActive(false);

        Time.timeScale = 1f;

    }

}
