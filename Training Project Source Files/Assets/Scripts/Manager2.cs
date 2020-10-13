using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager2 : MonoBehaviour
{
    public UpdateTime timer;
    float currenttime = 0;
    public float starttime = 0;
    public float spawnrate;
    public GameObject laser;
    public GameObject gameoverui;
    public GameObject arrowchecker;
    public GameObject damager;
    public GameObject walllaser;
    public Vector2 max_v;
    public float acceleration;
    public float dampspeed;
    public ParameterBoard parameters;
    //   public Dictionary<int, GameObject> arrowlist = new Dictionary<int, GameObject>();
    public int laserind = 0;
    public Dictionary<int, GameObject> laserlist = new Dictionary<int, GameObject>();
    public float speedup;
    public ArrowChecker[] arrowcheckers;

    //public InputManager inman;
    //public Transform agent;

    public bool over = true;
    private void Update()
    {
        //agent = GameObject.Find("BotAgent(Clone)").transform;
        if (!over & Time.time - currenttime > spawnrate / speedup)
        {
            laserind += 1;
            currenttime += spawnrate / speedup;
            Vector3 randpos = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
            Quaternion randrot = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));
            GameObject newlaser = Instantiate(laser, randpos, randrot);
            newlaser.GetComponent<Animator>().speed = speedup;
//            GameObject newarrow = Instantiate(arrowchecker);
//            arrowlist.Add(laserind, newarrow);
            laserlist.Add(laserind, newlaser);
        }
        /*int ind = 0;
        foreach (GameObject laser in laserlist.Values)
        {
            if (ind > 5) { break; }
            arrowcheckers[ind].UpdateSpot(inman.GetVector(laser.transform, agent));
            ind++;
        }*/
    }
    public void StartGame()
    {
        over = false;
        currenttime = Time.time;
        starttime = currenttime;
        timer.StartTimer();
        if (parameters.corner)
        {
            Instantiate(damager, new Vector3(-8.2f, 4.3f, 0), Quaternion.identity);
            Instantiate(damager, new Vector3(-8.2f, -4.3f, 0), Quaternion.identity);
            Instantiate(damager, new Vector3(8.2f, 4.3f, 0), Quaternion.identity);
            Instantiate(damager, new Vector3(8.2f, -4.3f, 0), Quaternion.identity);
        }
        if (parameters.side)
        {
            Instantiate(walllaser, new Vector3(8.75f, 0, 0), Quaternion.Euler(0, 0, 90));
            Instantiate(walllaser, new Vector3(-8.75f, 0, 0), Quaternion.Euler(0, 0, 90));
            Instantiate(walllaser, new Vector3(0, 4.87f, 0), Quaternion.identity);
            Instantiate(walllaser, new Vector3(0, -4.87f, 0), Quaternion.identity);
        }
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
    public void Remove(int index)
    {
        laserlist.Remove(index);
//        Destroy(arrowlist[index]);
//        arrowlist.Remove(index);
    }
}