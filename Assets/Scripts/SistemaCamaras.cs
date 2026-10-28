using UnityEngine;
using UnityEngine.UI; 

public class SistemaCamaras : MonoBehaviour
{
    [Header("Interfaz")]
    public GameObject panelCamaras; 
    public RawImage pantallaMonitor;

    [Header("Señales de Video")]
    public Texture[] vistasDeCamaras; 

    private int indiceActual = 0;

    public void AbrirCamaras()
    {
        if (panelCamaras != null) panelCamaras.SetActive(true);
        indiceActual = 0;
        ActualizarPantalla();
    }

    public void CerrarCamaras()
    {
        if (panelCamaras != null) panelCamaras.SetActive(false);
    }

    public void SiguienteCamara()
    {
        if (vistasDeCamaras.Length == 0) return;

        indiceActual++;
       
        if (indiceActual >= vistasDeCamaras.Length) indiceActual = 0;

        ActualizarPantalla();
    }

    public void AnteriorCamara()
    {
        if (vistasDeCamaras.Length == 0) return;

        indiceActual--;
        
        if (indiceActual < 0) indiceActual = vistasDeCamaras.Length - 1;

        ActualizarPantalla();
    }

    private void ActualizarPantalla()
    {
        if (pantallaMonitor != null && vistasDeCamaras.Length > 0)
        {
            
            pantallaMonitor.texture = vistasDeCamaras[indiceActual];
        }
    }
}