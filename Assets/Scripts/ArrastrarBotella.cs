using UnityEngine;
using UnityEngine.InputSystem;

public class ArrastrarBotella : MonoBehaviour
{
    private Rigidbody rb;
    private Quaternion rotacionInicial;
    private float alturaMesa;
    private float zInicial; 
    private Vector3 offsetAgarre;

    private bool estaAgarrada = false;
    private bool estaSirviendo = false;

    [Header("Límites de Movimiento")]
    public float limiteProfundidadZ = 0.6f; 
    public float suavidadMovimiento = 15f; 

    [Header("Configuración del Imán (Smooth)")]
    public string tagCopas = "Copa";
    public float distanciaParaServir = 0.8f;
    public Vector3 offsetServido = new Vector3(0.3f, 0.4f, 0f);
    public float suavidadIman = 5f; 

    [Header("Inclinación y Líquido")]
    public float anguloVolcadoMaximo = 130f;
    public float velocidadRotacion = 4f; 
    public float anguloParaQueSalgaLiquido = 45f; 

    public ParticleSystem particulasLiquido;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotacionInicial = transform.rotation;
        zInicial = transform.position.z; 
    }

    void OnMouseDown()
    {
        estaAgarrada = true;
        if (rb != null) rb.isKinematic = true;

        alturaMesa = transform.position.y;

        Ray rayo = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
        Plane planoMesa = new Plane(Vector3.up, new Vector3(0, alturaMesa, 0));
        if (planoMesa.Raycast(rayo, out float distancia))
        {
            offsetAgarre = transform.position - rayo.GetPoint(distancia);
        }
    }

    void OnMouseDrag()
    {
        Ray rayo = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
        Plane planoMesa = new Plane(Vector3.up, new Vector3(0, alturaMesa, 0));

        if (planoMesa.Raycast(rayo, out float distanciaAlPlano))
        {
            
            Vector3 posicionDedo = rayo.GetPoint(distanciaAlPlano) + offsetAgarre;
            posicionDedo.z = Mathf.Clamp(posicionDedo.z, zInicial - limiteProfundidadZ, zInicial + limiteProfundidadZ);

            GameObject copaObjetivo = BuscarCopaMasCercana();

            if (copaObjetivo != null)
            {
                float distanciaAlVaso = Vector3.Distance(posicionDedo, copaObjetivo.transform.position);

                
                if (distanciaAlVaso < distanciaParaServir)
                {
                    float lado = Mathf.Sign(posicionDedo.x - copaObjetivo.transform.position.x);
                    Vector3 posicionIdeal = copaObjetivo.transform.position + new Vector3(offsetServido.x * lado, offsetServido.y, offsetServido.z);

                    
                    transform.position = Vector3.Lerp(transform.position, posicionIdeal, Time.deltaTime * suavidadIman);

               
                    float distanciaIdeal = Vector3.Distance(transform.position, posicionIdeal);
                    float factorCentrado = 1f - Mathf.Clamp01(distanciaIdeal / distanciaParaServir);
                    float anguloObjetivo = Mathf.Lerp(0f, anguloVolcadoMaximo, factorCentrado);

                    Quaternion giroGlobal = Quaternion.AngleAxis(anguloObjetivo * lado, Vector3.forward);
                    transform.rotation = Quaternion.Lerp(transform.rotation, giroGlobal * rotacionInicial, Time.deltaTime * velocidadRotacion);
                }
                else
                {
                   
                    transform.position = Vector3.Lerp(transform.position, posicionDedo, Time.deltaTime * suavidadMovimiento);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotacionInicial, Time.deltaTime * velocidadRotacion);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, posicionDedo, Time.deltaTime * suavidadMovimiento);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotacionInicial, Time.deltaTime * velocidadRotacion);
            }

            
            float inclinacionFisicaActual = Quaternion.Angle(transform.rotation, rotacionInicial);
            estaSirviendo = (inclinacionFisicaActual > anguloParaQueSalgaLiquido);
        }
    }

    void OnMouseUp()
    {
        estaAgarrada = false;
        estaSirviendo = false;
        if (rb != null) rb.isKinematic = false;
        transform.rotation = rotacionInicial;
    }

    void Update()
    {
        if (particulasLiquido != null)
        {
            if (estaAgarrada && estaSirviendo)
            {
                if (!particulasLiquido.isPlaying) particulasLiquido.Play();
            }
            else
            {
                if (particulasLiquido.isPlaying) particulasLiquido.Stop();
            }
        }
    }

    GameObject BuscarCopaMasCercana()
    {
        GameObject[] todasLasCopas = GameObject.FindGameObjectsWithTag(tagCopas);
        GameObject copaMasCercana = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (GameObject copa in todasLasCopas)
        {
            float distancia = Vector3.Distance(transform.position, copa.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                copaMasCercana = copa;
            }
        }
        return copaMasCercana;
    }
}