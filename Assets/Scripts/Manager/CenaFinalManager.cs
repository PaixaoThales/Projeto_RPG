using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

///<summary>
/// Classe que representa a cena final do jogo
/// Contem os status do final do jogo dizendo se o jogador venceu ou perdeu
/// Contem menu para voltar a primeira cena, restart do jogo ou ir para os créditos
///</summary>
public class CenaFinalManager : MonoBehaviour
{

    public AudioClip vitoria; // guarda música da vitoria
    public AudioClip derrota; // guarda música da derrota

    /*
		Método que inicia e gerencia as cenas e qual áudio vai tocar
	*/
    public void Start()
    {
        if (SceneManager.GetActiveScene().name != "CenaFinal")
        {
            return;
        }
        if (CenaManager.instancia.venceu)
        {
            GameObject.Find("Resultado").GetComponent<Text>().text = "VOCÊ VENCEU";
            CenaManager.instancia.audioSource.clip = vitoria;
            CenaManager.instancia.audioSource.loop = false;
        }
        else
        {
            GameObject.Find("Resultado").GetComponent<Text>().text = "VOCÊ PERDEU";
            CenaManager.instancia.audioSource.clip = derrota;
            CenaManager.instancia.audioSource.loop = false;
        }
        CenaManager.instancia.audioSource.Play();
    }

    /*
		Método que carrega a cena do menu principal do game
	*/
    public void IrParaMenuPrincipal()
    {
        CenaManager.instancia.MudaCena("CenaInicial");
    }

    /*
		Método que reinicia o jogo indo para cena da fase 1
	*/
    public void Reiniciar()
    {
        CenaManager.instancia.MudaCena("Fase1");
    }

    /*
		Método que carrega a cena de créditos do jogo
	*/
    public void IrParaCreditos()
    {
        CenaManager.instancia.MudaCena("Creditos");
    }
}
