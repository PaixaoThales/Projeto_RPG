using UnityEngine;
using UnityEngine.SceneManagement;

public class CenaManager : MonoBehaviour {
	public static CenaManager instancia = null;

	public GameObject player;
	[HideInInspector]
	public bool venceu;
	[HideInInspector]
	public int faseAtual;

	public AudioSource audioSource;
	public AudioClip musicaPadrao;
	public AudioClip musicaFinal;

	private void Awake() {
		if (instancia != null && instancia != this) {
			Destroy(gameObject);
		}
		else {
			instancia = this;
			DontDestroyOnLoad(this);
		}
	}

	public void MudaCena(string nomeDaCena) {
		SceneManager.LoadScene(nomeDaCena);
		if (nomeDaCena.StartsWith("Fase")) {
			faseAtual = (int)char.GetNumericValue(nomeDaCena[4]);
			if (faseAtual == 5) {
				audioSource.clip = musicaFinal;
				audioSource.loop = true;
				audioSource.Play();
			}
			else if (faseAtual == 1) {
				audioSource.clip = musicaPadrao;
				audioSource.loop = true;
				audioSource.Play();
			}
		}
	}

	public void Finaliza(bool venceu) {
		this.venceu = venceu;
		Destroy(player);
		MudaCena("CenaFinal");
	}

	public void Sair() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
