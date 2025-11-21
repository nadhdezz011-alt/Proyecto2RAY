using UnityEngine;

public class CambiarColor : IEstadoEditor
{
    private GameObject objeto;
    private bool pintando = false;
    private LayerMask capaSuelo;
    private Camera camara;
    public Material material;

    public CambiarColor(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
        this.camara = Camera.main;
    }
    public void Entrar(EditorStateMachine maquina)
    {
        DebugUIManager.Show("Haz clic en un objeto para cambiar su color");
    }
    public void Ejecutar(EditorStateMachine maquina)
    {
        if (!pintando && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    pintando = true;
                    DebugUIManager.Show("Selecciona un color para el objeto.");
                }
            }
        }
        if (pintando && objeto != null)
        {
           if (Input.GetMouseButtonDown(0))
           {
                maquina.colores = material;
                objeto.GetComponent<MeshRenderer>().material = material;
           }
        }
    }
    public void Salir(EditorStateMachine maquina) 
    {
        Debug.Log("Saliendo del modo color");
        maquina.CambiarEstado(null);
    }
}























