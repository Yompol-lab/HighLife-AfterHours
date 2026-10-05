using System.Collections; 
using UnityEngine;
using TMPro;

public class RelojFNAF : MonoBehaviour
{
    [Header("UI del Reloj")]
    public TextMeshProUGUI textoReloj;

    [Header("Configuración de Tiempo")]
    public float segundosPorHora = 90f;

    [Header("Configuración de Parpadeo")]
    public int cantidadParpadeos = 3; 
    public float velocidadParpadeo = 0.15f; 

    private float tiempoTranscurrido = 0f;
    private int horaActual = 12;
    private bool nocheTerminada = false;

    void Start()
    {
        ActualizarTexto();
    }

    void Update()
    {
        if (nocheTerminada) return;

        tiempoTranscurrido += Time.deltaTime;

        if (tiempoTranscurrido >= segundosPorHora)
        {
            tiempoTranscurrido -= segundosPorHora;
            AvanzarHora();
        }
    }

    void AvanzarHora()
    {
        horaActual++;

        if (horaActual == 13)
        {
            horaActual = 1;
        }

        ActualizarTexto();

        
        StartCoroutine(ParpadearTexto());

        if (horaActual == 6)
        {
            nocheTerminada = true;
            GanarNoche();
        }
    }

    void ActualizarTexto()
    {
        textoReloj.text = horaActual.ToString() + ":00 AM";
    }

    void GanarNoche()
    {
        Debug.Log("¡Llegaste a las 6 AM! Noche superada.");
    }

    
    IEnumerator ParpadearTexto()
    {
        for (int i = 0; i < cantidadParpadeos; i++)
        {
            textoReloj.enabled = false; 
            yield return new WaitForSeconds(velocidadParpadeo); 

            textoReloj.enabled = true; 
            yield return new WaitForSeconds(velocidadParpadeo); 
        }

        textoReloj.enabled = true;
    }
}