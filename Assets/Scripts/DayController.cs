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
    StatManager stats;

    UnityEngine.Rendering.Universal.Light2D light;
    
    int daysPassed = 0;

    // Update is called once per frame
    void Update() {
        if(!workers) {
            workers = GetComponent<WorkerController>();
        }
        
        if(!light) {
            light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        if(!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        if(!GameManager.gameStarted) { return; }

        if(hourPercent < 17 && hourPercent > 5) {
            hourPercent += Time.deltaTime * daySpeed; 
        } else {
            hourPercent += Time.deltaTime * nightSpeed;
        }

        if(hourPercent >= 24) {
            daysPassed++;
            hourPercent %= 24;

            for(int i = 0; i < workers.workers.Count; i++) {
                stats.PayMoney(workers.wage / 30f);
            }
            stats.UpdateBoard();
        }

        GameManager.timeCycle = GetDayPercent();

        light.intensity = lightCurve.Evaluate(hourPercent / 24);
    }

    public float GetHour() {
        return hourPercent;
    }

    public float GetDayPercent() {
        return hourPercent / 24;
    }

    public int GetCurDay() {
        return daysPassed;
    }
}
