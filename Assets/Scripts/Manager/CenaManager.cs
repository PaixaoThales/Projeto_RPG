using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
/// Classe que gerencia as Cenas do Jogo
/// Guarda estados do jogo atual, como se o jogador venceu, fase atual e musicas
///</summary>
public class CenaManager : MonoBehaviour
{
    public static CenaManager instancia = null;

    public GameObject player; // Representa o objeto player 
    [HideInInspector]
    public bool venceu; // Bool que indica se o usuario venceu ou não
    [HideInInspector]
    public int faseAtual; // Inteiro que indica a fase atual do game

    public AudioSource audioSource; // Representa a fonte de audio do game
    public AudioSource sonsSource; // Representa o canal por onde os sons saem
    public AudioClip musicaPadrao; // Representa a musica de introdução do game
    public AudioClip musicaFinal; // Representa a musica final do game

    /*
		Método que carrega ou não destroi esse objeto sempre que uma cena nova é carregada
	*/
    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instancia = this;
            DontDestroyOnLoad(this);
        }
    }

    /*
		Método que carrega as cenas e suas respectivas musicas
	*/
    public void MudaCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
        // Verifica se a cena é uma fase do game
        if (nomeDaCena.StartsWith("Fase"))
        {
            faseAtual = (int)char.GetNumericValue(nomeDaCena[4]);
            // Se for fase final carrega musica da fase final
            if (faseAtual == 5)
            {
                audioSource.clip = musicaFinal;
                audioSource.loop = true;
                audioSource.Play();
            }
            // Se fase inicial carrega musica da fase inicial
            else if (faseAtual == 1)
            {
                audioSource.clip = musicaPadrao;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    /*
		Método que finaliza a gameplay passando para o menu final
	*/
    public void Finaliza(bool venceu)
    {
        this.venceu = venceu;
        Destroy(player);
        MudaCena("CenaFinal");
    }

    /*
		Método que caso o jogo não esteja rodando no editor, o jogo é fechado, caso contrario, ele setta para o jogar para false
		para o editor fechar o game
	*/
    public void Sair()
    {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
