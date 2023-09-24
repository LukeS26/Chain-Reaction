using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingStation : Placeable
{
    public float daysToGrow;
    public int moneyAmount;

    public float curGrowthPercent = 0;

    public List<Worker> workersAssigned = new List<Worker>();

    protected SpriteRenderer spriteRenderer;
    StatManager statManager;

    float squashAmount = 0.2f;
    float speed = 10f;

    float squashTime = 0;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public int WorkersAllowed() {
        return 3 - workersAssigned.Count;
    }

    public void AssignWorker(Worker worker) {
        workersAssigned.Add(worker);
    }

    public void RemoveWorker(Worker worker) {
        for (int i = 0; i < workersAssigned.Count; i++) {
            if (workersAssigned[i] == worker) {
                workersAssigned.RemoveAt(i);
                return;
            }
        }
    }

    void Update() {
        if(!spriteRenderer) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(!statManager) {
            statManager = FindObjectOfType<StatManager>();
        }

        int curWorkers = 0;

        for (int i = 0; i < workersAssigned.Count; i++) {
            if( (workersAssigned[i].transform.position - transform.position).sqrMagnitude < 0.25f) {
                curWorkers ++;
            }
        }

        curGrowthPercent += curWorkers * Time.deltaTime / (daysToGrow * 30);
        if(curGrowthPercent >= 1) {
            curGrowthPercent %= 1;
            statManager.AddMoney(moneyAmount);
        }

        Animate();

        if(squashTime > 0) {
            float squashScale = Mathf.Sin(squashTime * speed) * squashAmount;
            
            transform.localScale = new Vector3(originalScale.x * (1f + squashScale), originalScale.y * (1f - squashScale), originalScale.z);

            squashTime -= Time.deltaTime;
        } else {
            transform.localScale = originalScale;
        }
    }

    override public void ClickOn() {
        base.ClickOn();

        curGrowthPercent += 2f / (daysToGrow * 30);
        squashTime = Mathf.PI * 2 / speed;
    }

    virtual public void Animate() {}
}
