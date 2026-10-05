using UnityEngine;

public class ControladorCamaras : MonoBehaviour
{
    [Header("Interfaz (UI)")]
    public GameObject panelSubBotones;

    [Header("Cámaras de la Escena")]
    public GameObject[] camaras; 

    void Start()
    {
       
        if (panelSubBotones != null)
        {
            panelSubBotones.SetActive(false);
        }
    }

    
    public void AlternarMenuBotones()
    {
        if (panelSubBotones != null)
        {
           
            panelSubBotones.SetActive(!panelSubBotones.activeSelf);
        }
    }

   
    public void CambiarCamara(int indiceCamara)
    {
       
        for (int i = 0; i < camaras.Length; i++)
        {
           
            camaras[i].SetActive(i == indiceCamara);
        }

        
        panelSubBotones.SetActive(false);
    }
}