using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class MecanicaController : MonoBehaviour
{

    private ControladorPoemas _controladorPoema;
    public TextMesh Intentos;
    private int _intentos = 3;

    void Awake()
    {
        _controladorPoema = GetComponent<ControladorPoemas>();
        UpdateTextMesh(Intentos, _intentos.ToString());
    }

    private void UpdateTextMesh(TextMesh t, string n)
    {
        t.text = n;
    }

    public void Rendirse()//Chequea el estado de las Lineas del poema
    {
        _controladorPoema._poemaactual++;
        _controladorPoema.SetUI();
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

        for (int i = 0; i < lineasenInterfaz.Count; i++)
        {
            ManejadorLinea manejador = lineasenInterfaz[i].GetComponent<ManejadorLinea>();//Obtengo el script manejador para cada linea
            if (manejador.ContienePalabraDeResp())
            {
                if (manejador.GetActualPuesta() == poemaActual.GetPalabradeLina(lineasPema, i).palabra)//Si la palabra que tiene puesta coincide con la que le toca en el poema
                {
                    manejador.SetCorrectWord("#008000ff");//Pongo la palabra en Coorecta asignandole un nuevo color, verde en este caso
                }
            }
           
           
        }

       
    }

 

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
