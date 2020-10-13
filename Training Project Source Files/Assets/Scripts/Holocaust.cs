using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holocaust : MonoBehaviour
{
    public int population_count = 100;
    public float chernobyl_rate = 0.1f;
    public int best_n = 10;
    public int generation = 0;
    public int casualties = 0;
    public GameObject[] population;
    public NeuralNetwork[] best_networks;
    public NeuralNetwork[] best_networks_prev;
    public float[] score;
    public GameObject prisoner;
    public int brain_saved;
    public float recordtime;
    public float totalscore = 0;
    public bool particles;
    public string selectionmode;
    public GameObject laser;
    public ParameterBoard parameters;
    public Text gen;
    public float roundstarttime;
    public float longesttime = 0;
    public Text longesttext;
    public float speedup = 1;
    public HighScore highsc;
    public List<float> highscore;
    public HighScore high;
    public bool centerdetection;
    public Text genhigh;
    public BestNetwork bestofthebest;
    public NeuralNetwork bestBrain;


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


        highscore = highsc.HighScoreTable;
        highscore.Clear();

        population_count = parameters.population_count;
        chernobyl_rate = parameters.chernobyl_rate;
        best_n = parameters.best_n;
        selectionmode = parameters.selectionmode.ToString();
        centerdetection = parameters.CenterDetection;

        BotBrain.particleson = particles;
        population = new GameObject[population_count];
        best_networks = new NeuralNetwork[best_n];
        best_networks_prev = new NeuralNetwork[best_n];
        score = new float[best_n];
        NewGeneration();
    }
    void NewGeneration()
    {
        if (generation > 0)
        {
            float lastround = speedup * (Time.time - roundstarttime);
            if (lastround > longesttime) { longesttime = lastround; high.HighestScore = lastround; longesttext.text = "Highest score: " + longesttime.ToString(); }
            //currentgen.Add(lastround);
            highscore.Add(lastround);
            genhigh.text = GenerationHighScore(generation);
        }
        speedup = parameters.SpeedUp;
        FindObjectOfType<InputManager>().speedup = speedup;
        FindObjectOfType<Manager2>().speedup = speedup;
        if (centerdetection) { FindObjectOfType<InputManager>().centerdetection = 1; } else { FindObjectOfType<InputManager>().centerdetection = 0; }
        FindObjectOfType<Manager2>().laser.GetComponent<SpriteRenderer>().enabled = !parameters.HideLaser;
        prisoner.GetComponent<SpriteRenderer>().enabled = !parameters.HideBot;
        roundstarttime = Time.time;
        Lasers[] lasers = FindObjectsOfType<Lasers>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].Kill();
        }
        BotBrain.number = 0;
        casualties = 0;
        generation++;
        gen.text = "Generation: " + generation.ToString();
        brain_saved = 0;
        BotBrain.totalscore = totalscore;
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
            Debug.Log(best_networks[brain_saved]);
            if (parameters.IncludePrevGen & generation > 1)
            {
                best_networks_prev[brain_saved] = best_networks[brain_saved].Copy();
            }
            best_networks[brain_saved] = population[number].GetComponent<BotBrain>().brain.Copy();
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
        if (casualties == population_count)
        {
            float survivedTime = speedup * (Time.time - roundstarttime);
            if (survivedTime > longesttime)
            {
                bestBrain = best_networks[brain_saved - 1].Copy();
                bestofthebest.HighestScore = survivedTime;
                bestofthebest.ih_weight = best_networks[brain_saved - 1].ih_weight.Rowify();
                bestofthebest.ih_bias = best_networks[brain_saved - 1].ih_bias.Rowify();
                bestofthebest.ho_weight = best_networks[brain_saved - 1].ho_weight.Rowify();
                bestofthebest.ho_bias = best_networks[brain_saved - 1].ho_bias.Rowify();
            }
            NewGeneration(); 
        }
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
