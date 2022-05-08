using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que representa a barra de vida do player
/// </summary>
public class HealthBar : MonoBehaviour {
	public PontosDano pontosDano;     // Objeto de leitura dos dados de quantos pontos tem o Player
	public Player caractere;          // representa o objeto de Player
	public Image medidorImagem;       // representa a barra de medição
	public Text pdTexto;              // representa os dados de PD
	float maxPontosDano;              // representa quantidade limite de "saúde" do Player
	public int multiplicador = 1;     // representa o multiplicador de pontos de dano

	// Start is called before the first frame update
	void Start() {
		maxPontosDano = caractere.MaxPontosDano;
	}

	// Update is called once per frame
	void Update() {
		if (caractere != null) {
			medidorImagem.fillAmount = pontosDano.valor / maxPontosDano;
			pdTexto.text = "PD:" + (medidorImagem.fillAmount * 100 * multiplicador);
		}
	}
}
