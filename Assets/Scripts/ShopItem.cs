using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IDragHandler
{
    public float price;
    public GameObject prefab;

    CameraController controller;
    WorkerController workers;

    bool dragging;

    void Update() {
        if(!controller) {
            controller = FindObjectOfType<CameraController>();
        }

        if(!controller.IsClicking()) {
            dragging = false;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if(!controller) {
            controller = FindObjectOfType<CameraController>();
        }

        if(!workers) {
            workers = FindObjectOfType<WorkerController>();
        }

        if(dragging) {
            return;
        }

        dragging = true;

        if(prefab) {
            if(prefab.GetComponent<Placeable>()) {
                controller.SetPickedObject(Instantiate(prefab).GetComponent<Placeable>());
            } else if(prefab.GetComponent<Worker>()) {
                Worker worker = Instantiate(prefab, new Vector3(controller.transform.position.x, controller.transform.position.y, 0), Quaternion.identity).GetComponent<Worker>();

                workers.PutInResidence(worker);
            }
        }
    }
}
