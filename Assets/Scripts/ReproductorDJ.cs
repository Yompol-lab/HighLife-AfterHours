using UnityEngine;
using UnityEngine.UI; // Necesario para modificar imágenes de la interfaz

public class ReproductorDJ : MonoBehaviour
{
    [Header("Configuración Visual Play/Pausa")]
    public Image imagenBotonPlayPause; // Arrastra aquí el componente Image del botón central
    public Sprite iconoPlay;           // Arrastra aquí tu imagen de la flechita de Play
    public Sprite iconoPause;          // Arrastra aquí tu imagen de las dos rayitas de Pausa

    // Asumimos que la música arranca reproduciéndose
    private bool estaReproduciendo = true;

    // --- BOTÓN SPOTIFY ---
    public void AbrirSpotify()
    {
        Debug.Log("Abriendo aplicación de Spotify...");
        Application.OpenURL("https://open.spotify.com");

        // Opcional: Pausar Wwise automáticamente al abrir Spotify
        if (estaReproduciendo) AlternarPlayPause();
    }

    // --- BOTONES LATERALES (Wwise) ---
    public void SiguienteCancion()
    {
        Debug.Log("WWISE: Pasar a la siguiente canción");
        // Acá tu compañero agregará: AkSoundEngine.PostEvent("Siguiente", gameObject);
    }

    public void AnteriorCancion()
    {
        Debug.Log("WWISE: Volver a la canción anterior");
        // Acá tu compañero agregará el evento de Wwise para retroceder
    }

    // --- BOTÓN CENTRAL (Play / Pausa) ---
    public void AlternarPlayPause()
    {
        estaReproduciendo = !estaReproduciendo;

        if (estaReproduciendo)
        {
            Debug.Log("WWISE: Reanudar música");

            // Si está sonando, mostramos el botón de Pausa para que el jugador pueda frenarla
            if (imagenBotonPlayPause != null && iconoPause != null)
                imagenBotonPlayPause.sprite = iconoPause;

            // Acá tu compañero agregará el evento de Wwise para reanudar/reproducir
        }
        else
        {
            Debug.Log("WWISE: Pausar música");

            // Si está pausada, mostramos el botón de Play para que el jugador pueda darle inicio
            if (imagenBotonPlayPause != null && iconoPlay != null)
                imagenBotonPlayPause.sprite = iconoPlay;

            // Acá tu compañero agregará el evento de Wwise para detener/pausar
        }
    }
}