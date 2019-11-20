using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static int goldValue = 0;
    Text gold;

    void Start()
    {
        gold = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        gold.text = "Gold: " + UnitManager.GetMoney();
    }
}
