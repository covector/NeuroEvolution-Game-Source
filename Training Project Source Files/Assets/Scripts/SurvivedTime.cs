using UnityEngine;
using UnityEngine.UI;
public class SurvivedTime : MonoBehaviour
{
    public Text score;
    public void ChangeText(string timer)
    {
        score.text = "You survived " + timer;
    }
}
