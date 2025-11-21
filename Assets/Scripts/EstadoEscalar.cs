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
        DebugUIManager.Show("Haz clic en un objeto para escalarlo");
    }

    /// <summary>
    /// Selecciona un objeto al hacer click si no está escalando.
    /// </summary>
    public void Ejecutar(EditorStateMachine maquina)
    {
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
                    DebugUIManager.Show("Arrastra el ratón verticalmente para escalar. Haz clic de nuevo para confirmar.");
                }
            }
        }
        
        if (escalando && objeto != null)
        {
            float deltaY = Input.mousePosition.y - posicionInicialMouse.y;
            float factor = 1 + deltaY * 0.01f;

            if (factor < 1f)
            {
                factor = 1f;
            }
            if (factor > 3f)
            {
                factor = 3f;
            }

            objeto.transform.localScale = escalaInicial * factor;

            if (!esperandoConfirmacion)
            {
                esperandoConfirmacion = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                maquina.CambiarEstado(null);
            }
        }
    }
    public void Salir(EditorStateMachine maquina)
    {
        objeto = null;
        escalando = false;
        esperandoConfirmacion = false;
    }
}
