using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorStateMachine : MonoBehaviour
{
    public GameObject[] prefabs;
    public Button[] botonesCrear;
    public Button botonRotarY;
    public LayerMask capaSuelo;

    private IEstadoEditor estadoActual;

    public GameObject ultimoObjetoCreado { get; set; }

    //  Lista con todos los objetos creados
    public List<GameObject> objetosCreados = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < botonesCrear.Length; i++)
        {
            int index = i;
            if (botonesCrear[i] != null)
                botonesCrear[i].onClick.AddListener(() => ActivarModoCrear(index));
        }

        if (botonRotarY != null)
            botonRotarY.onClick.AddListener(ActivarModoRotarY);
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
}
