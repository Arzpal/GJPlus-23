using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogo
{
    public string dialogo;
    public Sprite reaccion;

    public string GetDialogo()
    {
        return dialogo;
    }

    public Sprite GetReaccion()
    {
        return reaccion;
    }

    public void SetPrite(Sprite a)
    {
        reaccion = a;
    }
}
