using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class InteractionSystem
{
    public List<string> dialogos;
    public List<Dialogo> dialogos2;
    public int price;
    public List<Sprite> personas;
    public List<int> panesAceptados;
    public string goodEnding;
    public string badEnding;
    public bool nobleza = false;
    public int characterRelevance = 0;
    
    public int getPrice()
    {
        int price = 0;
        for (int i = 0; i < panesAceptados.Count; i++)
        {
            switch (panesAceptados[i])
            {
                case 0:
                    price += 5;
                    break;
                case 1:
                    price += 3;
                    break;
                case 2:
                    price += 1;
                    break;
            }
        }
        return price;
    }
    
    public int getPriceWithTaxes()
    {
        int price = 0;
        for (int i = 0; i < panesAceptados.Count; i++)
        {
            switch (panesAceptados[i])
            {
                case 0:
                    price += 3;
                    break;
                case 1:
                    price += 2;
                    break;
                case 2:
                    price += 1;
                    break;
            }
        }
        return getPrice() + price;
    }
}
