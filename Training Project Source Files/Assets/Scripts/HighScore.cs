using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighScore", menuName = "ScriptableObjects/HighScoreBoard", order = 2)]

public class HighScore : ScriptableObject
{
    //public Dictionary<Dictionary<string, float>, List<float>> HighScoreTable = new Dictionary<Dictionary<string, float>, List<float>>();
    public List<float> HighScoreTable;
    public float HighestScore;
}
