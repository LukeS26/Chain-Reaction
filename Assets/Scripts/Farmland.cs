using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : WorkingStation
{
    public List<Sprite> farmStates = new List<Sprite>();


    override public void Animate() {
        int state = (int) (base.curGrowthPercent * farmStates.Count);

        base.spriteRenderer.sprite = farmStates[state];

    }
}
