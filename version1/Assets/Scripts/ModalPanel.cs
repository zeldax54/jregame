using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
public class ModalPanel : MonoBehaviour {

    //Empesamos declarando el los componentes que tendra 
    public GameObject ModalPanelObjeto;//Objeto panel de la UI
    public Text TextUI;//Aqui se muestra el mensaje que voy a poner en la ventana
    private static ModalPanel modalPanel;//Objeto privado usado para tener una instancia del panel 
    private bool activo;//Esto es para controlar cuando el panel esta activo
    public Sprite ok;
    public Sprite si;
  
    public static ModalPanel Instance()//Constructor
    {
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!modalPanel)
                Debug.LogError("No hay paneles en tu escena.");
        }

        return modalPanel;
    }

    //Metodo para llamar al panel recibe el texto , el event que seria el metodo a ejecutar en caso de aceptar
    //El boton aceptar y el boton cancelar
    public void Elejir(string pregunta, UnityAction yesEvent, Button aceptar, Button cancelar, bool bandera, Button help = null)
    {
        ModalPanelObjeto.SetActive(true);//Activo el panel porque inicialmente tiene que estar desactivado para que no se muestre en la escena
        TextUI.text = pregunta;//Le asigno al texto de la ui la pregunta que le mando al llamar a este metodo
        if (help != null && help.IsActive()) help.gameObject.SetActive(false);

        //Asigno los eventos al boton cancelar para que cierre el panel este lo hago aqui porque el simple 
        //Pero los eventos para el metodo de aceptar los hago en el otro script porque tengo que usar variables de ese script
        cancelar.onClick.RemoveAllListeners();
        cancelar.onClick.AddListener(CerrarPanel);
        //Ahora para los eventos de aceptar paso el UnityAction y al llamarlo le asigno un metodo en el otro script
        //O sea yesEvent=Metodo en el otro script
        aceptar.onClick.RemoveAllListeners();
        if (yesEvent != null)
        {
            aceptar.onClick.RemoveAllListeners();
            aceptar.onClick.AddListener(yesEvent);
        }
        aceptar.gameObject.SetActive(true);
        aceptar.GetComponentInChildren<Image>().sprite = si;
        if (bandera) //Esto es para los paneles informativos dejarle un solo boton
        {
            cancelar.gameObject.SetActive(false);
            aceptar.GetComponentInChildren<Image>().sprite = ok;
          
        }
        else
        {

            cancelar.gameObject.SetActive(true);
        }

        activo = true;
    }


    public void ChoiseHelp(string pregunta, UnityAction yesEvent, Button  aceptar,Button cancelar,Button help)
    {


        ModalPanelObjeto.SetActive(true);//Activo el panel porque inicialmente tiene que estar desactivado para que no se muestre en la escena
        TextUI.text = pregunta;//Le asigno al texto de la ui la pregunta que le mando al llamar a este metodo
        aceptar.gameObject.SetActive(false);
        cancelar.gameObject.SetActive(true);
        cancelar.onClick.RemoveAllListeners();
        cancelar.onClick.AddListener(CerrarPanel);
        //////////////
        help.gameObject.SetActive(true);
        help.onClick.RemoveAllListeners();
        help.onClick.AddListener(yesEvent);
        activo = true;
    }

    //Este es para Mostrar solo mensajes cuando llamo al metodo de las adivinanzas.
    public void MostrarMsg(string pregunta, Button aceptar, Button cancelar, Button help)
    {
        ModalPanelObjeto.SetActive(true);//Activo el panel porque inicialmente tiene que estar desactivado para que no se muestre en la escena
        TextUI.text = pregunta;//Le asigno al texto de la ui la pregunta que le mando al llamar a este metodo
        cancelar.gameObject.SetActive(false);
        help.gameObject.SetActive(true);
        aceptar.gameObject.SetActive(false);
        help.onClick.RemoveAllListeners();
        help.onClick.AddListener(CerrarPanel);
        help.GetComponentInChildren<Image>().sprite = ok;
        
    }

    public void CerrarPanel()
    {
        activo = false;
        ModalPanelObjeto.SetActive(false);//Cierro el Panel
    }

    public bool IsActivo()
    {
        return activo;
    }

    private void ActivarBotones(Button aceptar, Button cancelar, Button help)
    {
        cancelar.gameObject.SetActive(true);
        help.gameObject.SetActive(true);
        aceptar.gameObject.SetActive(true);
    }

}
