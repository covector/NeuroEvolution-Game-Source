using UnityEngine;

public class Manager : MonoBehaviour
{
    public float speed;
    public float maxangle;
    public float angularspeed;
    public float maxv;
    public float sinhgrav;
    public float corrrate;
    public float centergrav;
    public float sinhang;
    public GameObject[] lasers;
    public float[,,] gradient;
    [HideInInspector]
    public float linclock = 0;
    [HideInInspector]
    public float angclock = 0;

    private void SetGradient()
    {
        int bias = 0;
        for (int x = 0; x < lasers.GetLength(0); x++)
        {
            int offset = 0;
            for (int y = 0; y < lasers.GetLength(0); y++)
            {
                int localbias = 0;
                if (x == y) { offset = 1; continue; }
                float self = lasers[x].transform.eulerAngles.z;
                float opp = lasers[y].transform.eulerAngles.z;
                float grad = gradientmap(Mathf.Abs(self - opp));
                int[] dec = NormDec(lasers[x].transform.position.x, lasers[x].transform.position.y, lasers[y].transform.position.x, lasers[y].transform.position.y, opp);
                if (bias * dec[1] > 0) { dec[0] *= -1; }
                gradient[x, y - offset, 0] = corrrate * grad * Mathf.Sin((opp + 90 * dec[0]) * Mathf.PI / 180); // y
                gradient[x, y - offset, 1] = corrrate * grad * Mathf.Cos((opp + 90 * dec[0]) * Mathf.PI / 180); // x
                localbias += dec[1];
                if (y == lasers.GetLength(0) - 1)
                {
                    if (localbias > 0) { bias += 1; } else { bias -= 1; }
                }
            }
            offset = 0;
        }
    }
    private void Start()
    {
        gradient = new float[lasers.GetLength(0), lasers.GetLength(0) - 1, 2];
    }
    private void Update()
    {
        SetGradient();
        linclock += speed * Time.deltaTime;
        angclock += angularspeed * Time.deltaTime;
    }
    private float gradientmap(float delta)
    {
        int even = 1;
        if (Mathf.Abs(delta) % 180 > 90) { even = -1; }
        float moddelta = even * (Mathf.Abs(delta) % 90) + 45 * (1 - even);
        return Mathf.Pow(0.95f, moddelta);
    }
    public float[,] GetRow(int row)
    {
        float[,] res = new float[gradient.GetLength(1),2];
        for (int i = 0; i < gradient.GetLength(1); i++)
        {
            res[i, 0] = gradient[row, i, 0];
            res[i, 1] = gradient[row, i, 1];
        }
        return res;
    }
    private int[] NormDec(float selfx, float selfy, float oppx, float oppy, float oppang)
    {
        float m = Mathf.Tan(oppang * Mathf.PI / 180);
        float c = oppy - m * oppx;
        bool higher = selfy > m * selfx + c;
        if (higher)
        {
            if (oppang < 90 | oppang > 270) { return new int[2] { 1, 1 }; }
            else { return new int[2] { -1, 1 }; }
        }
        else
        {
            if (oppang < 90 | oppang > 270) { return new int[2] { -1, -1 }; }
            else { return new int[2] { 1, -1 }; }
        }
    }
}
