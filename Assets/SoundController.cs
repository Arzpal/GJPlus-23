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

    public void PlaySound(int listId, int id, AudioSource source = null)
	{
        Sounds = getSoundList(listId);
        if (source == null) source = audioSource;
        source.clip = Sounds[id].clip;
        source.volume = Sounds[id].volume;
        source.loop = Sounds[id].isLoop;
        source.Play();

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
