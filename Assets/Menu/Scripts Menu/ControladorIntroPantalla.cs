using UnityEngine;
using UnityEngine.Video;

public class ControladorIntroPantalla : MonoBehaviour
{
    [Header("Configuración")]
    public VideoPlayer reproductorVideo;
    public GameObject canvasBotones3D;

    void Start()
    {
        
        if (canvasBotones3D != null)
        {
            canvasBotones3D.SetActive(false);
        }

       
        if (reproductorVideo != null)
        {
            reproductorVideo.loopPointReached += AlTerminarVideo;
        }
    }

    
    void AlTerminarVideo(VideoPlayer vp)
    {
        
        if (canvasBotones3D != null)
        {
            canvasBotones3D.SetActive(true);
        }

        
        vp.gameObject.SetActive(false);
    }
}