using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe abstrata para os personagens do game
/// </summary>
public abstract class Caractere : MonoBehaviour {
	public float inicioPontosDano;  // valor mínimo inicial de "saúde" do Player
	public float MaxPontosDano;    // valor máximo permitido de "saúde" do Player

	/*
		Método que reseta os atributos do personagem
	*/
	public abstract void ResetCaractere();

	/*
		Método que executa a ação de troca de cor do personagem ao tomar dano
	*/
	public virtual IEnumerator FlickerCaractere() {
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = Color.white;
	}

	/*
		Método que gerencia o dano recebido pelo personagem
	*/
	public abstract IEnumerator DanoCaractere(int dano, float intervalo);

	/*
		Método que destroi os personagens do game
	*/
	public virtual void KillCaractere() {
		Destroy(gameObject);
	}
}
