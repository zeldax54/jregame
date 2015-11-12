using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.UI;

public class MecanicaController : MonoBehaviour
{

    private ControladorPoemas _controladorPoema;
    public TextMesh Intentos;
    public int IntentosValue = 3;
    public TextMesh Ayuda;
    private int _ayuda = 1;
    public TextMesh Diamantes;
    private int _diamantes = 5;
    public TextMesh Orgullo;
    private int _orgullo = 3;
    private ModalPanel _modalPanel; //Objeto de tipo del script ModalPanel
    public Text TextoPema; //Objeto del panel para mostrar el poema cuando lo resuelva
    public Button Aceptar;
    public Button Cancelar;
    private bool _incdiamantel = true;
    private bool _incorgullo=true;
    private bool lastPoema;
    private Poema PoemaActual;
    private void Awake()
    {
        _modalPanel = ModalPanel.Instance(); // Le asigno una instancia del script Modal Panel
        _controladorPoema = GetComponent<ControladorPoemas>();
        UpdateTextMesh(Intentos, IntentosValue.ToString());
        UpdateTextMesh(Ayuda, _ayuda.ToString());
        UpdateTextMesh(Diamantes, _diamantes.ToString());
        UpdateTextMesh(Orgullo, _orgullo.ToString());
    }

    public void UpdateTextMesh(TextMesh t, string n)
    {
        t.text = n;
    }

