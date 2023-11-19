using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    public GameObject player;
    int powerSupply;

    public TextMeshProUGUI powerSupplyText;

    // Start is called before the first frame update
    void Start()
    {
        //get player's power supply
        powerSupply = player.GetComponent<Player>().powerSupply;
    }

    // Update is called once per frame
    void Update()
    {
        powerSupplyText.text = "POWER: " + powerSupply.ToString();
    }
}
