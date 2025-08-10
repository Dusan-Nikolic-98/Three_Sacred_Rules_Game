using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{

    public Camera mainCamera;
    //public Canvas canvas;
    public float panDuration = 3f;
    public float skipDelay = 0.5f;
    public float imageDisplayTime = 7f;
    public GameObject cutscenePanel; //parent za sve UI vezane za cutsc

    //cutsc kontent
    public Texture2D[] images;

    public Sprite[] captionImages;
    public RawImage displayImage;
    public Image captionElement; // slika za captions

    //private bool isPlaying = true;
    private bool allowSkip = false;

    private Vector3 initialCameraPosition;

    public void BeginCutscene()
    {

        if (images == null || images.Length == 0) return;
        if (captionImages == null || captionImages.Length == 0) return;

        Time.timeScale = 0f;

        initialCameraPosition = Camera.main.transform.position;

        displayImage.texture = images[0];
        captionElement.sprite = captionImages[0];
        cutscenePanel.SetActive(true);

        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSecondsRealtime(skipDelay); //da makar trunku saceka xD
        allowSkip = true;

        for (int i = 0; i < images.Length; i++)
        {
            captionElement.sprite = captionImages[i];
            displayImage.texture = images[i];

            Camera.main.transform.position = initialCameraPosition;

            //reset kamere
            Vector3 startPos = initialCameraPosition; //mainCamera.transform.position
            Vector3 endPos = startPos + Vector3.right * GetPanDistance(i);

            yield return StartCoroutine(PanCamera(startPos, endPos, panDuration));

            if (i != 4)
                yield return new WaitForSecondsRealtime(imageDisplayTime);
            else
                yield return new WaitForSecondsRealtime(imageDisplayTime * 2);
        }
        EndCutscene();
    }

    private float GetPanDistance(int imageIndex)
    {
        if (images[imageIndex] == null) return 10f;

        float aspectRatio = (float)images[imageIndex].width / images[imageIndex].height;
        float screenAspect = (float)Screen.width / Screen.height;

        return (aspectRatio / screenAspect - 1f) * Camera.main.orthographicSize * 2f;
    }



    IEnumerator PanCamera(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (allowSkip && Input.anyKeyDown)
            {
                EndCutscene();
                yield break;
            }
            //mainCamera.transform.position = ..
            Camera.main.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Camera.main.transform.position = endPos;
    }

    void EndCutscene()
    {
        StopAllCoroutines();
        //isPlaying = false;
        allowSkip = false;
        cutscenePanel.SetActive(false);

        Camera.main.transform.position = initialCameraPosition;

        Time.timeScale = 1f;

        OnCutsceneEnd?.Invoke();
    }

    void Update()
    {
        if (allowSkip && Input.anyKeyDown)
        {
            EndCutscene();
        }
    }

    //eventovi za kad se zavrsi
    public delegate void CutsceneEvent();
    public event CutsceneEvent OnCutsceneEnd;
}

