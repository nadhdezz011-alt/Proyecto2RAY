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

    bool colocandoObjeto = false;


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
    public void AccionBottonAsset()
    {
        MenuLateral.SetActive(false);
        MenuInferior.SetActive(true);
        CrearObjeto();
    }
    public void CrearObjetoPorIndice(int index)
    {
        if (index >= 0 && index < prefabs.Length)
        {
            asset = Instantiate(prefabs[index]);
            asset.transform.position = Vector3.zero;
            colocandoObjeto = true;
        }
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

    void CrearObjeto()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        asset = Instantiate(prefabs[randomIndex]);
        asset.transform.position = Vector3.zero;
        colocandoObjeto = true;
    }

}
