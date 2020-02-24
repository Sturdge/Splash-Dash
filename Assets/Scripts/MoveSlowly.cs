using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSlowly : MonoBehaviour
{
    public float isTime;
    public bool completeTime=false;
    void Start()
    {
        Time.timeScale = 0.25f;
    }

    void Update()
    {
        if (completeTime == false)
        {
            Time.timeScale -= 0.01f;
        }
        if(Time.timeScale==0.025f)
        {
            completeTime = true;
        }
    }
    /*public Transform target;
    public float speed;
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }*/
}