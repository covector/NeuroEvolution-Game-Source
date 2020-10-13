using System.Collections;
using UnityEngine;
public class Health : MonoBehaviour
{
    public Transform obj;

    private int maxhealth;
    private float objy;
    private float minx = -6.77f;
    private float maxx = -4.83f;
    public void Start()
    {
        maxhealth = FindObjectOfType<PlayerControl>().maxhealth;
        objy = obj.position.y;
    }
    public IEnumerator Damage(int magnitude)
    {
        float travelling_x = (maxx - minx) * magnitude / maxhealth;
        for (int i = 0; i < 50; i++)
        {
            obj.position -= new Vector3(travelling_x * (1 - 1f / Mathf.Pow(5, i / 5f)) / 46.3665479413f, 0, 0);
            yield return null;
        }
    }
}
