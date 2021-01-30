﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    #region singleton
    public static CoinSpawner instance { private set; get; }

    private void Awake()
    {
        // First destroy any existing instance of it
        if (instance != null)
        {
            Destroy(instance);
        }

        // Then reassign a proper one
        instance = this;
    }
    #endregion

    [Header("Configuration")]
    public GameObject Coin;
    public int CoinsPerSecond = 10;

    private int _coinsToSpawn = 0;
    private bool _isSpawning = false;

    private void OnDrawGizmos()
    {
        // Draw a semitransparent cube at the transforms position
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 1f, 0.5f));
    }

    private void Update()
    {
        if(_coinsToSpawn > 0 && _isSpawning == false)
        {
            _isSpawning = true;
            InvokeRepeating("SpawnCoins", 0f, 1f);
        } else if(_coinsToSpawn <= 0 && _isSpawning == true) {
            CancelInvoke();
            _isSpawning = false;
            _coinsToSpawn = 0;
        }
    }

    public void AddCoinsToSpawn(int amount)
    {
        _coinsToSpawn += amount;
    }

    private void SpawnCoins()
    {
        int coinsSpawned = 0;
        for(int i = 0; i < CoinsPerSecond; i++)
        {
            Instantiate(Coin, transform);
            coinsSpawned++;
        }
        _coinsToSpawn = Mathf.Max(0, _coinsToSpawn - coinsSpawned);
    }
}
