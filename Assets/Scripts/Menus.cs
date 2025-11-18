using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [Header("Menús")]
    [SerializeField] GameObject MenuInicial;
    [SerializeField] GameObject MenuInferior;
    [SerializeField] GameObject MenuLateral;

    [Header("Animación")]
    [SerializeField] float duracion = 0.5f;

    // Curvas configurables en el Inspector
    [SerializeField] LeanTweenType curvaMovimiento = LeanTweenType.easeOutExpo;
    [SerializeField] LeanTweenType curvaFade = LeanTweenType.easeOutQuad;

    // Posiciones iniciales de cada menú
    private Vector2 posInicialInferior;
    private Vector2 posInicialLateral;

    private void Awake()
    {
        posInicialInferior = MenuInferior.GetComponent<RectTransform>().anchoredPosition;
        posInicialLateral = MenuLateral.GetComponent<RectTransform>().anchoredPosition;
    }

    /// Funciones de los botones
    public void BotonInicio()
    {
        Ocultar(MenuInicial, () => {
            Mostrar(MenuInferior, posInicialInferior);
        });

        SoundManager.Instance.PlayBotonJugar();
    }

    public void AccionBotonCrear()
    {
        Ocultar(MenuInferior, () => {
            Mostrar(MenuLateral, posInicialLateral);
        });

        SoundManager.Instance.PlayBotonMenu();
    }

    public void AccionBotonAsset()
    {
        Ocultar(MenuLateral, () => {
            Mostrar(MenuInferior, posInicialInferior);
        });

        SoundManager.Instance.PlayBotonMenu();
    }

    // ------- Métodos auxiliares --------

    private void Mostrar(GameObject menu, Vector2 destino)
    {
        // Activar sin tocar la escala
        menu.SetActive(true);

        // Movimiento: entrar desde fuera de pantalla a su posición original
        RectTransform rt = menu.GetComponent<RectTransform>();
        Vector2 original = destino;
        rt.anchoredPosition = original + new Vector2(800f, 0f);
        LeanTween.move(rt, original, duracion).setEase(curvaMovimiento);

        // Fade
        CanvasGroup cg = menu.GetComponent<CanvasGroup>();
        if (cg == null) cg = menu.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        LeanTween.alphaCanvas(cg, 1f, duracion).setEase(curvaFade);
    }

    private void Ocultar(GameObject menu, System.Action onComplete)
    {
        // Solo fade de salida. No tocamos la escala.
        CanvasGroup cg = menu.GetComponent<CanvasGroup>();
        if (cg == null) cg = menu.AddComponent<CanvasGroup>();

        LeanTween.alphaCanvas(cg, 0f, duracion).setEase(curvaFade)
            .setOnComplete(() => {
                menu.SetActive(false);
                onComplete?.Invoke();
            });
    }
}
