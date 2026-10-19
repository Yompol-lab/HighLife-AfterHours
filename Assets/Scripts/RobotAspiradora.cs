using UnityEngine;

public class RobotAspiradora : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 1.5f;
    public float tiempoDeLimpieza = 60f;

    private Vector3 posicionBase;
    private Quaternion rotacionBase;
    private float temporizador = 0f;

    
    private enum Estado { Esperando, Limpiando, Regresando }
    private Estado estadoActual = Estado.Esperando;

    void Start()
    {
        
        posicionBase = transform.position;
        rotacionBase = transform.rotation;
    }

    
    public void PresionarBoton()
    {
        if (estadoActual == Estado.Esperando)
        {
           
            estadoActual = Estado.Limpiando;
            temporizador = tiempoDeLimpieza;
        }
        else if (estadoActual == Estado.Limpiando)
        {
           
            estadoActual = Estado.Regresando;
        }
    }

    void Update()
    {
        switch (estadoActual)
        {
            case Estado.Limpiando:
                MoverLimpiando();

               
                temporizador -= Time.deltaTime;
                if (temporizador <= 0)
                {
                   
                    estadoActual = Estado.Regresando;
                }
                break;

            case Estado.Regresando:
                RegresarABase();
                break;
        }
    }

    void MoverLimpiando()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (estadoActual == Estado.Limpiando)
        {
            float giroAleatorio = Random.Range(90f, 180f);
            transform.Rotate(Vector3.up, giroAleatorio);
        }
    }

    void RegresarABase()
    {
       
        Vector3 direccionABase = posicionBase - transform.position;
        direccionABase.y = 0; 

        if (direccionABase != Vector3.zero)
        {
            Quaternion rotacionHaciaBase = Quaternion.LookRotation(direccionABase);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionHaciaBase, Time.deltaTime * 5f);
        }

      
        transform.position = Vector3.MoveTowards(transform.position, posicionBase, velocidad * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, posicionBase) < 0.1f)
        {
            
            transform.position = posicionBase;
            transform.rotation = rotacionBase;
            estadoActual = Estado.Esperando;
        }
    }
}