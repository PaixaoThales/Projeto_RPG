using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CenaFinalManager : MonoBehaviour {

	public AudioClip vitoria;
	public AudioClip derrota;

	public void Start() {
		if (SceneManager.GetActiveScene().name != "CenaFinal") {
			return;
		}
		if (CenaManager.instancia.venceu) {
			GameObject.Find("Resultado").GetComponent<Text>().text = "VOCÊ VENCEU";
			CenaManager.instancia.audioSource.clip = vitoria;
			CenaManager.instancia.audioSource.loop = false;
		}
		else {
			GameObject.Find("Resultado").GetComponent<Text>().text = "VOCÊ PERDEU";
			CenaManager.instancia.audioSource.clip = derrota;
			CenaManager.instancia.audioSource.loop = false;
		}
		CenaManager.instancia.audioSource.Play();
	}

	public void IrParaMenuPrincipal() {
		CenaManager.instancia.MudaCena("CenaInicial");
	}

	public void Reiniciar() {
		CenaManager.instancia.MudaCena("Fase1");
	}

	public void IrParaCreditos() {
		CenaManager.instancia.MudaCena("Creditos");
	}
}
