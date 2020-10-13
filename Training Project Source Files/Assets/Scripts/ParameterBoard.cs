using UnityEngine;

[CreateAssetMenu(fileName = "Parameters", menuName = "ScriptableObjects/ParameterBoard", order = 1)]
public class ParameterBoard : ScriptableObject
{
    [Header("Architecture")]
    public int HiddenLayer;
    [Space]
    public InputType input;
    public bool Invert;
    public bool CenterDetection;
    public float RayLength;
    public float theta;
    public float offset;
    public bool UseTimeMask;
    [Space]
    public OutputType output;

    [Header("Genetic Algorithm")]
    public int population_count;
    public float chernobyl_rate;
    public float chernobyl_magnitude;
    public float clamping;
    public int best_n;
    public bool IncludePrevGen;
    public bool BestStay;
    public SelectionMethod selectionmode;

    [Header("Environment")]
    public int maxhealth;
    public bool afk_kill;
    public float afk;
    public bool singlelineafkkill;
    public float singlelineafk;
    public bool corner;
    public bool side;
    [Space]
    [Range(1f, 10f)]
    public float SpeedUp;

    [Header("Save")]
    public TestMode PlayMode;
    public int LoadIndex;

    [Header("Display")]
    public bool epilepsy;
    public bool ShakeScreen;
    public bool DamageGlow;
    public bool HideBot;
    public bool HideLaser;

    public enum InputType
    {
        Vector, Raycast
    }
    public enum OutputType
    {
        Basisdirection, Direction, Keystroke
    }
    public enum SelectionMethod
    {
        Time, Place
    }
    public enum TestMode
    {
        Default, Save, Load
    }
}
