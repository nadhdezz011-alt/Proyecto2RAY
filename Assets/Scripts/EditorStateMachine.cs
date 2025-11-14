using UnityEngine;
using UnityEngine.UI;

public class EditorStateMachine : MonoBehaviour
{
    public GameObject[] prefabs;         // Array de prefabs para instanciar
    public Button[] botonesCrear;        // Botones que activan cada prefab
    public LayerMask capaSuelo;          // Capa para raycast en el suelo

    private IEstadoEditor estadoActual;

    private void Start()
    {
        // Asignar listeners a cada botón según su índice
        for (int i = 0; i < botonesCrear.Length; i++)
        {
            int index = i; // Captura segura del índice
            if (botonesCrear[i] != null)
            {
                botonesCrear[i].onClick.AddListener(() => ActivarModoCrear(index));
            }
        }
    }

    private void Update()
    {
        if (estadoActual != null)
        {
            estadoActual.Ejecutar(this);
        }
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
        else
        {
            Debug.LogError("Índice de prefab inválido o prefab no asignado");
        }
    }
}