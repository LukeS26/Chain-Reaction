using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;

public class Worker : MonoBehaviour {
    public float wage = 550.0f;
    public float happiness = 100.0f;
    private float happinessDecreaseRate = 0.1f;

    public bool isWorking;

    float gridSize = 0.5f; //increase patience or gridSize for larger maps
    float speed = 0.2f; //increase for faster movement
    Pathfinder<Vector2> pathfinder; //the pathfinder object that stores the methods and patience
    [SerializeField] LayerMask obstacles;
    bool searchShortcut = false; 
    bool snapToGrid = false; 
    Vector2 targetNode; //target in 2D space
    List <Vector2> path;
    List<Vector2> pathLeftToGo = new List<Vector2>();

    // Start is called before the first frame update
    void Start() {
        pathfinder = new Pathfinder<Vector2>(GetDistance,GetNeighbourNodes,1000); //increase patience or gridSize for larger maps
    }

    private void Update() {
        if(isWorking) {
            float wageFactor = 550.0f / wage;

            happiness -= happinessDecreaseRate * wageFactor * Time.deltaTime;
            
            happiness = Mathf.Max(happiness, 0.0f);
        } else {
            float wageFactor = wage / 550.0f;
            
            happiness += happinessDecreaseRate * wageFactor * Time.deltaTime * 0.5f;
            
            happiness = Mathf.Min(happiness, 100.0f);
        }
        
        if (pathLeftToGo.Count > 0)  {
            Vector3 dir =  (Vector3)pathLeftToGo[0]-transform.position ;
            transform.position += dir.normalized * speed * Time.deltaTime;
            if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < Time.deltaTime * Time.deltaTime * speed * speed) {
                transform.position = pathLeftToGo[0];
                pathLeftToGo.RemoveAt(0);
            }
        }
    }

    void GetMoveCommand(Vector2 target) {
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