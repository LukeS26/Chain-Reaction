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
    StatManager stats;

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

        if(!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        if(dragging) {
            return;
        }

        dragging = true;

        if(stats.totalMoney < price) {
            return;
        }  

        stats.BuyItem(price);

        if(prefab) {
            if(prefab.GetComponent<Placeable>()) {
                controller.SetPickedObject(Instantiate(prefab).GetComponent<Placeable>());
            }
        }
    }
}
