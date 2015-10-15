using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class BotonJugar : MonoBehaviour {


	
    private Button boton;//Este es el boton que cree en la pantalla principal

	// Use this for initialization
	void Start ()
	{
	    boton = FindObjectsOfType<Button>().First(a => a.name == "BotonMinij");//Busco el boton que cree
        boton.onClick.RemoveAllListeners();//Quito los eventos onclick por si los tubiera
        boton.onClick.AddListener(Accion);//Le agrego en el evento onclick el metodo accion
		
	}

    private void Accion()
    {
        Application.LoadLevel("MinijuegoAdivinar");//Cargo la escena del minigame esto es para probar se que despues la cargaras desde otrea parte
    }

    // Update is called once per frame
	void Update () {
	
	}

	/*Reacciona al click del objeto que tiene este scrip*/
	void OnMouseDown(){

		//Desactivamos sonido actualmente
//		MainCamera.GetComponent<AudioSource>().Stop();
		//Se activa el sonido del boton
		GetComponent<AudioSource> ().Play ();


		//Cargamos la siguiente escena
		Invoke("CargarEscena", GetComponent<AudioSource>().clip.length);


	}

	void CargarEscena(){
		Application.LoadLevel ("IntroBosque");
	}
}
