using UnityEngine;

public class ArrowChecker : MonoBehaviour
{
    public Transform obj;
    public Transform agent;

    private void Update()
    {
        agent = GameObject.Find("BotAgent(Clone)").GetComponent<Transform>();

    }
    public void UpdateSpot(float[] vec)
    {
        obj.position = agent.position;
        obj.eulerAngles = new Vector3(0, 0, Mathf.Atan2(vec[1], vec[0]) * 180 / Mathf.PI + 90);
        obj.localScale = new Vector3(1.878842f, 3.103779f * Mathf.Sqrt(vec[1] * vec[1] + vec[0] * vec[0]), 1);
    }

}
