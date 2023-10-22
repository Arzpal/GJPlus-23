using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public int thisbread;
    

    public void movePanel()
    {
        if (!open) { MoverPanelIzquierda(); }
        else { MoverPanelDerecha(); }
    }
    public void MoverPanelIzquierda()
    {
        SoundController.Instance.PlaySound(0,0, gameObject.GetComponent<AudioSource>());
        breadInv.GetComponent<Animator>().SetBool("isOut", true);
        Vector2 currentPosition = breadInv.anchoredPosition;
        float newX = currentPosition.x - moveAmount;
        breadInv.anchoredPosition = new Vector2(newX, currentPosition.y);
        text.text = ">";
        open = true;
    }
    
    public void MoverPanelDerecha()
    {
        SoundController.Instance.PlaySound(0, 1, gameObject.GetComponent<AudioSource>());
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
            Image nuevoObjetoHijo = Instantiate(prefabs[type], canva.transform, true);
            float scaleReadjust = 2 / canva.transform.localScale.x;
            nuevoObjetoHijo.transform.localScale = Vector3.one * scaleReadjust;
            actualImages[type] = nuevoObjetoHijo;
            nuevoObjetoHijo.rectTransform.position = positions[type].position;
            quantitys[type] --;
            positions[type].GetComponentInChildren<TMP_Text>().text = "x " + quantitys[type];
        }
        
    }
}
