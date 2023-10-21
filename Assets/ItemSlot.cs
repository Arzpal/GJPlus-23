using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private List<int> ventas;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("estoy entrando");
        ventas.Add(col.GetComponent<DragDrop>().breadtype);
        col.GetComponent<DragDrop>().falling = 2;
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<DragDrop>().falling = 0;
        ventas.Remove(other.GetComponent<DragDrop>().breadtype);
    }

   
}
