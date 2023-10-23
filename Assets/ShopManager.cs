using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    
    // cosas que no se deben mover
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
    [SerializeField] private Image diezmo;
    [SerializeField] private List<Sprite> diezmoEmocions;
    [SerializeField] private List<Button> botones;
    [SerializeField] private Image s;
    [SerializeField] private Button buybutton;
    [SerializeField] private Sprite close;
    [SerializeField] private Sprite open;



    private InteractionSystem actual;
    private Vector3 posicionInicial;
    private Vector3 posicionCentro;
    private Vector3 posicionTeletransporte;
    
    // cosas que se pueden mover pero mejor no
    [Header("movible")] 
    [SerializeField] private float duracionFade = 1.0f;
    [SerializeField] private float velocidad = 1.0f;
    [SerializeField] private int precioDiezmo = 20;
    [SerializeField] private string textoDiezmo;
    [SerializeField] private int moralpoints;
    private int precioaux;
    private int textaux = 0;
    private int personasaux = 0;
    private int diasaux = 0;
    private int angleArrow = 0;
    private bool escribir = false;
    private void Start()
    {
        dinero.text = ""+precioaux;
        // setea las posiciones de donde salen y a donde van los customers, basicamente va de izquierda a derecha y luego de centro a izquierda
        posicionInicial = basePersona.rectTransform.position;
        posicionCentro = new Vector3(Screen.width / 2, posicionInicial.y, 0);
        posicionTeletransporte = new Vector3(Screen.width + 300, posicionInicial.y, 0);
        StartGame();
    }

    //setea los auxiliares como 0
    public void StartGame()
    {
        diasaux = 0;
        personasaux = 0;
        DiasStart();
    }

    // dias start es un loop: diasstart - personas start - todo el juego - personas start - dias start
    // es como un for pero con variables auxiliares
    public void DiasStart()
    {
        if (diasaux < dias.Count)
        {
            if (diasaux != 0)
            {
                StartCoroutine(RealizarFade("Día " + (diasaux+1)));
            }
            else
            {
               Debug.Log(diasaux);
               PersonasStart(); 
            }
            
        }
    }

    // este es el for dentro del for, si hay muchos dias, este es las personas que pasan cada dia
    public void PersonasStart()
    {
        if (personasaux < dias[diasaux].dia.Count)
        {
            Debug.Log(personasaux);
            actual = dias[diasaux].dia[personasaux];
            basePersona.sprite = actual.personas[0];
            if(actual.characterRelevance == 1)
            {
                StartCoroutine(SoundController.Instance.MusicFade(1, 0.3f, 1));
			}
			else if(actual.characterRelevance == 2)
            {
                StartCoroutine(SoundController.Instance.MusicFade(2, 0.3f, 1));
            }

            StartCoroutine(MoverImagenEntrante());
        }
        else
        {
            diasaux++;
            DiasStart();
        }
    }
    
    // una corrutina que hace que la persona entre al medio del mapa
    private IEnumerator MoverImagenEntrante()
    {
        
        float t = 0;
        while (t < 1)
        {
            //COMENZAR CAMINAR
            t += Time.deltaTime * velocidad;
            basePersona.rectTransform.position = Vector3.Lerp(posicionInicial, posicionCentro, t);
            yield return null;
        }
        //DETENER ANIMACIÓN
        textaux = 0;
        foreach (var boton in botones)
        {
            boton.interactable = true;
        }
        InteraccionPersona();
    }

    //este es llamado por un boton invisible dentro de la zona de texto de personas, va mostrando texto por texto
    public void InteraccionPersona()
    {
        textPanel.SetActive(true);
        if (textaux < actual.dialogos.Count)
        {
            escribir = true;
            StartCoroutine(MostrarTextoLentamente(actual.dialogos));
        }
        
        textaux++;
    }
    
    private IEnumerator MostrarTextoLentamente(List<string> dialogos)
    {
        text.text = "";
        botones[1].interactable = false;
        foreach (char letra in dialogos[textaux])
        {
            if(escribir)
			{
                text.text += letra; 
                yield return new WaitForSeconds(0.1f);
			}
        }
        botones[1].interactable = true;
    }
    
    //cuando se da click al cofre finaliza la venta, este es más complicado
    public void FinalizarVenta()
    {
        textaux = 0;
        escribir = false;
        // desactivamos botones

        foreach (var boton in botones)
        {
            boton.interactable = false;
        }
        bool todosEnB = true;
        
        //chequeamos si todos los valores pedidos estan dentro de la bandeja de panes
        foreach (int valorA in actual.panesAceptados)
        {
            bool encontrado = false;
            foreach (int valorB in bandeja.getVentas())
            {
                if (valorA == valorB)
                {
                    bandeja.getVentas().Remove(valorB);
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                todosEnB = false;
                break;
            }
        }

        // si si estan, te muestra un good ending (string) y te aumenta o disminuye grados de moral 
        if (todosEnB)
        {
            text.text = actual.goodEnding;
            basePersona.sprite = actual.personas[1];
            if (actual.nobleza) 
            {
                angleArrow = -15;
                StartCoroutine(arrowAnimation());
            }
            else
            {
                angleArrow = 15;
                StartCoroutine(arrowAnimation());
            }
            // si tiene puestos los impuestos, los suma por un for y los muestra
            if (impuestos.isOn)
            {
                int taxes = 0;
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
                SoundController.Instance.PlaySound(2, 1, gameObject.GetComponent<AudioSource>());
            }
            else
            {
                SoundController.Instance.PlaySound(2, 0, gameObject.GetComponent<AudioSource>());
			}
        }
        else
        {
            // si no los encuentra, bad ending y suma todas las taxes de la bandeja
            text.text = actual.badEnding;
            SoundController.Instance.PlaySound(2, 2, gameObject.GetComponent<AudioSource>());
            basePersona.sprite = actual.personas[2];
            if (actual.nobleza)// hacer algo malo para la nobleza -->
            {
                angleArrow = 15;
                StartCoroutine(arrowAnimation());
            }
            else
            {
                angleArrow = -15;
                StartCoroutine(arrowAnimation());
            }
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
                precio.text = $"{precioaux + taxes}";
                precioaux += taxes;
            }
        }
        
        // si el precio de la bandeja es mayor al pedido, se queda el precio maximo pedido
        if (actual.price < bandeja.price)
        {
            precioaux += actual.price;
        }
        else
        {
            
            precioaux += bandeja.price;
        }

        //se llama al metodo para que la persona se vaya y luego se eliminan todos los objetos de la bandeja
        StartCoroutine(MoverImagenSaliente());
        
        for (int i = 0; i < bandeja.objetos.Count;)
        {
            
            Destroy(bandeja.objetos[0]);
        }
        dinero.text = $"{precioaux}";
    }
    
    // la persona se va xd 
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
    
    //cuando se termina el dia se hace un fade que te muestra que dia es y pasa al siguiente dia 
    private IEnumerator RealizarFade(string text)
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
        obispotext.GetComponent<TMP_Text>().text = text;
        yield return new WaitForSeconds(2.0f);
        
        // como se empieza el dia, se cobra el diezmo que es una actividad extra
        StartCoroutine(CobrarDiezmo(precioDiezmo));
        
        obispotext.gameObject.SetActive(false);

        
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duracionFade;
            panel.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        
    }

    // es como una persona normal pero este te cobra el diezmo sin que puedas hacer nada, basicamente te roba
    public IEnumerator CobrarDiezmo(int costo)
    {
        diezmo.sprite = diezmoEmocions[0];
        float t = 0;
        Debug.LogWarning("Diezmo");
        StartCoroutine(SoundController.Instance.MusicFade(3, 0.3f, 1));
        while (t < 1)
        {
            t += Time.deltaTime * velocidad;
            diezmo.rectTransform.position = Vector3.Lerp(posicionInicial, posicionCentro, t);
            yield return null;
        }
        dias[diasaux].textosDiezmo.Add(textoDiezmo + costo + " francs");
        escribir = true;
        textPanel.SetActive(true);
        for (int i = 0; i < dias[diasaux].textosDiezmo.Count; i++)
        {
            text.text = "";
            botones[1].interactable = false;
            foreach (char letra in dias[diasaux].textosDiezmo[i])
            {
                if(escribir)
                {
                    text.text += letra; 
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(2f);
            botones[1].interactable = true;
            botones[1].interactable = true;
        }
        escribir = false;
        
        yield return new WaitForSeconds(2);

        precioaux -= costo;
        dinero.text = $"{precioaux}";
        if(precioaux <= 0)
		{
            personasaux = 0;
            diasaux = 0;
            text.text = "te cargo el payaso";
		}
        SoundController.Instance.PlaySound(2, 3, gameObject.GetComponent<AudioSource>());

        yield return new WaitForSeconds(3);

        textPanel.SetActive(false);
        if (precioaux <= 0)
        {
            diezmo.sprite = diezmoEmocions[2];
            RealizarFade("Perdiste");
        }
        else
        {
            diezmo.sprite = diezmoEmocions[1];
        }
        
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * velocidad;
            diezmo.rectTransform.position = Vector3.Lerp(posicionCentro, posicionTeletransporte, t);
            yield return null;
        }
        // se llama al inicio del loop de nuevo
        
        personasaux = 0;
        PersonasStart();
    }
    
    //calcula a tiempo real las cosas que estan dentro de la bolsa para que se actualizen los precios y los sprites del cofre
    // es como el de dinero de arriba
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

        

        if (Int32.Parse(precio.text) == 0)
        {
            vender.text = "Don't sell it";
            buybutton.GetComponent<Image>().sprite = close;
        }
        else
        {
            vender.text = "Sell it";
            buybutton.GetComponent<Image>().sprite = open;
        }

        
    }
	public void rotateArrow()
    {
        s.rectTransform.Rotate(new Vector3(0, 0, angleArrow));
	}

    private IEnumerator arrowAnimation()
    {
        GameObject parentS = s.transform.parent.transform.parent.gameObject;

        yield return new WaitForSeconds(1);

        parentS.GetComponent<Animator>().SetTrigger("Move");
        SoundController.Instance.PlaySound(0, 2, parentS.GetComponent<AudioSource>());
        yield return null;
    }
}
