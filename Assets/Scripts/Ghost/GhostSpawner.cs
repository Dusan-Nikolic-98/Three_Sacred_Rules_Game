using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{

    public static GhostSpawner Instance;
    //pracenje duhica da ih sve duhnem odjednom :)
    public List<GameObject> activeGhosts = new List<GameObject>();

    public GameObject ghostPrefab;
    public Transform player;
    public Camera mainCam;


    public void SpawnGhosts()
    {
        if (MusicManager.Instance != null) 
        {
            MusicManager.Instance.PlaySFX(MusicManager.Instance.ghostSpawnSFX);
        }

        for (int i = 0; i < 3; i++) {
            Vector3 spawnPos = GetRandomEdgePosition();
            GameObject ghost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
            //ghost.GetComponent<GhostFollow>().player = player;
            activeGhosts.Add(ghost);
        }
    }

    public void SpawnGhostsFixed()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlaySFX(MusicManager.Instance.ghostSpawnSFX);
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 camPos = mainCam.transform.position;
            float camHeight = 2f * mainCam.orthographicSize;
            float camWidth = camHeight*mainCam.aspect;

            float left = camPos.x - camWidth / 2f;
            float right = camPos.x + camWidth / 2f;
            float top = camPos.y + camHeight / 2f;
            float bottom = camPos.y - camHeight / 2f;

            Vector3 pos1 = new Vector3(right, top, 0f);
            Vector3 pos2 = new Vector3(right, bottom, 0f);
            Vector3 pos3 = new Vector3(camPos.x + 1f, top, 0f);
            Vector3 pos4 = new Vector3(camPos.x + 5f, top, 0f);
            Vector3 pos5 = new Vector3(right, top - 5f, 0f);

            SpawnGhostAt(pos1); SpawnGhostAt(pos2); SpawnGhostAt(pos3);
            SpawnGhostAt(pos4); SpawnGhostAt(pos5);
        }
    }

    private void SpawnGhostAt(Vector3 pos)
    {
        GameObject ghost = Instantiate(ghostPrefab, pos, Quaternion.identity);
        activeGhosts.Add(ghost);
    }

    Vector3 GetRandomEdgePosition() {

        Vector3 screenPos = Vector3.zero;
        int edge = Random.Range(0, 4); // random strana levo/gore/desno/dole
        float camHeight = 2f * mainCam.orthographicSize;
        float camWidth = camHeight * mainCam.aspect;

        float left = mainCam.transform.position.x - camWidth / 2f;
        float right = mainCam.transform.position.x + camWidth / 2f;
        float top = mainCam.transform.position.y + camHeight / 2f;
        float bottom = mainCam.transform.position.y - camHeight / 2f;

        switch (edge)
        {
            case 0:
                screenPos = new Vector3(left, Random.Range(bottom, top), 0f);
                break;
            case 1:
                screenPos = new Vector3(Random.Range(left, right), top, 0f);
                break;
            case 2:
                screenPos = new Vector3(right, Random.Range(bottom, top), 0f);
                break;
            case 3:
                screenPos = new Vector3(Random.Range(left, right), bottom, 0f);
                break;
        }
        return screenPos;

    }

    public void DestroyAllGhosts()
    {
        
        foreach (GameObject ghost in activeGhosts)
        {
            if (ghost != null) {
                Ghost ghostSc = ghost.GetComponent<Ghost>();
                if (ghostSc != null) 
                {
                    ghostSc.PlayDeathEffectAndDestroy();
                }
            }
        }
        activeGhosts.Clear();
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnGhosts), 2f, 120f); //bese 10 / 60
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //za lvl3

    public void StartLevel3SpawnSequence()
    {
        CancelInvoke(nameof(SpawnGhosts));
        StartCoroutine(Level3SpawnRoutine());
    }

    private IEnumerator Level3SpawnRoutine()
    {
        float duration = 40f;
        float interval = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            SpawnGhosts();
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

    }

    void Update()
    {
        
    }
}
