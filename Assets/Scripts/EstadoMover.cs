using UnityEngine;

public class EstadoMover : IEstadoEditor
{
    private GameObject objeto;
    private bool moviendo = false;
    private LayerMask capaSuelo;

    public EstadoMover(LayerMask capaSuelo)
    {
        this.capaSuelo = capaSuelo;
    }
    public void Entrar(EditorStateMachine maquina)
    {
        DebugUIManager.Show("Haz clic en un objeto para moverlo");
    }
    /// <summary>
    /// Si no está moviendo y se hace click, se lanza un rayo, si colisiona un objeto creado, se guarda y selecciona para mover., si selecciona el suelo no se confirma.
    /// </summary>
    public void Ejecutar(EditorStateMachine maquina)
    {
        if (!moviendo && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    moviendo = true;
                    SoundManager.Instance.PlayMover();
                    DebugUIManager.Show("Mantén pulsado el botón y mueve el ratón. Suelta para confirmar.");
                }
                else if (((1 << hit.collider.gameObject.layer) & capaSuelo) != 0)
                {
                    maquina.CambiarEstado(null);
                }
            }
        }
        if (moviendo && objeto != null && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, capaSuelo))
            {
                objeto.transform.position = hit.point;
            }
        }
        if (moviendo && objeto != null && Input.GetMouseButtonUp(0))
        {
            SoundManager.Instance.PlayMover();
            objeto = null;
            moviendo = false;
            Debug.Log("Haz clic en otro objeto para moverlo o en el suelo para salir.");
        }

    }
    public void Salir(EditorStateMachine maquina)
    {
        objeto = null;
        moviendo = false;
    }
}
