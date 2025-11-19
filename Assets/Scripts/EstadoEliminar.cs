using UnityEngine;
using UnityEngine.EventSystems; // necesario para detectar UI

public class EstadoEliminar : IEstadoEditor
{
    private GameObject objetoSeleccionado;

    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo ELIMINAR");
        DebugUIManager.Show("Haz clic en un objeto para eliminarlo o en el suelo para salir.");

        if (maquina.popupEliminar != null)
            maquina.popupEliminar.SetActive(false);
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        // Ignorar clics si son sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Si clicas un objeto creado  mostrar popup
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    MostrarPopup(maquina);
                }
                // Si clicas el suelo  salir del estado
                else if (((1 << hit.collider.gameObject.layer) & maquina.capaSuelo) != 0)
                {
                    maquina.CambiarEstado(null);
                }
            }
        }
    }

    private void MostrarPopup(EditorStateMachine maquina)
    {
        if (maquina.popupEliminar == null) return;

        maquina.popupEliminar.SetActive(true);

        maquina.botonConfirmarEliminar.onClick.RemoveAllListeners();
        maquina.botonCancelarEliminar.onClick.RemoveAllListeners();

        maquina.botonConfirmarEliminar.onClick.AddListener(() =>
        {
            if (objetoSeleccionado != null)
            {
                maquina.objetosCreados.Remove(objetoSeleccionado);
                GameObject.Destroy(objetoSeleccionado);
                DebugUIManager.Show("Objeto eliminado: " + objetoSeleccionado.name);

                // Sonido de eliminar
                SoundManager.Instance.PlayEliminar();
            }
            maquina.popupEliminar.SetActive(false);
            objetoSeleccionado = null;
        });

        maquina.botonCancelarEliminar.onClick.AddListener(() =>
        {
            maquina.popupEliminar.SetActive(false);
            objetoSeleccionado = null;
            Debug.Log("Cancelado...");

            // Sonido de cancelar
            SoundManager.Instance.PlayCancelar();
        });
    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo ELIMINAR");
        if (maquina.popupEliminar != null)
            maquina.popupEliminar.SetActive(false);

        objetoSeleccionado = null;
    }
}
