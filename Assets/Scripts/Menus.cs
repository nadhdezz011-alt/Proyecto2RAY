using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [Header("Menús")]
    [SerializeField] GameObject MenuInicial;
    [SerializeField] GameObject MenuInferior;
    [SerializeField] GameObject MenuLateral;
    [SerializeField] GameObject MenuColores;

    [Header("Animación")]
    [SerializeField] float duracion = 0.5f;

    /// <summary>
    /// Curva de movimiento para las transiciones de los menús
    /// </summary>
    [SerializeField] LeanTweenType curvaMovimiento = LeanTweenType.easeOutExpo;
    [SerializeField] LeanTweenType curvaFade = LeanTweenType.easeOutQuad;
    [SerializeField] LeanTweenType curva = LeanTweenType.easeInBack;

    /// <summary>
    /// Posiciones iniciales de los menús para las animaciones
    /// </summary>
    private Vector2 posInicialInferior;
    private Vector2 posInicialLateral;
    private Vector2 posInicialColor;
    private void Awake()
    {
        posInicialInferior = MenuInferior.GetComponent<RectTransform>().anchoredPosition;
        posInicialLateral = MenuLateral.GetComponent<RectTransform>().anchoredPosition;
        posInicialColor = MenuColores.GetComponent<RectTransform>().anchoredPosition;
    }

    /// <summary>
    /// Funciones de los botones
    /// </summary>
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
    public void AccionBotonColor()
    {

        Mostrar(MenuColores, posInicialLateral);
    }

    /// <summary>
    /// muestra el menú con animación
    /// </summary>
    private void Mostrar(GameObject menu, Vector2 destino)
    {
        menu.SetActive(true);

        RectTransform rt = menu.GetComponent<RectTransform>();
        Vector2 original = destino;
        rt.anchoredPosition = original + new Vector2(800f, 0f);
        LeanTween.move(rt, original, duracion).setEase(curvaMovimiento);

        CanvasGroup cg = menu.GetComponent<CanvasGroup>();
        if (cg == null) cg = menu.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        LeanTween.alphaCanvas(cg, 1f, duracion).setEase(curvaFade);
    }

    /// <summary>
    /// Oculta el menú con animación
    /// </summary>
    private void Ocultar(GameObject menu, System.Action onComplete)
    {
        CanvasGroup cg = menu.GetComponent<CanvasGroup>();
        if (cg == null) cg = menu.AddComponent<CanvasGroup>();

        LeanTween.alphaCanvas(cg, 0f, duracion).setEase(curvaFade)
            .setOnComplete(() => {
                menu.SetActive(false);
                onComplete?.Invoke();
            });
    }
}
