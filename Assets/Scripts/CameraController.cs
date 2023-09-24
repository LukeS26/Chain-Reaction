using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {
    float pickUpThreshold = 0.5f;
    float curClickTime = 0;

    bool isClicking, isDraggingCamera, clickedLastFrame;
    Vector3 curDrag;
    Vector3 curPos;
    float curZoom;

    float totalMove = 0;

    public float moveSpeed;
    public float zoomSpeed = 0.05f;

    GameObject clickingOn;

    Placeable pickedObj;

    StatManager stats;

    Camera ppc;

    // Update is called once per frame
    void Update() {
        if (!ppc) {
            ppc = GetComponent<Camera>();
        }

        if (!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        ppc.orthographicSize -= curZoom * zoomSpeed;
        ppc.orthographicSize = Mathf.Clamp(ppc.orthographicSize, 1, 4);
        
        if(isClicking) {
            RaycastHit2D hit = Physics2D.Raycast(curPos, Vector3.zero);

            //Get the first object clicked on when the mouse is pressed down
            if(!clickedLastFrame && hit) {
                clickingOn = hit.transform.gameObject;
            }

            //if clicking on an object for long enough, drag it
            if(curClickTime > pickUpThreshold && clickingOn != null) {
                if(hit && pickedObj == null && hit.transform.GetComponent<Placeable>()) {
                    pickedObj = hit.transform.GetComponent<Placeable>();
                }
                if(hit && pickedObj == null && hit.transform.GetComponent<Rainforest>()) {
                    hit.transform.GetComponent<Rainforest>().DestroyEffect();
                    stats.DestoryTree();
                    Destroy(hit.transform.gameObject);
                }

                if(pickedObj) { pickedObj.Drag(curPos); }
                
            } else if(curDrag.magnitude < 2f && hit && hit.transform.gameObject == clickingOn) {
                //Otherwise if you're not really moving the mouse, and still clicking on it, add a bit more to the timer
                curClickTime += Time.deltaTime;
            } else {
                //Otherwise you moved off it, or are dragging the camera, so drop it
                clickingOn = null;
                curClickTime = 0;

                totalMove += curDrag.magnitude;
                transform.localPosition -= curDrag * moveSpeed;
            }
        } else {
            //You're not clicking
            if(pickedObj != null) {
                pickedObj.Drop();
                pickedObj = null;
            } else if(clickedLastFrame && clickingOn != null && curClickTime < pickUpThreshold) {
                Placeable placeable = clickingOn.GetComponent<Placeable>();
                if(placeable) { placeable.ClickOn(); }
            }

            curClickTime = 0;
        }

        clickedLastFrame = isClicking;
    }

    public void DragAction(InputAction.CallbackContext obj) {
        curDrag = Vec2ToVec3(obj.ReadValue<Vector2>());
    }

    public void ClickAction(InputAction.CallbackContext obj) {
        isClicking = obj.performed;
    }

    public void PositionAction(InputAction.CallbackContext obj) {
        curPos = Vec2ToVec3(Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>()));
    }

    public void ZoomAction(InputAction.CallbackContext obj) {
        curZoom = obj.ReadValue<float>();
    }

    Vector3 Vec2ToVec3(Vector2 vec) {
        return new Vector3(vec.x, vec.y, 0);
    }
}
