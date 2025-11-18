using UnityEngine;

// Estado encargado de crear un objeto en la escena
public class EstadoModoCrear : IEstadoEditor
{
    private GameObject prefab;              // Prefab a instanciar
    private LayerMask capaSuelo;            // Capa para raycast del suelo
    private Camera camara;                  // Cámara principal
    private GameObject objetoInstanciado;   // Preview antes de confirmar

    public EstadoModoCrear(GameObject prefab, LayerMask capaSuelo)
    {
        this.prefab = prefab;
        this.capaSuelo = capaSuelo;
        this.camara = Camera.main;

        if (this.prefab == null)
            Debug.LogError("Constructor recibió prefab nulo");
    }

    // Al entrar en el estado: instanciamos el preview
    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo CREAR");
        InstanciarObjeto();
    }

    // En cada frame: movemos el preview y confirmamos con clic
    public void Ejecutar(EditorStateMachine maquina)
    {  
        MoverPreviewSobreSuelo();
        if (Input.GetMouseButtonDown(0))
            ConfirmarCreacion(maquina);
    }

    // Al salir del estado
    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo CREAR");
    }

    // ---------------- FUNCIONES AUXILIARES ----------------

    // Instancia el prefab como preview
    // Instancia el prefab como preview
    private void InstanciarObjeto()
    {
        if (prefab == null) return;

        objetoInstanciado = GameObject.Instantiate(prefab);

        if (objetoInstanciado == null)
        {
            Debug.LogError("Falló la instanciación");
            return;
        }

        //  Asegurarse de que tiene collider
        if (objetoInstanciado.GetComponent<Collider>() == null)
        {
            objetoInstanciado.AddComponent<BoxCollider>();
        }
    }


    // Coloca el preview bajo el ratón sobre el suelo
    private void MoverPreviewSobreSuelo()
    {
        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayo, out RaycastHit hit, 100f, capaSuelo))
        {
            objetoInstanciado.transform.position = hit.point;
        }
    }

    // Confirma la creación: renombra, guarda referencia y sale del estado
    private void ConfirmarCreacion(EditorStateMachine maquina)
    {
        GameObject objetoFinal = objetoInstanciado;
        objetoInstanciado = null;

        if (objetoFinal != null)
        {
            objetoFinal.name = prefab.name;

            maquina.ultimoObjetoCreado = objetoFinal;

            //  Añadir a la lista de objetos creados
            maquina.objetosCreados.Add(objetoFinal);
            //  Sonido de crear
            SoundManager.Instance.PlayCrear();
            maquina.CambiarEstado(null);
        }
    }

}
