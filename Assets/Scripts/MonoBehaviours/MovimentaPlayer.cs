using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia o movimento do player na cena
/// </summary>
public class MovimentaPlayer : MonoBehaviour {
	public float VelocidadeMovimento = 3.0f;      // equivale ao momento (impulso) a ser dado ao player
	Vector2 Movimento = new Vector2();            // detectar movimento pelo teclado

	Animator animator;                            // guarda a componente Controlador de Animação
	Rigidbody2D rb2D;                             // guarda a componente CorpoRigido do Player

	// Start is called before the first frame update
	void Start() {
		animator = GetComponent<Animator>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update() {
		UpdateEstado();
	}

	/*
		Método que atualiza a posição do player na cena quando acionamos um comando
	*/
	private void FixedUpdate() {
		MoveCaractere();
	}

	/*
		Método que move a sprite adicionando o movimento a ela de acordo com os eixos 
		em que os movimentos ocorreram
	*/
	private void MoveCaractere() {
		Movimento.x = Input.GetAxisRaw("Horizontal");
    	Movimento.y = Input.GetAxisRaw("Vertical");
		Movimento.Normalize();
		rb2D.velocity = Movimento * VelocidadeMovimento;
	}

	/*
		Método que controla o estado do player (Caminhando)
	*/
	void UpdateEstado() {
		if (Mathf.Approximately(Movimento.x, 0) && Mathf.Approximately(Movimento.y, 0)) {
			animator.SetBool("Caminhando", false);
		}
		else {
			animator.SetBool("Caminhando", true);
		}
		animator.SetFloat("dirX", Movimento.x);
		animator.SetFloat("dirY", Movimento.y);
	}
}
