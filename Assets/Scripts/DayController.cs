using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour
{
    public float hourPercent;

    public float daySpeed = 0.1f;
    public float nightSpeed = 0.2f;

    public AnimationCurve lightCurve;

    WorkerController workers;

    UnityEngine.Rendering.Universal.Light2D light;
    
    int daysInQuarter = 10;
    int daysPassed = 0;
    int curQuarter = 1;

    // Update is called once per frame
    void Update() {
        if(!workers) {
            workers = GetComponent<WorkerController>();
        }
        
        if(!light) {
            light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        if(hourPercent < 17 && hourPercent > 5) {
            hourPercent += Time.deltaTime * daySpeed; 
        } else {
            hourPercent += Time.deltaTime * nightSpeed;
        }

        if(hourPercent >= 24) {
            daysPassed++;
            hourPercent %= 24;
        }

        if(daysPassed >= daysInQuarter) {
            daysPassed = 0;
            curQuarter++;
        }

        light.intensity = lightCurve.Evaluate(hourPercent / 24);
    }

    public float GetQuarter() {
        return curQuarter;
    }

    public float GetHour() {
        return hourPercent;
    }

    public float GetDayPercent() {
        return hourPercent / 24;
    }
}
