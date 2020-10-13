using UnityEngine.UI;
using UnityEngine;

public class ParameterManager2 : MonoBehaviour
{
    public Text speedvalue;
    public Slider speedslider;

    public void UpdateSlider()
    {
        speedvalue.text = speedslider.value.ToString();
    }
}
