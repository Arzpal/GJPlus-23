using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _instance;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<Sound> BasicSounds;
    [SerializeField] private List<Sound> BreadSounds;
    [SerializeField] private List<Sound> MoneySounds;
    [SerializeField] private List<Sound> Music;
    private List<Sound> Sounds;
    [SerializeField] private AudioSource source;
    [SerializeField] public List<GameObject> MusicPlayers;

    // Start is called before the first frame update
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        StartCoroutine(playEngineSound());
    }

    IEnumerator playEngineSound()
    {
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        source.Stop();
        PlaySound(3, 0, MusicPlayers[0].GetComponent<AudioSource>());
        PlaySound(3, 1, MusicPlayers[1].GetComponent<AudioSource>(), 0);
        PlaySound(3, 2, MusicPlayers[2].GetComponent<AudioSource>(), 0);
        PlaySound(3, 3, MusicPlayers[3].GetComponent<AudioSource>(), 0);
    }

    public static SoundController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SceneChanger is null");
            }
            return _instance;
        }
    }

    public void PlaySound(int listId, int id, AudioSource source = null, int volume = -1)
	{
        Sounds = getSoundList(listId);
        if (source == null) source = audioSource;
        source.clip = Sounds[id].clip;
        source.volume = volume == -1 ? Sounds[id].volume : volume;
        source.loop = Sounds[id].isLoop;
        source.Play();

	}

    public IEnumerator MusicFade(int id)
	{
        if (MusicPlayers[id].GetComponent<AudioSource>().volume == 0.2f) yield return null;
        float timeElapsed = 0;

        while (timeElapsed < 1)
        {
            for(int i = 0; i < MusicPlayers.Count; i++)
			{
                if(id == i)
				{
                    MusicPlayers[i].GetComponent<AudioSource>().volume = Mathf.Lerp(0, 0.2f, timeElapsed / 1);
                }else if(MusicPlayers[i].GetComponent<AudioSource>().volume > 0.1)
				{
                    MusicPlayers[i].GetComponent<AudioSource>().volume = Mathf.Lerp(0.2f, 0, timeElapsed / 1);
                }
			}
            timeElapsed += Time.deltaTime;
            yield return null;
        }
	}

    private List<Sound> getSoundList(int id)
    {
        switch (id)
        {
            case 0:
                return BasicSounds;
            case 1:
                return BreadSounds;
            case 2:
                return MoneySounds;
            case 3:
                return Music;
        }
        return BasicSounds;
    }
}
