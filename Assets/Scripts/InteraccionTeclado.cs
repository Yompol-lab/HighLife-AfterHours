using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class InteraccionTeclado : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    public Camera camaraAUsar;
    public Transform puntoMonitor;
    public float velocidadTransicion = 3f;

    [Header("Configuración de Zoom")]
    public Slider sliderZoom;
    public float fovNormal = 60f; 
    public float fovZoom = 25f;   

    private Vector3 posicionGuardada;
    private Quaternion rotacionGuardada;
    private float fovGuardado;
    private bool viendoMonitor = false;
    private Coroutine transicionActual;

    void Start()
    {
        
        if (sliderZoom != null)
        {
            sliderZoom.gameObject.SetActive(false);
            sliderZoom.onValueChanged.AddListener(ActualizarZoom);
        }
    }

    void OnMouseDown()
    {
        if (camaraAUsar == null || puntoMonitor == null) return;

        if (transicionActual != null) StopCoroutine(transicionActual);

        if (!viendoMonitor)
        {
           
            posicionGuardada = camaraAUsar.transform.position;
            rotacionGuardada = camaraAUsar.transform.rotation;
            fovGuardado = camaraAUsar.fieldOfView;
            viendoMonitor = true;

           
            if (sliderZoom != null)
            {
                sliderZoom.value = 0f;
                sliderZoom.gameObject.SetActive(true);
            }

            transicionActual = StartCoroutine(MoverCamara(puntoMonitor.position, puntoMonitor.rotation, -1f));
        }
        else
        {
            
            viendoMonitor = false;
            if (sliderZoom != null) sliderZoom.gameObject.SetActive(false);

            transicionActual = StartCoroutine(MoverCamara(posicionGuardada, rotacionGuardada, fovGuardado));
        }
    }

    private IEnumerator MoverCamara(Vector3 posDestino, Quaternion rotDestino, float fovDestino)
    {
        float tiempo = 0;
        Vector3 posInicio = camaraAUsar.transform.position;
        Quaternion rotInicio = camaraAUsar.transform.rotation;
        float fovInicio = camaraAUsar.fieldOfView;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadTransicion;
            float t = Mathf.SmoothStep(0, 1, tiempo);

            camaraAUsar.transform.position = Vector3.Lerp(posInicio, posDestino, t);
            camaraAUsar.transform.rotation = Quaternion.Lerp(rotInicio, rotDestino, t);

            
            if (fovDestino > 0)
            {
                camaraAUsar.fieldOfView = Mathf.Lerp(fovInicio, fovDestino, t);
            }

            yield return null;
        }
    }

   
    private void ActualizarZoom(float valorSlider)
    {
        if (viendoMonitor && camaraAUsar != null)
        {
            
            camaraAUsar.fieldOfView = Mathf.Lerp(fovNormal, fovZoom, valorSlider);
        }
    }
}