using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text moneyText;
    void Start()
    {
        moneyText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        string moneyString = "Money: ";
        //moneyString += money.ToString();
        moneyString += " (Use less units per round to get more money)";
        moneyText.text = moneyString;
    }
}
