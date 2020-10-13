using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class delLaster : MonoBehaviour
{
    public Lasers laser;
    public Text text;
    void Update()
    {
        string textP = laser.spawntime.ToString();
        if (textP.Length > 6)
        {
            textP = textP.Substring(0, 6);
        }
        text.text = "Internal Timer = "+textP;
    }
}
