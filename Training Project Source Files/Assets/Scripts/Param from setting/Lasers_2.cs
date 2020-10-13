using System.Collections;
using UnityEngine;
public class Lasers_2 : MonoBehaviour
{
    public Transform self;
    public SpriteRenderer obj;
    public bool deadly = false;
    public int index;
    public float spawntime;
    private void Start()
    {
        index = FindObjectOfType<Manager2_2>().laserind;
    }
    private void Update()
    {
        obj.color = new Color(0f, 0f, 0f, 1f);
    }
    public void Spawn()
    {
        StartCoroutine(FindObjectOfType<Shake_2>().CameraShake(0.5f, 0.25f));
        deadly = true;
    }
    public void Kill()
    {
        FindObjectOfType<Manager2_2>().Remove(index);
        Destroy(gameObject);
    }
}
