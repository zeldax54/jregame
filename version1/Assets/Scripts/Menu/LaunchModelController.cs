using UnityEngine;
using System.Collections;

/*
Controlo el objeto lanzado
 */
public class LaunchModelController : MonoBehaviour {

	//Vida que tendra el objeto
	public float vida = 1;
	//Minimo de vida 
	public float minVida = 1;
	//Posicion donde se sobrescribe
	//private Vector3 originalScale;

	// Use this for initialization
	void Start () {
	//	originalScale = transform.localScale;
		if (transform.childCount > 0) {
			GameObject child = transform.GetChild(Mathf.FloorToInt(Random.value * (transform.childCount))).gameObject;
			child.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {

		//Decrementamos
		vida -= Time.deltaTime;

		//Escala los objetos cuando caen
		/*if (vida < minVida) {
			transform.localScale  = originalScale * (vida/minVida);
		}
*/
		//Si es menor o igual a 0, destruyo el objeto que hizo la llamada
		if (vida <= 0)
			Destroy(gameObject);
	}
}
