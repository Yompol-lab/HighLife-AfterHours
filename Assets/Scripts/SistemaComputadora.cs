using UnityEngine;
using TMPro;
using System.Collections; // Necesario para los segundos de las imágenes

public class SistemaComputadora : MonoBehaviour
{
    [Header("Ventanas de la PC")]
    public GameObject ventanaGoogle;
    public GameObject ventanaMercadoPago;
    public GameObject ventanaServicios; // Tu lista de botones
    public GameObject ventanaConfirmacion; // Tu nueva ventana con la factura

    [Header("Imágenes Temporales")]
    public GameObject imagenPagoExitoso;
    public GameObject imagenPagoSeco;

    [Header("Economía")]
    public float saldo = 50000f;

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

    // Controladores
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

    [Header("Textos UI - Generales")]
    public TMP_Text textoSaldo;
    public TMP_Text textoNotificaciones;

    [Header("Textos UI - Ventana Confirmación")]
    public TMP_Text textoMontoFactura; // El que dice de cuánto es la factura
    public TMP_Text textoDineroDisponible; // El que dice tu plata disponible

    // Variables internas para recordar qué tocó el jugador
    private float costoPendiente;
    private string servicioPendienteNombre;
    private int idServicioPendiente;

    void Start()
    {
        CerrarTodo();
        if (imagenPagoExitoso != null) imagenPagoExitoso.SetActive(false);
        if (imagenPagoSeco != null) imagenPagoSeco.SetActive(false);
    }

    // --- NAVEGACIÓN PRINCIPAL ---
    public void AbrirGoogle()
    {
        CerrarTodo();
        if (ventanaGoogle != null) ventanaGoogle.SetActive(true);
    }

    public void AbrirMercadoPago()
    {
        CerrarTodo();
        if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(true);
        ActualizarPantalla();
    }

    public void AbrirServicios()
    {
        CerrarTodo();
        if (ventanaServicios != null) ventanaServicios.SetActive(true);
        ActualizarPantalla();
    }

    public void VolverAMercadoPago()
    {
        CerrarTodo();
        if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(true);
        ActualizarPantalla();
    }

    // ESTA ES LA FLECHA NUEVA PARA VOLVER DE LA FACTURA A LA LISTA
    public void VolverAServicios()
    {
        CerrarTodo();
        if (ventanaServicios != null) ventanaServicios.SetActive(true);
    }

    public void CerrarTodo()
    {
        if (ventanaGoogle != null) ventanaGoogle.SetActive(false);
        if (ventanaMercadoPago != null) ventanaMercadoPago.SetActive(false);
        if (ventanaServicios != null) ventanaServicios.SetActive(false);
        if (ventanaConfirmacion != null) ventanaConfirmacion.SetActive(false);
    }

    // --- 1. PREPARAR EL PAGO (Mantiene tus nombres originales) ---
    public void PagarArca() { ConfigurarConfirmacion(costoArca, "ARCA", 1, arcaPagado); }
    public void PagarSadaic() { ConfigurarConfirmacion(costoSadaic, "SADAIC", 2, sadaicPagado); }
    public void PagarMunicipalidades() { ConfigurarConfirmacion(costoMunicipalidades, "Municipalidades", 3, municipalidadesPagado); }
    public void PagarAadi() { ConfigurarConfirmacion(costoAadi, "AADI CAPIF", 4, aadiPagado); }
    public void PagarEpe() { ConfigurarConfirmacion(costoEpe, "EPE (Luz)", 5, epePagado); }
    public void PagarAgua() { ConfigurarConfirmacion(costoAgua, "Aguas Santafesinas", 6, aguaPagada); }
    public void PagarClaro() { ConfigurarConfirmacion(costoClaro, "Claro (Internet)", 7, claroPagado); }
    public void PagarDri() { ConfigurarConfirmacion(costoDri, "DReI", 8, driPagado); }
    public void PagarTgi() { ConfigurarConfirmacion(costoTgi, "TGI", 9, tgiPagado); }
    public void PagarSancor() { ConfigurarConfirmacion(costoSancor, "Sancor (ART)", 10, sancorPagado); }
    public void PagarSueldo() { ConfigurarConfirmacion(costoSueldo, "Sueldos", 11, sueldoPagado); }

    private void ConfigurarConfirmacion(float costo, string nombre, int id, bool yaEstaPagado)
    {
        if (yaEstaPagado) return; // Si ya se pagó, no hace nada

        // Guarda los datos en memoria
        costoPendiente = costo;
        servicioPendienteNombre = nombre;
        idServicioPendiente = id;

        // Cierra la lista y abre la ventana de factura
        CerrarTodo();
        if (ventanaConfirmacion != null) ventanaConfirmacion.SetActive(true);

        // Actualiza tus dos textos por separado
        if (textoMontoFactura != null) textoMontoFactura.text = "Factura de " + nombre + ": $" + costo;
        if (textoDineroDisponible != null) textoDineroDisponible.text = "Dinero Disponible: $" + saldo;
    }

    // --- 2. CONFIRMAR PAGO (Botón "Pagar" en la factura) ---
    public void ConfirmarPago()
    {
        // Al apretar pagar, te devuelve a la lista de servicios
        CerrarTodo();
        if (ventanaServicios != null) ventanaServicios.SetActive(true);

        if (saldo >= costoPendiente)
        {
            saldo -= costoPendiente;
            MarcarComoPagado(idServicioPendiente);
            ActualizarPantalla();

            // Muestra imagen de éxito por 2 segundos
            StartCoroutine(MostrarImagenTemporal(imagenPagoExitoso, 2f));
        }
        else
        {
            // Muestra imagen de SECO por 2 segundos
            StartCoroutine(MostrarImagenTemporal(imagenPagoSeco, 2f));
        }
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

    // --- 3. TEMPORIZADOR DE IMÁGENES ---
    private IEnumerator MostrarImagenTemporal(GameObject imagen, float tiempo)
    {
        if (imagen != null)
        {
            imagen.SetActive(true);
            yield return new WaitForSeconds(tiempo);
            imagen.SetActive(false);
        }
    }

    void ActualizarPantalla()
    {
        if (textoSaldo != null) textoSaldo.text = "$ " + saldo.ToString("0.00");
    }
}