using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private SoundController Instance;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<Sound> Sounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySound(int id, AudioSource source)
	{
        if (source == null) source = audioSource;
        source.clip = Sounds[id].clip;
        source.volume = Sounds[id].volume;
        source.loop = Sounds[id].isLoop;
        source.Play();

	}
}
