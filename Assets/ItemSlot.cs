using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private List<int> ventas;
    public List<GameObject> objetos;
    public int price;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("estoy entrando");
        ventas.Add(col.GetComponent<DragDrop>().breadtype);
        objetos.Add(col.gameObject);
        price += col.GetComponent<DragDrop>().price;
        col.GetComponent<DragDrop>().falling = 2;
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        objetos.Remove(other.gameObject);
        other.GetComponent<DragDrop>().falling = 0;
        ventas.Remove(other.GetComponent<DragDrop>().breadtype);
        price -= other.GetComponent<DragDrop>().price;
    }

    public List<int> getVentas()
    {
        return ventas;
    }
}
