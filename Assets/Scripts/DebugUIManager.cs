using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField] private GameObject contenedor;       // Panel/imagen que contiene el texto y botón
    [SerializeField] private TextMeshProUGUI debugText;   // Texto dentro del contenedor
    [SerializeField] private Button botonEliminar;        // Botón para limpiar

    [Header("Animación")]
    [SerializeField] private float duracion = 0.5f;
    [SerializeField] private LeanTweenType curvaMovimiento = LeanTweenType.easeOutExpo;
    [SerializeField] private LeanTweenType curvaFade = LeanTweenType.easeOutQuad;

    private static DebugUIManager instance;
    private Vector2 posInicial;

    private void Awake()
    {
        instance = this;

        if (contenedor != null)
        {
            contenedor.SetActive(false);
            posInicial = contenedor.GetComponent<RectTransform>().anchoredPosition;
        }

        if (botonEliminar != null)
            botonEliminar.onClick.AddListener(() => LimpiarMensaje());
    }

    public static void Show(string message)
    {
        if (instance != null)
            instance.MostrarMensaje(message);

        Debug.Log(message); // Opcional
    }

    private void MostrarMensaje(string message)
    {
        if (debugText != null)
            debugText.text = message;

        if (contenedor != null)
        {
            contenedor.SetActive(true);

            // Movimiento desde fuera de pantalla hacia su posición original
            RectTransform rt = contenedor.GetComponent<RectTransform>();
            rt.anchoredPosition = posInicial + new Vector2(600f, 0f);
            LeanTween.move(rt, posInicial, duracion).setEase(curvaMovimiento);

            // Fade
            CanvasGroup cg = contenedor.GetComponent<CanvasGroup>();
            if (cg == null) cg = contenedor.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            LeanTween.alphaCanvas(cg, 1f, duracion).setEase(curvaFade);
        }
    }

    private void LimpiarMensaje()
    {
        if (debugText != null)
            debugText.text = "";

        if (contenedor != null)
        {
            CanvasGroup cg = contenedor.GetComponent<CanvasGroup>();
            if (cg == null) cg = contenedor.AddComponent<CanvasGroup>();

            // Fade out y luego desactivar
            LeanTween.alphaCanvas(cg, 0f, duracion).setEase(curvaFade)
                .setOnComplete(() => contenedor.SetActive(false));
        }
    }
}
