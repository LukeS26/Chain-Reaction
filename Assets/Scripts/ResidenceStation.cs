using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceStation : Placeable {
    int workersAssigned = 0;

    public bool WorkersAllowed() {
        return workersAssigned < 10;
    }

    public void AssignWorker() {
        workersAssigned++;
    }

    public void RemoveWorker() {
        workersAssigned--;
    }
}