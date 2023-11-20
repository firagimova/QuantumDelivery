using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] deliveryBuildings; 
    private string[] supplyTypes = { "grocery", "supermarket", "restaurant", "petShop", "butcher", "restaurant", "supermarket", "pharmacy", "grocery", "technology" };
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI questText;

    private float questTimer = 480f; 
    private bool isQuestActive = false;
    private int currentQuestIndex = 0;


    private Dictionary<GameObject, Color> originalBuildingColors = new Dictionary<GameObject, Color>();

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

            
            int minutes = Mathf.FloorToInt(questTimer / 60);
            int seconds = Mathf.FloorToInt(questTimer % 60);

            
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (questTimer <= 0)
            {
                
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
            
            SceneManager.LoadScene(0);
        }
    }

    public void EndQuest()
    {
        Debug.Log("Ending quest...");
        RestoreBuildingColors();
        isQuestActive = false;
    }
    private void ChangeBuildingColor(GameObject building, Color color)
    {
        Renderer buildingRenderer = building.GetComponent<Renderer>();

        if (buildingRenderer != null)
        {
            if (!originalBuildingColors.ContainsKey(building))
            {
                originalBuildingColors[building] = buildingRenderer.material.color;
                Debug.Log("Saving original color for building: " + building.name);
            }

            buildingRenderer.material.color = color;
            Debug.Log("Changing color of building: " + building.name + " to " + color);
        }
        else
        {
            Debug.LogError("Building does not have a Renderer component!");
        }
    }

    private void RestoreBuildingColors()
    {
        

        Debug.Log("Restoring building colors...");
        foreach (var kvp in originalBuildingColors)
        {
            GameObject building = kvp.Key;
            Color originalColor = kvp.Value;

            Renderer buildingRenderer = building.GetComponent<Renderer>();

            if (buildingRenderer != null)
            {
                buildingRenderer.material.color = originalColor;
                Debug.Log("Restoring original color for building: " + building.name);
            }
            else
            {
                Debug.LogError("Building does not have a Renderer component!");
            }
        }

        
        originalBuildingColors.Clear();
    }
}
