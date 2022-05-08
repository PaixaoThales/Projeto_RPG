using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Caractere {
	float pontosVida;              // equivalente à saúde do inimigo
	public int forcaDano;          // poder de dano
	public GameObject dropPrefab;  // prefab da item a aparecer quando inimigo morre
	public float probabilidadeDrop;// probabilidade de um item aparecer

	Coroutine danoCoroutine;

	// Start is called before the first frame update
	void Start() {

	}

	private void OnEnable() {
		ResetCaractere();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			Player player = collision.gameObject.GetComponent<Player>();
			if (danoCoroutine == null) {
				danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			if (danoCoroutine != null) {
				StopCoroutine(danoCoroutine);
				danoCoroutine = null;
			}
		}
	}

	public override IEnumerator DanoCaractere(int dano, float intervalo) {
		while (true) {
			StartCoroutine(FlickerCaractere());
			pontosVida = pontosVida - dano;
			if (pontosVida <= float.Epsilon) {
				KillCaractere();
				break;
			}
			if (intervalo > float.Epsilon) {
				yield return new WaitForSeconds(intervalo);
			}
			else break;
		}
	}

	public override void ResetCaractere() {
		pontosVida = inicioPontosDano;
	}

	public override void KillCaractere() {
		float prob = Random.Range(0.0f, 1.0f);
		if (prob <= probabilidadeDrop) {
			SpawnDrop();
		}
		base.KillCaractere();
	}

	void SpawnDrop() {
		GameObject drop = Instantiate(dropPrefab);
		drop.transform.position = transform.position;
	}

	// Update is called once per frame
	void Update() {

	}
}
