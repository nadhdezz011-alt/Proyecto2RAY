using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    private AudioSource audioSource;

    [Header("Clips de sonido")]
    public AudioClip crearClip;
    public AudioClip moverClip;
    public AudioClip eliminarClip;
    public AudioClip cancelarClip;
    public AudioClip rotarClip;
    public AudioClip botonJugarClip;
    public AudioClip botonMenuClip;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); //  mantiene el objeto entre escenas

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject); // destruye duplicados, no el original
        }
    }


    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // Métodos específicos para cada acción
    public void PlayCrear() => PlaySound(crearClip);
    public void PlayMover() => PlaySound(moverClip);
    public void PlayEliminar() => PlaySound(eliminarClip);
    public void PlayCancelar() => PlaySound(cancelarClip);
    public void PlayRotar() => PlaySound(rotarClip);
    public void PlayBotonJugar() => PlaySound(botonJugarClip);
    public void PlayBotonMenu() => PlaySound(botonMenuClip);
}
