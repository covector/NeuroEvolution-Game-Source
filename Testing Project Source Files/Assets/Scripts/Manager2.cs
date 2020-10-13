using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager2 : MonoBehaviour
{
    public UpdateTime timer;
    float currenttime = 0;
    public float starttime = 0;
    public float spawnrate = 0.5f;
    public GameObject laser;
    public GameObject gameoverui;
    public GameObject walllaser;
    public Vector2 max_v;
    public int maxhealth = 10;
    public float acceleration = 0.6f;
    public float dampspeed = 4;
    public bool over = true;
    public bool bot1ded = false;
    public bool bot2ded = false;
    public bool reallyOver = true;
    public BestNetwork one;
    public BestNetwork two;

    private void Update()
    {

        if (!reallyOver & Time.time - currenttime > spawnrate)
        {
            currenttime += spawnrate;
            Vector3 randpos = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
            Quaternion randrot = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));
            Instantiate(laser, randpos, randrot);
        }
        if (over & bot1ded & bot2ded)
        {
            reallyOver = true;
        }
    }
    public void BotDead(int no)
    {
        if (no == 1)
        {
            bot1ded = true;
        }
        if (no == 2)
        {
            bot2ded = true;
        }
    }
    public void StartGame()
    {
        over = false;
        reallyOver = false;
        currenttime = Time.time;
        starttime = currenttime;
        timer.StartTimer();
        Instantiate(walllaser, new Vector3(8.75f, 0, 0), Quaternion.Euler(0, 0, 90));
        Instantiate(walllaser, new Vector3(-8.75f, 0, 0), Quaternion.Euler(0, 0, 90));
        Instantiate(walllaser, new Vector3(0, 4.87f, 0), Quaternion.identity);
        Instantiate(walllaser, new Vector3(0, -4.87f, 0), Quaternion.identity);
    }
    public void GameOver()
    {
        Debug.Log("game over");
        FindObjectOfType<PlayerControl>().Disable();
        over = true;
        gameoverui.SetActive(true);
        FindObjectOfType<SurvivedTime>().ChangeText(timer.StopTimer());
    }
    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}