using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Generador : MonoBehaviour
{

    public GameObject CuadroResp; //El cuadro que va a dibujar en la respuesta
    public GameObject CuadroLetra; //El cuadro que va a dibujar en las letras
    public SpriteRenderer SpriteResp; //la imagen dentro del cuadro en la respuesta
    public SpriteRenderer SpriteLetra; //la imagen dentro del cuadro  en las letras
    public TextMesh TextorResp;//TextMesh dentro de los cuadros
    public TextMesh TextoLetras;//TextMesh dentro de los cuadros
    //Generadores
    public GameObject GeneradorIzq;
    public GameObject GeneradorDer;
    public GameObject GeneradorLetras;

    private float _posinigen;
        //Estas dos variables son para generar las letras y bajar las que tengo que ponder cuando hay dos lineas

    private float _posinoy;
    private int _contizq; // va contando las letras generadas
    private int _contder; // va contando las letras generadas
    private int _contletras = 0;
    private Image _spriteContainer; // el contenedor de la imagen que se va a mostrar

    // string para generar los cuadros
    private string _palabra;
    private string _palabrarespizq;
    private string _palabrarespder;
    private string _palabraletras;

    private float _incrementaizq = 0f;// Sumatoria para incrementar los espacios entre letras
    private float _incrementader = 0f;// Sumatoria para incrementar los espacios entre letras
    private float __incrementaletras = 0f;

    //Creo las variables que me guardaran los datos 
    //Guardo los datos en arreglos , respuesta primero , letras despues y luego  la pista del segundo tipo.
    private object[] primera = {"PEPE", "HENGPIHPPGAEDW","Ingeniero"};
        //OJO en las letras ramdon tengo que tener asegurado que mi palabra este contenida ahi.

    private object[] segunda = {"CALIXTO", "HEYCBWXOTXILAC","Cantante"};
    private object[] tercera = { "EUGENIO", "OGLIQCUYEWEVNA",  "Soldador" };
    private List<object[]> ListaPalabras = new List<object[]>();
    private object[] arregloactual; //Creo este para tener guardado en todo momento en el que estoy 
    private int contpantallas = 0; //para saber en cual pantalla estoy

    //Estas son las variables
    private int orgullo = 5;
    private int diamantesl = 5;
    private int fallos = 0;
    private int aciertos = 0;
    private int ayudas = 3;
    private int siguientes = 0;


    //Estos son los controles Text para ser actualizados aqui asigno el valor no el indicador
    private TextMesh OrgulloTextMesh;
    private TextMesh DiamantesTextMesh;
    private TextMesh FallosTextMesh;
    private TextMesh AciertosTextMesh;
    private TextMesh AyudasTextMesh;
    private TextMesh SiguientesTextMesh;
    private TextMesh cuadernilloTextMesh;//Este es para la 2da pista

   
    //Parte de las pistas
    private ModalPanel _modalPanel;//Objeto de tipo del script ModalPanel
   //Estos los asigno desde el inspector
    public Button ayudaButton;//Boton ayuda
    public Button aceptarButton;//Boton aceptar del panel
    public Button cancelarButton;//Boton cancelar del panel
    public Button SiguientButton;//Boton para siguiente rendirce
    public Button cuadernillo;//Boton para las pistas de segundo tipo
    private int ayudaactual = 0;//variable para saber si ya ha usado la ayuda en este nivel
    public GameObject Marcador;//Esto es para marcar las y de las letras de abajo
    private bool rindiendose;//variable para controlar si se rinde en la imagen
    private int vecesrendidas;//Controlo cuantas veces se rinde
    private void Awake()
    {
        //Agregar palabras a la lista
        ListaPalabras.Add(primera);
        ListaPalabras.Add(segunda);
        ListaPalabras.Add(tercera);

        //Aqui encuentro los textsMesh esto es Linq el lenguaje de consultas de .net unity lo entiende bien.
        //Lo que hace ahi es de todos los objetos textmesh dame el primero con nombre X o Y y asignalo a una variable.
        OrgulloTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Orgullo Valor");
        DiamantesTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Diamantes Valor");
        FallosTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Fallos Valor");
        AciertosTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Aciertos valor");
        AyudasTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Ayudas Valor");
        SiguientesTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Siguientes Valor");
        cuadernilloTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Cuadernillo");

        //Aqui asino los textMesh lo hago con la funcion para no estar repitiendo tanto codigo
        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
        UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());
        UpdateTextMesh(FallosTextMesh, fallos.ToString());
        UpdateTextMesh(AciertosTextMesh, aciertos.ToString());
        UpdateTextMesh(AyudasTextMesh, ayudas.ToString());
        UpdateTextMesh(SiguientesTextMesh, siguientes.ToString());

        //Genero la escena por Primera vez
        PrepararGenerar(ListaPalabras[contpantallas], contpantallas);//La primera vez que se iniciar el scrip genero con el primer arreglo y la primera imagen
        arregloactual = ListaPalabras[contpantallas];//Actualizo a actual
        //Boton siguiente
        SiguientButton.onClick.RemoveAllListeners();
        SiguientButton.onClick.AddListener(Rendirce);
        //parte de las Pistas;
        _modalPanel = ModalPanel.Instance();// Le asigno una instancia del script Modal Panel
        //Le asigno su evento o sea que accion va a ejecutar este boton
        ayudaButton.onClick.RemoveAllListeners();
        ayudaButton.onClick.AddListener(LevantarPanel);//LevantarPanel es un metodo q lo unico que hace es llamar al panel
        //Pistas del segundo tipo
        cuadernillo.onClick.RemoveAllListeners();
        cuadernillo.onClick.AddListener(Pistas2Lanzar);

    }

    private void Pistas2Lanzar()
    {
        if (ayudaactual == 1)
        {
            _modalPanel.Elejir("Ya ha usado su ayuda en este nivel",// Le decimos que ya ha usado su pista
               _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
        }
        else
        {
            _modalPanel.Elejir("Esta pista cuesta un Orgullo.Continuar?",
               Pistas2Ejecutar, aceptarButton, cancelarButton, false);//Asignar los objetos al choise con el Poner Pista que es el evento 
        }
       
    }

    private void Pistas2Ejecutar()
    {
        ayudaactual++;
        orgullo--;
        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
        _modalPanel.Elejir("Este personaje es "+ListaPalabras[contpantallas][2].ToString(),
              _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);//Muestro la pista
        //Actualizo un Textmesh para que se me quede la pista en pantalla
        UpdateTextMesh(cuadernilloTextMesh, ListaPalabras[contpantallas][2].ToString());
    }


    public void Rendirce()//Metodo para saltar de imagen
    {
        rindiendose = true;
        _modalPanel.Elejir("Rendirse en esta imagen?",
                Nextpantalla, aceptarButton, cancelarButton, false);//Asignar los o
    }

    

    private void LevantarPanel()
    {
        IEnumerable<GameObject> resp = GetRespObjs();//Obtengo los cuadros de la respuesta
        List<GameObject> posvacias =
           (from r in resp let t = r.GetComponentInChildren<TextMesh>() where t.text == "" select r).ToList();
        if (!posvacias.Any() && ayudaactual==0) //Si no hay ningun cuadro vacio entoncces
        {
            _modalPanel.CerrarPanel();
            _modalPanel.Elejir("Desocupe un cuadro de respuesta.",
                _modalPanel.CerrarPanel, aceptarButton, cancelarButton,true);//Pedimos que se desocupe uno
        }
        else//Si hay alguno disponible
        {
            if (ayudaactual == 1)//Si ya ha usado la ayuda en este nivel
            {

                _modalPanel.Elejir("Ya ha usado su ayuda en este nivel",// Le decimos que ya ha usado su pista
                _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
            }
            else
            {
                _modalPanel.Elejir("Una pista cuesta un Diamante.Continuar?",
                 PonerPista, aceptarButton, cancelarButton,false);//Asignar los objetos al choise con el Poner Pista que es el evento 
            }
        }
       
        
    }

  

    private void PonerPista()
    {
       
        //Ahora empesamos a poner la letra
        IEnumerable<GameObject> resp = GetRespObjs();//Obtengo los cuadros de la respuesta
        //Selecciono los cuadros que no tienen letras
        List<GameObject> posvacias =
            (from r in resp let t = r.GetComponentInChildren<TextMesh>() where t.text == "" select r).ToList();
        
            ayudaactual++;//iNCREMENTAMOS AYUDACTUAL PARA NO TENER MAS AYUDAS DISPONIBLES EN ESTE NIVEL
            ayudas--;//El contador general de ayudas
            diamantesl--;//Quitamos un diamante
            UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());//Atualizamos los textmesh
            UpdateTextMesh(AyudasTextMesh, ayudas.ToString());
            int posrandom = Random.Range(0, posvacias.Count - 1);//Escojo una posicion al azar
            //Encontrando Posicion
            string palabraact = GetActualResp();//Obtengo la pabra actual de la respuesta
            string l = PosReal(palabraact, posrandom, ListaPalabras[contpantallas][0].ToString());//Aqui obtengo la letra que voy a poner
            TextMesh tt = posvacias[posrandom].GetComponentInChildren<TextMesh>();//Obtengo el textmesh donde va la pista
            BoxCollider c = posvacias[posrandom].GetComponentInChildren<BoxCollider>();//Obtengo su collider
            Destroy(c);//y lo destruyo para que el jugador no pueda quitar la pista
            tt.text = l;// le pongo la pista
            tt.color = Color.green;//pinto la pista de verde
            FindAndDeactivate(l);//Esto es para la letra que puse arriba quitarla de abajo
            //Update y decrementar ptos total
            var actual = GetActual(GetRespObjs());//Obtengo las letras de abajo
            CheckandSet(actual, ListaPalabras[contpantallas][0].ToString());//Chequeo a ver si ya completo la palabra

        


        _modalPanel.CerrarPanel();
    }

    //Metodo para actualizar los textMesh
    private void UpdateTextMesh(TextMesh t, string valor)
    {
        t.text = valor;
    }

    // Use this for initialization


    private void PrepararGenerar(object[] datos, int numero)
        //Funcion para pintar los cuadraros y asignar la imagen recibe que datos voy a pintar y el numero de la imagen que le toque
    {

        _palabra = datos[0].ToString(); //tomo de los datos la respuesta
        int inc = 0; // Pongo inc a 0 
        if (_palabra.Length%2 != 0) // SI la palabra tiene letras par o sea estan pareja las letras 
        {
            inc++;
        }
        _palabrarespizq = Voltear(_palabra.Substring(0, _palabra.Length/2));
                // Tomo la mitad de la palabra a la izquierda del centro y le doy la vuelta para que aparezca en el orden correcto , esto es asi porque estoy generando de derecha a izquiera 

        _palabrarespder = _palabra.Substring(_palabra.Length / 2, _palabra.Length / 2 + inc);
                // Tomo la mitad de la palabra a la derecha del centro no le doy la vuelta porque ahora si genero en el orden correcto de izquierda a derecha

        TextorResp.text = "";//Borro el texto del cuadro

        _posinigen = GeneradorLetras.transform.position.x;//La posicion inicial en x del generador
        _posinoy = GeneradorLetras.transform.position.y;// y la Y
        _spriteContainer = GameObject.Find("Imagen").GetComponentInChildren<Image>();//El objeto imagen que hay en la escena lo encuentro
        Sprite s = (Sprite)Resources.Load(numero.ToString(), typeof(Sprite));// Cargo la Imagen hay que ponerla dentro de la carpeta resources
        _spriteContainer.color = Color.white;//Cambio el fondo a blanco para que no afecte a la imagen
        _spriteContainer.sprite = s;//le asigno la imagen que le toca

        _palabraletras = datos[1].ToString();// Asigno las letras
       
     

        _contizq = 0;
        _contder = 0;
        _contletras = 0;
        GenerarIzq();
       GenerarDer();
        GenerarLetras();

    }

    private static string Voltear(string x)//Darle la vuelta a una palabra o sea si es "PEPE" ponerla EPEP
    {
        string fin = "";
        int cont = x.Length - 1;
        while (cont != -1)
        {
            fin += x[cont];
            cont--;
        }
        return fin;
    }
  
    private void GenerarIzq()
    {
        SpriteResp.color = Color.blue;// Pinto el cuadrado de azul
        TextorResp.text = "";// Como esto es respuesta borro el texto q exista en el cuadro
        CuadroResp.name = "*" + _contizq + "i";// Asigno el nombre para guiarme a la hora de resolver el asertijo
        Instantiate(CuadroResp, new Vector3(GeneradorIzq.transform.position.x + _incrementaizq, GeneradorIzq.transform.position.y),// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
        Quaternion.identity);
        _incrementaizq += -1.75f;
        Invoke("GenerarIzq", 0.01f);//LLamo al mismo metodo cada 0.01 segundos
        _contizq++;// Aqui incremento cont para ir iterando por las letras

        if (_contizq == _palabrarespizq.Length)// Cuando cont alcance lo que quiero generar corto el metodo y reinicio cont
        {
            _contizq = 0;
            CancelInvoke("GenerarIzq");
          
        }
    }


    private void GenerarDer()
    {
       
        SpriteResp.color = Color.blue;// Pinto el cuadrado de azul
        TextorResp.text = "";// Como esto es respuesta borro el texto q exista en el cuadro
        CuadroResp.name = "*" + _contder + "d";// Asigno el nombre para guiarme a la hora de resolver el asertijo
        Instantiate(CuadroResp, new Vector3(GeneradorDer.transform.position.x + _incrementader, GeneradorDer.transform.position.y),// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
        Quaternion.identity);
        _incrementader += 1.75f;
        Invoke("GenerarDer", 0.01f);//LLamo al mismo metodo cada 0.01 segundos
        _contder++;// Aqui incremento cont para ir iterando por las letras

        if (_contder == _palabrarespder.Length)// Cuando cont alcance lo que quiero generar corto el metodo y reinicio cont
        {
            _contder = 0;
            CancelInvoke("GenerarDer");

        }
    }

    private float y = 0;
    private void GenerarLetras()
    {
         y = GeneradorLetras.transform.position.y;//Posicion del generador en y
        var letra = _palabraletras[_contletras].ToString();// Voy tomando las letras que voy genrando
        SpriteLetra.color = Color.red;// lo pongo en rojo
        TextoLetras.text = letra;// AHora si escriobo la letra que le toca
        CuadroLetra.name = _contletras + "!";// Asigno el nombre al cuadro
        if (_contletras == _palabraletras.Length / 2)//Veo si ya esta la mitad puesta
        {
            __incrementaletras = 0f;//pongo la variable al principio
            y = _posinoy - 1.45f;// bajo la y para que se dibuje abajo
            GeneradorLetras.transform.position = new Vector3(_posinigen, _posinoy - 1.45f, GeneradorLetras.transform.position.z);// bajo el objeto generador
        }
        Instantiate(CuadroLetra, new Vector3(GeneradorLetras.transform.position.x + __incrementaletras, y),// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
           Quaternion.identity);
        __incrementaletras += 1.75f;
        Invoke("GenerarLetras", 0.01f);//LLamo al mismo metodo cada 0.01 segundos
        _contletras++;
        if (_contletras == _palabraletras.Length)// Cuando cont alcance lo que quiero generar corto el metodo y reinicio cont
        {
            _contletras = 0;
            CancelInvoke("GenerarLetras");

        }
    }


    //Aqui jugar
    //Aqui la parte de Jugar
    public List<GameObject> EstadopLetras;//variable para guardar objetos temporalmente
    void Update()
    {

        if (Input.GetMouseButton(0) && !_modalPanel.IsActivo())//Si hago click y si el modal no esta activo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Creo un rayo y lo lanzo desde la camara
            RaycastHit hit = new RaycastHit();//Esto es para guardar donde impacte el rayo
            if (Physics.Raycast(ray, out hit))// Si impact� en algo?
            {
                GameObject o = GameObject.Find(hit.collider.name);//Encuentro donde impacto el rayo por el nombre del collider del hit
                if (o.name.Contains("!"))// Si el nombre contine ! entonces se que es un cuadro de los de abajo recuerda que para eso les puse el simbolo al crearlos
                {
                    TextMesh tletras = o.GetComponentInChildren<TextMesh>();//Encuentro el textmesh que contiene la letra 
                    var resp = GetRespObjs();//Obtengo los cuadros de la respuesta para saber cual no tiene letra puesta
                    foreach (var GO in resp)//Por cada cuadro en la respuesta 
                    {
                        var tresp = GO.GetComponentInChildren<TextMesh>();//Obtengo su TextMesh hijo
                        if (tresp.text == "")//Si esta en blanco
                        {
                            tresp.text = tletras.text;//Le asignoi la letra del cuadro que pinch�
                            //  tletras.text = "";
                            o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 20f,
                                o.transform.position.z);//cambio la posicion del cuadro que pinche aumentando su y en 20
                            //para que no se vea en la pantalla
                            EstadopLetras.Add(o);//Guardo ese objeto en esta variable para saber que esta arriba
                            var actual = GetActual(GetRespObjs());//Llamo al metodo para formar la palabra actual de respuesta
                            CheckandSet(actual, arregloactual[0].ToString());//Compruebo el estado de la respuesta y actuo en consecuencia
                            break; //salgo hasta el otro click
                        }
                    }
                }
                //Si el rayo impacta en un cuadro de los de la respuesta
                if (o.name.Contains("*"))
                {

                    var tletras = o.GetComponentInChildren<TextMesh>();//Caputo el texto que este en el cuadro
                    var letras = GetLetras();//Es6tado de las letras que estan arriba o sea las que estan asignadas
                    foreach (var go in letras)//Recorro el arreglo 
                    {
                        var tresp = go.GetComponentInChildren<TextMesh>();//Capturo el textMesh
                        if (tresp.text != tletras.text) continue;//Si son iguales las letras la que pinche y la que este en el arreglo
                        tresp.text = tletras.text;//Asigno la letra 
                        FindAndActivate(tresp.text);//Bajo la letra de arriba y la ubico otra vez en su posicion inicial
                        EstadopLetras.Remove(go);//La quito del arreglo porque ya esta abajo otra vez
                        tletras.text = "";// y borro la letra del cuadro de respuesta
                        break;
                    }
                }
            }

        }


    }

    private static IEnumerable<GameObject> GetRespObjs()//Encontrar todos los cuadrtos de la respuesta
    {
        //Con una expresion linq los encuentro si el nombre contiene un * y luego los ordeno por la posicion x para formar correctamente la respuesta
        //Consejo estudia Linq es super util y superfacil no soolo para esto sino para todo lo de .net voy a adjuntarte un doc de linq.
        return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("*")).OrderBy(a => a.transform.position.x);
    }

    private static IEnumerable<GameObject> GetLetrasrand()
    {
        return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("!"));
    }

    public string GetActual(IEnumerable<GameObject> letras)//Este metodo me da la palabra que esta formada en las respuestas
    {
        //Recibe la lista actual de cuadros de respuesta
        // O sea si hay 3 letras y falta una por poner por supuesto me va a dar una palabra que no es la respuesta
        return letras.Select(o => o.GetComponentInChildren<TextMesh>()).Aggregate("", (current, t) => current + t.text);
    }

    public void CheckandSet(string actual, string respuestaa)//Chequeo la palabra con la respuesta 
    {
        if (actual.Length == respuestaa.Length)// Si las longitudes son iguales prosigo porque si no es por gusto nunca seran dos palabras iguales si no tienen la misma cantidad de letras
        {
            if (Check(actual))//Chequeo la palabra actual que se forma
            {
                //Si es igual significa que ha completado el reto
                //Incremento los aciertos
                aciertos++;
                UpdateTextMesh(AciertosTextMesh, aciertos.ToString());//Actualizo el textMesh

                Nextpantalla();//cargo la imagen siguiente 



            }
            else//Si no es igual es que fall� en la adivinacion
            {
                //Incremento los fallos
                fallos++;
                UpdateTextMesh(FallosTextMesh, fallos.ToString());// Y actualizo el TextMesh

            }
        }
    }

    public bool Check(string orden)//Chequear palabra con la respuesta qe tengo guardada en este arrgelo
    {
        return orden == arregloactual[0].ToString();//Chequear palabra con la respuesta qe tengo guardada en este arrgelo
    }

    private IEnumerable<GameObject> GetLetras()//Es6tado de las letras que estan arriba o sea las que estan asignadas
    {
        return EstadopLetras;
    }

    private void FindAndActivate(string letra)//Bajo la letra de arriba y la ubico otra vez en su posicion inicial
    {
        foreach (var o in EstadopLetras)
        {
            TextMesh tresp = o.GetComponentsInChildren<TextMesh>().First();
            if (tresp.text.Contains(letra))
            {
                o.transform.position = new Vector3(o.transform.position.x, (o.transform.position.y - 20f),
                                o.transform.position.z);
                break;
            }
        }
    }

    private void Nextpantalla()
    {

        EstadopLetras = new List<GameObject>(); //Reinicio los objetos guardados 
        foreach (var obj in GetRespObjs()) //Segundo destruto los cuadros de respuesta
        {
            DestroyObject(obj);
        }
        foreach (var obj in GetLetrasrand()) //Destruyo los cuadros de abajo
        {
            DestroyObject(obj);
        }
        contpantallas++; //Siguiente Imagen
       
        if (contpantallas > 2)
        {
            // Si ya no hay mas imagenesasignar orgullo, diamantes y salir del minijuego
            //No se como cuentas si se rindio o no que vas a hacer con los diamantes
          //Mira a ver y despues se implementa esta parte
            diamantesl += 1;

            UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
            UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());

        }
        else //Generar los cuadrados con la otra imagen , letras y respuesta
        {
            if (rindiendose)
            {
                vecesrendidas++;
                orgullo--;
                UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
                rindiendose = false;
                _modalPanel.CerrarPanel();
            }

          
            if (ayudaactual == 1)
            {
                ayudaactual--;//RESTO DE LA AYUDA PARA TENERLA DISPONIBLE EN EL PROXIMO NIVEL
            }
            arregloactual = ListaPalabras[contpantallas];//actualizar arreglo actual
            //Reseteando variables y generadores
            __incrementaletras = 0;
            _incrementader = 0;
            _incrementaizq = 0;
            cuadernilloTextMesh.text = "";//Quito la pista si la hubiera
            GeneradorIzq.transform.position = new Vector3(-1.82f, -1.09f, 0f);
            GeneradorDer.transform.position = new Vector3(0.02999997f, -1.09f, 0f);
            GeneradorLetras.transform.position = new Vector3(-5.45f, -2.48f, 0f);
           
            PrepararGenerar(ListaPalabras[contpantallas], contpantallas);//Genero otra vez los cuadros pero con la segunda imagen
            
        }
    }

    #region Metodos para pistas
    private string GetActualResp()
    {

        IEnumerable<GameObject> resp = FindObjectsOfType<GameObject>().Where(a => a.name.Contains("*")).OrderBy(a => a.transform.position.x);
        var fin = "";
        bool bandera = false;

        foreach (var r in resp)
        {

            var t = r.GetComponentInChildren<TextMesh>();
            if (t.text == "")
            {
                fin += "*";
            }


            else
            {
                if (t.color == Color.red)
                {
                    fin += "#" + t.text;
                }
                else
                {
                    fin += t.text;
                }

                bandera = true;
            }
        }
        if (bandera == false)
        {
            fin = null;
        }
        else
        {
            List<int> c = new Procesador().findposelemt(ListaPalabras[contpantallas][0].ToString());
            if (c.Count != 0)
            {
                fin = c.Aggregate(fin, (current, i) => current.Insert(i, "_"));
            }
            fin = new Procesador().Reindex_(fin);
        }


        return fin;
    }

    public string PosReal(string palabraact, int pos, string respuestaax)
    {
        int cont = 0;
        if (palabraact == null)
            cont = pos;
        else
        {
            palabraact = new Procesador().Removepista(palabraact);
            palabraact = new Procesador().Remove_(palabraact);
            for (int i = 0; i < palabraact.Length; i++)
            {
                if (palabraact[i] == '*')
                {

                    if (cont == pos)
                    {
                        cont = i;

                        break;
                    }
                    cont++;
                }
            }
        }

        return respuestaax[cont].ToString();
    }

    private void FindAndDeactivate(string letra)
    {
        IEnumerable<GameObject> objs = Getactualrandomabajo(GetActualRandom());
        foreach (var o in objs)
        {
            var tresp = o.GetComponentInChildren<TextMesh>();
            if (tresp.text == letra)
            {
                o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 20f,
                    o.transform.position.z);
                EstadopLetras.Add(o);
                break;
            }
        }
    }

    private IEnumerable<GameObject> Getactualrandomabajo(IEnumerable<GameObject> todos)
    {
        List<GameObject> fin = new List<GameObject>();
        todos = todos.OrderBy(a => a.transform.position.x);
        foreach (var l in todos)
        {
            if (l.transform.position.y - Marcador.transform.position.y < 0.2)
            {
                fin.Add(l);
            }
        }
        return fin;
    }

    private static IEnumerable<GameObject> GetActualRandom()
    {

        return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("!"));
    }
    #endregion

}
