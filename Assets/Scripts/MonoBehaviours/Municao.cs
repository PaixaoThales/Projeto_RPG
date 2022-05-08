using UnityEngine;

public class Municao : MonoBehaviour {
	public int danoCausado;        // poder de dano da munição

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision is BoxCollider2D) {
			Inimigo inimigo = collision.gameObject.GetComponent<Inimigo>();
			if (inimigo == null) return;
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
