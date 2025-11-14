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
        Debug.Log("Entrando en modo MOVER");
        Debug.Log("Haz clic en un objeto para moverlo");
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        // Selección de objeto al pulsar botón izquierdo
        if (!moviendo && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objeto = hit.collider.gameObject;
                    moviendo = true;
                    Debug.Log("Objeto seleccionado: " + objeto.name);
                    Debug.Log("Mantén pulsado el botón y mueve el ratón. Suelta para confirmar.");
                }
                else if (((1 << hit.collider.gameObject.layer) & capaSuelo) != 0)
                {
                    // Si clicas directamente en el suelo sin objeto  salir
                    maquina.CambiarEstado(null);
                }
            }
        }

        // Mientras mantienes pulsado el botón izquierdo, el objeto sigue al ratón
        if (moviendo && objeto != null && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, capaSuelo))
            {
                objeto.transform.position = hit.point;
            }
        }

        // Al soltar el botón izquierdo, confirmamos la posición
        if (moviendo && objeto != null && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Movimiento confirmado en objeto: " + objeto.name);
            objeto = null;
            moviendo = false;
            Debug.Log("Haz clic en otro objeto para moverlo o en el suelo para salir.");
        }
    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo MOVER");
        objeto = null;
        moviendo = false;
    }
}
