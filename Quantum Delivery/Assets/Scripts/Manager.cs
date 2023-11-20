using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject player;
    int powerSupply;

    public TextMeshProUGUI powerSupplyText;

    public GameObject mainCam;
    public GameObject mapCam;

    public Canvas canvas;

    
    

    // Update is called once per frame
    void Update()
    {
        powerSupply = player.GetComponent<Player>().powerSupply;
        powerSupplyText.text = powerSupply.ToString();
        

        
        if (powerSupply <= 0)
        {
            
            SceneManager.LoadScene(0);
        }

        
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapCam.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }


        }

        void PauseGame()
        {
            
            mainCam.SetActive(false);
            mapCam.SetActive(true);

            
            Time.timeScale = 0f;
            canvas.enabled = false;

        }


        void ResumeGame()
        {
            
            mapCam.SetActive(false);
            mainCam.SetActive(true);
            canvas.enabled = true;

            
            Time.timeScale = 1f;


        }

    }
}
