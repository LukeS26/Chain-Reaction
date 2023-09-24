using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkerController : MonoBehaviour
{
    public List<Worker> workers = new List<Worker>();

    float timeWorking;
    public int wage = 0;
    public TextMeshProUGUI wageIn;
    float startWorkTime = 5; //Sunrise = 6
    float endWorkTime = 17; //Sunset = 18

    bool isWorkingDay, wasWorkingDay;

    DayController dayCycle;
    StatManager stats;

    int minimumWage = 300;

    // Update is called once per frame
    void Update() {
        if(!dayCycle) {
            dayCycle = GetComponent<DayController>();
        }

        if (!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        isWorkingDay = dayCycle.GetHour() > startWorkTime && dayCycle.GetHour() < endWorkTime;

        if(!isWorkingDay && GetAvgHappiness() < 0.3f && UnityEngine.Random.Range(0f, 1f) < 0.4f * Time.deltaTime * 0.4f / 12f) {
            //Unionize
            stats.UnionEvent();
            minimumWage = 575;
        }

        wage = int.Parse((string) wageIn.GetComponent<TextMeshProUGUI>().text);
        
        if(wage < minimumWage) {
            wage = minimumWage;
            UpdateWages(wage);
        }

        if(isWorkingDay && !wasWorkingDay) {
            UpdateWorkers(true);
        } else if(!isWorkingDay && wasWorkingDay) {
            UpdateWorkers(false);
        }

        wasWorkingDay = isWorkingDay;
    }

    public void RemoveWorker(Worker worker) {
        for(int i = 0; i < workers.Count; i++) {
            if(worker == workers[i]) {
                if(worker.residenceStation) {
                    worker.residenceStation.RemoveWorker();
                }
                if(worker.workStation) {
                    worker.workStation.RemoveWorker(worker);
                }
                workers.RemoveAt(i);
                return;
            }
        }
    }

    public void DropHappiness(float amount, ResidenceStation residence) {
        for(int i = 0; i < workers.Count; i++) {
            workers[i].happiness -= amount;
            if(workers[i].residenceStation == residence) {
                workers[i].happiness -= amount;
            }
        }
    }

    public void UpdateWages(int new_wage) {
        wage = new_wage;

        for(int i = 0; i < workers.Count; i++) {
            workers[i].wage = wage;
        }
    }

    public float GetAvgHappiness() {
        if(workers.Count < 1) { return 0.5f; }

        float sum = 0;
        for(int i = 0; i < workers.Count; i++) {
            sum += workers[i].happiness;
        }

        return sum / workers.Count;
    }

    public void BuyHappiness(float incr) {
        for(int i = 0; i < workers.Count; i++) {
            workers[i].happiness += incr;
        }
    }

    public void PutInResidence(Worker worker) {
        ResidenceStation[] residenceStations = FindObjectsByType<ResidenceStation>(FindObjectsSortMode.None);

        if(residenceStations.Length < 0) {
            return;
        }

        for(int j = 0; j < residenceStations.Length; j++) {
            if(residenceStations[j].WorkersAllowed()) {
                worker.isWorking = false;
                worker.residenceStation = residenceStations[j];
                residenceStations[j].AssignWorker();
                worker.transform.SetParent(residenceStations[j].transform.Find("workers"));
                worker.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    public void UpdateWorkers(bool doWork) {
        if(doWork) {
            WorkingStation[] workStations = FindObjectsByType<WorkingStation>(FindObjectsSortMode.None);

            if(workStations.Length < 0) {
                return;
            }

            for(int i = 0; i < workers.Count; i++) {
                Worker worker = workers[i];

                float closestDistance = (workStations[0].transform.position - worker.transform.position).sqrMagnitude;
                int closestWork = 0;
                int spotsFree = workStations[0].WorkersAllowed();

                for(int j = 0; j < workStations.Length; j++) {
                    if(workStations[j].WorkersAllowed() > spotsFree || ((workStations[j].WorkersAllowed() == spotsFree && (workStations[j].transform.position - worker.transform.position).sqrMagnitude < closestDistance))) {
                        closestDistance = (workStations[j].transform.position - worker.transform.position).sqrMagnitude;
                        closestWork = j;
                        spotsFree = workStations[j].WorkersAllowed();
                    }
                }

                if(spotsFree > 0) {
                    if(worker.transform.parent != null) {
                        worker.transform.SetParent(null);
                    }

                    if(worker.residenceStation) {
                        worker.residenceStation.RemoveWorker();
                        worker.residenceStation = null;
                    }
                    worker.isWorking = true;
                    worker.workStation = workStations[closestWork];
                    worker.WorkingStationIsSlaughter = workStations[closestWork].gameObject.GetComponent<Animalpen>() != null;
                    workStations[closestWork].AssignWorker(worker);
                    worker.GetMoveCommand((Vector2) workStations[closestWork].transform.position);
                }
            }
        } else {
            ResidenceStation[] residenceStations = FindObjectsByType<ResidenceStation>(FindObjectsSortMode.None);

            if(residenceStations.Length < 0) {
                return;
            }

            for(int i = 0; i < workers.Count; i++) {
                Worker worker = workers[i];
                
                if(worker.workStation) {
                    worker.workStation.RemoveWorker(worker);
                    worker.workStation = null;
                }

                if(worker.residenceStation) {
                    continue;
                }

                float closestDistance = (residenceStations[0].transform.position - worker.transform.position).sqrMagnitude;
                int closestResidence = 0;

                for(int j = 0; j < residenceStations.Length; j++) {
                    if(residenceStations[j].WorkersAllowed() && (residenceStations[j].transform.position - worker.transform.position).sqrMagnitude > closestDistance) {
                        closestDistance = (residenceStations[j].transform.position - worker.transform.position).sqrMagnitude;
                        closestResidence = j;
                    }
                }

                if(residenceStations[closestResidence].WorkersAllowed()) {
                    worker.isWorking = false;
                    worker.residenceStation = residenceStations[closestResidence];
                    residenceStations[closestResidence].AssignWorker();
                    worker.GetMoveCommand((Vector2) residenceStations[closestResidence].transform.position);
                }
            }
        }
    }
}
