using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Cargando la escena siguiente a la actual
   public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   //Saliendo del juego
    public void QuitGame ()
    {
        Application.Quit();
    }
}
