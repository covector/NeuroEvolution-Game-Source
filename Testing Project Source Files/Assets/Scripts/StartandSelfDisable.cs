using UnityEngine;
public class StartandSelfDisable : MonoBehaviour
{
    private void StartGame()
    {
        FindObjectOfType<Manager2>().StartGame();
        gameObject.SetActive(false);
    }
}
