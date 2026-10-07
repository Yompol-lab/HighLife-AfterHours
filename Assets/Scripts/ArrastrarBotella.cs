using UnityEngine;
using UnityEngine.InputSystem;

public class ArrastrarBotella : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private float posInicialZ;
    private Rigidbody rb;
    private Quaternion rotacionInicial;

    private bool estaAgarrada = false;
    private bool estaSirviendo = false; 

    [Header("Configuración para servir")]
    public string tagCopas = "Copa";
    public float distanciaParaServir = 2.5f;
    public float anguloVolcadoMaximo = 180f;
    public float velocidadRotacion = 6f;

    [Header("Líquido")]
    public ParticleSystem particulasLiquido;
   

    void Start()
    {
        posInicialZ = transform.position.z;
        rb = GetComponent<Rigidbody>();
        rotacionInicial = transform.rotation;
    }

    void OnMouseDown()
    {
        estaAgarrada = true;
        if (rb != null) rb.isKinematic = true;

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Vector3 cursorPoint = new Vector3(pointerPos.x, pointerPos.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

        transform.position = new Vector3(cursorPosition.x, cursorPosition.y, posInicialZ);

        GameObject copaObjetivo = BuscarCopaMasCercana();

        if (copaObjetivo != null)
        {
            float distancia = Vector3.Distance(transform.position, copaObjetivo.transform.position);

            if (distancia < distanciaParaServir)
            {
                float difX = transform.position.x - copaObjetivo.transform.position.x;
                float direccion = (difX > 0) ? 1f : -1f;

                float factorCentrado = 1f - Mathf.Clamp01(Mathf.Abs(difX) / distanciaParaServir);
                float anguloObjetivo = Mathf.Lerp(0f, anguloVolcadoMaximo, factorCentrado);

                Quaternion giroGlobal = Quaternion.AngleAxis(anguloObjetivo * direccion, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, giroGlobal * rotacionInicial, Time.deltaTime * velocidadRotacion);

              
                if (factorCentrado > 0.3f)
                {
                    estaSirviendo = true;
                }
                else
                {
                    estaSirviendo = false;
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotacionInicial, Time.deltaTime * velocidadRotacion);
                estaSirviendo = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionInicial, Time.deltaTime * velocidadRotacion);
            estaSirviendo = false;
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