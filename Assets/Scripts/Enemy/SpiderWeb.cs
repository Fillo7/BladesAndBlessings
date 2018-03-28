﻿using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    bool playerImmobilized = false;
    PlayerMovement movement;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movement = other.GetComponent<PlayerMovement>();
            movement.SetSpeed(0.0f);
            playerImmobilized = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movement.SetSpeed(0.0f);
        }
    }

    void OnDestroy()
    {
        if (playerImmobilized)
        {
            movement.ResetSpeed();
        }
    }
}
