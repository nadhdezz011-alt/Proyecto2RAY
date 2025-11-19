using UnityEngine;

public class EstadoRotarY : IEstadoEditor
{
    private GameObject objeto;
    private float sensibilidad = 200f;
    private bool rotando = false;
    private bool esperandoConfirmacion = false; //  nuevo flag
    private LayerMask capaSuelo;

    /// <summary>
    /// Recibe el LayerMask de la capa suelo
    /// </summary>
    public EstadoRotarY(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
    }
    public void Entrar(EditorStateMachine maquina)
    {
        DebugUIManager.Show("Haz clic en un objeto para rotarlo");
    }
    /// <summary>
    /// Si no se está rotando y se hace click, se lanza un rayo, si colisiona un objeto creado, se guarda y selecciona para rotar sin confirmar.
    /// Calcula el giro en y al mover el ratón, se espera confirmación tras el primer frame de rotación y al hacer click se confirma la rotación
    /// </summary>
    public void Ejecutar(EditorStateMachine maquina)
    {
        if (!rotando && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    rotando = true;
                    esperandoConfirmacion = false;
                    DebugUIManager.Show("Arrastra el ratón para rotar. Haz clic de nuevo para confirmar.");
                }
            }
        }
        if (rotando && objeto != null)
        {
            float GiroY = Input.GetAxis("Mouse X");
            objeto.transform.Rotate(Vector3.up, GiroY * sensibilidad * Time.deltaTime);

            if (!esperandoConfirmacion)
            {
                esperandoConfirmacion = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Rotación confirmada en objeto: " + objeto.name);
                SoundManager.Instance.PlayRotar();
                maquina.CambiarEstado(null);
            }
        }
    }
    /// <summary>
    /// elimina referencias y resetea los estados
    /// </summary>
    public void Salir(EditorStateMachine maquina)
    {
        objeto = null;
        rotando = false;
        esperandoConfirmacion = false;
    }
}
