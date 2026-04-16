using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    public float linearSpeed = 3.0f;
    public float angularSpeed = 200.0f;
    Cannon[] cannons;
    //Testing movement...
    Vector2 movementDir = Vector2.zero;

    MusicPlayer musicPlayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cannons = GetComponentsInChildren<Cannon>();
        musicPlayer = GetComponent<MusicPlayer>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !cannons[0].reloading) //left click
        {
            cannons[0].Shoot();
            musicPlayer.Shoot();
        }

        if (Input.GetMouseButtonDown(1) && !cannons[0].reloading) //right click
        {
            cannons[1].Shoot();
            musicPlayer.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerData.currentHealth -= 15;
            Debug.Log("Player new health: " + PlayerData.currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerData.currentHealth += 15;
            Debug.Log("Player new health: " + PlayerData.currentHealth);
        }
    }
    void FixedUpdate()
    {
        movementDir = Vector2.zero;
        float angularTarget = 0f;

        if (Input.GetKey(KeyCode.S))
        {
            movementDir += (Vector2)transform.up;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movementDir -= (Vector2)transform.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            angularTarget = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angularTarget = -1;
        }

        //Debug.DrawLine(transform.position, transform.position + transform.up); transform up seems to point in correct direction
        //Debug.Log(movementDir.x + "," + movementDir.y);
        rb.linearVelocity = PlayerData.speedMult * linearSpeed * movementDir.normalized;
        rb.angularVelocity = angularSpeed * angularTarget;
    }
}
