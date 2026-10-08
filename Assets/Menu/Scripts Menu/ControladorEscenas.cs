using UnityEngine;
using UnityEngine.SceneManagement; 

public class ControladorEscenas : MonoBehaviour
{
    
    public void CambiarDeEscena(string nombreDeLaEscena)
    {
        SceneManager.LoadScene(nombreDeLaEscena);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}