using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    Text health;
    void Start()
    {
        health = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "Player Health: " + UnitManager.playerHealth;
    }
}
