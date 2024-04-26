using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame(){
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}
public void volverMenu(){
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}

//También se utilizará otra función para el botón Restart.

public void RestartGame(){
     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      // loads current scene
}
}
