using UnityEngine;
using Cinemachine;

///<summary>
/// Classe que gerencia a camera do jogo
/// Guarda a camera virtual do jogo
///</summary>
public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager instanciaCompartilhada = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    /*
		Método que carrega ou não destroi esse objeto sempre que uma cena nova é carregada
		Além disso, busca a camera virtual e guarda ela nesse objeto 
	*/
    private void Awake()
    {
        if (instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }
        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
