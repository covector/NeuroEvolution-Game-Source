using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform obj;
    public void VectorVisual(float y, float x)
    {
        obj.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(y, x) * 180 / Mathf.PI + 270) % 360);
        obj.localScale = new Vector3(5, Mathf.Pow(Mathf.Pow(y, 2) + Mathf.Pow(x, 2), 0.5f), 1);
    }
}
