using UnityEngine;
using TMPro;
using System.Collections;

public class SistemaComputadora : MonoBehaviour
{
    [Header("Ventanas de la PC")]
    public GameObject ventanaGoogle;
    public GameObject ventanaMercadoPago;
    public GameObject ventanaServicios;
    public GameObject ventanaConfirmacion;
    public GameObject ventanaMercadoLibre;
    public GameObject panelNotificacionesML;

    [Header("Imágenes Temporales")]
    public GameObject imagenPagoExitoso;
    public GameObject imagenPagoSeco;

    [Header("Economía")]
    public float saldo = 50000f;

    [Header("Pedido Jefe de Barra (Mercado Libre)")]
    public float costoStockHoy = 15000f;
    private bool compraBarraProcesada = false; // Controla si ya aceptaste o rechazaste

    [Header("Costos de Servicios")]
    public float costoArca = 5000f;
    public float costoSadaic = 2000f;
    public float costoMunicipalidades = 3000f;
    public float costoAadi = 1500f;
    public float costoEpe = 3500f;
    public float costoAgua = 1200f;
    public float costoClaro = 1000f;
    public float costoDri = 2500f;
    public float costoTgi = 4500f;
    public float costoSancor = 2000f;
    public float costoSueldo = 15000f;

    // Controladores de servicios
    private bool arcaPagado = false;
    private bool sadaicPagado = false;
    private bool municipalidadesPagado = false;
    private bool aadiPagado = false;
    private bool epePagado = false;
    private bool aguaPagada = false;
    private bool claroPagado = false;
    private bool driPagado = false;
    private bool tgiPagado = false;
    private bool sancorPagado = false;
    private bool sueldoPagado = false;

    [Header("Textos UI")]
    public TMP_Text textoSaldo;
    public TMP_Text textoMontoFactura;
    public TMP_Text textoDineroDisponible;
    public TMP_Text textoMensajeCampanita;

    private float costoPendiente;
    private int idServicioPendiente;

    void Start()
    {
        CerrarTodo();
        ActualizarPantalla();
        if (imagenPagoExitoso != null) imagenPagoExitoso.SetActive(false);
        if (imagenPagoSeco != null) imagenPagoSeco.SetActive(false);
    }

    // --- NAVEGACIÓN PRINCIPAL ---
    public void AbrirGoogle() { CerrarTodo(); if (ventanaGoogle != null) ventanaGoogle.SetActive(true); }
    public void AbrirMercadoPago() { CerrarTodo(); if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(true); ActualizarPantalla(); }
    public void AbrirServicios() { CerrarTodo(); if (ventanaServicios != null) ventanaServicios.SetActive(true); ActualizarPantalla(); }
    public void VolverAMercadoPago() { CerrarTodo(); if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(true); ActualizarPantalla(); }
    public void VolverAServicios() { CerrarTodo(); if (ventanaServicios != null) ventanaServicios.SetActive(true); }

    // --- NAVEGACIÓN MERCADO LIBRE ---
    public void AbrirMercadoLibre()
    {
        CerrarTodo();
        if (ventanaMercadoLibre != null) ventanaMercadoLibre.SetActive(true);

        // Abre el panel automáticamente al entrar
        if (panelNotificacionesML != null)
        {
            panelNotificacionesML.SetActive(true);
            if (textoMensajeCampanita != null)
            {
                if (!compraBarraProcesada)
                    textoMensajeCampanita.text = $"Total a pagar: ${costoStockHoy}";
                else
                    textoMensajeCampanita.text = "No hay compras pendientes.";
            }
        }
    }

    public void AceptarCompraBarra()
    {
        if (compraBarraProcesada) return;

        if (saldo >= costoStockHoy)
        {
            saldo -= costoStockHoy;
            compraBarraProcesada = true;
            ActualizarPantalla();
            StartCoroutine(RutinaCompraML(imagenPagoExitoso, "Pago exitoso."));
        }
        else
        {
            StartCoroutine(RutinaCompraML(imagenPagoSeco, "Saldo insuficiente."));
        }
    }

