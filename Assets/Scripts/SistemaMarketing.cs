using UnityEngine;
using TMPro;
using System.Collections;

public class SistemaMarketing : MonoBehaviour
{
    [Header("Ventanas de la PC Marketing")]
    public GameObject ventanaGoogle;
    public GameObject ventanaInstagram;
    public GameObject ventanaInstagramSeguidores;

    [Header("Estadísticas y Chat")]
    public float seguidoresActuales = 10000f;
    public TMP_Text textoSeguidores;
    public TMP_Text textoMensajeInfluencer;

    [Header("Botones de Decisión")]
    public GameObject botonMandarSolicitud;
    public GameObject botonEliminar;

    private float costoInfluencer = 15000f;
    private bool hayMensajePendiente = false;

    void Start()
    {
        CerrarTodo();
        ActualizarPantalla();

        // Inicia el temporizador que simula que te hablan solos
        StartCoroutine(GeneradorDeMensajes());
    }

    // --- NAVEGACIÓN ---
    public void AbrirGoogle() { CerrarTodo(); if (ventanaGoogle != null) ventanaGoogle.SetActive(true); }
    public void AbrirInstagram() { CerrarTodo(); if (ventanaInstagram != null) ventanaInstagram.SetActive(true); }
    public void AbrirInstagramSeguidores() { CerrarTodo(); if (ventanaInstagramSeguidores != null) ventanaInstagramSeguidores.SetActive(true); ActualizarPantalla(); }
    public void VolverAGoogle() { CerrarTodo(); if (ventanaGoogle != null) ventanaGoogle.SetActive(true); }
    public void VolverAInstagram() { CerrarTodo(); if (ventanaInstagram != null) ventanaInstagram.SetActive(true); }

    public void CerrarTodo()
    {
        if (ventanaGoogle != null) ventanaGoogle.SetActive(false);
        if (ventanaInstagram != null) ventanaInstagram.SetActive(false);
        if (ventanaInstagramSeguidores != null) ventanaInstagramSeguidores.SetActive(false);
    }

    // --- SISTEMA AUTOMÁTICO DE MENSAJES ---
    private IEnumerator GeneradorDeMensajes()
    {
        // Genera el primer mensaje instantáneamente (listo para cuando abras la ventana)
        GenerarNuevoDM();

        while (true)
        {
            // Espera 15 segundos antes de intentar mandar otro mensaje
            yield return new WaitForSeconds(15f);

            // Solo manda uno nuevo si ya aceptaste o rechazaste el anterior
            if (!hayMensajePendiente)
            {
                GenerarNuevoDM();
            }
        }
    }

    private void GenerarNuevoDM()
    {
        hayMensajePendiente = true;

        if (textoMensajeInfluencer != null)
        {
            textoMensajeInfluencer.text = $"¡Hola! Por subir historias cobro ${costoInfluencer}. Avisame y le damos para adelante.";
        }

        if (botonMandarSolicitud != null) botonMandarSolicitud.SetActive(true);
        if (botonEliminar != null) botonEliminar.SetActive(true);
    }

    public void MandarSolicitudAFinanzas()
    {
        if (textoMensajeInfluencer != null)
        {
            textoMensajeInfluencer.text = "Solicitud enviada. Esperando autorización de Finanzas...";
        }
        OcultarBotonesDecision();
        hayMensajePendiente = false; // Libera el chat para que el temporizador pueda mandar otro
    }

    public void EliminarOferta()
    {
        if (textoMensajeInfluencer != null)
        {
            textoMensajeInfluencer.text = "Chat eliminado. Esperando nuevos DMs...";
        }
        OcultarBotonesDecision();
        hayMensajePendiente = false; // Libera el chat
    }

    private void OcultarBotonesDecision()
    {
        if (botonMandarSolicitud != null) botonMandarSolicitud.SetActive(false);
        if (botonEliminar != null) botonEliminar.SetActive(false);
    }

    // --- ACTUALIZACIÓN ---
    public void AumentarSeguidores(float cantidadGanada)
    {
        seguidoresActuales += cantidadGanada;
        ActualizarPantalla();
    }

    private void ActualizarPantalla()
    {
        if (textoSeguidores != null)
        {
            textoSeguidores.text = seguidoresActuales.ToString("N0") + " Seguidores";
        }
    }
}