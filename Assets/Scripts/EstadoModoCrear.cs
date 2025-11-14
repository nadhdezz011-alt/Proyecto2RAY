using UnityEngine;

public class EstadoModoCrear : IEstadoEditor
{
    private GameObject prefab;
    private LayerMask capaSuelo;
    private Camera camara;
    private GameObject objetoInstanciado;

    public EstadoModoCrear(GameObject prefab, LayerMask capaSuelo)
    {
        this.prefab = prefab;
        this.capaSuelo = capaSuelo;
        this.camara = Camera.main;

        if (this.prefab == null)
        {
            Debug.LogError("Constructor recibió prefab nulo");
        }
    }

    public void Entrar(EditorStateMachine ModoCrear)
    {
        Debug.Log("Entrando en modo CREAR");

        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado en Entrar()");
            return;
        }

        objetoInstanciado = GameObject.Instantiate(prefab);
        if (objetoInstanciado == null)
        {
            Debug.LogError("Falló la instanciación");
            return;
        }

        objetoInstanciado.name = "Preview_" + prefab.name;
    }

    public void Ejecutar(EditorStateMachine ModoCrear)
    {
        if (objetoInstanciado == null)
        {
            Debug.LogWarning("objetoInstanciado es null en Ejecutar");
            return;
        }

        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayo, out RaycastHit hit, 100f, capaSuelo))
        {
            objetoInstanciado.transform.position = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject objetoFinal = objetoInstanciado;
            objetoInstanciado = null; //  primero anulamos
            ModoCrear.CambiarEstado(null); //  luego salimos del estado

            if (objetoFinal != null)
            {
                objetoFinal.name = prefab.name;
            }

        }
    }

    public void Salir(EditorStateMachine ModoCrear)
    {
        Debug.Log("Saliendo del modo CREAR");
    }
}