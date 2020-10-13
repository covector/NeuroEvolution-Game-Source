using UnityEngine;

[CreateAssetMenu(fileName = "BestNetwork", menuName = "ScriptableObjects/BestNetwork", order = 1)]
public class BestNetwork : ScriptableObject
{
    public float[] ih_weight;
    public float[] ih_bias;
    public float[] ho_weight;
    public float[] ho_bias;
    public int Input;
    public int Hidden;
    public int Output;
}
