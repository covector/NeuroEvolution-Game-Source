using System.Collections;
using UnityEngine;
public class Lasers : MonoBehaviour
{
    public Transform self;
    public SpriteRenderer obj;
    public bool deadly = false;
    public int index;
    public float spawntime;
    private void Start()
    {
        index = FindObjectOfType<Manager2>().laserind;
    }
    private void Update()
    {
        obj.color = new Color(0f, 0f, 0f, 1f);
    }
    public void Spawn()
    {
        StartCoroutine(FindObjectOfType<Shake>().CameraShake(0.5f, 0.25f));
        deadly = true;
    }
    public void Kill()
    {
        FindObjectOfType<Manager2>().Remove(index);
        Destroy(gameObject);
    }
}