    public void RechazarCompraBarra()
    {
        if (compraBarraProcesada) return;

        compraBarraProcesada = true;
        // Al rechazar, el panel simplemente se cierra
        if (panelNotificacionesML != null) panelNotificacionesML.SetActive(false);
    }

    private IEnumerator RutinaCompraML(GameObject imagenMostrar, string mensajeFinal)
    {
        if (imagenMostrar != null) imagenMostrar.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (imagenMostrar != null) imagenMostrar.SetActive(false);

        if (textoMensajeCampanita != null) textoMensajeCampanita.text = mensajeFinal;
    }

    public void CerrarTodo()
    {
        if (ventanaGoogle != null) ventanaGoogle.SetActive(false);
        if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(false);
        if (ventanaServicios != null) ventanaServicios.SetActive(false);
        if (ventanaConfirmacion != null) ventanaConfirmacion.SetActive(false);
        if (ventanaMercadoLibre != null) ventanaMercadoLibre.SetActive(false);
    }

    // --- PREPARAR Y CONFIRMAR SERVICIOS ---
    public void PagarArca() { ConfigurarConfirmacion(costoArca, 1, arcaPagado); }
    public void PagarSadaic() { ConfigurarConfirmacion(costoSadaic, 2, sadaicPagado); }
    public void PagarMunicipalidades() { ConfigurarConfirmacion(costoMunicipalidades, 3, municipalidadesPagado); }
    public void PagarAadi() { ConfigurarConfirmacion(costoAadi, 4, aadiPagado); }
    public void PagarEpe() { ConfigurarConfirmacion(costoEpe, 5, epePagado); }
    public void PagarAgua() { ConfigurarConfirmacion(costoAgua, 6, aguaPagada); }
    public void PagarClaro() { ConfigurarConfirmacion(costoClaro, 7, claroPagado); }
    public void PagarDri() { ConfigurarConfirmacion(costoDri, 8, driPagado); }
    public void PagarTgi() { ConfigurarConfirmacion(costoTgi, 9, tgiPagado); }
    public void PagarSancor() { ConfigurarConfirmacion(costoSancor, 10, sancorPagado); }
    public void PagarSueldo() { ConfigurarConfirmacion(costoSueldo, 11, sueldoPagado); }

    private void ConfigurarConfirmacion(float costo, int id, bool yaEstaPagado)
    {
        if (yaEstaPagado) return;
        costoPendiente = costo;
        idServicioPendiente = id;
        CerrarTodo();
        if (ventanaConfirmacion != null) ventanaConfirmacion.SetActive(true);
        if (textoMontoFactura != null) textoMontoFactura.text = costo.ToString();
        if (textoDineroDisponible != null) textoDineroDisponible.text = saldo.ToString();
    }

    public void ConfirmarPago()
    {
        if (saldo >= costoPendiente)
        {
            saldo -= costoPendiente;
            MarcarComoPagado(idServicioPendiente);
            ActualizarPantalla();
            StartCoroutine(RutinaPago(imagenPagoExitoso));
        }
        else
        {
            StartCoroutine(RutinaPago(imagenPagoSeco));
        }
    }

    private IEnumerator RutinaPago(GameObject imagenMostrar)
    {
        if (imagenMostrar != null) imagenMostrar.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (imagenMostrar != null) imagenMostrar.SetActive(false);
        CerrarTodo();
        if (ventanaServicios != null) ventanaServicios.SetActive(true);
    }

    private void MarcarComoPagado(int id)
    {
        if (id == 1) arcaPagado = true;
        else if (id == 2) sadaicPagado = true;
        else if (id == 3) municipalidadesPagado = true;
        else if (id == 4) aadiPagado = true;
        else if (id == 5) epePagado = true;
        else if (id == 6) aguaPagada = true;
        else if (id == 7) claroPagado = true;
        else if (id == 8) driPagado = true;
        else if (id == 9) tgiPagado = true;
        else if (id == 10) sancorPagado = true;
        else if (id == 11) sueldoPagado = true;
    }

    void ActualizarPantalla()
    {
        if (textoSaldo != null) textoSaldo.text = saldo.ToString();
    }
}