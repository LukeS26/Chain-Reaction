using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    float shortLongThreshold = 0.2f;
    int clickMoveThreshold = 500;
    float curClickTime = 0;

    bool isClicking;
    Vector3 curDrag;
    Vector3 curPos;

    float totalMove = 0;

    public float moveSpeed;

    Placable pickedObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(isClicking && curDrag.magnitude > shortLongThreshold) {
            //hold
            totalMove += curDrag.magnitude;
            transform.localPosition -= curDrag * moveSpeed;
        }

        if(!isClicking && curClickTime < shortLongThreshold && curClickTime > 0 && totalMove / curClickTime < clickMoveThreshold) {
            if (pickedObj != null) {
                pickedObj = null;
            } else {
                RaycastHit2D hit = Physics2D.Raycast(curPos, new Vector3(0, -1, 0));
                if(hit && hit.transform.GetComponent<Placable>()) {
                    pickedObj = hit.transform.GetComponent<Placable>();
                }
            }
        }

        if (isClicking) {
            curClickTime += Time.deltaTime;
        } else {
            totalMove = 0;
            curClickTime = 0;
        }

        if(pickedObj != null) {
            pickedObj.Drag(curPos);
        }
    }

    public void DragAction(InputAction.CallbackContext obj) {
        curDrag = vec2ToVec3(obj.ReadValue<Vector2>());
    }

    public void ClickAction(InputAction.CallbackContext obj) {
        isClicking = obj.performed;
    }

    public void PositionAction(InputAction.CallbackContext obj) {
        curPos = vec2ToVec3(Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>()));
    }

    Vector3 vec2ToVec3(Vector2 vec) {
        return new Vector3(vec.x, vec.y, 0);
    }
}
