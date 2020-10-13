using UnityEngine;
using UnityEngine.UI;

public class InputManager_2 : MonoBehaviour
{
    public string input;
    public string output;
    public float RayLength;
    public float theta;
    public Manager2_2 man;
    public int iter;
    public bool usetimemask;
    public float speedup;
    public bool invert;
    public int centerdetection;
    public float offset;
    private void Start()
    {
        input = PlayerPrefs.GetString("Input", "Vector");
        output = PlayerPrefs.GetString("Output", "Keystroke");
        RayLength = PlayerPrefs.GetFloat("RayLength", 3f);
        theta = PlayerPrefs.GetFloat("Angle", 30f) * Mathf.PI / 180;
        if (PlayerPrefs.GetInt("TimeMask", 1) == 1) { usetimemask = true; } else { usetimemask = false; }
        if (PlayerPrefs.GetInt("Invert", 1) == 1) { invert = true; } else { invert = false; }
        offset = PlayerPrefs.GetFloat("Offset", 0f);
        centerdetection = PlayerPrefs.GetInt("CenterDetection", 1);

        int correction = 1;
        if (2 * Mathf.PI % theta == 0) { correction = 0; }
        iter = div(2 * Mathf.PI, theta) + correction;
    }
    public float[] GetInput(GameObject obj)
    {
        switch (input)
        {
            case "Vector":
                int ind = 0;
                float[] array_out = new float[20];
                foreach (GameObject laser in man.laserlist.Values)
                {
                    float[] vec = GetVector(laser.transform, obj.transform);
                    if (usetimemask)
                    {
                        float time = laser.GetComponent<Lasers_2>().spawntime;
                        array_out[2 * ind] = time * InvertMap_Neg(vec[0], 4f);
                        array_out[2 * ind + 1] = time * InvertMap_Neg(vec[1], 9f / 4f);
                    }
                    else
                    {
                        array_out[2 * ind] = InvertMap_Neg(vec[0], 4f);
                        array_out[2 * ind + 1] = InvertMap_Neg(vec[1], 9f / 4f);
                    }
                    ind++;
                }
                for (int i = 2 * ind + 2; i < 18; i++)
                {
                    array_out[i] = 0;
                }
                return array_out;
            case "Raycast":
                return DistanceFromRays(obj.transform, 4 + centerdetection);
            default:
                return null;
        }
    }
    public float[] GetVector(Transform laser, Transform agent)
    {
        float tangent = Mathf.Tan(laser.eulerAngles.z * Mathf.PI / 180);
        float delta_x = (tangent * (agent.position.x - laser.position.x) + laser.position.y - agent.position.y) / (tangent + (1 / tangent));
        float delta_y = (tangent * (laser.position.x - agent.position.x) + agent.position.y - laser.position.y) / (tangent * tangent + 1);
        return new float[] { delta_x, delta_y };
    }
    public float[] DistanceFromRays(Transform agent, int spare)
    {
        float[] RayArray = new float[iter + spare];
        for (int i = 0; i < iter + spare; i++)
        {
            if (i == iter)
            {
                RaycastHit2D center = Physics2D.Raycast(agent.position, Vector2.zero, RayLength, 1 << 9);
                if (center) { RayArray[i] = 1; } else { RayArray[i] = 0; }
                continue;
            }
            if (i >= iter + centerdetection)
            {
                RayArray[i] = 0;
                continue;
            }
            Vector2 direction = new Vector2(Mathf.Sin(i * theta), Mathf.Cos(i * theta));
            RaycastHit2D hit = Physics2D.Raycast((Vector2)agent.position + direction * offset, direction, RayLength, 1 << 9);
            //Debug.DrawRay((Vector2)agent.position + direction * offset, direction * RayLength, Color.green);
            if (hit)
            {
                if (usetimemask)
                {
                    float time = hit.collider.GetComponent<Lasers_2>().spawntime;
                    RayArray[i] = time * InvertMap(hit.distance, RayLength);
                }
                else { RayArray[i] = InvertMap(hit.distance, RayLength); }
            }
            else
            {
                RayArray[i] = 0;
            }
        }
        return RayArray;
    }
    private int div(float a, float b)
    {
        return (int)((a - (a % b)) / b);
    }
    public int GetIn()
    {
        switch (input) {
            case "Vector":
                return 20;
            case "Raycast":
                return iter + 4 + centerdetection; 
            default:
                return 0;
        }
    }
    public int GetOut()
    {
        switch (output) {
            case "Basisdirection":
                return 2;
            case "Direction":
                return 9;
            case "Keystroke":
                return 4;
            default:
                return 0;
        }
    }
    public float InvertMap(float dis, float maxval)
    {
        if (!invert) { return dis; }
        return Mathf.Pow(0.85f, 10f * dis / maxval);
    }
    public float InvertMap_Neg(float dis, float maxval)
    {
        if (!invert) { return dis; }
        return Mathf.Sign(dis) * Mathf.Pow(0.85f, 10f * Mathf.Abs(dis) / maxval);
    }
}
