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

    [SerializeField]
    GameObject[] prefabs;
    GameObject objetoSeleccionado = null;

    bool colocandoObjeto = false;
    bool modoMover = false;
   
    [SerializeField] Text mensajeUI; // Asigna un Text en el canvas para mostrar instrucciones


    int currentPrefab = 0;
    GameObject asset = null;

    //[SerializeField]
    //Material[] materials;
    void Update()
    {
        if (colocandoObjeto && asset != null)
        {
            MoverObjeto();

            if (Input.GetMouseButtonDown(0))
            {
                colocandoObjeto = false;
                asset = null;
            }
        }
    
        {
            if (modoMover)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.CompareTag("Movible"))
                        {
                            objetoSeleccionado = hit.collider.gameObject;
                            mensajeUI.text = "Mueve el ratón y suelta el clic para colocar.";
                        }
                        else
                        {
                            // Clic en el suelo u otro objeto -> salir del modo
                            modoMover = false;
                            objetoSeleccionado = null;
                            mensajeUI.text = "Modo mover desactivado.";
                        }
                    }
                }

                if (Input.GetMouseButton(0) && objetoSeleccionado != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    objetoSeleccionado.SetActive(false); // evitar interferencia
                    if (Physics.Raycast(ray, out hit))
                    {
                        objetoSeleccionado.transform.position = hit.point + Vector3.up;
                    }
                    objetoSeleccionado.SetActive(true);
                }

                if (Input.GetMouseButtonUp(0) && objetoSeleccionado != null)
                {
                    objetoSeleccionado = null;
                    mensajeUI.text = "Haz clic en otro objeto o en el suelo para salir.";
                }
            }
        }

    }
    //Funcines de los botones
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

    public void CrearObjetoPorIndice(int index)
    {
        if (index >= 0 && index < prefabs.Length)
        {
            asset = Instantiate(prefabs[index]);
            asset.transform.position = Vector3.zero;
            colocandoObjeto = true;
            MenuInferior.SetActive(true);
            MenuLateral.SetActive(false);
        }
    }
    public void ActivarModoMover()
    {
        modoMover = true;
        objetoSeleccionado = null;
        mensajeUI.text = "Haz clic en un objeto para moverlo.";
    }


    //Funciones de creación y manipulación de objetos

    void MoverObjeto()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        asset.SetActive(false);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Bounds bounds = asset.GetComponent<Renderer>().bounds;
            float altura = bounds.extents.y;
            asset.transform.position = hit.point + new Vector3(0, altura, 0);
        }
        asset.SetActive(true);
    }

   

}
