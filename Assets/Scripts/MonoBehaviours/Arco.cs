using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que manipula os disparos
/// </summary>
public class Arco : MonoBehaviour {
	
	/*
		Método que calcula a trajetoria do disparo do ponto de inicio até o final
		para cada instante de tempo adicionado configuramos a posição do projétil
	*/
	public IEnumerator arcoTrajetoria(Vector3 destino, float duracao) {
		var posicaoInicial = transform.position;
		var percentualCompleto = 0.0f;
		while (percentualCompleto < 1.0f) {
			percentualCompleto += Time.deltaTime / duracao;
			var alturaCorrente = Mathf.Sin(Mathf.PI * percentualCompleto);
			transform.position = Vector3.Lerp(posicaoInicial, destino, percentualCompleto) + Vector3.up * alturaCorrente;
			percentualCompleto += Time.deltaTime / duracao;
			yield return null;
		}
		gameObject.SetActive(false);
	}
}
