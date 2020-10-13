using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holocaust_2 : MonoBehaviour
{
    public int population_count = 100;
    public float chernobyl_rate = 0.1f;
    public int best_n = 10;
    public int generation = 0;
    public int casualties = 0;
    public GameObject[] population;
    public NeuralNetwork[] best_networks;
    public float[] score;
    public GameObject prisoner;
    public int brain_saved;
    public float recordtime;
    public float totalscore = 0;
    public bool particles;
    public string selectionmode;
    public GameObject laser;
    public Text gen;
    public float roundstarttime;
    public float longesttime = 0;
    public Text longesttext;
    public float speedup = 1;
    public HighScore highsc;
    public List<float> highscore;
    public bool centerdetection;
    public Text genhigh;
    public Slider speedupslider;
    public bool HideBot = false;
    public bool HideLaser = false;
    public bool damageglow = true;
    public InputManager_2 inputman;
    public Manager2_2 man;
    public Toggle hidebot;
    public Toggle hidelaser;
    public Toggle glow;


    /*
    public Dictionary<Dictionary<string, float>, List<float>> scoretable;
    public Dictionary<string, float> currentset;
    public List<float> currentgen;
    */

    private void Start()
    {
        /*
        scoretable = highsc.HighScoreTable;
        currentset = CurrentSettings();
        scoretable.Add(currentset, new List<float>());
        currentgen = scoretable[currentset];
        */

        man = FindObjectOfType<Manager2_2>();
        inputman = FindObjectOfType<InputManager_2>();

        highscore = highsc.HighScoreTable;
        highscore.Clear();

        population_count = PlayerPrefs.GetInt("Population", 100);
        chernobyl_rate = PlayerPrefs.GetFloat("MutationRate", 0.1f);
        best_n = PlayerPrefs.GetInt("BestN", 5);
        selectionmode = PlayerPrefs.GetString("SelectionMode", "Time");


        BotBrain_2.particleson = particles;
        population = new GameObject[population_count];
        best_networks = new NeuralNetwork[best_n];
        score = new float[best_n];
        NewGeneration();
    }
    void NewGeneration()
    {
        if (generation > 0)
        {
            float lastround = speedup * (Time.time - roundstarttime);
            if (lastround > longesttime) { longesttime = lastround; longesttext.text = "Highest score: " + longesttime.ToString(); }
            //currentgen.Add(lastround);
            highscore.Add(lastround);
            genhigh.text = GenerationHighScore(generation);
        }
        damageglow = glow.isOn;
        speedup = speedupslider.value;
        man.laser.GetComponent<SpriteRenderer>().enabled = !hidelaser.isOn;
        prisoner.GetComponent<SpriteRenderer>().enabled = !hidebot.isOn;
        man.speedup = speedup;
        inputman.speedup = speedup;
        roundstarttime = Time.time;
        Lasers_2[] lasers = FindObjectsOfType<Lasers_2>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].Kill();
        }
        BotBrain_2.number = 0;
        casualties = 0;
        generation++;
        gen.text = "Generation: " + generation.ToString();
        brain_saved = 0;
        BotBrain_2.totalscore = totalscore;
        totalscore = 0;
        for (int i = 0; i < population_count; i++)
        {
            population[i] = Instantiate(prisoner, Vector3.zero, Quaternion.identity);
        }
    }
    public void ImDeadLOL(int number)
    {
        if (casualties + best_n + 1 == population_count) { recordtime = Time.time; }
        if (casualties + best_n >= population_count)
        {
            best_networks[brain_saved] = population[number].GetComponent<BotBrain_2>().brain;
            switch (selectionmode)
            {
                case "Time":
                    score[brain_saved] = Time.time - recordtime;
                    totalscore += Time.time - recordtime;
                    break;
                case "Place":
                    score[brain_saved] = brain_saved + 1;
                    totalscore += brain_saved + 1;
                    break;
            }
            brain_saved++;
        }
        casualties++;
        if (casualties == population_count) { NewGeneration(); }
    }
    public string GenerationHighScore(int currgen)
    {
        int startgen = 0;
        string string_out = "";
        if (currgen >= 20) { startgen = currgen - 19; }
        for (int i = startgen; i < currgen; i++)
        {
            string_out = string_out + "Generation " + (i + 1).ToString() + ": " + highscore[i].ToString() + "\n";
        }
        return string_out;
    }
    /*
    public Dictionary<string, float> CurrentSettings()
    {
        Dictionary<string, float> dict_out = new Dictionary<string, float>();
        dict_out.Add("Hidden Layer", parameters.HiddenLayer);
        dict_out.Add("Input Type", (int)parameters.input);
        if (parameters.Invert) { dict_out.Add("Invert", 1); } else { dict_out.Add("Invert", 0); }
        dict_out.Add("Ray Length", parameters.RayLength);
        dict_out.Add("Theta", parameters.theta);
        if (parameters.UseTimeMask) { dict_out.Add("Use Time Mask", 1); } else { dict_out.Add("Use Time Mask", 0); }
        dict_out.Add("Output Type", (int)parameters.output);
        dict_out.Add("Mutation Rate", parameters.chernobyl_rate);
        dict_out.Add("Mutation Magnitude", parameters.chernobyl_magnitude);
        dict_out.Add("Best n", parameters.best_n);
        dict_out.Add("Selection Mode", (int)parameters.selectionmode);
        dict_out.Add("MaxHealth", parameters.maxhealth);
        if (parameters.afk_kill) { dict_out.Add("Afk Kill", parameters.afk); } else { dict_out.Add("Afk Kill", 0); }
        if (parameters.singlelineafkkill) { dict_out.Add("1D Afk Kill", parameters.singlelineafk); } else { dict_out.Add("1D Afk Kill", 0); }
        if (parameters.corner) { dict_out.Add("Corner", 1); } else { dict_out.Add("Corner", 0); }
        if (parameters.side) { dict_out.Add("Side", 1); } else { dict_out.Add("Side", 0); }

        return dict_out;
    }
    */
}
