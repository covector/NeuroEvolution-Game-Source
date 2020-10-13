using UnityEngine;

[System.Serializable]
public class NetData
{
    public float HighestScore;
    public float[] ih_weight;
    public float[] ih_bias;
    public float[] ho_weight;
    public float[] ho_bias;
    public int Input;
    public int Hidden;
    public int Output;
    public string outputType;

    public string inputType;
    public bool Invert;
    public bool CenterDetection;
    public float RayLength;
    public float theta;
    public float offset;
    public bool UseTimeMask;

    public int population_count;
    public float chernobyl_rate;
    public float chernobyl_magnitude;
    public float clamping;
    public int best_n;
    public bool IncludePrevGen;
    public bool BestStay;
    public string selectionmode;

    public NetData(BestNetwork brain, ParameterBoard parameters)
    {
        HighestScore = brain.HighestScore;
        ih_weight = brain.ih_weight;
        ih_bias = brain.ih_bias;
        ho_weight = brain.ho_weight;
        ho_bias = brain.ho_bias;
        Input = brain.Input;
        Hidden = brain.Hidden;
        Output = brain.Output;
        outputType = brain.outputType;

        inputType = parameters.input.ToString();
        Invert = parameters.Invert;
        CenterDetection = parameters.CenterDetection;
        RayLength = parameters.RayLength;
        theta = parameters.theta;
        offset = parameters.offset;
        UseTimeMask = parameters.UseTimeMask;

        population_count = parameters.population_count;
        chernobyl_rate = parameters.chernobyl_rate;
        chernobyl_magnitude = parameters.chernobyl_magnitude;
        clamping = parameters.clamping;
        best_n = parameters.best_n;
        IncludePrevGen = parameters.IncludePrevGen;
        BestStay = parameters.BestStay;
        selectionmode = parameters.selectionmode.ToString();
    }
}
