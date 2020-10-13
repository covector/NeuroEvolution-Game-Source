using System.Collections;
using UnityEngine;
public class Shake_2 : MonoBehaviour
{
    public Transform obj;
    float time_passed;
    public GameObject glow;
    public bool epilepsy = false;
    public bool shake = false;
    public IEnumerator CameraShake(float time, float magnitude)
    {
        if (epilepsy)
        {
            Instantiate(glow, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));
        }
        if (shake)
        {
            float variation = Random.Range(0f, 100f);
            time_passed = 0;
            Vector3 origin = new Vector3(0, 0, -10);
            for (int i = 0; i < 256; i++)
            {
                obj.position = origin + magnitude * (1 - time_passed / time) * new Vector3(Mathf.PerlinNoise(variation + 20f * time_passed, 0) - 0.5f, Mathf.PerlinNoise(0, variation + 20f * time_passed) - 0.5f, 0);
                time_passed += Time.deltaTime;
                if (time_passed > time) { break; }
                yield return null;
            }
        }
    }
    public void UpdateEpilepsy(bool toggle)
    {
        epilepsy = toggle;
    }
    public void UpdateShake(bool toggle)
    {
        shake = toggle;
    }
}
