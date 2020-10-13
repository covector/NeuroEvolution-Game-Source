using UnityEngine;

public class InputManager : MonoBehaviour
{
    public int input = 17;
    public int hidden = 5;
    public int output = 4;
    public float RayLength = 3;
    public float theta = 30;
    public float offset = 0.2f;
    public Manager2 man;
    private void Start()
    {
        man = gameObject.GetComponent<Manager2>();
    }
    public float[] GetInput(Transform obj, Vector2 velocity)
    {
        float[] RayArray = new float[17];
        for (int i = 0; i < 12; i++)
        {
            Vector2 direction = new Vector2(Mathf.Sin(i * theta), Mathf.Cos(i * theta));
            RaycastHit2D hit = Physics2D.Raycast((Vector2)obj.position + direction * offset, direction, RayLength, 1 << 9);
            Debug.DrawRay((Vector2)obj.position + direction * offset, direction * RayLength, Color.green);
            if (hit)
            {
                float time = 1;
                Lasers collide = hit.collider.GetComponent<Lasers>();
                if (collide != null)
                {
                    time = collide.spawntime;
                }
                RayArray[i] = time * InvertMap(hit.distance, RayLength);
            }
            else
            {
                RayArray[i] = 0;
            }
        }
        RaycastHit2D center = Physics2D.Raycast(obj.position, Vector2.zero, RayLength, 1 << 9);
        if (center) { RayArray[12] = 1; } else { RayArray[12] = 0; }
        RayArray[13] = Tanh(obj.position.x, 4f);
        RayArray[14] = Tanh(obj.position.y, 9f / 4f);
        RayArray[15] = Tanh(velocity.x, man.max_v.x / 2f);
        RayArray[16] = Tanh(velocity.y, man.max_v.y / 2f);
        return RayArray;
    }
    public float InvertMap(float dis, float maxval)
    {
        return Mathf.Pow(0.85f, 10f * dis / maxval);
    }
    public float Tanh(float number, float maxval)
    {
        float exp = Mathf.Exp(2 * number / maxval);
        float invexp = Mathf.Exp(-2 * number / maxval);
        if (((exp - invexp) / (exp + invexp)) > 1 | ((exp - invexp) / (exp + invexp)) < -1) { Debug.Log("fuck"); }
        return (exp - invexp) / (exp + invexp);
    }
}
