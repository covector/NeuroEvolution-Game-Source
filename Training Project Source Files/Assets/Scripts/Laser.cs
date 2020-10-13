using UnityEngine;

public class Laser : MonoBehaviour
{
    #region variables
    public Transform obj;
    public float seed;
    public int key;
    public float decay;
    // translation
    private float linclock;
    private float maxv;
    private float sinhgrav;
    private float centergrav;
    // rotation
    private float angclock;
    private float maxangle;
    private float sinhang;
    #endregion

    private void Start()
    {
        maxangle = FindObjectOfType<Manager>().maxangle;
        sinhang = FindObjectOfType<Manager>().sinhang;

        maxv = FindObjectOfType<Manager>().maxv;
        sinhgrav = FindObjectOfType<Manager>().sinhgrav;
        centergrav = FindObjectOfType<Manager>().centergrav;
    }
    private void Update()
    {
        Start();
        linclock = FindObjectOfType<Manager>().linclock;
        angclock = FindObjectOfType<Manager>().angclock;
        float v_x = sinh(Mathf.PerlinNoise(seed + linclock, 0), sinhgrav, maxv) + CorrectX() - centergrav * obj.position.x;
        float v_y = sinh(Mathf.PerlinNoise(0, seed + linclock), sinhgrav, maxv) + CorrectY() - centergrav * obj.position.y;
        if ((obj.position.x > 12 & v_x > 0) | (obj.position.x < -12 & v_x < 0)) { v_x = 0; }
        if ((obj.position.y > 6 & v_y > 0) | (obj.position.y < -6 & v_y < 0)) { v_y = 0; }
        FindObjectOfType<Arrow>().VectorVisual(v_y, v_x);
        //Debug.Log(Mathf.PerlinNoise(seed + linclock, 0));
        obj.position += new Vector3(v_x * Time.deltaTime, v_y * Time.deltaTime, 0);
        obj.eulerAngles += new Vector3(0, 0, Time.deltaTime * sinh(Mathf.PerlinNoise(seed + angclock + 100, 0), sinhang, maxangle));
    }
    private float sinh(float x, float b, float v)
    {
        float exp_x = Mathf.Exp(2 * b * (x - 0.5f));
        float exp_b = Mathf.Exp(b);
        float exp_negx = Mathf.Exp(-2 * b * (x - 0.5f));
        float exp_negb = Mathf.Exp(-1 * b);
        return v * (exp_x - exp_negx) / (exp_b - exp_negb);
    }
    private float CorrectX()
    {
        float sum = 0;
        float[,] grad = FindObjectOfType<Manager>().GetRow(key);
        for (int i = 0; i < grad.GetLength(0); i++)
        {
            sum += grad[i, 1];
        }
        return sum;
    }
    private float CorrectY()
    {
        float sum = 0;
        float[,] grad = FindObjectOfType<Manager>().GetRow(key);
        for (int i = 0; i < grad.GetLength(0); i++)
        {
            sum += grad[i, 0];
        }
        return sum;
    }
}
