using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class InteractionSystem
{
    public List<string> dialogos;
    public int price;
    public List<Sprite> personas;
    public List<int> panesAceptados;
    public string goodEnding;
    public string badEnding;
    public bool nobleza = false;
}
