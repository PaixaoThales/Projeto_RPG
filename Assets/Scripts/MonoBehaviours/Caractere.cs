using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour {
	public float inicioPontosDano;           // valor mínimo inicial de "saúde" do Player
	public float MaxPontosDano;              // valor máximo permitido de "saúde" do Player

	public abstract void ResetCaractere();

	public virtual IEnumerator FlickerCaractere() {
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = Color.white;
	}

	public abstract IEnumerator DanoCaractere(int dano, float intervalo);

	public virtual void KillCaractere() {
		Destroy(gameObject);
	}
}
