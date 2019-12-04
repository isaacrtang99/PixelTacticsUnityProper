using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    Text level;
    void Start()
    {
        level = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        level.text = "Level: " + UnitManager.level;
    
    }
}
