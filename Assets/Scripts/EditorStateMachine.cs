using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorStateMachine : MonoBehaviour
{
    public GameObject[] prefabs;
    public Button[] botonesCrear;
    public Button botonMover;
    public Button botonRotarY;
    public Button botonEliminar; 
    public GameObject popupEliminar;
    public Button botonConfirmarEliminar;
    public Button botonCancelarEliminar;

    public Button botonEscalar;

    public Button BotonesColores;
    public Material colores;

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


    /// <summary>
    /// Se le asignan las funciones a los botones
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < botonesCrear.Length; i++)
        {
            int index = i;  
            if (botonesCrear[i] != null)   
            {
                botonesCrear[i].onClick.AddListener(() => ActivarModoCrear(index));
            }
        }

        if (botonRotarY != null)
            botonRotarY.onClick.AddListener(ActivarModoRotarY);

        if (botonMover != null)
            botonMover.onClick.AddListener(ActivarModoMover);

        if (botonEliminar != null)
            botonEliminar.onClick.AddListener(ActivarModoEliminar);

        if (botonEscalar != null)
            botonEscalar.onClick.AddListener(ActivarModoEscalar);

        if (botonEscalar != null)
            botonEscalar.onClick.AddListener(ActivarModoEscalar);

        if (botonEscalar != null)
            BotonesColores.onClick.AddListener(ActivarModoColor);

    }
    /// <summary>
    /// En cada frame se ejecuta la lógica del estado activo
    /// </summary>
    private void Update()
    {
        estadoActual?.Ejecutar(this);
    }
    /// <summary>
    /// Permite cambiar a los diferentes estados del editor
    /// </summary>
    public void CambiarEstado(IEstadoEditor nuevoEstado)
    {
        estadoActual?.Salir(this);
        estadoActual = nuevoEstado;
        estadoActual?.Entrar(this);
    }
    /// <summary>
    /// Comprueba que el objeto no sea nulo, se lo pasa al array de prebafs y cambia al estado crear
    /// </summary>
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
    public void ActivarModoEliminar()
    {
        CambiarEstado(new EstadoEliminar());
    }
    public void ActivarModoEscalar()
    {
        CambiarEstado(new EstadoEscalar(capaSuelo));
    }
    public void ActivarModoColor()
    {
        CambiarEstado(new CambiarColor(capaSuelo));
    }
}

