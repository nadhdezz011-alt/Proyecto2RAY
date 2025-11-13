using UnityEngine;

/// <summary>
/// Controlador de la interfaz. Llama al EditorManager según los botones pulsados.
/// </summary>
public class UIController : MonoBehaviour
{
    public EditorManager manager;

    public void CrearObjeto(int index)
    {
        manager.BotonCrear(index);
    }

    public void ActivarMover()
    {
        manager.BotonMover();
    }

    public void ActivarRotar()
    {
        manager.BotonRotar();
    }

    public void ActivarEliminar()
    {
        manager.BotonEliminar();
    }
}