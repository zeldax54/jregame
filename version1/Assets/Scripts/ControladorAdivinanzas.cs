using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControladorAdivinanzas : MonoBehaviour
{


    private readonly Adivinanza _adivinanza = new Adivinanza();
        //Objeto de la clase adivinanza que contiene la lista de adivinanzas y el metodo buscar adivinanza

    public Text AdivinzadasContainer; //Texto de la Ui que mostrara las adivinanzas
    //Botones donde escribire las posibles respuestas
    public Button Posible1;
    public Button Posible2;
    public Button Posible3;
    //Variables de control
    private int _adivinanzaactual=1;//Sabes en q imagen estoy

    private int _fallos;
    private int _diamantes=3;
    private int _orgullo=3;
    private int _rendidas = 0;
    //TextMesh de los avisos
    public TextMesh FallosTextMesh;
    public TextMesh DiamantesTextMesh;
    public TextMesh OrgullosTextMesh;
    public TextMesh GameOver;

    private string _respuestactual = ""; //Guardo la respuesta de la adivinaza actual


    private void Awake()
    {
        UpdateTextMesh(DiamantesTextMesh, _diamantes.ToString());
        UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
        
        _adivinanza.InicializarLista(); //Inicio la lista de adivinanzas


    }

    //Metodo para actualizar los textMeshs
    private void UpdateTextMesh(TextMesh t, string valor)
    {
        t.text = valor;
    }


    // Use this for initialization
    private void Start()
    {
        //Empieso por Agregar la primera adivinaza a mis controles

        Adivinanza a = _adivinanza.Getadivinazas("1");
        AdivinzadasContainer.text = a.adivinanza;
        //Asigno los botones a las respuestas posibles y a la respuesta real.
        Colcar(new List<Button>(){Posible1,Posible2,Posible3}, a);
        _respuestactual = a.respuesta;

        //Asigno los eventos de los botones de las respuestas desde el inspector porque recibo parametros en la funion Comprobar
        //Por codigo es posible hacerlo tambien pero no pude investigarlo 



    }
    //Metodo para chequar si donde doy click el texto es el de la respuesta
  

    public void Comprobar(Button x)
    {
        if (_adivinanzaactual <= 3)//SI no ha completado las adivinanzas
        {
            if (x.GetComponentInChildren<Text>().text == _respuestactual)    //Si acierta
            {
                _orgullo++;
                UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
                _adivinanzaactual++;

                NextAdivinanza(false);
            }
            //Si falla
            else
            {
                _fallos++;
                UpdateTextMesh(FallosTextMesh, _fallos.ToString());
                _adivinanzaactual++;
                NextAdivinanza(false);
            }
        }
    
        if (_adivinanzaactual == 4)// Si esta en la ultima
        {
            if (_rendidas == 0 && _fallos == 0)
            {
                _orgullo++;
                UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
            }
        }
    }

    public void NextAdivinanza(bool rindiendose)
    {
       
        if (_adivinanzaactual >= 4)
        {
            GameOver.text = "Todo Completado";
        }
        else
        {
            if (rindiendose)
            {
                _orgullo--;
                _rendidas++;
                UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
                _adivinanzaactual++;
                if (_adivinanzaactual <=3)//Si hay proxima adivinanza
                  Pasar();
            }


            else
            {
                Pasar();

            }
        }
    }

    private void Pasar()
    {
        Adivinanza a = _adivinanza.Getadivinazas(_adivinanzaactual.ToString());
        List<Button> lista = new List<Button>() { Posible1, Posible2, Posible3 };
        AdivinzadasContainer.text = a.adivinanza;
        _respuestactual = a.respuesta;
        Colcar(lista, a);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void Colcar(List<Button> butos,Adivinanza a) //Esto es para q la respuesta salga en un boton al azar
    {
        string[] respuestas = a.falsasrespuestas;
        int pos = Random.Range(0, 2);
        Button b = butos[pos];
        b.GetComponentInChildren<Text>().text = a.respuesta;

        foreach (var boton in butos)
        {
            if (boton != b)
            {
               boton.GetComponentInChildren<Text>().text=respuestas[0];
                respuestas[0] = respuestas[1];
                respuestas[1] = null;
            }
        }


    }







}
