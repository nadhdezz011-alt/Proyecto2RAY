using UnityEngine;

// Estado encargado de crear un objeto en la escena
public class EstadoModoCrear : IEstadoEditor
{
    private GameObject prefab;              
    private LayerMask capaSuelo;            
    private Camera camara;                  
    /// <summary>
    /// Prewiev del objeto antes de confirmar
    /// </summary>
    private GameObject objetoInstanciado;

    /// <summary>
    /// Conserva los parámetros temporales para crear el objeto
    /// </summary>
    public EstadoModoCrear(GameObject prefab, LayerMask capaSuelo)
    {
        this.prefab = prefab;
        this.capaSuelo = capaSuelo;
        this.camara = Camera.main;
    }
    public void Entrar(EditorStateMachine maquina)
    {
        InstanciarObjeto();
    }
    public void Ejecutar(EditorStateMachine maquina)
    {  
        MoverPreviewSobreSuelo();
        if (Input.GetMouseButtonDown(0))
            ConfirmarCreacion(maquina);
    }
    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo crear");
    }

    private void InstanciarObjeto()
    {
      objetoInstanciado = GameObject.Instantiate(prefab);
        if (objetoInstanciado.GetComponent<Collider>() == null)
        {
            objetoInstanciado.AddComponent<BoxCollider>();
        }
    }
    private void MoverPreviewSobreSuelo()
    {
        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayo, out RaycastHit hit, 100f, capaSuelo))
        {
            objetoInstanciado.transform.position = hit.point;
        }
    }
    /// <summary>
    /// Guarda la referencia, elimina el preview, renombra el objeto y sale del estado
    /// </summary>
    private void ConfirmarCreacion(EditorStateMachine maquina)
    {
        GameObject objetoFinal = objetoInstanciado;
        objetoInstanciado = null;
        if (objetoFinal != null)
        {
            objetoFinal.name = prefab.name;
            maquina.ultimoObjetoCreado = objetoFinal;
            maquina.objetosCreados.Add(objetoFinal);
            SoundManager.Instance.PlayCrear();
            maquina.CambiarEstado(null);
        }
    }
}