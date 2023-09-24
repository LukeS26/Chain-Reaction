using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animalpen : WorkingStation {
    float timeCount;
    override public void Animate() {
        for(int i = 0; i < transform.childCount; i++) {
            if(curGrowthPercent < 0.1f) {
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, curGrowthPercent * 10);
                transform.GetChild(i).localScale = new Vector2(0.65f, 0.65f);
            } else if(base.curGrowthPercent < 0.5f) {
                transform.GetChild(i).rotation = Quaternion.identity;
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                transform.GetChild(i).localScale = new Vector2((base.curGrowthPercent * 1.5f) + 0.5f, (base.curGrowthPercent * 1.5f) + 0.5f);
            } else {
                transform.GetChild(i).localScale = new Vector2(1.25f, 1.25f);

                if(curGrowthPercent > 0.7f && curGrowthPercent < 0.9f) {
                    transform.GetChild(i).rotation = Quaternion.Lerp(transform.GetChild(i).rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime / 0.2f);
                } else {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (10 * (curGrowthPercent - 0.9f)));
                }
            }
        }
    }
}
