using UnityEngine;
using System.Collections;

/*
Controla el lanzamiento de los objetos, y calcula la posicion de caida
Invoca LaunchModel la encargada de obtener los elementos y destruirlos
 */
public class objectSpawnerController : MonoBehaviour {

	//Tiempo delay y aleatorio para llamar al Launch
	public float timeDelay;
	public float timeRandom;
	//Obengo el objeto lanzador
	public GameObject spawObject;
	
	// Use this for initialization
	void Start () {
		//Llamamos al launch este se encargara de invocar a su 
		//respectiva funcion
		LaunchModel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LaunchModel() {

		//Obtenemos el objeto para calcula para aplicar fuerza
		GameObject launched = (GameObject)Instantiate(spawObject);
		//Obtengo el vector del objeto que lleva este script
		Vector3 randPos = transform.localScale*.5f;
		//Recalculo posicion donde sera lanzado
		randPos.x *=(Random.value*2 - 1);
		randPos.y *=(Random.value*2 - 1);
		randPos.z *=(Random.value*2 - 1);
		launched.transform.position = transform.position + randPos;
		launched.GetComponent<Rigidbody>().AddForce(transform.up*(50+100*Random.value));
		//Invoco a funcion para que trabaje con este objeto
		Invoke ("LaunchModel",timeDelay + timeRandom*Random.value);
	}
}
