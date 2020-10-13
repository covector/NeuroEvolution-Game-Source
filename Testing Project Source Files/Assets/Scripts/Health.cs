using System.Collections;
using UnityEngine;
public class Health : MonoBehaviour
{
    public Transform obj;

    private int maxhealth;
    private float minx = -6.77f;
    private float maxx = -4.83f;
    public void Start()
    {
        maxhealth = FindObjectOfType<Manager2>().maxhealth;
    }
    public IEnumerator Damage(int currhealth)
    {
        for (int i = 0; i < 350; i++)
        {
            if (obj.position.x > minx + (maxx - minx) * currhealth / maxhealth)
            {
                obj.position -= new Vector3(0.01f, 0, 0);
            }
            else {
                break;
            }
            yield return null;
        }
    }
}
