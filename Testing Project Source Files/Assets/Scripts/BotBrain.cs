using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
    public int hidden;
    public Transform obj;
    public Vector2 max_v;
    public float acceleration;
    public float dampspeed;
    public int maxhealth;
    public Vector2 velocity = new Vector2(0, 0);
    public GameObject particlesystem;
    public SpriteRenderer spren;
    public ParticleSystem part;
    public NeuralNetwork brain;
    public InputManager inman;
    public int currhealth;
    public static float totalscore;
    public bool inlas = false;
    public bool dead = false;
    public BestNetwork nn;
    public int number;

    private float cooldown = 0;
    private bool firstbump_h;
    private bool firstbump_v;
    public bool particleson = true;
    public Health healthBar;

    private void Start()
    {
        Manager2 manager = FindObjectOfType<Manager2>();
        max_v = manager.max_v;
        acceleration = manager.acceleration;
        dampspeed = manager.dampspeed;
        maxhealth = manager.maxhealth;
        inman = FindObjectOfType<InputManager>();
        currhealth = maxhealth;

        //Load brain
        int inputNeuron = nn.Input;
        int hiddenNeuron = nn.Hidden;
        int outputNeuron = nn.Output;
        R2Tensor ihWeight = new R2Tensor(hiddenNeuron, inputNeuron, nn.ih_weight);
        R2Tensor ihBias = new R2Tensor(hiddenNeuron, 1, nn.ih_bias);
        R2Tensor ohWeight = new R2Tensor(outputNeuron, hiddenNeuron, nn.ho_weight);
        R2Tensor ohBias = new R2Tensor(outputNeuron, 1, nn.ho_bias);
        brain = new NeuralNetwork(ihWeight, ihBias, ohWeight, ohBias, inputNeuron, hiddenNeuron, outputNeuron);

        //Particles
        if (particleson)
        {
            part = Instantiate(particlesystem, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<ParticleSystem>();
        }
    }
    private void Update()
    {
        #region Think
        float[] input_array = inman.GetInput(obj, velocity);
        R2Tensor finproc = brain.ForwardPropagation(R2Tensor.ToMatrix(input_array, "col"));
        float[] thought = R2Tensor.ToVector(finproc);

        int vert = 0;
        int hori = 0;
        if (thought[0] == 1) { vert += 1; }
        if (thought[1] == 1) { vert -= 1; }
        if (thought[2] == 1) { hori -= 1; }
        if (thought[3] == 1) { hori += 1; }
        #endregion
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

        if ((hori == -1 & obj.position.x > -8.25f) | (hori == 1 & obj.position.x < 8.25f))
        {
            if (hori * velocity.x < max_v.x) { if (hori != 0) { velocity += new Vector2(hori * acceleration * Time.deltaTime, 0); } }
        }
        else
        {
            if (Mathf.Abs(velocity.x) < 0.001) { velocity = new Vector2(0, velocity.y); }
            else { velocity -= new Vector2(dampspeed * velocity.x * Time.deltaTime, 0); }
        }
        if ((vert == 1 & obj.position.y < 4.37f) | (vert == -1 & obj.position.y > -4.37f))
        {
            if (vert * velocity.y < max_v.y) { if (vert != 0) { velocity += new Vector2(0, vert * acceleration * Time.deltaTime); } }
        }
        else
        {
            if (Mathf.Abs(velocity.y) < 0.001) { velocity = new Vector2(velocity.x, 0); }
            else { velocity -= new Vector2(0, dampspeed * velocity.y * Time.deltaTime); }
        }

        obj.position += new Vector3(velocity.x, velocity.y, 0);
        if (obj.position.x < -8.25f) { obj.position = new Vector3(-8.25f, obj.position.y, 0); velocity.x = 0; }
        if (obj.position.x > 8.25f) { obj.position = new Vector3(8.25f, obj.position.y, 0); velocity.x = 0; }
        if (obj.position.y > 4.37f) { obj.position = new Vector3(obj.position.x, 4.37f, 0); velocity.y = 0; }
        if (obj.position.y < -4.37f) { obj.position = new Vector3(obj.position.x, -4.37f, 0); velocity.y = 0; }
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
            if (collision.GetComponent<Lasers>().deadly & particleson) { part.Play(); }
            if (cooldown > 0) { cooldown -= Time.fixedDeltaTime; }
            if (collision.GetComponent<Lasers>().deadly & cooldown <= 0f)
            {
                cooldown = 3f;
                currhealth -= 1;
                StartCoroutine(healthBar.Damage(currhealth));
                StartCoroutine(RedBlink());
                if (currhealth < 0 & !dead)
                {
                    dead = true;
                    //FindObjectOfType<Holocaust>().ImDeadLOL(prisoner_number);
                    FindObjectOfType<Manager2>().BotDead(number);
                    StartCoroutine(healthBar.Damage(0));
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.name == "Damager(Clone)" | collision.name == "WallLaser(Clone)")
            {
                if (particleson)
                {
                    part.Play();
                }
                if (cooldown > 0) { cooldown -= Time.fixedDeltaTime; }
                if (cooldown <= 0f)
                {
                    cooldown = 3f;
                    currhealth -= 1;
                    StartCoroutine(healthBar.Damage(currhealth));
                    StartCoroutine(RedBlink());
                    if (currhealth < 0 & !dead)
                    {
                        dead = true;
                        //FindObjectOfType<Holocaust>().ImDeadLOL(prisoner_number);
                        FindObjectOfType<Manager2>().BotDead(number);
                        StartCoroutine(healthBar.Damage(0));
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inlas = true;
        if (collision.name == "Laser(Clone)" | collision.name == "WallLaser(Clone)")
        {
            cooldown = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inlas = false;
        if ((collision.name == "Laser(Clone)" | collision.name == "WallLaser(Clone)") & particleson)
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
    private string Truncate(float textstring)
    {
        string stringtext = textstring.ToString();
        if (stringtext.Length > 4)
        {
            return stringtext.Substring(0, 4);
        }
        else
        {
            return stringtext;
        }
    }
}