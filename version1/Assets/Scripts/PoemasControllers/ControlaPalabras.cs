using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditorInternal.VersionControl;

public class ControlaPalabras : MonoBehaviour
{

    private bool _soltando;
    
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name.Contains("Linea")) //Si el collider con el que choca es una linea
        {
            TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);
            linea.characterSize += 0.05f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        _soltando = false;
        if (other.name.Contains("Linea")) //Si el collider con el que choca es una linea
        {
            TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);
            ManejadorLinea manejadorLinea = linea.GetComponent<ManejadorLinea>();
          //linea.fontSize+= 5;
            if (Input.GetMouseButtonUp(0))//Si suelta el clic 
            {
                //Caso 1:Si la linea contiene _ (significa que no hay palabra puesta)
                if (manejadorLinea.LineaDisponible())
                {
                    int posicion = manejadorLinea.FindFirst_();//Encontrar la posicion de la 1ra _
                    manejadorLinea.PutText(gameObject.GetComponent<TextMesh>().text,posicion);//Agrego la palabra a la linea donde esten las __
                    manejadorLinea.Remove_();//Quitar todas las _ de la linea
                    _soltando = true;
                }
                //Caso 2 Si la linea  tiene palabras que ya han sido puestas 
                else if (manejadorLinea.ContienePalabraDeResp() && !manejadorLinea.IsRespuesta()) //Si la linea tiene color rojo es q tiene palabras puestas y no es la respuesta correcta.
                {
                 string oldword=manejadorLinea.FindandReplaceRedWord(gameObject.GetComponent<TextMesh>().text);
                 _soltando = true;
                 BajarPalabra(oldword);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Linea")) //Si no solto la palabra ahi volver a su color original
        {
            TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);
            linea.characterSize -= 0.05f;
            if (_soltando)//Saco de la pantalla  la palabra para que no este disponible en la lista de palabras
                SubirPalabra(gameObject);
            
        }
    }



   





    public void SubirPalabra(GameObject o)
    {
        o.transform.position = new Vector3(o.transform.position.x+20f,o.transform.position.y,o.transform.position.z);
    }
    public void BajarPalabra(string palabra)
    {
        TextMesh o =
            FindObjectsOfType<TextMesh>().First(a => a.text == palabra);
        o.transform.position = new Vector3(o.transform.position.x-20f,o.transform.position.y,o.transform.position.z);

    }

}
