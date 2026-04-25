using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float linearSpeed = 3.0f;
    public float angularSpeed = 200.0f;
    Cannon[] cannons;
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

        if (Input.GetMouseButtonDown(1) && !cannons[1].reloading) //right click
        {
            cannons[1].Shoot();
            musicPlayer.Shoot();
        }

    }

    // Movement
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

        rb.linearVelocity = PlayerData.speedMult * linearSpeed * movementDir.normalized;
        rb.angularVelocity = angularSpeed * angularTarget;
    }
}
