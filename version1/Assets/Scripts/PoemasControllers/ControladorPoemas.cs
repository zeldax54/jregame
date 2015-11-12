using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class ControladorPoemas : MonoBehaviour
{

    //Interface
    public TextMesh prefabLinea;//Este prefab esta creado en la carpeta prefab
       //Posibles Palaras
    public TextMesh Palabra1;
    public TextMesh Palabra2;
    public TextMesh Palabra3;
    public TextMesh Palabra4; 
    public TextMesh Palabra5;

    public GameObject LineStarterMarcador;//Posicion donde voy a empesar a dibujar los TextMesh
    public float DecrementaY=1;
    //

    public List<Poema> Poemas;//Lista de poemas
    private readonly Poema poema=new Poema();//Para traer la lista del scritp Poema
    public int _poemaactual = 0;//Saber en todo momento en que poema estoy y acceder desde el script de la mecanica
    private const string raya = "_________";
   
    private void Awake()
    {
        Poemas = poema.InicializarLista();
        SetUI();//Inicializar la Ui por primera vez
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//Si dio click
        {

            RaycastHit hitInfo;
            _target = GetClickedObject(out hitInfo);//Obtener el objeto donde dio el rayo que se lanza en el metodo GetClickedObject
            if (_target != null)//Si dio en algo el rayo obtener ese gameobject
            {
              
                if (hitInfo.collider.name.Contains("Palabra"))//Si fue en un TextMesh de las palabras
                {
                    _originalPosition = _target.transform.position;//Guardar la posicion Original del objeto
                    _mouseState = true;
                    _screenSpace = Camera.main.WorldToScreenPoint(_target.transform.position);
                    _offset = _target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenSpace.z));
                }

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_target != null)//Si me encuentro arrastrando algo en ese momento
            {
                _target.transform.position = _originalPosition;//A su pos inicial
            }
            _mouseState = false;
        }
        if (_mouseState)
        {
            //mantener chequeada la posicion del mouse
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenSpace.z);
            //Convertir de World posicion a Screen posicion
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + _offset;
            //Arastrar
            _target.transform.position = curPosition;
        }
    }



    public void SetUI()
    {
        //Reiniciar los intentos
        MecanicaController m = GetComponent<MecanicaController>();
        m.IntentosValue = 3;
        m.UpdateTextMesh(m.Intentos,m.IntentosValue.ToString());
        ////////////////
            InicializarPalabrasPosibles(Poemas[_poemaactual]);
            //Inicializar Lineas del Poema
            int cont = 1;//para asignar el nombre a las lineas
            float decrementa = 0;
            if (!LimpiarLineas(Poemas[_poemaactual].TextoPoemaLineas))
            {
                foreach (var linea in Poemas[_poemaactual].TextoPoemaLineas)
                {
                 
                    prefabLinea.name = "Linea" + cont;
                    prefabLinea.text = Remaster(linea);
                    // Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
                    //Igual que en los cuadros
                    Instantiate(prefabLinea,
                        new Vector3(LineStarterMarcador.transform.position.x, LineStarterMarcador.transform.position.y - decrementa,
                            0f), Quaternion.identity);
                    //Incremento contador y Y
                    cont++;
                    decrementa += DecrementaY;
                }
            }
    }

    private bool LimpiarLineas(List<string>lineaspoema )
    {
        bool bandera = false;
        List<TextMesh> lineas = FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a => a.name).ToList();
        if (!lineas.Any())
            return false;//Si no hay lineas las creo en el otro metodo
        
        for (int i = 0; i < lineas.Count; i++)
        {
            lineas[i].text = Remaster(lineaspoema[i]);
            bandera = true;
        }
      
        return bandera;
    }

    private void InicializarPalabrasPosibles(Poema p)//Metodo toma los 5 TextMesh y asigna las palabras
    {
        List<TextMesh> palabrasUi = new List<TextMesh>() {Palabra1, Palabra2, Palabra3, Palabra4, Palabra5};
        List<Palabra> todaspalabras = p.Palabras.Concat(p.Falsaspalabras).ToList();//Uno las palabras verdaderas y las falsas en una lista
        int cont = 0;//contador para iterar por la lista de palabras que forme
        while (palabrasUi.Count!=0)//Cilco par asignar random las palabras del poema a los mesh botones
        {
            int pos = Random.Range(0, palabrasUi.Count);//Posicion random en la lista de palabras
            TextMesh t = palabrasUi[pos];//Obtengo el Textmesh que esta en la lista
            t.text = todaspalabras[cont].palabra;//Asigno al texto la primera palabra de mi lista de palabras
            if (t.gameObject.GetComponent<BoxCollider>()==null)
            {
                t.gameObject.AddComponent<BoxCollider>();//Asignar un collider al objeto para poder arrastrarlo en la Interfaz
                t.GetComponent<Collider>().isTrigger = true;//Se dispara cuando choca con otro collider 
            }
            ActivarMesh(t);//Lo pongo en la x original por si no estaba ahi
           
            palabrasUi.RemoveAt(pos);//Elimino el textMesh de la lista para no repetirlo en la sig Iteracion
            cont++;//Incremento cont para pasar el siguiente elemento en mi lista de palabras

        }
    }

    private void ActivarMesh(TextMesh palabra)//Activarlo por si alguno no esta en la pantalla
    {
        Transform p = palabra.transform;//Transform de la palabra
        palabra.transform.position = new Vector3(8.008784f, p.position.y, p.position.z);
    }


    private GameObject _target;//Guardo en donde se hace click
    private bool _mouseState;//Si estoy arrastrando o no
    private Vector3 _originalPosition;//Posicion original desde donde estaba el Objeto que arrastro
    private Vector3 _screenSpace;
    private Vector3 _offset;
   
    GameObject GetClickedObject(out RaycastHit hit)//Obtener el objeto donde el rayo dio 
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    private string Remaster(string text) //Donde hay un * en una linea poner ________
    {
        string fin = "";
        foreach (char c in text)
        {
            if (c == '*')
            {
                fin += raya;
            }
            else
            {
                fin += c;
            }
        }
        return fin;
    }


}
