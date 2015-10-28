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
    private int _intentos = 3;
    private ModalPanel _modalPanel;//Objeto de tipo del script ModalPanel
    public Text TextoPema;//Objeto del panel para mostrar el poema cuando lo resuelva
    public Button Aceptar;
    public Button Cancelar;

    void Awake()
    {
        _modalPanel = ModalPanel.Instance();// Le asigno una instancia del script Modal Panel
        _controladorPoema = GetComponent<ControladorPoemas>();
        UpdateTextMesh(Intentos, _intentos.ToString());
    }

    private void UpdateTextMesh(TextMesh t, string n)
    {
        t.text = n;
    }

    public void Rendirse()//Chequea el estado de las Lineas del poema
    {
        if (_controladorPoema._poemaactual < 2)
        {
            _controladorPoema._poemaactual++;
            _modalPanel.Rendirse("Desea Rendirse en el Poema?", TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
        }
        else
        {
            _modalPanel.ShowPoema("Este es el Ultimo Poema", TextoPema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
        }
        
    }

    public void CheckAndSet()
    {
        //Obtengo las lineas que estan en pantalla en ese momento
        List<TextMesh> lineasenInterfaz =
            FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a => a.name).ToList();

        if (lineasenInterfaz.Select(l => l.GetComponent<ManejadorLinea>()).Any(m => m.GetActualPuesta() == ""))
        {
            Debug.Log("<color=blue>FALSO</color>");
            return;//No estan completas retorno falso para q el jugador termine de completar el poema
        }
        // Si todas estan completas Obtengo el poema actual
        Poema poemaActual = _controladorPoema.Poemas[_controladorPoema._poemaactual];
        //Obtengo las palabras correctas del poema las ordeno por su posicion para asegurar el orden
        List<Palabra> lineasPema = poemaActual.Palabras.OrderBy(a=>a.posicion).ToList();
        bool bandera = true;//para controlar si se resolvieron todos los poemas
        string poema = "";//Guardar texto del poema para mostrarlo
        for (int i = 0; i < lineasenInterfaz.Count; i++)
        {
            poema += lineasenInterfaz[i].text + "\n";
            ManejadorLinea manejador = lineasenInterfaz[i].GetComponent<ManejadorLinea>();//Obtengo el script manejador para cada linea
            if (manejador.ContienePalabraDeResp()){
                if (manejador.GetActualPuesta() == poemaActual.GetPalabradeLina(lineasPema, i).palabra)//Si la palabra que tiene puesta coincide con la que le toca en el poema
                {
                    manejador.SetCorrectWord("#008000ff"); //Pongo la palabra en Coorecta asignandole un nuevo color, verde en este caso
                }
                else
                {
                    bandera = false;
                    _intentos--;
                    SetIntentos(_intentos);
                    if (_intentos == 0)//Si ya ha agotado sus intentos significa que fallo el poema hay que mostrar el otro opema y resetear los intentos
                    {
                       
                        if (_controladorPoema._poemaactual < 2)
                        {
                            _intentos = 3;
                            SetIntentos(3);
                            _controladorPoema._poemaactual++;
                            _modalPanel.ShowPoema("Tres errores.Poema Fallido.", TextoPema, Aceptar, Cancelar,_controladorPoema.SetUI);
                           
                        }
                    }
                }
            }
           
        }
        if (bandera)
        {
            if (_controladorPoema._poemaactual < 2)
            {
                _intentos = 3;
                SetIntentos(_intentos);
                _controladorPoema._poemaactual++;
                _modalPanel.ShowPoema(poema, TextoPema, Aceptar, Cancelar, _controladorPoema.SetUI);
            }
            else
            {
                _intentos = 0;
                SetIntentos(_intentos);
                _modalPanel.ShowPoema(poema, TextoPema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
            }
        
        }
    }

    private void SetIntentos(int intentos)
    {
        _intentos = intentos;
        UpdateTextMesh(Intentos, _intentos.ToString());
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
