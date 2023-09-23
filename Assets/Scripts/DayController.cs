using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour
{
    float hourPercent;

    public float daySpeed = 0.04f;
    public float nightSpeed = 0.08f;

    WorkerController workers;

    // Update is called once per frame
    void Update() {
        if(!workers) {
            workers = GetComponent<WorkerController>();
        }

        //During Day
        if(hourPercent < 18 && hourPercent > 6) {
            hourPercent += Time.deltaTime * daySpeed; 
        } else {
            hourPercent += Time.deltaTime * nightSpeed;
        }

        hourPercent %= 24;
    }

    public float GetHour() {
        return hourPercent;
    }

    public float GetDayPercent() {
        return hourPercent / 24;
    }
}
