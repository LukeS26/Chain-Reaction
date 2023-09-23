using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingStation : Placeable
{
    int workersAssigned = 0;

    public bool WorkersAllowed() {
        return workersAssigned < 3;
    }
}
