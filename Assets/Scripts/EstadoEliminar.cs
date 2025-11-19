using UnityEngine;
using UnityEngine.EventSystems;

public class EstadoEliminar : IEstadoEditor
{
    private GameObject objetoSeleccionado;
    public void Entrar(EditorStateMachine maquina)
    {
        DebugUIManager.Show("Haz clic en un objeto para eliminarlo o en el suelo para salir.");

        if (maquina.popupEliminar != null)
            maquina.popupEliminar.SetActive(false);
    }
    /// <summary>
    ///  Ignorar clics si son sobre UI. 
    ///  Si clicas un objeto creado  mostrar popup. 
    ///  Si clicas el suelo  salir del estado.
    /// </summary>
    public void Ejecutar(EditorStateMachine maquina)
    {
       
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
                SoundManager.Instance.PlayEliminar();
            }
            maquina.popupEliminar.SetActive(false);
            objetoSeleccionado = null;
        });

        maquina.botonCancelarEliminar.onClick.AddListener(() =>
        {
            maquina.popupEliminar.SetActive(false);
            objetoSeleccionado = null;
            SoundManager.Instance.PlayCancelar();
        });
    }
    public void Salir(EditorStateMachine maquina)
    {
        if (maquina.popupEliminar != null)
            maquina.popupEliminar.SetActive(false);

        objetoSeleccionado = null;
    }
}
/*
 * maquina.botonConfirmarEliminar.onClick.AddListener(() =>
{
    if (objetoSeleccionado != null)
    {
        // Recuperar dinero al vender
        ObjetoInfo info = objetoSeleccionado.GetComponent<ObjetoInfo>();
        if (info != null)
        {
            maquina.dinero += info.precioVenta;
            Debug.Log("Vendiste " + objetoSeleccionado.name + " por " + info.precioVenta +
                      ". Dinero actual: " + maquina.dinero);
        }

        maquina.objetosCreados.Remove(objetoSeleccionado);
        GameObject.Destroy(objetoSeleccionado);
        SoundManager.Instance.PlayEliminar();
    }
    maquina.popupEliminar.SetActive(false);
    objetoSeleccionado = null;
});
*/