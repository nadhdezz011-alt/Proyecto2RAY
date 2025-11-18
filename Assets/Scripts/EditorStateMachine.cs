using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorStateMachine : MonoBehaviour
{
    public GameObject[] prefabs;
    public Button[] botonesCrear;
    public Button botonRotarY;
    public Button botonMover;
    public Button botonEliminar; 

    public GameObject popupEliminar;
    public Button botonConfirmarEliminar;
    public Button botonCancelarEliminar;

    public LayerMask capaSuelo;
    private IEstadoEditor estadoActual;

    /// <summary>
    /// Guarda el último objeto creado en el editor
    /// </summary>
    public GameObject ultimoObjetoCreado { get; set; }

    /// <summary>
    /// Lista de todos los objetos creados en el editor (con el botón crear)
    /// </summary>
    public List<GameObject> objetosCreados = new List<GameObject>();

    private void Start()
    {
        // Botones de creación
        for (int i = 0; i < botonesCrear.Length; i++)
        {
            int index = i;
            if (botonesCrear[i] != null)
                botonesCrear[i].onClick.AddListener(() => ActivarModoCrear(index));
        }

        // Botón de rotar
        if (botonRotarY != null)
            botonRotarY.onClick.AddListener(ActivarModoRotarY);

        // Botón de mover
        if (botonMover != null)
            botonMover.onClick.AddListener(ActivarModoMover);

        //  Botón de eliminar
        if (botonEliminar != null)
            botonEliminar.onClick.AddListener(ActivarModoEliminar);
    }

    private void Update()
    {
        estadoActual?.Ejecutar(this);
    }

    public void CambiarEstado(IEstadoEditor nuevoEstado)
    {
        estadoActual?.Salir(this);
        estadoActual = nuevoEstado;
        estadoActual?.Entrar(this);
    }

    public void ActivarModoCrear(int index)
    {
        if (index >= 0 && index < prefabs.Length && prefabs[index] != null)
        {
            var estadoCrear = new EstadoModoCrear(prefabs[index], capaSuelo);
            CambiarEstado(estadoCrear);

            
        }
    }


    public void ActivarModoRotarY()
    {
        CambiarEstado(new EstadoRotarY(capaSuelo));
    }

    public void ActivarModoMover()
    {
        CambiarEstado(new EstadoMover(capaSuelo));
    }

    //  Nuevo método para activar el estado eliminar
    public void ActivarModoEliminar()
    {
        CambiarEstado(new EstadoEliminar(
            capaSuelo,
            popupEliminar,
            botonConfirmarEliminar,
            botonCancelarEliminar
        ));
    }

}
