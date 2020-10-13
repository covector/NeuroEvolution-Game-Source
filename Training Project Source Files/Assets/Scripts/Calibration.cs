using UnityEngine.UI;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public Text maxspeedtext;
    public Slider maxspeedslider;

    public Text acceltext;
    public Slider accelslider;

    public Text resistancetext;
    public Slider resistanceslider;

    public void UpdateMaxSpeed()
    {
        float currval = maxspeedslider.value;
        maxspeedtext.text = Truncate(currval.ToString());
        PlayerPrefs.SetFloat("MaxSpeed", currval);
        FindObjectOfType<PlayerControl>().max_v = new Vector2(currval, currval);
    }
    public void UpdateAccel()
    {
        float currval = accelslider.value;
        acceltext.text = Truncate(currval.ToString());
        PlayerPrefs.SetFloat("Accel", currval);
        FindObjectOfType<PlayerControl>().acceleration = currval;
    }
    public void UpdateResistance()
    {
        float currval = resistanceslider.value;
        resistancetext.text = Truncate(currval.ToString());
        PlayerPrefs.SetFloat("Resistance", currval);
        FindObjectOfType<PlayerControl>().dampspeed = currval;
    }
    private void Start()
    {
        maxspeedslider.value = PlayerPrefs.GetFloat("MaxSpeed", 0.2f);
        accelslider.value = PlayerPrefs.GetFloat("Accel", 0.01f);
        resistanceslider.value = PlayerPrefs.GetFloat("Resistance", 4f);
    }
    private string Truncate(string textstring)
    {
        if (textstring.Length > 5)
        {
            return textstring.Substring(0, 5);
        }
        else
        {
            return textstring;
        }
    }
    public void ActivateLasers(bool lol)
    {
        FindObjectOfType<Manager2>().over = !lol;
    }
}