    public void Rendirse() //Chequea el estado de las Lineas del poema
    {
        if (_controladorPoema._poemaactual < 2)
        {
            _controladorPoema._poemaactual++;
            _modalPanel.Rendirse("Desea Rendirse en el Poema?", TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
        }
        else
        {
            _modalPanel.ShowPoema("Este es el Ultimo Poema.Desea ir al <color=green>Menu Principal?</color> del Juego?", TextoPema, Aceptar, Cancelar, Salir);
        }
    }

    private void Salir()
    {
        Application.LoadLevel("MenuMain");
    }

    public void CheckAndSet()
    {
        //Obtengo las lineas que estan en pantalla en ese momento
        List<TextMesh> lineasenInterfaz =
            FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a => a.name).ToList();

        if (lineasenInterfaz.Select(l => l.GetComponent<ManejadorLinea>()).Any(m => m.GetActualPuesta() == ""))
        {
            _modalPanel.ShowPoema("Hay espacios sin completar.", TextoPema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
            return; //No estan completas retorno falso para q el jugador termine de completar el poema
        }
        // Si todas estan completas Obtengo el poema actual
        Poema poemaActual = _controladorPoema.Poemas[_controladorPoema._poemaactual];
        PoemaActual = poemaActual;
        //Obtengo las palabras correctas del poema las ordeno por su posicion para asegurar el orden
        List<Palabra> lineasPema = poemaActual.Palabras.OrderBy(a => a.posicion).ToList();
        bool bandera = true; //para controlar si se resolvieron todos los poemas
        string poema = ""; //Guardar texto del poema para mostrarlo
        for (int i = 0; i < lineasenInterfaz.Count; i++)
        {
            ManejadorLinea manejador = lineasenInterfaz[i].GetComponent<ManejadorLinea>();
            //Obtengo el script manejador para cada linea
            poema += manejador.PintarPalabra(lineasenInterfaz[i].text, "#008000ff") + "\n";
            if (manejador.ContienePalabraDeResp())
            {
                if (manejador.GetActualPuesta() == poemaActual.GetPalabradeLina(lineasPema, i).palabra)
                    //Si la palabra que tiene puesta coincide con la que le toca en el poema
                {
                    manejador.SetCorrectWord("#008000ff");
                    //Pongo la palabra en Coorecta asignandole un nuevo color, verde en este caso
                }
                else
                {
                    bandera = false;
                    IntentosValue--;
                    _incorgullo = false;
                    SetIntentos(IntentosValue);
                    if (IntentosValue == 0)
                        //Si ya ha agotado sus intentos significa que fallo el poema hay que mostrar el otro opema y resetear los intentos
                    {

                        if (_controladorPoema._poemaactual <= 2)
                        {
                            if (lastPoema == false)
                            {
                                IntentosValue = 3;
                                SetIntentos(3);
                                CheckAYuda(); //Si utilizo la ayuda la reseteo para el siguiente poema
                                DecDiamantes();//Le quito un diamante fallo el poema
                            }
                           
                            if (_controladorPoema._poemaactual == 2)
                            {
                                lastPoema = true;
                                _modalPanel.ShowPoema("<color=red>Poema Fallido</color>\n" + GetCorrectPoema(),
                                    TextoPema, Aceptar, Cancelar,_modalPanel.CerrarPanel);
                            }
                            if (_controladorPoema._poemaactual <2)
                            {
                                _controladorPoema._poemaactual++;
                                _modalPanel.ShowPoema("<color=red>Poema Fallido</color>\n" + GetCorrectPoema(), TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
                            }
                        }
                     
                    }
                }
            }

        }
        if (bandera)
        {
            if (_incdiamantel)
            {
                if (_controladorPoema._poemaactual == 2)
                    _incdiamantel = false;
                else
                {
                    _diamantes++;
                    UpdateTextMesh(Diamantes, _diamantes.ToString());
                }

            }

            if (_controladorPoema._poemaactual < 2)
            {


                IntentosValue = 3;
                SetIntentos(IntentosValue);
                _controladorPoema._poemaactual++;
                CheckAYuda();
                _modalPanel.ShowPoema("<color=blue>Correcto</color>\n"+poema, TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
            }
            else
            {
                if (_incorgullo)
                {
                    IncOrgullo();
                    _incorgullo = false;
                }
                IntentosValue = 0;
                SetIntentos(IntentosValue);
                _modalPanel.ShowPoema("<color=blue>Correcto</color>\n" + poema, TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
            }

        }
    }


  

    private void SetIntentos(int intentos)
    {
        IntentosValue = intentos;
        UpdateTextMesh(Intentos, IntentosValue.ToString());
    }

    private void CheckAYuda()
    {
        if (_ayuda == 0)
        {
            _ayuda++;
            UpdateTextMesh(Ayuda, _ayuda.ToString());
        }

    }

    private void DecDiamantes()
    {
        _diamantes--;
        UpdateTextMesh(Diamantes,_diamantes.ToString());
    }

    private void IncOrgullo()
    {
        _orgullo++;
        UpdateTextMesh(Orgullo,_orgullo.ToString());
    }

    public void RequestHelp()
    {
        if(_ayuda>0)
         _modalPanel.Rendirse("Una ayuda cuesta 1 Dianante", TextoPema, Aceptar, Cancelar, Help);
        else
        {
            _modalPanel.ShowPoema("No quedan Ayudas para este poema", TextoPema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
        }
    }

    private void Help()
    {
        Poema poemaActual = _controladorPoema.Poemas[_controladorPoema._poemaactual];
       
        List<TextMesh> lineas = FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a=>a.name).ToList();
        int cont = 0;//Para saber en que linea estoy
        foreach (var l in lineas)
        {
            ManejadorLinea m = l.GetComponent<ManejadorLinea>();
            if (m.LineaDisponible() ||m.GetPalabraInsegura())
            {
                foreach (var p in poemaActual.Palabras)//Recorro la lista de palabras del poema donde estoy
                {
                    if (p.posicion == cont) //Si la popsicion de la palabra es de la linea donde estoy
                    {
                        GameObject o;
                        if (m.GetActualPuesta() != "")//Si hay una palabra puesta la cambio o la marco como correcta
                        {
                            o = FindObjectsOfType<TextMesh>().First(a => a.text ==m.GetActualPuesta()).gameObject;//Aguardar la que esta puesta antes de cambiarla
                            Debug.Log(o.GetComponent<TextMesh>().text);
                            m.FindandReplaceRedWord(p.palabra);//Cambiando
                            m.SetCorrectWord("#008000ff");//marcando correcta
                            //Quitar la palara correcta de la lista y bajar la que habia
                            GameObject correcta = FindObjectsOfType<TextMesh>().First(a => a.text == p.palabra).gameObject;
                            ControlaPalabras cp = o.GetComponent<ControlaPalabras>();
                            cp.SubirPalabra(correcta);
                            cp.BajarPalabra(o.GetComponent<TextMesh>().text);//Bajando la que estaba puesta
                        }
                        else//Si no hay la pongo
                        {
                            m.PutText(p.palabra, m.FindFirst_());
                            m.Remove_();
                            m.SetCorrectWord("#008000ff");
                            //Desactivando la palabra para que no la pueda seleccionar mas
                             o = FindObjectsOfType<TextMesh>().First(a => a.text == p.palabra).gameObject;
                             ControlaPalabras cp = o.GetComponent<ControlaPalabras>();
                             cp.SubirPalabra(o);
                        }
                       
                        _ayuda--;
                        UpdateTextMesh(Ayuda, _ayuda.ToString());
                        _diamantes--;
                        UpdateTextMesh(Diamantes, _diamantes.ToString());
                       
                        break;
                    }
                }
                break;
            }
            cont++;
        }
    }

    private string GetCorrectPoema()//Tomar el poema y conformarlo con las palabras correctas
    {
        
        string final = "";

        for (int i = 0; i < PoemaActual.TextoPoemaLineas.Count; i++)
        {
            string linea = PoemaActual.TextoPoemaLineas[i];
            if (linea.Contains("*"))
            {
                foreach (var pals in PoemaActual.Palabras)
                {
                    if (pals.posicion == i)
                    {
                        int pos = FindAster(linea);
                        final += linea.Insert(pos, "<color=green>" + pals.palabra + "</color>\n");
                        break;
                    }
                }
            }
            else
            {
                final += linea + "\n";
            }
        }
        return final;
    }

    int FindAster(string t)
    {
            for (int i = 0; i < t.Length; i++)
        {
            if (t[i] == '*')
            {
                return i;
            }
        }
        return -1;
    }
 
}


