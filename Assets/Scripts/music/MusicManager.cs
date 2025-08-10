using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;

    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip stage3Music;

    //za sound efekte
    public AudioClip ghostSpawnSFX;
    private AudioSource sfxSource; //za s.e.

    private AudioSource audioSource;

    private void Awake()
    {

        // da bude singlt
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject); //za kad budem imao vise scena
        //muzika
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        //sfx
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
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


    public void PlaySFX(AudioClip clip)
    {
        if(clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

}
