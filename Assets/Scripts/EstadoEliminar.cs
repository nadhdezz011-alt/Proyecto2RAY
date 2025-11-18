using UnityEngine;
using UnityEngine.EventSystems; //  necesario para detectar UI

public class EstadoEliminar : IEstadoEditor
{
    private LayerMask capaSuelo;
    private GameObject objetoSeleccionado;

    private GameObject popupConfirmacion;
    private UnityEngine.UI.Button botonConfirmar;
    private UnityEngine.UI.Button botonCancelar;

    public EstadoEliminar(LayerMask capaSuelo, GameObject popup, UnityEngine.UI.Button confirmar, UnityEngine.UI.Button cancelar)
    {
        this.capaSuelo = capaSuelo;
        this.popupConfirmacion = popup;
        this.botonConfirmar = confirmar;
        this.botonCancelar = cancelar;
    }

    public void Entrar(EditorStateMachine maquina)
    {
        Debug.Log("Entrando en modo ELIMINAR");
        DebugUIManager.Show("Haz clic en un objeto para eliminarlo o en el suelo para salir.");
        if (popupConfirmacion != null) popupConfirmacion.SetActive(false);
    }

    public void Ejecutar(EditorStateMachine maquina)
    {
        //  Ignorar clics si son sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (maquina.objetosCreados.Contains(hit.collider.gameObject))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    MostrarPopup(maquina);
                }
                else if (((1 << hit.collider.gameObject.layer) & capaSuelo) != 0)
                {
                    maquina.CambiarEstado(null);
                }
            }
        }
    }

    private void MostrarPopup(EditorStateMachine maquina)
    {
        if (popupConfirmacion == null) return;

        popupConfirmacion.SetActive(true);

        botonConfirmar.onClick.RemoveAllListeners();
        botonCancelar.onClick.RemoveAllListeners();

        botonConfirmar.onClick.AddListener(() =>
        {
            if (objetoSeleccionado != null)
            {
                maquina.objetosCreados.Remove(objetoSeleccionado);
                GameObject.Destroy(objetoSeleccionado);
                DebugUIManager.Show("Objeto eliminado: " + objetoSeleccionado.name);

                //  Sonido de eliminar
                SoundManager.Instance.PlayEliminar();
            }
            popupConfirmacion.SetActive(false);
            objetoSeleccionado = null;
        });


        botonCancelar.onClick.AddListener(() =>
        {
            popupConfirmacion.SetActive(false);
            objetoSeleccionado = null;
            Debug.Log("Cancelado...");

            //  Sonido de cancelar
            SoundManager.Instance.PlayCancelar();
        });

    }

    public void Salir(EditorStateMachine maquina)
    {
        Debug.Log("Saliendo del modo ELIMINAR");
        if (popupConfirmacion != null) popupConfirmacion.SetActive(false);
        objetoSeleccionado = null;
    }
}
