using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador principal del editor. Gestiona los estados y la lógica de interacción.
/// </summary>
public class EditorManager : MonoBehaviour
{
    [Header("Prefabs disponibles para crear")]
    public GameObject[] prefabs;

    [Header("Referencia al suelo (con collider)")]
    public GameObject suelo;

    [Header("Texto UI para mostrar instrucciones")]
    public Text mensajeUI;

    [HideInInspector] public GameObject objetoActivo;

    private IEstadoEditor estadoActual;

    void Update()
    {
        if (estadoActual != null)
        {
            estadoActual.Ejecutar(this);
        }
    }

    /// <summary>
    /// Cambia el estado actual del editor.
    /// </summary>
    /// <param name="nuevoEstado">Nuevo estado a activar.</param>
    public void CambiarEstado(IEstadoEditor nuevoEstado)
    {
        if (estadoActual != null)
        {
            estadoActual.Salir(this);
        }

        estadoActual = nuevoEstado;

        if (estadoActual != null)
        {
            estadoActual.Entrar(this);
        }
    }

    // Métodos públicos que se conectan con los botones de la UI

    /// <summary>
    /// Instancia un objeto y entra en modo colocar.
    /// </summary>
    /// <param name="index">Índice del prefab a crear.</param>
    public void BotonCrear(int index)
    {
        if (index >= 0 && index < prefabs.Length)
        {
            objetoActivo = Instantiate(prefabs[index]);
            objetoActivo.tag = "Modificable"; // Asegura que puede ser manipulado
            CambiarEstado(new EstadoCrear());
        }
    }

    /// <summary>
    /// Activa el modo mover.
    /// </summary>
    public void BotonMover()
    {
        CambiarEstado(new EstadoMover());
    }

    /// <summary>
    /// Activa el modo rotar.
    /// </summary>
    public void BotonRotar()
    {
        CambiarEstado(new EstadoRotar());
    }

    /// <summary>
    /// Activa el modo eliminar.
    /// </summary>
    public void BotonEliminar()
    {
        CambiarEstado(new EstadoEliminar());
    }
}
