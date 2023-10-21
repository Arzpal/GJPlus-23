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
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private GameObject obispotext;
    [SerializeField] private List<Dias> dias;
    
    
    
    private InteractionSystem actual;
    private Vector3 posicionInicial;
    private Vector3 posicionCentro;
    private Vector3 posicionTeletransporte;
    [Header("movible")] 
    [SerializeField] private float duracionFade = 1.0f;
    [SerializeField] private float velocidad = 1.0f;
    [SerializeField] private float teletransporteX = 100.0f; // Posición X para el teletransporte
    
    private int precioaux;
    private int textaux = 0;
    private int personasaux = 0;
    private int diasaux = 0;
    private void Start()
    {
        dinero.text = ""+precioaux;
        posicionInicial = basePersona.rectTransform.position;
        posicionCentro = new Vector3(Screen.width / 2, posicionInicial.y, 0);
        posicionTeletransporte = new Vector3(Screen.width+100, posicionInicial.y, 0);
        StartGame();
    }

    public void StartGame()
    {
        diasaux = 0;
        personasaux = 0;
        DiasStart();
    }

    public void DiasStart()
    {
        if (diasaux < dias.Count)
        {
            if (diasaux != 0)
            {
                StartCoroutine(RealizarFade());
            }
            else
            {
               Debug.Log(diasaux);
               PersonasStart(); 
            }
            
        }
    }

    public void PersonasStart()
    {
        if (personasaux < dias[diasaux].dia.Count)
        {
            Debug.Log(personasaux);
            actual = dias[diasaux].dia[personasaux];
            StartCoroutine(MoverImagenEntrante());
        }
        else
        {
            diasaux++;
            DiasStart();
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
            
            text.text = actual.goodEnding;
        }
        else
        {
            text.text = actual.badEnding;
        }
        
        if (actual.price < bandeja.price)
        {
            precioaux += actual.price;
        }
        else
        {
            
            precioaux += bandeja.price;
        }

        if (impuestos.isOn)
        {
            int taxes=0;
            for (int i = 0; i < actual.panesAceptados.Count; i++)
            {
                switch (actual.panesAceptados[i])
                {
                    case 0:
                        taxes += 3;
                        break;
                    case 1:
                        taxes += 2;
                        break;
                    case 2:
                        taxes += 1;
                        break;
                }
            }
            precio.text = $"{precioaux + taxes}";
            precioaux += taxes;
        }
        StartCoroutine(MoverImagenSaliente());
        
        for (int i = 0; i < bandeja.objetos.Count;)
        {
            
            Destroy(bandeja.objetos[0]);
        }
        dinero.text = $"{precioaux}";
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
        personasaux++;
        PersonasStart();
    }
    private IEnumerator RealizarFade()
    {
        panel.alpha = 0;
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duracionFade;
            panel.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        
        obispotext.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        precioaux -= 20;
        dinero.text = $"{precioaux}";
        
        obispotext.gameObject.SetActive(false);

        
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duracionFade;
            panel.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        personasaux = 0;
        PersonasStart();
    }
    
    private void Update()
    {
        
        if (actual.price < bandeja.price)
        {
            precio.text = $"{actual.price}";
            if (impuestos.isOn)
            {
                int taxes=0;
                for (int i = 0; i < actual.panesAceptados.Count; i++)
                {
                    switch (actual.panesAceptados[i])
                    {
                        case 0:
                            taxes += 3;
                            break;
                        case 1:
                            taxes += 2;
                            break;
                        case 2:
                            taxes += 1;
                            break;
                    }
                }
                precio.text = $"{Int32.Parse(precio.text) + taxes}";
            }
        }
        else
        {
            precio.text = $"{bandeja.price}";
            if (impuestos.isOn)
            {
                int taxes=0;
                for (int i = 0; i < bandeja.getVentas().Count; i++)
                {
                    switch (bandeja.getVentas()[i])
                    {
                        case 0:
                            taxes += 3;
                            break;
                        case 1:
                            taxes += 2;
                            break;
                        case 2:
                            taxes += 1;
                            break;
                    }
                }
                precio.text = $"{Int32.Parse(precio.text) + taxes}";
            }
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
