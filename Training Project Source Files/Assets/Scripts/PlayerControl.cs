using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region variables
    public Transform obj;
    public Vector2 max_v;
    public float acceleration;
    public float dampspeed;
    public Vector2 velocity = new Vector2(0,0);
    public int maxhealth;
    public SpriteRenderer spren;
    public Health healthbar;
    public ParticleSystem part;
    public ParameterBoard parameters;

    private bool over = false;
    private float cooldown = 0;
    private int currhealth;
    private bool firstbump_h;
    private bool firstbump_v;

    #endregion
    private void Start()
    {
        max_v = FindObjectOfType<Manager2>().max_v;
        acceleration = FindObjectOfType<Manager2>().acceleration;
        dampspeed = FindObjectOfType<Manager2>().dampspeed;
        maxhealth = parameters.maxhealth;
        currhealth = maxhealth;
    }
    private void Update()
    {
        
        #region Translation
        if (firstbump_h & Mathf.Abs(obj.position.x) > 8.25f)
        {
            velocity = new Vector2(0, velocity.y);
            firstbump_h = false;
        }
        if (!firstbump_h & Mathf.Abs(obj.position.x) < 8.25f) { firstbump_h = true; }
        if (firstbump_v & Mathf.Abs(obj.position.y) > 4.37f)
        {
            velocity = new Vector2(velocity.x, 0);
            firstbump_v = false;
        }
        if (!firstbump_v & Mathf.Abs(obj.position.y) < 4.37f) { firstbump_v = true; }

        if (!over & ((Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow) & obj.position.x > -8.25f) | (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow) & obj.position.x < 8.25f)))
        {
            if (-1 * velocity.x < max_v.x) { if (Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow)) { velocity -= new Vector2(acceleration * Time.deltaTime, 0); } }
            if (velocity.x < max_v.x) { if (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow)) { velocity += new Vector2(acceleration * Time.deltaTime, 0); } }
        }
        else
        {
            if (Mathf.Abs(velocity.x) < 0.001) { velocity = new Vector2(0, velocity.y); }
            else { velocity -= new Vector2(dampspeed * velocity.x * Time.deltaTime, 0); }
        }
        if (!over & ((Input.GetKey(KeyCode.UpArrow) & !Input.GetKey(KeyCode.DownArrow) & obj.position.y < 4.37f) | (Input.GetKey(KeyCode.DownArrow) & !Input.GetKey(KeyCode.UpArrow) & obj.position.y > -4.37f)))
        {
            if (-1 * velocity.y < max_v.y) { if (Input.GetKey(KeyCode.DownArrow) & !Input.GetKey(KeyCode.UpArrow)) { velocity -= new Vector2(0, acceleration * Time.deltaTime); } }
            if (velocity.y < max_v.y) { if (Input.GetKey(KeyCode.UpArrow) & !Input.GetKey(KeyCode.DownArrow)) { velocity += new Vector2(0, acceleration * Time.deltaTime); } }
        }
        else
        {
            if (Mathf.Abs(velocity.y) < 0.001) { velocity = new Vector2(velocity.x, 0); }
            else { velocity -= new Vector2(0, dampspeed * velocity.y * Time.deltaTime); }
        }
        if ((obj.position.x > -8.25f & velocity.x < 0) | (obj.position.x < 8.25f & velocity.x > 0)) { obj.position += new Vector3(velocity.x, 0, 0); }
        if ((obj.position.y > -4.37f & velocity.y < 0) | (obj.position.y < 4.37f & velocity.y > 0)) { obj.position += new Vector3(0, velocity.y, 0); }
        #endregion
        #region Rotation
        float anglef = ((Mathf.Atan2(velocity.y, velocity.x) * 180 / Mathf.PI + 270) % 360);
        if (velocity.y != 0 | velocity.x != 0)
        { obj.eulerAngles = new Vector3(0, 0, anglef); }
        #endregion 
    }
    #region Collision
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Laser(Clone)")
        {
            if (collision.GetComponent<Lasers>().deadly) { part.Play(); }
            if (cooldown > 0) { cooldown -= Time.fixedDeltaTime; }
            if (!over & collision.GetComponent<Lasers>().deadly & cooldown <= 0f)
            {
                cooldown = 3f;
                currhealth -= 1;
                StartCoroutine(healthbar.Damage(1));
                StartCoroutine(RedBlink());
                //           Debug.Log(currhealth);
                if (currhealth < 0)
                {
                    FindObjectOfType<Manager2>().GameOver();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Laser(Clone)")
        {
            cooldown = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Laser(Clone)")
        {
            part.Stop();
        }
    }
    IEnumerator RedBlink()
    {
        float time_passed = 0;
        for (int i = 0; i < 300; i++)
        {
            if (time_passed > 2) { break; }
            float gb = 0.5f + 0.5f * Mathf.Cos(time_passed * 2 * Mathf.PI);
            spren.color = new Color(1, gb, gb);
            time_passed += Time.deltaTime;
            yield return null;
        }
        spren.color = new Color(1, 1, 1);
    }
    #endregion
    public void Disable()
    {
        over = true;
    }
}
