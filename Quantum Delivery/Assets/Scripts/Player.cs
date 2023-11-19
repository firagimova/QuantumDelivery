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
        
        Moving();



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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("power"))
        {
            //powerSupply += 10;
            

            // Start the coroutine to add power over time
            StartCoroutine(ChargePowerOverTime());
        }
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
