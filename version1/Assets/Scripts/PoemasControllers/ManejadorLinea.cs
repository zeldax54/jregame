using UnityEngine;
using System.Collections;

public class ManejadorLinea : MonoBehaviour {


    private TextMesh t;
    void Awake()
    {
        t = GetComponent<TextMesh>();
    }

    public int FindFirst_()
    {
        for (int i = 0; i < t.text.Length; i++)
        {
            if (t.text[i] == '_')
            {
                return i;
            }
        }
        return -1;
    }

    public bool LineaDisponible()
    {
        if (t.text.Contains("_"))
            return true;
        return false;
    }

    public string GetActualPuesta()
    {
        string x = t.text;
        if (x.Contains("_"))
            return "";
        string olddword = "";
        int posprimercierre = 0;
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] == '>') //encontrar primer cierre del color
            {
                posprimercierre = i + 1;
                break;
            }
        }
        
        for (int i = posprimercierre; i < x.Length; i++)
        {
            if (x[i] == '<') //Abre segundo marcador
            {
                break;
            }

            olddword += x[i];
        }
        return olddword;
    }

    public void SetCorrectWord(string newcolor)
    {
        t.text = t.text.Replace("#ff0000ff", newcolor);//Cambio el color y eso hace que no aparezca como disponible
      
    }

    public string PintarPalabra(string palabra, string color)
    {
        palabra = palabra.Replace("#ff0000ff", color);
        return palabra;
    }

    public bool ContienePalabraDeResp()//Metodo para saber si hay una palabra en rojo o sea si hay una respuesta ahi
    {
        return t.text.Contains("ff0000ff") || t.text.Contains("008000ff");
    }

    public bool IsRespuesta()//Saber si la palabra puesta tiene respuesta
    {
        return t.text.Contains("008000ff");

    }

    public bool GetPalabraInsegura()//Para saber si hay una palabra en rojo ahi
    {
        if (t.text.Contains("ff0000ff"))
            return true;
        return false;
    }

    public void PutText(string palabra, int startpos)//Pongo el texto en la linea y lo coloreo
    {
        t.text = t.text.Insert(startpos, "<color=#ff0000ff>" + palabra + "</color>");
    }

    public void Remove_()//Quitar las __________
    {
        string fin = "";
        for (int i = 0; i < t.text.Length; i++)
        {
            if (t.text[i] != '_')
                fin += t.text[i];
        }
        t.text= fin;
    }

    public string FindandReplaceRedWord(string newpalabra)//Encontrar la palabra en rojo reemplazarla por la nueva y devolver la vieja para activar el gameobject en la pantalla
    {
        string x = t.text;
        string olddword = "";
        int posprimercierre = 0;
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] == '>') //encontrar primer cierre del color
            {
                posprimercierre = i + 1;
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
        x = x.Insert(posprimercierre, newpalabra);
        t.text = x;
        return olddword;
    }



    internal int FindFirst_(string p)
    {
        throw new System.NotImplementedException();
    }
}
