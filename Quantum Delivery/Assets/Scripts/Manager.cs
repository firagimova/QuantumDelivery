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

    // Start is called before the first frame update
    void Start()
    {
        //get player's power supply
        powerSupply = player.GetComponent<Player>().powerSupply;
    }

    // Update is called once per frame
    void Update()
    {
        powerSupplyText.text = powerSupply.ToString();

        //if player runs out of power for 10 seconds, game over
        if (powerSupply <= 0)
        {
            //load game over scene
            SceneManager.LoadScene(0);
        }

        //if player presses M, switch to map view
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
            // Set the mapCam active
            mainCam.SetActive(false);
            mapCam.SetActive(true);

            // Pause the game
            Time.timeScale = 0f;
            canvas.enabled = false;

        }


        void ResumeGame()
        {
            // Set the mainCam active
            mapCam.SetActive(false);
            mainCam.SetActive(true);
            canvas.enabled = true;

            // Resume the game
            Time.timeScale = 1f;


        }

    }
}
