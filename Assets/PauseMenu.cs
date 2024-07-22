using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    // Start is called before the first frame update
    public void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menu()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
	}

    public void onClose()
	{
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
	}
}
