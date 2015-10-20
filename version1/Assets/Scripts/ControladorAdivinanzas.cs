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
    public TextMesh AdivinanzaActualTextMesh;

    private string _respuestactual = ""; //Guardo la respuesta de la adivinaza actual
    //Parte del panel para las notificaiones
    private ModalPanel _modalPanel;//Objeto de tipo del script ModalPanel
    public Button aceptarButton;//Boton aceptar del panel
    public Button cancelarButton;//Boton cancelar del panel
    public Button helpButton;
    public bool ayudaActual;// Para saber si ya ha usado su ayuda la adivinanza actual
  
    private void Awake()
    {
        UpdateTextMesh(DiamantesTextMesh, _diamantes.ToString());
        UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
        
        _adivinanza.InicializarLista(); //Inicio la lista de adivinanzas

        _modalPanel = ModalPanel.Instance();// Le asigno una instancia del script Modal Panel
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
        UpdateTextMesh(AdivinanzaActualTextMesh,a.numero+"/"+_adivinanza.ContAdivinanzas());//Actualiza TextMesh para mostrar en q adivinanza estoy

        //Asigno los eventos de los botones de las respuestas desde el inspector porque recibo parametros en la funion Comprobar
        //Por codigo es posible hacerlo tambien pero no pude investigarlo 
    }
    //Metodo para chequar si donde doy click el texto es el de la respuesta
  

    public void Comprobar(Button x)
    {
        if (_adivinanzaactual <= _adivinanza.ContAdivinanzas() && !_modalPanel.IsActivo())//SI no ha completado las adivinanzas
        {
            if (x.GetComponentInChildren<Text>().text == _respuestactual)//Si acierta
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

        if (_adivinanzaactual == _adivinanza.ContAdivinanzas())// Si esta en la ultima
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
        ayudaActual = false;
        if (_adivinanzaactual > _adivinanza.ContAdivinanzas())
        {
            if (_modalPanel.IsActivo())
                _modalPanel.CerrarPanel();//Cierro el panel si esta abierto
            GameOver.text = "Todo Completado";
        }
        else
        {
            if (rindiendose)
            {
                if(_modalPanel.IsActivo())
                    _modalPanel.CerrarPanel();//Cierro el panel si esta abierto
                _orgullo--;
                _rendidas++;
                UpdateTextMesh(OrgullosTextMesh, _orgullo.ToString());
                _adivinanzaactual++;
                if (_adivinanzaactual <=_adivinanza.ContAdivinanzas())//Si hay proxima adivinanza
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
        UpdateTextMesh(AdivinanzaActualTextMesh, a.numero + "/" + _adivinanza.ContAdivinanzas());
        List<Button> lista = new List<Button>() { Posible1, Posible2, Posible3 };
        AdivinzadasContainer.text = a.adivinanza;
        _respuestactual = a.respuesta;
        Colcar(lista, a);
    }

    
    private void Colcar(List<Button> butos,Adivinanza a) //Esto es para q la respuesta salga en un boton al azar
    {
        foreach (var boton in butos)//Los activo por si alguno fue desactivado en la ayuda
        {
            boton.gameObject.SetActive(true);
        }
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


    //Metodo para llamar al Panel de notofocaciones
    //Estos eventos los asigno desde el inspector es otra forma y al final es mas facil
    //Este iria en el boton de rendirse

    public void Choise()
    {
        if(_adivinanzaactual<=_adivinanza.ContAdivinanzas())
          _modalPanel.Elejir("Rendirse en esta adivinanza?",
              null, aceptarButton, cancelarButton, false,helpButton);//Asignar los objetos al choise con el Poner Pista que es el evento 
    }

    public void ChoiceHelp()
    {
        if (ayudaActual)
        {
            _modalPanel.MostrarMsg("Ya ha usado la ayuda en esta Adivinanza."// Le decimos que ya ha usado su pista
                ,aceptarButton, cancelarButton,helpButton);
        }
        else
        {
            if (_diamantes <= 0)
            {
                _modalPanel.MostrarMsg("No hay diamantes disponibles."// Le decimos que ya ha usado su pista
                , aceptarButton, cancelarButton, helpButton);
            }
            else
            {
                if (_adivinanzaactual <= _adivinanza.ContAdivinanzas())
                {
                    _modalPanel.ChoiseHelp("Una ayuda cuesta un diamante.Continuar?",
                        DoHelp, aceptarButton, cancelarButton, helpButton);//Asignar los objetos al choise con el Poner Pista que es el evento 
                }
            }
        }
    }

    public void DoHelp()
    {
        
        List<Button> botones = new List<Button>{Posible1, Posible2, Posible3};
        Adivinanza a = _adivinanza.Getadivinazas(_adivinanzaactual.ToString());
        Button botonresp=null;
        foreach (var b in botones)
        {
            if (b.GetComponentInChildren<Text>().text == a.respuesta)
            {
                botonresp = b;//Guardo el boton re la respuesta
            }
        }
        foreach (var b in botones)//Recorro otra vez la lista para eliminar el primero que no sea la respuesta
        {
            if (b != botonresp)
            {
                b.gameObject.SetActive(false);
                break;
            }
        }
        _diamantes--;
        ayudaActual = true;
        UpdateTextMesh(DiamantesTextMesh,_diamantes.ToString());
        _modalPanel.CerrarPanel();
    }


}
