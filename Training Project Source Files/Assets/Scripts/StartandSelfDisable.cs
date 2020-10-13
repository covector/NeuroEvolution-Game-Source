using UnityEngine;
public class StartandSelfDisable : MonoBehaviour
{
    public ParameterBoard parameters;
    private void Start()
    {
        GetComponent<Animator>().speed = parameters.SpeedUp;
    }
    private void StartGame()
    {
        FindObjectOfType<Manager2>().StartGame();
        gameObject.SetActive(false);
    }
}
