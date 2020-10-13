using UnityEngine;
public class Self_Deactivate : MonoBehaviour
{
    public ParameterBoard parameters;
    private void Start()
    {
        GetComponent<Animator>().speed = parameters.SpeedUp;
    }
    public void Deactivate()
    {
        Destroy(gameObject);
    }
}
