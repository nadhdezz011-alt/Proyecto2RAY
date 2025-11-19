using UnityEngine;

public class EstadoEscalar : IEstadoEditor
{
    private GameObject objeto;
    private bool escalando = false;
    private bool esperandoConfirmacion = false;
    private LayerMask capaSuelo;
    private Vector3 escalaInicial;
    private Vector3 posicionInicialMouse;

    public EstadoEscalar(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
    }

    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo ESCALAR");
        DebugUIManager.Show("Haz clic en un objeto para escalarlo");
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        // Selección de objeto
        if (!escalando && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    escalando = true;
                    esperandoConfirmacion = false;
                    escalaInicial = objeto.transform.localScale;
                    posicionInicialMouse = Input.mousePosition;

                    Debug.Log("Objeto seleccionado: " + objeto.name);
                    DebugUIManager.Show("Arrastra el ratón horizontalmente para escalar. Haz clic de nuevo para confirmar.");
                }
            }
        }

        // Escalado dinámico con el ratón
        if (escalando && objeto != null)
        {
            float deltaX = Input.mousePosition.x - posicionInicialMouse.x;
            float factor = 1 + deltaX * 0.01f; // sensibilidad del escalado
            objeto.transform.localScale = escalaInicial * Mathf.Max(factor, 0.1f);

            // Activar confirmación después del primer frame
            if (!esperandoConfirmacion)
            {
                esperandoConfirmacion = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Escalado confirmado en objeto: " + objeto.name);
                maquina.CambiarEstado(null);
                
            }
        }
    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo ESCALAR");
        objeto = null;
        escalando = false;
        esperandoConfirmacion = false;
    }
}
