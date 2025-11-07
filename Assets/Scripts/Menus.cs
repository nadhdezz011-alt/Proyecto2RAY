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

    int currentPrefab = 0;
    GameObject asset = null;

    //[SerializeField]
    //Material[] materials;
    void Update()
    {
        
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
      
        ComprobarClick();

    }

    //Funciones de creación y manipulación de objetos
    void ComprobarClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            {
                asset = null;
            }
        }
    }
    void MoverObjeto()
    {
        if (asset != null)
        {
            //Posición del ratón Input.mousePosition (Vector3 la posicióndel ratón)
            //Debug.Log(Input.mousePosition);

            //Rayo con info desde la pantalla al punto en el que está el ratón
            //pero el rayo no se detecta por si solo
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Puedo dibujar mi rayo en la escena para interpretar visualmente qué está pasando
            //Esta funcion se le dice el origen, la dirección y el color(*100 son 100m de longitud)
            Debug.DrawRay(myRay.origin, myRay.direction * 100, Color.yellow);

            //Raycasthit es un tipo de dato que almacena la info aobre la colisión (si es que existe el rayo)
            //podré saber la posición, objeto con el que colisiona, etc
            RaycastHit hit;


            asset.SetActive(false); //Desactivo el objeto para que no interfiera en el raycast
            if (Physics.Raycast(myRay, out hit, 100f))
            {
                Debug.Log("Veo algo" + hit.point);
                //colocar el currentGameObject en la posición del hitpoint
                asset.transform.position = hit.point + (Vector3.up);
                //colocar el currentGameObject en la normal del hitpoint (+Vector3.up para que no se hunda en el suelo y le sume altura)
            }
            else
            {
                Debug.Log("No veo nada");
            }
            asset.SetActive(true); //Vuelvo a activar el objeto
        }
    }
    void CrearObjeto()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        asset = Instantiate(prefabs[randomIndex]);
        asset.transform.position = Vector3.zero;
        MoverObjeto();
    }
}
