using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    Rigidbody rb;

    float speed = 5.0f;
    float turnSpeed = 30.0f;

    public int powerSupply = 100;
    int maxPowerSupply = 100;
    bool isCharging = false;

    float timeSinceLastPowerDecrease = 0f;
    float powerDecreaseInterval = 60f;


    public GameObject[] lights; 
    bool lightsOn = false;

    public GameObject[] wheels;
    float[] initialWheelsRotationY;

    public ParticleSystem smoke;


    //quest part
    private GameObject currentDeliveryBuilding;
    private bool isQuestActive = false;
    private string requiredSupply;
    private string collectedSupply = "";

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject light in lights)
        {
           light.SetActive(lightsOn);
        }


        initialWheelsRotationY = new float[wheels.Length];
        for(int i = 0; i < 2; i++)
        {
            initialWheelsRotationY[i] = wheels[i].transform.eulerAngles.y;
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(powerSupply >0)
        {
            Moving();
        }
        



        if (horizontalInput != 0)
        {
            for (int i = 0; i < 2; i++)
            {
                wheels[i].transform.Rotate(Vector3.left * Time.deltaTime * 10 * horizontalInput);
            }
        }
        else if (horizontalInput == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 180, -90);
                wheels[i].transform.localRotation = Quaternion.Lerp(wheels[i].transform.localRotation, targetRotation, Time.deltaTime * 10);
            }
            
        }
        


        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(!lightsOn);
                

            }
            lightsOn = !lightsOn;
        }

        
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.up * Time.deltaTime * 1000 * verticalInput);
        }

        
        if (verticalInput > 0)
        {
            smoke.Play();
        }
        else
        {
            smoke.Stop();
        }

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            timeSinceLastPowerDecrease += Time.deltaTime;

            if (timeSinceLastPowerDecrease >= powerDecreaseInterval)
            {
                timeSinceLastPowerDecrease = 0f;
                DecreasePower();
            }
        }


    }

    private void Moving()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime);
    }

    private void DecreasePower()
    {
        // Decrease power by 2
        powerSupply -= 2;

        // Ensure powerSupply doesn't go below 0
        powerSupply = Mathf.Max(powerSupply, 0);
    }

    public void AssignQuest(GameObject deliveryBuilding, string requiredSupply)
    {
        currentDeliveryBuilding = deliveryBuilding;
        this.requiredSupply = requiredSupply;
        isQuestActive = true;
        collectedSupply = "";


        
        
        
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("power"))
        {
            
            StartCoroutine(ChargePowerOverTime());
        }

        if (isQuestActive && other.gameObject.CompareTag("delivery") && other.gameObject == currentDeliveryBuilding)
        {
            // Check if the player has the required supply for the quest
            if (CheckSupply())
            {
                // Quest completed successfully
                Debug.Log("Quest Completed!");
                EndQuest();
            }
            else
            {
                // Quest failed, player delivered the wrong supply
                Debug.Log("Wrong Supply!");
                
            }
        }

        if (isQuestActive ) 
        {
            CheckSupplyPlaceInteraction(other);
        }


    }

    private void CheckSupplyPlaceInteraction(Collider other)
    {
        if (other.gameObject.CompareTag("pharmacy"))
        {
            collectedSupply = "pharmacy";
            
        }
        else if (other.gameObject.CompareTag("grocery"))
        {
            collectedSupply = "grocery";
            
        }
        else if (other.gameObject.CompareTag("supermarket"))
        {
            collectedSupply = "supermarket";
            
        }
        else if (other.gameObject.CompareTag("restaurant"))
        {
            collectedSupply = "restaurant";

        }
        else if (other.gameObject.CompareTag("petShop"))
        {
            collectedSupply = "petShop";

        }
        else if (other.gameObject.CompareTag("butcher"))
        {
            collectedSupply = "butcher";

        }
        else if (other.gameObject.CompareTag("technology"))
        {
            collectedSupply = "technology";

        }
        // Add more else-if blocks for other supply places as needed
    }


    private bool CheckSupply()
    {
        
        return collectedSupply == requiredSupply;
    }

    private void EndQuest()
    {
        // Reset quest-related variables
        currentDeliveryBuilding = null;
        requiredSupply = "";
        isQuestActive = false;
        collectedSupply = "";

        // Start a new quest
        FindObjectOfType<QuestManager>().StartNewQuest();
    }

    private IEnumerator ChargePowerOverTime()
    {
        isCharging = true;
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Add 10 units to powerSupply after waiting
        powerSupply += 10;

        // Limit powerSupply to the maximum value
        powerSupply = Mathf.Min(powerSupply, maxPowerSupply);
        isCharging = false;
    }

}
