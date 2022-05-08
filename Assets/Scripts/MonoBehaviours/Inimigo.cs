using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que modela o inimigo
/// </summary>
public class Inimigo : Caractere {
	float pontosVida;              // representa à saúde do inimigo
	public int forcaDano;          // representa o dano
	public GameObject dropPrefab;  // prefab de quando o inimigo morre
	public float probabilidadeDrop;// probabilidade de um item (coração) aparecer

	Coroutine danoCoroutine;

	// Start is called before the first frame update
	void Start() {

	}

	/*
		Método que é chamado quando o inimigo fica ativo na cena para inicializar seus atributos
	*/
	private void OnEnable() {
		ResetCaractere();
	}

	/*
		Método que verifica se o inimigo colidiu com o player
	*/
	private void OnCollisionEnter2D(Collision2D collision) {
		// Se a colisão foi com o player inserimos dano 
		if (collision.gameObject.CompareTag("Player")) {
			Player player = collision.gameObject.GetComponent<Player>();
			if (danoCoroutine == null) {
				// Gera a corrotina para inserir dano no player
				danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
			}
		}
	}

	/*
		Método que verifica se o player parou de colidir com o inimigo
	*/
	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			if (danoCoroutine != null) {
				// Se a corrotina existe paramos ela para parar a adição de dano no player
				StopCoroutine(danoCoroutine);
				danoCoroutine = null;
			}
		}
	}

	/*
		Método que avalia o dano recebido do player no inimigo
	*/
	public override IEnumerator DanoCaractere(int dano, float intervalo) {
		while (true) {
			// Enquanto recebe dano o inimigo troca cor (vermelho-branco) 'flicker'
			StartCoroutine(FlickerCaractere());
			pontosVida = pontosVida - dano;
			// Se o dano for suficiente para mata-lo retiramos a sprite da cena
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

	/*
		Método que inicia o imigo com vida cheia
	*/
	public override void ResetCaractere() {
		pontosVida = inicioPontosDano;
	}

	/*
		Método que destroi os inimigos e avalia o drop de cosumiveis
	*/
	public override void KillCaractere() {
		float prob = Random.Range(0.0f, 1.0f);
		if (prob <= probabilidadeDrop) {
			SpawnDrop();
		}
		base.KillCaractere();
	}

	/*
		Método que da spawn nos consumiveis como corações
	*/
	void SpawnDrop() {
		GameObject drop = Instantiate(dropPrefab);
		drop.transform.position = transform.position;
	}

	// Update is called once per frame
	void Update() {

	}
}
