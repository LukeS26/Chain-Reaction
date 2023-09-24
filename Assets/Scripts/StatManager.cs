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

    float totalDayMoney = 100;

    WorkerController workers;
    DayController days;

    float enviroHateDecayRate = 0.001f;
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
        popularity += Mathf.Clamp(workers.GetAvgHappiness() - 0.5f, -0.5f, 0.25f) * Time.deltaTime * 0.005f;
        popularity -= Mathf.Clamp(forestsDestroyed * 0.001f, 0, 1) * Time.deltaTime * 0.1f;;

        popularity = Mathf.Clamp01(popularity);

        GameManager.money = totalMoney;
        GameManager.avgWorkHap = (int) (workers.GetAvgHappiness() * 100);
        GameManager.ceoHap = (int) (boardPopularity * 100);
        GameManager.custHap = (int) (popularity * 100);

        if(popularity < 0.01f) {
            FindObjectOfType<GameManager>().PopularityLose();
        }

        if(boardPopularity < 0.01f) {
            FindObjectOfType<GameManager>().SharesLose();
        }
    }

    public void UpdateBoard() {
        boardPopularity += 0.3f * (totalDayMoney - ExpectedProfit()) / (totalDayMoney + ExpectedProfit());
        totalDayMoney = 0;

        boardPopularity = Mathf.Clamp01(boardPopularity);
    }
    
    float ExpectedProfit() {
        return 150 * Mathf.Pow(2, 0.3f * days.GetCurDay());
    }

    public void DestoryTree() {
        enviroHate += 0.03f;
        popularity -= 0.03f;
        forestsDestroyed++;
    }

    public void BuyGreenwash() {
        enviroHate -= 0.3f;
        popularity += 0.15f;
    }

    public void BuyHappiness() {
        workers.BuyHappiness(0.2f);
    }

    public void UnionEvent() {
        boardPopularity -= 0.2f;
        popularity += 0.1f;
    }

    public void AddMoney(float amount) {
        totalMoney += amount;
        totalDayMoney += amount;
    }

    public void PayMoney(float amount) {
        totalMoney -= amount;
        totalDayMoney -= amount;
    }

    public void BuyItem(float amount) {
        totalMoney -= amount;
    }
}
