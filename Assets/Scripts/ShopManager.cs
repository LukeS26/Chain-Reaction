using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject workerPrefab;

    WorkerController workers;
    StatManager stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyWorker() {
        if(!workers) {
            workers = FindObjectOfType<WorkerController>();
        }
        
        Worker worker = Instantiate(workerPrefab).GetComponent<Worker>();
        workers.PutInResidence(worker);
    }

    public void BuyGreenwash() {
        if(stats.totalMoney < 1000) {
            return;
        }

        stats.PayMoney(1000);

        if(!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        stats.BuyGreenwash();
    }

    public void BuyHappiness() {
        if(stats.totalMoney < 1000) {
            return;
        }

        stats.PayMoney(1000);
        
        if(!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        stats.BuyHappiness();
    }
}
