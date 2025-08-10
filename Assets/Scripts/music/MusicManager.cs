using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip stage3Music;

    private AudioSource audioSource;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject); //za kad budem imao vise scena

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayStageMusic(1);
    }

    public void PlayStageMusic(int stage)
    {
        AudioClip clipToPlay = null;
        switch (stage) 
        {
            case 1:
                clipToPlay = stage1Music;
                break;
            case 2:
                clipToPlay = stage2Music;
                break;
            case 3:
                clipToPlay = stage3Music;
                break;
        }

        if (clipToPlay != null && audioSource.clip != clipToPlay) 
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }

    //za smooth prelaz
    public void PlayStageMusicSmooth(int stage)
    {
        StartCoroutine(FadeToStageMusic(stage));
    }

    private IEnumerator FadeToStageMusic(int stage)
    {
        float fadeTime = 1f;
        float startVolume = audioSource.volume;

        //fejd aut
        while(audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        PlayStageMusic(stage);

        while(audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

    }

}
