using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteraccionJugador : MonoBehaviour
{
    [Header("Configuración")]
    public Camera camaraPrincipal;
    public Transform puntoDeMano;
    public float distanciaAgarre = 10f;
    public float toleranciaToque = 0.12f;

    [Header("Filtro Tap vs Movimiento de Cámara")]
    public float distanciaMaximaTap = 30f; 
    public float tiempoMaximoTap = 0.35f;  

    private VasoInteractuable vasoActual;
    private ItemPancheria comidaActual;

    private struct DatosToque
    {
        public Vector2 posicionInicial;
        public float tiempoInicio;
        public bool empezoEnUI;
    }

    private readonly Dictionary<int, DatosToque> toquesActivos = new Dictionary<int, DatosToque>();
    private readonly List<RaycastResult> resultadosUI = new List<RaycastResult>();
    private DatosToque datosMouse;
    private bool mouseActivo;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            mouseActivo = false;

            foreach (Touch toque in Input.touches)
            {
                if (toque.phase == TouchPhase.Began)
                {
                    toquesActivos[toque.fingerId] = new DatosToque
                    {
                        posicionInicial = toque.position,
                        tiempoInicio = Time.time,
                        empezoEnUI = EstaSobreUI(toque.position)
                    };
                }
                else if (toque.phase == TouchPhase.Ended)
                {
                    if (toquesActivos.TryGetValue(toque.fingerId, out DatosToque datos))
                    {
                        toquesActivos.Remove(toque.fingerId);
                        ValidarYProcesarTap(datos, toque.position);
                    }
                }
                else if (toque.phase == TouchPhase.Canceled)
                {
                    toquesActivos.Remove(toque.fingerId);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseActivo = true;
                datosMouse = new DatosToque
                {
                    posicionInicial = Input.mousePosition,
                    tiempoInicio = Time.time,
                    empezoEnUI = EstaSobreUI(Input.mousePosition)
                };
            }
            else if (Input.GetMouseButtonUp(0) && mouseActivo)
            {
                mouseActivo = false;
                ValidarYProcesarTap(datosMouse, Input.mousePosition);
            }
        }
    }

    private void ValidarYProcesarTap(DatosToque datos, Vector2 posicionFinal)
    {
        if (datos.empezoEnUI || EstaSobreUI(posicionFinal)) return;

        float distancia = Vector2.Distance(datos.posicionInicial, posicionFinal);
        float duracion = Time.time - datos.tiempoInicio;

        if (distancia <= distanciaMaximaTap && duracion <= tiempoMaximoTap)
        {
            ProcesarToque(posicionFinal);
        }
    }

    private void ProcesarToque(Vector2 posicionPantalla)
    {
        if (camaraPrincipal == null || !camaraPrincipal.gameObject.activeInHierarchy) return;

        Ray rayo = camaraPrincipal.ScreenPointToRay(posicionPantalla);

        RaycastHit[] golpes = Physics.RaycastAll(rayo, distanciaAgarre, ~0, QueryTriggerInteraction.Ignore);
        if (IntentarInteractuar(golpes)) return;

        RaycastHit[] golpesAmplios = Physics.SphereCastAll(rayo, toleranciaToque, distanciaAgarre, ~0, QueryTriggerInteraction.Ignore);
        if (IntentarInteractuar(golpesAmplios)) return;

        IntentarSoltarEnSuperficie(golpes);
    }

    private bool IntentarInteractuar(RaycastHit[] golpes)
    {
        if (golpes.Length == 0) return false;

        Array.Sort(golpes, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit golpe in golpes)
        {
            if (golpe.transform.IsChildOf(transform)) continue;

            if (vasoActual == null && comidaActual == null)
            {
                VasoInteractuable vasoTocado = golpe.collider.GetComponentInParent<VasoInteractuable>();
                ItemPancheria comidaTocada = golpe.collider.GetComponentInParent<ItemPancheria>();
                GeneradorComida generador = golpe.collider.GetComponentInParent<GeneradorComida>();
                TablaPreparacion tablaPrepa = golpe.collider.GetComponentInParent<TablaPreparacion>();

                if (vasoTocado != null)
                {
                    AgarrarVaso(vasoTocado);
                    return true;
                }
                if (comidaTocada != null)
                {
                    AgarrarComida(comidaTocada);
                    return true;
                }
                if (generador != null)
                {
                    ItemPancheria nuevo = generador.GenerarItem();
                    if (nuevo != null) AgarrarComida(nuevo);
                    return true;
                }
                if (tablaPrepa != null && tablaPrepa.TienePanchoListo())
                {
                    ItemPancheria pancho = tablaPrepa.EntregarPanchoListo();
                    if (pancho != null) AgarrarComida(pancho);
                    return true;
                }
            }
            else if (comidaActual != null)
            {
                TablaPreparacion tablaPrepa = golpe.collider.GetComponentInParent<TablaPreparacion>();
                TablaEntregaPancheria tablaEntrega = golpe.collider.GetComponentInParent<TablaEntregaPancheria>();

                if (tablaPrepa != null)
                {
                    if (tablaPrepa.RecibirIngrediente(comidaActual))
                    {
                        Destroy(comidaActual.gameObject);
                        comidaActual = null;
                    }
                    return true;
                }
                if (tablaEntrega != null)
                {
                    tablaEntrega.ProcesarEntrega(comidaActual);
                    comidaActual = null;
                    return true;
                }
            }
            else if (vasoActual != null)
            {
                ZonaPreparado zonaBolichePrep = golpe.collider.GetComponentInParent<ZonaPreparado>();
                ZonaEntrega zonaBolicheEntr = golpe.collider.GetComponentInParent<ZonaEntrega>();

                if (zonaBolichePrep != null)
                {
                    zonaBolichePrep.RecibirVaso(vasoActual);
                    vasoActual = null;
                    return true;
                }
                if (zonaBolicheEntr != null)
                {
                    zonaBolicheEntr.RecibirVaso(vasoActual);
                    vasoActual = null;
                    return true;
                }
            }
        }

        return false;
    }

    private void IntentarSoltarEnSuperficie(RaycastHit[] golpes)
    {
        if (comidaActual == null && vasoActual == null) return;

        Array.Sort(golpes, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit golpe in golpes)
        {
            if (golpe.transform.IsChildOf(transform)) continue;

            if (comidaActual != null)
            {
                comidaActual.DejarEnMesa(golpe.point);
                comidaActual = null;
                return;
            }
            if (vasoActual != null)
            {
                SoltarVaso();
                return;
            }
        }
    }

    private void AgarrarComida(ItemPancheria comida)
    {
        comidaActual = comida;
        comidaActual.BloquearEnMano(puntoDeMano);
    }

    private void AgarrarVaso(VasoInteractuable vaso)
    {
        vasoActual = vaso;
        vasoActual.transform.SetParent(puntoDeMano);
        vasoActual.BloquearEnPosicion(puntoDeMano);
        vasoActual.transform.localPosition = Vector3.zero;
        vasoActual.transform.localRotation = Quaternion.identity;
    }

    private void SoltarVaso()
    {
        vasoActual.transform.SetParent(null);
        vasoActual.LiberarVaso();
        vasoActual = null;
    }

    private bool EstaSobreUI(Vector2 posicionPantalla)
    {
        if (EventSystem.current == null) return false;

        PointerEventData evento = new PointerEventData(EventSystem.current)
        {
            position = posicionPantalla
        };
        resultadosUI.Clear();
        EventSystem.current.RaycastAll(evento, resultadosUI);

        foreach (RaycastResult r in resultadosUI)
        {
            if (r.gameObject.GetComponentInParent<Selectable>() != null ||
                r.gameObject.GetComponentInParent<IDragHandler>() != null)
            {
                Debug.Log("Bloqueado por UI: " + r.gameObject.name);
                return true;
            }
        }

        return false;
    }
}