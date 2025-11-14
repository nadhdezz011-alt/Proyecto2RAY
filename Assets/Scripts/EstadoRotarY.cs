using UnityEngine;

public class EstadoRotarY : IEstadoEditor
{
    private GameObject objeto;
    private float sensibilidad = 100f;
    private bool rotando = false;
    private bool objetoSeleccionado = false; //  nuevo flag
    private LayerMask capaSuelo;

    public EstadoRotarY(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
    }

    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo ROTAR Y");
        Debug.Log("Haz clic en un objeto para rotarlo");
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        // Selección de objeto (solo si aún no estamos rotando)
        if (!rotando && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log("Raycast golpeó: " + hit.collider.gameObject.name);

                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    rotando = true;
                    Debug.Log("Objeto seleccionado: " + objeto.name);
                }
            }
        }


        // Rotación libre con el ratón
        if (rotando && objeto != null)
        {
            float deltaX = Input.GetAxis("Mouse X");
            objeto.transform.Rotate(Vector3.up, deltaX * sensibilidad * Time.deltaTime);
        }

        // Finalizar al clicar en suelo (solo después de haber seleccionado)
        if (rotando && objetoSeleccionado && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, capaSuelo))
            {
                Debug.Log("Rotación finalizada en el suelo");
                maquina.CambiarEstado(null);
            }
        }
    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo ROTAR Y");
        objeto = null;
        rotando = false;
        objetoSeleccionado = false;
    }
}
