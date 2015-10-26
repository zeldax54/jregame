using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
          //linea.fontSize+= 5;
            if (Input.GetMouseButtonUp(0))//Si suelta el clic 
            {
                //Caso 1:Si la linea contiene _ (significa que no hay palabra puesta)
                if (linea.text.Contains("_"))
                {
                    int posicion = FindFirst_(linea.text);//Encontrar la posicion de la 1ra _
                    PutText(gameObject.GetComponent<TextMesh>(), linea,posicion);//Agrego la palabra a la linea donde esten las __
                    linea.text=Remove_(linea.text);//Quitar todas las _ de la linea
                    _soltando = true;
                }
                //Caso 2 Si la linea  tiene palabras que ya han sido puestas 
                else if (linea.text.Contains("#ff0000ff")) //Si la linea tiene color rojo es q tiene palabras puestas
                {
                 string oldword=FindandReplaceRedWord(linea,gameObject.GetComponent<TextMesh>().text);
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

 

    private void PutText(TextMesh palabra, TextMesh linea,int startpos)//Pongo el texto en la linea y lo coloreo
    {
        linea.text=linea.text.Insert(startpos,"<color=#ff0000ff>" + palabra.text + "</color>");
    }

    public int FindFirst_(string x)
    {
        for (int i = 0; i <x.Length ; i++)
        {
            if (x[i] == '_')
            {
                return i;
            }
        }
        return -1;
    }

    private string Remove_(string palabra)//Quitar las __________
    {
        string fin = "";
        for (int i = 0; i < palabra.Length; i++)
        {
            if (palabra[i] != '_')
                fin += palabra[i];
        }
        return fin;
    }

    private string FindandReplaceRedWord(TextMesh linea,string newpalabra)//Encontrar la palabra en rojo reemplazarla por la nueva y devolver la vieja para activar el gameobject en la pantalla
    {
        string x = linea.text;
        string olddword = "";
        int posprimercierre=0;
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] == '>') //encontrar primer cierre del color
            {
                posprimercierre = i+1;
                break;
            }
        }
        
         int posultimocierre = 0;
        for (int i = posprimercierre; i < x.Length; i++)
        {
            if (x[i] == '<') //Abre segundo marcador
            {
                posultimocierre = i;
                break;
            }
            
            olddword += x[i];
        }
       
        x = x.Remove(posprimercierre, posultimocierre - posprimercierre);
       
        x=x.Insert(posprimercierre, newpalabra);
        linea.text=x;
        return olddword;
    }

    private void SubirPalabra(GameObject o)
    {
        o.transform.position = new Vector3(o.transform.position.x+20f,o.transform.position.y,o.transform.position.z);
    }

    private void BajarPalabra(string palabra)
    {
        TextMesh o =
            FindObjectsOfType<TextMesh>().First(a => a.text == palabra);
        o.transform.position = new Vector3(o.transform.position.x-20f,o.transform.position.y,o.transform.position.z);

    }

}
