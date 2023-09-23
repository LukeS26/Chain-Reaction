using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{
    List<Worker> workers = new List<Worker>();

    float timeWorking;
    float wage;
    float startWorkTime = 5; //Sunrise = 6
    float endWorkTime = 17; //Sunset = 18

    bool isWorkingDay, wasWorkingDay;

    DayController dayCycle;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(!dayCycle) {
            dayCycle = GetComponent<DayController>();
        }

        isWorkingDay = dayCycle.GetHour() > startWorkTime && dayCycle.GetHour() < endWorkTime;

        if(isWorkingDay && !wasWorkingDay) {
            UpdateWorkers(true);
        } else if(!isWorkingDay && wasWorkingDay) {
            UpdateWorkers(false);
        }

        wasWorkingDay = isWorkingDay;
    }

    public void UpdateWages(float new_wage) {
        wage = new_wage;

        for(int i = 0; i < workers.Count; i++) {
            workers[i].wage = wage;
        }
    }

    public void UpdateWorkers(bool doWork) {
        if(doWork) {
            WorkingStation[] workStations = FindObjectsByType<WorkingStation>(FindObjectsSortMode.None);

            for(int i = 0; i < workers.Count; i++) {
                Worker worker = workers[i];
                //Working
                worker.isWorking = true;
            }
        } else {
            ResidenceStation[] residenceStations = FindObjectsByType<ResidenceStation>(FindObjectsSortMode.None);

            for(int i = 0; i < workers.Count; i++) {
                Worker worker = workers[i];
                //Working
                worker.isWorking = true;
            }
        }
    }
}
