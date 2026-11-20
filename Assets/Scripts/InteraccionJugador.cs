using UnityEngine;
using UnityEngine.EventSystems;

public class InteraccionJugador : MonoBehaviour
{
    [Header("Configuración")]
    public Camera camaraPrincipal;
    public Transform puntoDeMano;
    public float distanciaAgarre = 10f;

    private VasoInteractuable vasoActual;
    private ItemPancheria comidaActual;

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(ObtenerIDToque()))
                return;

            Ray rayo = camaraPrincipal.ScreenPointToRay(Input.mousePosition);
            RaycastHit golpe;

            if (Physics.Raycast(rayo, out golpe, distanciaAgarre))
            {
                VasoInteractuable vasoTocado = golpe.collider.GetComponent<VasoInteractuable>();
                ItemPancheria comidaTocada = golpe.collider.GetComponent<ItemPancheria>();
                GeneradorComida generador = golpe.collider.GetComponent<GeneradorComida>();
                TablaPreparacion tablaPrepa = golpe.collider.GetComponent<TablaPreparacion>();
                TablaEntregaPancheria tablaEntrega = golpe.collider.GetComponent<TablaEntregaPancheria>();

              
                if (vasoActual == null && comidaActual == null)
                {
                    if (vasoTocado != null)
                        AgarrarVaso(vasoTocado);
                    else if (comidaTocada != null)
                        AgarrarComida(comidaTocada);
                    else if (generador != null)
                        AgarrarComida(generador.GenerarItem());
                    else if (tablaPrepa != null && tablaPrepa.TienePanchoListo())
                        AgarrarComida(tablaPrepa.EntregarPanchoListo()); 
                }
                
                else if (comidaActual != null)
                {
                    if (tablaPrepa != null)
                    {
                        
                        if (tablaPrepa.RecibirIngrediente(comidaActual))
                        {
                            Destroy(comidaActual.gameObject);
                            comidaActual = null;
                        }
                    }
                    else if (tablaEntrega != null)
                    {
                       
                        tablaEntrega.ProcesarEntrega(comidaActual);
                        comidaActual = null;
                    }
                    else
                    {
                        
                        comidaActual.DejarEnMesa(golpe.point);
                        comidaActual = null;
                    }
                }
              
                else if (vasoActual != null)
                {
                    ZonaPreparado zonaBolichePrep = golpe.collider.GetComponent<ZonaPreparado>();
                    ZonaEntrega zonaBolicheEntr = golpe.collider.GetComponent<ZonaEntrega>();

                    if (zonaBolichePrep != null)
                    {
                        zonaBolichePrep.RecibirVaso(vasoActual);
                        vasoActual = null;
                    }
                    else if (zonaBolicheEntr != null)
                    {
                        zonaBolicheEntr.RecibirVaso(vasoActual);
                        vasoActual = null;
                    }
                    else
                    {
                        SoltarVaso();
                    }
                }
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

    private int ObtenerIDToque()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return -1;
#else
            if (Input.touchCount > 0) return Input.GetTouch(0).fingerId;
            return -1;
#endif
    }
}