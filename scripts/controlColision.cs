using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class controlColision : MonoBehaviour
{
private int monedas=0;
public TextMeshProUGUI contadorliciernaga;
public GameObject Meta;
public Transform spawnPoint; //mueve el gameobj spawnpoint
private int numeroCoins;
public GameObject background;
private TextMeshProUGUI scoreText;

void Start(){
    numeroCoins = GameObject.FindGameObjectsWithTag("coin").Length;
    contadorliciernaga.text= ": "+monedas.ToString()+"/"+numeroCoins;
    
}
    private void OnTriggerEnter(Collider collision){
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "coin")
        {   monedas +=1;
            // Aquí puedes poner el código que deseas ejecutar cuando el jugador colisiona con un enemigo
            Destroy(collision.gameObject);
            Debug.Log(monedas);
            contadorliciernaga.text= ": "+monedas.ToString()+"/"+numeroCoins;
            AudioManager.instance.PlaySoundEffect(0);
        }
        if (monedas==numeroCoins){
            Meta.GetComponent<MeshRenderer>().material.color = Color.green;
            if (collision.gameObject.tag == "meta")
        {   Debug.Log("llegaste a la meta");
            background.SetActive(true);
            GameObject scoreObject = GameObject.FindGameObjectWithTag("score");
            scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
            scoreText.text= "Score : "+monedas.ToString()+"/"+numeroCoins;
            
        }
        }
        
         
    }

 



    // private void OnCollisionEnter(Collision collision)
    // {   

    //     // Verificar si la colisión es con un objeto específico (por ejemplo, un enemigo)
    //     Debug.Log(collision.gameObject.name);
    //     Debug.Log("sichoca weeeeeeeee");
    //     if (collision.gameObject.tag == "coin")
    //     {
    //         // Aquí puedes poner el código que deseas ejecutar cuando el jugador colisiona con un enemigo
    //         Debug.Log("¡Colisión con enemigo!");
    //     }
    // }
}
