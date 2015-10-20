using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Adivinanza  {


    public string numero { get; set; }
    public string adivinanza { get; set; }
    public string respuesta { get; set; }

    public string[] falsasrespuestas { get; set; }


    private List<Adivinanza> _adivinanzas;

  

    public Adivinanza Getadivinazas(string numeroparam)
    {
        return _adivinanzas.FirstOrDefault(item => item.numero == numeroparam);
    }

    public int ContAdivinanzas()
    {
        return _adivinanzas.Count;
    }

    public void InicializarLista()
    {
        _adivinanzas = new List<Adivinanza>();
        /////////////
        Adivinanza primeraAdivinanza = new Adivinanza();
        primeraAdivinanza.numero = "1";
        primeraAdivinanza.adivinanza = "Oro parece plata no es quien no lo adivine bien tonto es";
        primeraAdivinanza.respuesta = "Platano";
        primeraAdivinanza.falsasrespuestas = new string []{ "La Plata", "Oro es" };

        Adivinanza segundaAdivinanza = new Adivinanza();
        segundaAdivinanza.numero = "2";
        segundaAdivinanza.adivinanza = "Ya vez Ya vez quien no lo adivine bien tonto es";
        segundaAdivinanza.respuesta = "Llaves";
        segundaAdivinanza.falsasrespuestas = new string[] { "Lo vi", "Un Ojo" };

        Adivinanza terceraAdivinanza = new Adivinanza();
        terceraAdivinanza.numero = "3";
        terceraAdivinanza.adivinanza = "Que tengo en el bolsillo?Dijo Bilbo.";
        terceraAdivinanza.respuesta = "Un anillo";
        terceraAdivinanza.falsasrespuestas = new string[] { "Una Piedra", "o Nada"};

        Adivinanza cuartaAdivinanza = new Adivinanza();
        cuartaAdivinanza.numero = "4";
        cuartaAdivinanza.adivinanza = "Canta sin voz, vuela sin alas sin dientes muerde sin boca habla.";
        cuartaAdivinanza.respuesta = "El Viento";
        cuartaAdivinanza.falsasrespuestas = new string[] { "Una Hoja", "El cielo" };

        //Agrego las adivinzanas creadas
        _adivinanzas.Add(primeraAdivinanza); _adivinanzas.Add(segundaAdivinanza); _adivinanzas.Add(terceraAdivinanza);
        _adivinanzas.Add(cuartaAdivinanza);
    }
}
