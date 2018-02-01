﻿using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    private float nextArrowTimer;
    private float arrowCooldown = 5.0f;
    private EnemyHealth health;

    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        nextArrowTimer = Random.Range(2.0f, 5.0f);
    }

    void Update()
    {
        if (health.IsDead())
        {
            return;
        }

        nextArrowTimer += Time.deltaTime;

        if (nextArrowTimer > arrowCooldown)
        {
            spawnArrow();
            nextArrowTimer = 0.0f;
        }
    }

    private void spawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward + transform.up,
            Quaternion.LookRotation(transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDirection(transform.forward);
    }
}
