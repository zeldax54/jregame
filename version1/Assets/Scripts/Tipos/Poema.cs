
    using System.Collections.Generic;
    using System.Linq;

public class Poema
{

    public List<string> TextoPoemaLineas { get; set; }//Lineas del poema en donde hay * es porque falta la palabra
    public List<Palabra> Palabras { get; set; } //Lista de palabras que faltan con su posicion dentro del texto
    public List<Palabra> Falsaspalabras { get; set; } //palabras que no van en el texto la posicion la asigno en -1

    

    public Poema()//Construcor Vacio para usar fuera de la clase
    {

    }

    private Poema(List<string> pTextoPoemaLineas,List<Palabra>pPalabras,List<Palabra>pFalsasPalabras )//Constructor con parametros para usar dentro de la misma clase
    {
        TextoPoemaLineas = pTextoPoemaLineas;
        Palabras = pPalabras;
        Falsaspalabras = pFalsasPalabras;
    }

    public List<Poema> InicializarLista()
    {
        var poemas = new List<Poema>();
        //Primer Poema
        var poemaTexto = new List<string>()
        {
            "Yo quiero cuando me *",
            "Sin Patria pero sin *",
            "Tener en mi Losa un ramo",
            "De flores y una *"
        };
                       
        var poemaPalabras = new List<Palabra>(){new Palabra("Muera",0),new Palabra("Amo", 1),new Palabra("Bandera",3)};
        var poemaFalsas = new List<Palabra>(){new Palabra("Vaya",-1),new Palabra("Espada",-1)};
        poemas.Add(new Poema(poemaTexto,poemaPalabras,poemaFalsas));//Poema 1
        //Segundo Poema
        poemaTexto = new List<string>()
        {
            "Si quieren que de este *",
            "Lleve una memoria grata,",
            "Llevaré * profundo,",
            "Tu cabellera de *"
        };
        poemaPalabras = new List<Palabra>(){new Palabra("Mundo",0),new Palabra("Padre", 2),new Palabra("Plata",3)};
        poemaFalsas = new List<Palabra>() { new Palabra("Pais", -1), new Palabra("Oro", -1)};
        poemas.Add(new Poema(poemaTexto, poemaPalabras, poemaFalsas));//Poema 2
        //Tercer Poema
        poemaTexto = new List<string>()
        {
            "Estimo a quien de un revés,",
            "Echa por * a un tirano",
            "Lo estimo, si es un *",
            "Lo estimo, si es *"
        };
        poemaPalabras = new List<Palabra>() { new Palabra("Tierra", 1), new Palabra("Cubano", 2), new Palabra("Aragonés", 3) };
        poemaFalsas = new List<Palabra>() { new Palabra("Suelo", -1), new Palabra("Mexicano", -1) };
        poemas.Add(new Poema(poemaTexto, poemaPalabras, poemaFalsas));//Poema 3
        return poemas;
    }

    public List<Poema> Poemas()
    {
        return new List<Poema>();
    }

    public Palabra GetPalabradeLina(List<Palabra> palabras, int linea)
    {
        return palabras.FirstOrDefault(p => p.posicion == linea);
    }
}
    
    
    
    public class Palabra
{
        public string palabra {get; set; }
        public int posicion { get; set; }//Posicion donde esta la palabra en las lineas del poema empesando a enumerar las lineas del poema en 0

        public Palabra(string ppalabra,int pposicion)
    {
        palabra = ppalabra;
        posicion = pposicion;
    }


}

