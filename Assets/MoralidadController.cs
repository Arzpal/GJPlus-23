using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoralidadController : MonoBehaviour
{
    [SerializeField] private Slider s;
    public void ChangeValue(int value)
    {
        s.value += value;
    }
}
