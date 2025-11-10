using UnityEditor.VersionControl;
using UnityEngine;

public class CreadorDeObjetosConRaton : MonoBehaviour
 
{
    [SerializeField]
    GameObject[] prefabs;

    int currentPrefab = 0;
    GameObject currentGameObject = null;

    [SerializeField]
    Material[] materials;
    
    void Update()
    {

        ComprobarTeclado();
        
        MoverObjeto();
        CambiarColor();
        ComprobarClick();


    }
    void CambiarColor()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            currentGameObject.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }
    void MoverObjeto()
    {
        if (currentGameObject != null)
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


            currentGameObject.SetActive(false); //Desactivo el objeto para que no interfiera en el raycast
            if (Physics.Raycast(myRay, out hit, 100f))
            {
                Debug.Log("Veo algo" + hit.point);
                //colocar el currentGameObject en la posición del hitpoint
                currentGameObject.transform.position = hit.point + (Vector3.up);
                //colocar el currentGameObject en la normal del hitpoint (+Vector3.up para que no se hunda en el suelo y le sume altura)
            }
            else
            {
                Debug.Log("No veo nada");
            }
            currentGameObject.SetActive(true); //Vuelvo a activar el objeto
        }
    }
    
    void ComprobarTeclado()
    {
        int oldNumber = currentPrefab;
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            currentPrefab = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            currentPrefab = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Keypad3))
        {
            currentPrefab = 2;
        }

        if (currentGameObject != null && oldNumber != currentPrefab)
        {
            DestroyImmediate(currentGameObject);
        }
    }
   
    void ComprobarClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            {
                currentGameObject = null;
            }
        }
    }
   

}
