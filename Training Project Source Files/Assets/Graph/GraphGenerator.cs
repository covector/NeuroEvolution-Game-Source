using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphGenerator : MonoBehaviour
{
    public HighScore score;
    private List<float> points;
    private float maxVal;

    private float winMaxY = 4;
    private float winMinY = -4;
    private float winMaxX = 8;
    private float winMinX = -8;

    private LineRenderer graph;

    public float IncrementX;
    public float IncrementY;
    public GameObject Label;
    public GameObject Canvas;

    private float axisMinX = -284.88f;
    private float axisMaxX = 425.8f;
    private float axisMinY = -184.42f;
    private float axisMaxY = 171.21f;
    private float zeroX = -297.4f;
    private float zeroY = -196.1f;

    private void Start()
    {
        //Get components
        points = score.HighScoreTable;
        maxVal = score.HighestScore;
        graph = GetComponent<LineRenderer>();
        int MaxX = points.Count;

        int failSave = 0;
        //set axis
        for (int i = 0; i * IncrementX < MaxX ; i++)
        {
            failSave++;
            if (failSave >= 300) { break; }
            GameObject currLabel = Instantiate(Label, Canvas.transform);
            currLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(axisMinX + ((i + 1) * IncrementX / MaxX) * (axisMaxX - axisMinX), zeroY);
            currLabel.GetComponent<Text>().text = ((int)((i + 1) * IncrementX)).ToString();
        }
        for (int i = 0; i * IncrementY < maxVal; i++)
        {
            failSave++;
            if (failSave >= 300) { break; }
            GameObject currLabel = Instantiate(Label, Canvas.transform);
            currLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(zeroX, axisMinY + ((i + 1) * IncrementY / maxVal) * (axisMaxY - axisMinY));
            currLabel.GetComponent<Text>().text = ((int)((i + 1) * IncrementY)).ToString();
        }

        //create point array
        graph.positionCount = MaxX;
        Vector3[] pointsVector = new Vector3[MaxX];

        for(int i = 0; i < points.Count; i++)
        {
            float x = winMinX + (i / (float)MaxX) * (winMaxX - winMinX);
            float y = winMinY + (points[i] / maxVal) * (winMaxY - winMinY);
            pointsVector[i] = new Vector3(x, y, 0);
        }
        graph.SetPositions(pointsVector);
    }
}
