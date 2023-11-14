using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
[System.Serializable]
public class BreadInventory : MonoBehaviour
{
    [SerializeField] private GameObject canva;
    [SerializeField] private GameObject canvaPadre;
    [SerializeField] private RectTransform breadInv;
    [SerializeField] private float moveAmount = 400f;
    [SerializeField] private TMP_Text text;
    private bool open = false;

    [SerializeField] private List<RectTransform> positions;
    [SerializeField] private List<Image> prefabs;
    [SerializeField] private List<int> quantitys;
    [SerializeField] private List<Image> actualImages;
    [SerializeField] private AudioSource source;
    public int crustSoundID = 0;
    public int breadFallSoundID = 0;
    public int thisbread;

    private void Start()
    {
        
    }

    public void Spawn()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            Image nuevoObjetoHijo = Instantiate(prefabs[i], canva.transform, true);
            float scaleReadjust = 2 / canva.transform.localScale.x;
            nuevoObjetoHijo.transform.localScale = Vector3.one * scaleReadjust;
            nuevoObjetoHijo.gameObject.GetComponent<DragDrop>().breadInv = gameObject.GetComponent<BreadInventory>();
            actualImages[i] = nuevoObjetoHijo;
            nuevoObjetoHijo.rectTransform.position = positions[i].position;
        }
    }

    public void ChangeBreads(List<int> a)
    {
        quantitys = a;
        for (int i = 0; i < quantitys.Count; i++)
        {
            positions[i].GetComponentInChildren<TMP_Text>().text = "x " + (quantitys[i]+1);
        }
        Spawn();
    }
    public void movePanel()
    {
        if (!open) { MoverPanelIzquierda(); }
        else { MoverPanelDerecha(); }
    }
    public void MoverPanelIzquierda()
    {
        SoundController.Instance.PlaySound(0,0, source);
        breadInv.GetComponent<Animator>().SetBool("isOut", true);
        Vector2 currentPosition = breadInv.anchoredPosition;
        float newX = currentPosition.x - moveAmount;
        breadInv.anchoredPosition = new Vector2(newX, currentPosition.y);
        text.text = ">";
        open = true;
    }
    
    public void MoverPanelDerecha()
    {
        SoundController.Instance.PlaySound(0, 1, source);
        breadInv.GetComponent<Animator>().SetBool("isOut", false);
        Vector2 currentPosition = breadInv.anchoredPosition;
        float newX = currentPosition.x + moveAmount;
        breadInv.anchoredPosition = new Vector2(newX, currentPosition.y);
        text.text = "<";
        open = false;
    }

    public void GrabbedBread(int type, Image obj)
    {
        if (obj == actualImages[type])
        {
            actualImages[type].transform.SetParent(canvaPadre.transform);
            quantitys[type] --;
            positions[type].GetComponentInChildren<TMP_Text>().text = "x " + (quantitys[type]+1);
            if (quantitys[type] < 0) return;
            Image nuevoObjetoHijo = Instantiate(prefabs[type], canva.transform, true);
            float scaleReadjust = 2 / canva.transform.localScale.x;
            nuevoObjetoHijo.transform.localScale = Vector3.one * scaleReadjust;
            nuevoObjetoHijo.gameObject.GetComponent<DragDrop>().breadInv = gameObject.GetComponent<BreadInventory>();
            actualImages[type] = nuevoObjetoHijo;
            nuevoObjetoHijo.rectTransform.position = positions[type].position;
            
        }
        
    }
    public int selectRandomNumber(int min, int max, bool isCrust)
    {
        int id = isCrust ? crustSoundID : breadFallSoundID;
        int randomID = id;
        while (randomID == id)
        {
            randomID = Random.Range(min, max + 1);
        }
        if (isCrust) crustSoundID = randomID;
        else breadFallSoundID = randomID;
        return randomID;
    }
}
