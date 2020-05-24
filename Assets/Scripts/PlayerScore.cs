using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public int empatia;
    public int comodidad;
    public int emisiones;

    public Text EmpatiaDisplay;
    public Text ComodidadDisplay;
    public Text EmisionesDisplay;
    public Text timerDisplay;

    public string loadScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(timerScene());
    }

    // Update is called once per frame
    void Update()
    {
        EmpatiaDisplay.text = "Empatia: " +empatia;
        ComodidadDisplay.text = "Comodidad: " + comodidad;
        EmisionesDisplay.text = "Emisiones: " + emisiones;
    }

    IEnumerator timerScene()
    {
        for(int i = 50; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerDisplay.text = "tienes " + i + " minutos para ir al trabajo"; 
        }
        SceneManager.LoadScene(loadScene);
    }
}
