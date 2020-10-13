using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestPrison : MonoBehaviour
{
    public GameObject prisoner;
    public float recordtime;
    public bool particles;
    public string selectionmode;
    public GameObject laser;
    public ParameterBoard parameters;
    public Text gen;
    public float roundstarttime;
    public float longesttime = 0;
    public Text longesttext;
    public float speedup = 1;
    public bool centerdetection;
    public Text genhigh;
    public BestNetwork bestofthebest;

    private void Start()
    {
        //PlayerPrefs.SetInt("SaveIndex", 0);

        if (parameters.PlayMode.ToString() == "Load")
        {
            NetData loadedBrain = DataManager.LoadBrain(parameters.LoadIndex);
            parameters.HiddenLayer = loadedBrain.Hidden;
            System.Enum.TryParse(loadedBrain.outputType, out parameters.output);

            System.Enum.TryParse(loadedBrain.inputType, out parameters.input);
            parameters.Invert = loadedBrain.Invert;
            parameters.CenterDetection = loadedBrain.CenterDetection;
            parameters.RayLength = loadedBrain.RayLength;
            parameters.theta = loadedBrain.theta;
            parameters.offset = loadedBrain.offset;
            parameters.UseTimeMask = loadedBrain.UseTimeMask;

            parameters.population_count = loadedBrain.population_count;
            parameters.chernobyl_rate = loadedBrain.chernobyl_rate;
            parameters.chernobyl_magnitude = loadedBrain.chernobyl_magnitude;
            parameters.clamping = loadedBrain.clamping;
            parameters.best_n = loadedBrain.best_n;
            parameters.IncludePrevGen = loadedBrain.IncludePrevGen;
            parameters.BestStay = loadedBrain.BestStay;
            System.Enum.TryParse(loadedBrain.selectionmode, out parameters.selectionmode);
        }

        selectionmode = parameters.selectionmode.ToString();
        centerdetection = parameters.CenterDetection;
        if (centerdetection) { FindObjectOfType<InputManager>().centerdetection = 1; } else { FindObjectOfType<InputManager>().centerdetection = 0; }

        bestofthebest.Input = FindObjectOfType<InputManager>().GetIn();
        bestofthebest.Output = FindObjectOfType<InputManager>().GetOut();
        bestofthebest.Hidden = parameters.HiddenLayer;
        bestofthebest.outputType = parameters.output.ToString();

        BestBotBrain.particleson = particles;

        if (parameters.PlayMode.ToString() == "Save")
        {
            DataManager.SaveBrain(new NetData(bestofthebest, parameters));
        }

        NewGeneration();
    }
    void NewGeneration()
    {
        float lastround = speedup * (Time.time - roundstarttime);
        Debug.Log(lastround);
        if (lastround > longesttime) { longesttime = lastround; longesttext.text = "Highest score: " + longesttime.ToString(); }
        speedup = parameters.SpeedUp;
        FindObjectOfType<InputManager>().speedup = speedup;
        FindObjectOfType<Manager2>().speedup = speedup;
        roundstarttime = Time.time;
        Lasers[] lasers = FindObjectsOfType<Lasers>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].Kill();
        }
        Instantiate(prisoner, Vector3.zero, Quaternion.identity);
    }
    public void ImDeadLOL()
    {
        NewGeneration(); 
    }
}
