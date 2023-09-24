using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public float totalMoney;
    public float popularity = 1;
    public int forestsDestroyed;
    float enviroHate = 0;
    public float boardPopularity = 1;

    WorkerController workers;
    DayController days;

    float enviroHateDecayRate = 0.01f;
    // Update is called once per frame
    void Update() {
        if(!workers) {
            workers = FindObjectOfType<WorkerController>();
        }

        if(!days) {
            days = FindObjectOfType<DayController>();
        }

        enviroHate -= Time.deltaTime * enviroHateDecayRate;

        if(enviroHate > 1) {
            enviroHate = 1;
        }

        if(enviroHate < 0) {
            enviroHate = 0;
        }

        popularity -= enviroHate * 0.1f * Time.deltaTime;
        popularity += (workers.GetAvgHappiness() - 0.5f) * Time.deltaTime * 0.1f;

        boardPopularity += 0;
    }

    float ExpectedProfit() {
        return 0;
    }

    public void DestoryTree() {
        enviroHate += 0.15f;
        popularity -= 0.1f;
        forestsDestroyed++;
    }

    public void BuyGreenwash() {
        enviroHate -= 0.3f;
        popularity += 0.2f;
        //Queue broadcast
    }

    public void BuyHappiness() {
        workers.BuyHappiness(0.2f);
        //Queue broadcast
    }

    public void UnionEvent() {
        boardPopularity -= 0.2f;
        popularity += 0.1f;
    }

    public void AddMoney(float amount) {
        totalMoney += amount;
    }

    public void PayMoney(float amount) {
        totalMoney -= amount;
    }
}
