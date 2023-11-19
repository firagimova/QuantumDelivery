using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] deliveryBuildings; // Array of delivery buildings
    private string[] supplyTypes = { "grocery", "supermarket", "restaurant", "petShop", "butcher", "restaurant", "supermarket", "pharmacy", "grocery", "technology" };
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI questText;

    private float questTimer = 600f; // 10 minutes in seconds
    private bool isQuestActive = false;
    private int currentQuestIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartNewQuest();
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuestActive)
        {
            questTimer -= Time.deltaTime;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(questTimer / 60);
            int seconds = Mathf.FloorToInt(questTimer % 60);

            // Update timer UI
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (questTimer <= 0)
            {
                // Quest failed, reset and start a new one
                EndQuest();
                StartNewQuest();
            }
        }
    }

    public void StartNewQuest()
    {
        if (currentQuestIndex < supplyTypes.Length)
        {
            int randomDeliveryIndex = Random.Range(0, deliveryBuildings.Length);
            GameObject deliveryBuilding = deliveryBuildings[randomDeliveryIndex];
            string requiredSupply = supplyTypes[currentQuestIndex];

            player.GetComponent<Player>().AssignQuest(deliveryBuilding, requiredSupply);

            ChangeBuildingColor(deliveryBuilding, Color.red);

            questText.text = "New Quest: Deliver " + requiredSupply + " to " + deliveryBuilding.name;

            questTimer = 600f;
            isQuestActive = true;
            currentQuestIndex++;
        }
        else
        {
            // All quests completed
            Debug.Log("All quests completed!");
        }
    }

    private void EndQuest()
    {
        foreach (GameObject deliveryBuilding in deliveryBuildings)
        {
            ChangeBuildingColor(deliveryBuilding, Color.white);
        }
        isQuestActive = false;
    }
    private void ChangeBuildingColor(GameObject building, Color color)
    {
        // Assuming the building has a Renderer component
        Renderer buildingRenderer = building.GetComponent<Renderer>();

        if (buildingRenderer != null)
        {
            buildingRenderer.material.color = color;
        }
        else
        {
            Debug.LogError("Building does not have a Renderer component!");
        }
    }

}
