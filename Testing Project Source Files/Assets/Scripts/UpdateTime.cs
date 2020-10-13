using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour
{
    private bool started = false;
    private float starttime;
    public Text timer;
    private float lasttime;
    public void StartTimer()
    {
        starttime = FindObjectOfType<Manager2>().starttime;
        lasttime = starttime;
        started = true;
    }
    private void Update()
    {
        if (started)
        {
            if (Time.time - lasttime > 1)
            {
                timer.text = "Time survived: " + Sec2Time((int)(Time.time - starttime));
                lasttime += 1;
            }
        }
    }
    private string Sec2Time(int second)
    {
        int hr = div(second, 3600);
        int min = div(second - hr * 3600, 60);
        int sec = second - hr * 3600 - min * 60;
        return AddZero(hr) + ":" + AddZero(min) + ":" + AddZero(sec);
    }
    private int div(int a, int b)
    {
        return (a - (a % b)) / b;
    }
    private string AddZero(int num)
    {
        if (num < 10)
        {
            if (num == 0)
            {
                return "00";
            }
            return "0" + num.ToString();
        }
        return num.ToString();
    }
    public string StopTimer()
    {
        started = false;
        return Sec2Time((int)(Time.time - starttime));
    }
}
