using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deLaster : MonoBehaviour
{
    public Text text;
    public Lasers las;

    // Update is called once per frame
    void Update()
    {
        string pt = las.spawntime.ToString();
        if (pt.Length > 6)
        {
            pt = pt.Substring(0, 6);
        }
        text.text = "Urgency = "+pt;
    }
}
