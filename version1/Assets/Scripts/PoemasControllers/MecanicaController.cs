using UnityEngine;
using System.Collections;

public class MecanicaController : MonoBehaviour
{

    private ControladorPoemas _controladorPoema;
    public TextMesh Intentos;
    private int _intentos = 3;

    void Awake()
    {
        _controladorPoema = GetComponent<ControladorPoemas>();
        UpdateTextMesh(Intentos, _intentos.ToString());
    }

    private void UpdateTextMesh(TextMesh t, string n)
    {
        t.text = n;
    }

    public void Check()//Chequea el estado de las Lineas del poema
    {
        _controladorPoema._poemaactual++;
        _controladorPoema.SetUI();
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
