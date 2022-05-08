using UnityEngine;

/// <summary>
/// Classe que modela a munição
/// </summary>
public class Municao : MonoBehaviour {
	public int danoCausado;  // dano da munição

	/*
		Método que verifica a colisão dos projéteis com os inimigos
	*/
	private void OnTriggerEnter2D(Collider2D collision) {
		// Se colidir com o inimigo
		if (collision is BoxCollider2D) {
			Inimigo inimigo = collision.gameObject.GetComponent<Inimigo>();
			if (inimigo == null) return;
			// Inicia a corrotina de dar dano no inimigo
			StartCoroutine(inimigo.DanoCaractere(danoCausado, 0.0f));
			gameObject.SetActive(false);
		}
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}
}
