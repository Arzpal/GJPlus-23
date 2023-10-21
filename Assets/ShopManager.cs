using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    
    
    [Header("no movible")] 
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image basePersona;
    [SerializeField] private TMP_Text precio;
    [SerializeField] private TMP_Text dinero;
    [SerializeField] private TMP_Text vender;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private Toggle impuestos;
    [SerializeField] private ItemSlot bandeja;
    
    [SerializeField] private List<Dias> dias;
    
    public float velocidad = 1.0f;
    public float teletransporteX = 100.0f; // Posición X para el teletransporte
    private InteractionSystem actual;
    private Vector3 posicionInicial;
    private Vector3 posicionCentro;
    private Vector3 posicionTeletransporte;
    private int precioaux;
    [SerializeField] private int textaux = 0;
    private void Start()
    {
        posicionInicial = basePersona.rectTransform.position;
        posicionCentro = new Vector3(Screen.width / 2, posicionInicial.y, 0);
        posicionTeletransporte = new Vector3(Screen.width+100, posicionInicial.y, 0);
        

        StartGame();
    }

    public void StartGame()
    {
        for (int i = 0; i < dias.Count; i++)
        {
            
            for (int j = 0; j < dias[i].dia.Count; j++)
            {
                Debug.Log("wa intentar el " + i);
                actual = dias[i].dia[j];
                //basePersona = actual.persona;
                StartCoroutine(MoverImagenEntrante());
            }
            
        }
    }
    private IEnumerator MoverImagenEntrante()
    {
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * velocidad;
            basePersona.rectTransform.position = Vector3.Lerp(posicionInicial, posicionCentro, t);
            yield return null;
        }

        textaux = 0;
        InteraccionPersona();
    }

    public void InteraccionPersona()
    {
        textPanel.SetActive(true);
        if (textaux < actual.dialogos.Count)
        {
            text.text = actual.dialogos[textaux];
        }

        textaux++;
    }

    public void FinalizarVenta()
    {
        textaux = 0;
        Debug.Log(actual.panesAceptados.OrderBy(x => x).SequenceEqual(bandeja.getVentas().OrderBy(x => x)));
        if (actual.panesAceptados.OrderBy(x => x).SequenceEqual(bandeja.getVentas().OrderBy(x => x)))
        {
            //estoy entrando
            text.text = actual.goodEnding;
        }
        else
        {
            text.text = actual.badEnding;
        }
        
        StartCoroutine(MoverImagenSaliente());
        
        for (int i = 0; i < bandeja.objetos.Count;)
        {
            
            Destroy(bandeja.objetos[0]);
        }
        dinero.text += precioaux;
    }
    private IEnumerator MoverImagenSaliente()
    {
        yield return new WaitForSeconds(4);
        textPanel.SetActive(false);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * velocidad;
            basePersona.rectTransform.position = Vector3.Lerp(posicionCentro, posicionTeletransporte, t);
            yield return null;
        }

        basePersona.rectTransform.position = posicionInicial;
        
    }

    private void Update()
    {
        
        if (actual.price < bandeja.price)
        {
            precio.text = ""+actual.price;
            precioaux = actual.price;
        }
        else
        {
            precio.text = ""+bandeja.price;
            precioaux = bandeja.price;
        }

        if (impuestos.isOn)
        {
            precio.text = "" + (precioaux + 3);
            precioaux += 3;
        }

        if (precioaux == 0)
        {
            vender.text = "No vender";
        }
        else
        {
            vender.text = "Vender";
        }
    }
}
