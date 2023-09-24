using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;

public class Worker : MonoBehaviour {
    public float wage = 550.0f;
    public float happiness = 100.0f;
    private float happinessDecreaseRate = 0.1f;

    public bool isWorking;

    WorkerController workers;

    public WorkingStation workStation = null;
    public ResidenceStation residenceStation = null;

    public bool WorkingStationIsSlaughter;

    float gridSize = 0.5f; //increase patience or gridSize for larger maps
    float speed = 0.2f; //increase for faster movement
    Pathfinder<Vector2> pathfinder; //the pathfinder object that stores the methods and patience
    [SerializeField] LayerMask obstacles;
    bool searchShortcut = false; 
    bool snapToGrid = false; 
    Vector2 targetNode; //target in 2D space
    List <Vector2> path;
    List<Vector2> pathLeftToGo = new List<Vector2>();

    Animator animator;

    float waitTime;

    // Start is called before the first frame update
    void Awake() {
        pathfinder = new Pathfinder<Vector2>(GetDistance,GetNeighbourNodes,1000); //increase patience or gridSize for larger maps

        FindObjectOfType<WorkerController>().workers.Add(this);
    }

    private void Update() {
        if(!animator) {
            animator = GetComponent<Animator>();
        }

        if(!workers) {
            workers = FindObjectOfType<WorkerController>();
        }
        
        if(isWorking) {
            float wageFactor = 18.0f / wage;

            happiness -= (WorkingStationIsSlaughter ? 1.5f : 1f) * happinessDecreaseRate * wageFactor * Time.deltaTime / 100f;
            
            happiness = Mathf.Max(happiness, 0.0f);
        } else {
            float wageFactor = wage / 18.0f;
            
            happiness += happinessDecreaseRate * wageFactor * Time.deltaTime * 0.5f / 100f;
            
            happiness = Mathf.Min(happiness, 1.0f);

            if(happiness < 0.1f) {
                if(Random.Range(0f, 1f) < Time.deltaTime * 0.8f * 0.2f / 12f) {
                    workers.RemoveWorker(this);
                    workers.DropHappiness(0.2f, residenceStation);

                    Destroy(gameObject);
                }
            }
        }
        
        if (pathLeftToGo.Count > 0) {
            animator.SetBool("Walking", true);

            Vector3 dir = (Vector3)pathLeftToGo[0]-transform.position;

            transform.localScale = new Vector3(0.1f * Mathf.Sign(dir.normalized.x), 0.1f, 1f);

            transform.position += dir.normalized * speed * Time.deltaTime;
            if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < Time.deltaTime * Time.deltaTime * speed * speed) {
                transform.position = pathLeftToGo[0];
                pathLeftToGo.RemoveAt(0);
            }
        } else {
            animator.SetBool("Walking", false);

            if(workStation) {
                targetNode = (Vector2) workStation.transform.position;

                waitTime -= Time.deltaTime;
                if(waitTime < 0) {
                    waitTime = Random.Range(0.2f, 0.5f);
                    GetMoveCommand(targetNode);
                }
            } else if(residenceStation) {
                transform.SetParent(residenceStation.transform.Find("workers"));
            }
        }
    }

    public void GetMoveCommand(Vector2 target) {
        target += new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f));

        Vector2 closestNode = GetClosestNode(transform.position);
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) {
            if (searchShortcut && path.Count>0) {
                pathLeftToGo = ShortenPath(path);
            } else {
                pathLeftToGo = new List<Vector2>(path);
                if (!snapToGrid) pathLeftToGo.Add(target);
            }
        }
    }

    Vector2 GetClosestNode(Vector2 target) {
        return new Vector2(Mathf.Round(target.x/gridSize)*gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    float GetDistance(Vector2 A, Vector2 B) {
        return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
    }

    Dictionary<Vector2,float> GetNeighbourNodes(Vector2 pos) {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i=-1;i<2;i++) {
            for (int j=-1;j<2;j++) {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j)*gridSize;
                if (!Physics2D.Linecast(pos,pos+dir, obstacles)) {
                    neighbours.Add(GetClosestNode( pos + dir), dir.magnitude);
                }
            }

        }
        return neighbours;
    }

    
    List<Vector2> ShortenPath(List<Vector2> path) {
        List<Vector2> newPath = new List<Vector2>();
        
        for (int i=0;i<path.Count;i++) {
            newPath.Add(path[i]);
            for (int j=path.Count-1;j>i;j-- ) {
                if (!Physics2D.Linecast(path[i],path[j], obstacles)) {
                    
                    i = j;
                    break;
                }
            }
            newPath.Add(path[i]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }
}