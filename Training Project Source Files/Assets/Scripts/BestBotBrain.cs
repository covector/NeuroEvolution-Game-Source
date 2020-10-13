using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BestBotBrain : MonoBehaviour
{
    public int hidden;
    public Transform obj;
    public Vector2 max_v;
    public float acceleration;
    public float dampspeed;
    public int maxhealth;
    public Vector2 velocity = new Vector2(0, 0);
    public SpriteRenderer spren;
    public GameObject particlesystem;
    public ParticleSystem part;
    public string output;
    public NeuralNetwork brain;
    public InputManager inman;
    public int currhealth;
    public float deathtimer = 0;
    public float deathtimer2 = 0;
    public float afk;
    public bool inlas = false;
    public bool dead = false;
    public bool afkenable;
    public bool singafkenable;
    public float singafk;
    public int xory = 1;
    public float speedup;
    public bool invert;
    public ParameterBoard parameters;
    public GameObject networkvisual;
    public Text inputactivation;
    public Text hiddenactivation;
    public Text outputactivation;
    public Text inputhiddenweight;
    public Text inputhiddenbias;
    public Text hiddenoutputweight;
    public Text hiddenoutputbias;

    private float cooldown = 0;
    private bool firstbump_h;
    private bool firstbump_v;
    public static bool particleson;

    public BestNetwork bestbrain;

    private void Start()
    {
        speedup = parameters.SpeedUp;
        hidden = parameters.HiddenLayer;
        output = parameters.output.ToString();
        afk = parameters.afk;
        afkenable = parameters.afk_kill;
        singafk = parameters.singlelineafk;
        singafkenable = parameters.singlelineafkkill;
        invert = parameters.Invert;
        // BestPrison stuff
        BestPrison camp = FindObjectOfType<BestPrison>();

        networkvisual = GameObject.Find("Network");
        if (networkvisual != null)
        {
            inputactivation = GameObject.Find("InputText").GetComponent<Text>();
            hiddenactivation = GameObject.Find("HiddenText").GetComponent<Text>();
            outputactivation = GameObject.Find("OutputText").GetComponent<Text>();
            inputhiddenweight = GameObject.Find("InputHiddenWeight").GetComponent<Text>();
            inputhiddenbias = GameObject.Find("InputHiddenBias").GetComponent<Text>();
            hiddenoutputweight = GameObject.Find("HiddenOutputWeight").GetComponent<Text>();
            hiddenoutputbias = GameObject.Find("HiddenOutputBias").GetComponent<Text>();
        }
        //load brain
        if (parameters.PlayMode.ToString() == "Load")
        {
            NetData loadedBrain = DataManager.LoadBrain(parameters.LoadIndex);
            int inputNeuron = loadedBrain.Input;
            int hiddenNeuron = loadedBrain.Hidden;
            int outputNeuron = loadedBrain.Output;
            R2Tensor ihWeight = new R2Tensor(hiddenNeuron, inputNeuron, loadedBrain.ih_weight);
            R2Tensor ihBias = new R2Tensor(hiddenNeuron, 1, loadedBrain.ih_bias);
            R2Tensor ohWeight = new R2Tensor(outputNeuron, hiddenNeuron, loadedBrain.ho_weight);
            R2Tensor ohBias = new R2Tensor(outputNeuron, 1, loadedBrain.ho_bias);
            brain = new NeuralNetwork(ihWeight, ihBias, ohWeight, ohBias, inputNeuron, hiddenNeuron, outputNeuron, loadedBrain.outputType);
        }
        else
        {
            int inputNeuron = bestbrain.Input;
            int hiddenNeuron = bestbrain.Hidden;
            int outputNeuron = bestbrain.Output;
            R2Tensor ihWeight = new R2Tensor(hiddenNeuron, inputNeuron, bestbrain.ih_weight);
            R2Tensor ihBias = new R2Tensor(hiddenNeuron, 1, bestbrain.ih_bias);
            R2Tensor ohWeight = new R2Tensor(outputNeuron, hiddenNeuron, bestbrain.ho_weight);
            R2Tensor ohBias = new R2Tensor(outputNeuron, 1, bestbrain.ho_bias);
            brain = new NeuralNetwork(ihWeight, ihBias, ohWeight, ohBias, inputNeuron, hiddenNeuron, outputNeuron, bestbrain.outputType);
        }

        if (networkvisual != null)
        {
            if (networkvisual.activeSelf)
            {
                inputhiddenweight.text = PrintNeuron(brain.ih_weight);
                inputhiddenbias.text = PrintNeuron(brain.ih_bias);
                hiddenoutputweight.text = PrintNeuron(brain.ho_weight);
                hiddenoutputbias.text = PrintNeuron(brain.ho_bias);
            }
        }

        Manager2 manager = FindObjectOfType<Manager2>();
        max_v = manager.max_v;
        acceleration = manager.acceleration;
        dampspeed = manager.dampspeed;
        maxhealth = parameters.maxhealth;
        inman = FindObjectOfType<InputManager>();
        currhealth = maxhealth;

        //Particles
        if (particleson)
        {
            part = Instantiate(particlesystem, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<ParticleSystem>();
        }
    }
    private void Update()
    {
        if (networkvisual == null) { networkvisual = GameObject.Find("Network"); }
        if (afkenable) 
        {
            if (speedup * deathtimer >= afk & !inlas & !dead)
            {
                FindObjectOfType<BestPrison>().ImDeadLOL();
                Destroy(gameObject);
                dead = true;
            }
            if (velocity.x == 0 & velocity.y == 0) { deathtimer += Time.deltaTime; }
            else { deathtimer = 0; }
        }
        if (singafkenable)
        {
            if (speedup * deathtimer2 >= singafk & !inlas & !dead)
            {
                FindObjectOfType<BestPrison>().ImDeadLOL();
                Destroy(gameObject);
            }
            if (velocity.x == 0 & xory == 1) { deathtimer2 += Time.deltaTime; }
            else
            {
                if (velocity.y == 0 & xory == 0) { deathtimer2 += Time.deltaTime; }
                else { deathtimer2 = 0; xory = 1 - xory; }
            }
        }

        #region Think
        float[] input_array = inman.GetInput(gameObject);
        int input_array_length = input_array.Length;
        input_array[input_array_length - 4] = Tanh(obj.position.x, 4f);
        input_array[input_array_length - 3] = Tanh(obj.position.y, 9f / 4f);
        input_array[input_array_length - 2] = Tanh(velocity.x, max_v.x / 2f);
        input_array[input_array_length - 1] = Tanh(velocity.y, max_v.y / 2f);

        R2Tensor finproc = brain.ForwardPropagation(R2Tensor.ToMatrix(input_array, "col"));
        float[] thought = R2Tensor.ToVector(finproc);

        if (networkvisual != null)
        {
            if (networkvisual.activeSelf)
            {
                inputactivation.text = PrintNeuron(R2Tensor.ToMatrix(input_array, "col"));
                hiddenactivation.text = PrintNeuron(brain.ForwardPropagation(R2Tensor.ToMatrix(input_array, "col"), false));
                outputactivation.text = PrintNeuron(finproc);
            }
        }
        
        //R2Tensor.ToMatrix(thought, "row").PrintMatrix();
        int vert = 0;
        int hori = 0;
        switch(output)
        {
            case "Basisdirection":
                if (thought[0] > 0.5f) { hori = 1; }
                if (thought[0] < -0.5f) { hori = -1; }
                if (thought[1] > 0.5f) { vert = 1; }
                if (thought[1] < -0.5f) { vert = -1; }
                break;
            case "Direction":
                if (thought[0] == 1 | thought[4] == 1 | thought[5] == 1) { vert = 1; }
                if (thought[1] == 1 | thought[6] == 1 | thought[7] == 1) { vert = -1; }
                if (thought[3] == 1 | thought[5] == 1 | thought[7] == 1) { hori = 1; }
                if (thought[2] == 1 | thought[4] == 1 | thought[6] == 1) { hori = -1; }
                break;
            case "Keystroke":
                if (thought[0] == 1) { vert += 1; }
                if (thought[1] == 1) { vert -= 1; }
                if (thought[2] == 1) { hori -= 1; }
                if (thought[3] == 1) { hori += 1; }
                break;
        }
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
            if (hori * velocity.x < max_v.x) { if (hori != 0) { velocity += new Vector2(hori * acceleration * Time.deltaTime * speedup, 0); } }
        }
        else
        {
            if (Mathf.Abs(velocity.x) < 0.001) { velocity = new Vector2(0, velocity.y); }
            else { velocity -= new Vector2(dampspeed * velocity.x * Time.deltaTime * speedup, 0); }
        }
        if ((vert == 1 & obj.position.y < 4.37f) | (vert == -1 & obj.position.y > -4.37f))
        {
            if (vert * velocity.y < max_v.y) { if (vert != 0) { velocity += new Vector2(0, vert * acceleration * Time.deltaTime * speedup); } }
        }
        else
        {
            if (Mathf.Abs(velocity.y) < 0.001) { velocity = new Vector2(velocity.x, 0); }
            else { velocity -= new Vector2(0, dampspeed * velocity.y * Time.deltaTime * speedup); }
        }

        obj.position += new Vector3(velocity.x * speedup, velocity.y * speedup, 0);
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
                cooldown = 3f / speedup;
                currhealth -= 1;
                if (parameters.DamageGlow) { StartCoroutine(RedBlink()); }
                if (currhealth < 0 & !dead)
                {
                    dead = true;
                    FindObjectOfType<BestPrison>().ImDeadLOL();
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
                    cooldown = 3f / speedup;
                    currhealth -= 1;
                    if (parameters.DamageGlow) { StartCoroutine(RedBlink()); }
                    if (currhealth < 0 & !dead)
                    {
                        dead = true;
                        FindObjectOfType<BestPrison>().ImDeadLOL();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inlas = true;
        if (collision.name == "Laser(Clone)" | collision.name == "Damager(Clone)" | collision.name == "WallLaser(Clone)")
        {
            cooldown = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inlas = false;
        if ((collision.name == "Laser(Clone)" | collision.name == "Damager(Clone)" | collision.name == "WallLaser(Clone)") & particleson)
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
            time_passed += Time.deltaTime * speedup;
            yield return null;
        }
        spren.color = new Color(1, 1, 1);
    }
    #endregion
    public string PrintNeuron(R2Tensor tensor)
    {
        string string_out = "";
        for (int y = 0; y < tensor.row; y++)
        {
            string_out = string_out + " | ";
            for (int x = 0; x < tensor.col; x++)
            {
                string_out = string_out + Truncate(tensor.tensor[y, x]) + " | ";
            }
            string_out = string_out + "\n";
        }
        return string_out;
    }
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
    private static float Tanh(float number, float maxval)
    {
        float exp = Mathf.Exp(2 * number / maxval);
        float invexp = Mathf.Exp(-2 * number / maxval);
        if (((exp - invexp) / (exp + invexp)) > 1 | ((exp - invexp) / (exp + invexp)) < -1) { Debug.Log("fuck"); }
        return (exp - invexp) / (exp + invexp);
    }
}