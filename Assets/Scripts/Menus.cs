using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [SerializeField]
    GameObject MenuInicial;
    [SerializeField]
    GameObject MenuInferior;
    [SerializeField]
    GameObject MenuLateral;

    ///Funcines de los botones
    public void BotonInicio() 
    { 
        MenuInicial.SetActive(false);
        MenuInferior.SetActive(true);
    }
    public void AccionBotonCrear()
    {
        MenuInferior.SetActive(false);
        MenuLateral.SetActive(true);
        
    }
    public void AccionBotonAsset()
    {
        MenuInferior.SetActive(true);
        MenuLateral.SetActive(false);

    }


}
