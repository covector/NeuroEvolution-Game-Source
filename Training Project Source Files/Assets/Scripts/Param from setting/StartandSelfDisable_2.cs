using UnityEngine;
public class StartandSelfDisable_2 : MonoBehaviour
{
    private void StartGame()
    {
        FindObjectOfType<Manager2_2>().StartGame();
        gameObject.SetActive(false);
    }
}
