using UnityEngine;

public class EstadoRotarY : IEstadoEditor
{
    private GameObject objeto;
    private float sensibilidad = 100f;
    private bool rotando = false;
    private bool esperandoConfirmacion = false; //  nuevo flag
    private LayerMask capaSuelo;

    public EstadoRotarY(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
    }

    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo ROTAR Y");
        DebugUIManager.Show("Haz clic en un objeto para rotarlo");
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        // Selección de objeto
        if (!rotando && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    rotando = true;
                    esperandoConfirmacion = false; //  aún no confirmamos
                    Debug.Log("Objeto seleccionado: " + objeto.name);
                    DebugUIManager.Show("Arrastra el ratón para rotar. Haz clic de nuevo para confirmar.");
                }
            }
        }

        // Rotación libre con el ratón
        if (rotando && objeto != null)
        {
            float deltaX = Input.GetAxis("Mouse X");
            objeto.transform.Rotate(Vector3.up, deltaX * sensibilidad * Time.deltaTime);

            // Activar confirmación solo después del primer frame de rotación
            if (!esperandoConfirmacion)
            {
                esperandoConfirmacion = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Rotación confirmada en objeto: " + objeto.name);
                maquina.CambiarEstado(null);
                SoundManager.Instance.PlayRotar();
            }
        }
    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo ROTAR Y");
        objeto = null;
        rotando = false;
        esperandoConfirmacion = false;
    }
}
