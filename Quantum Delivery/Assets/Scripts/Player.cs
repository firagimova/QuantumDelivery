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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime);



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

    }
}
