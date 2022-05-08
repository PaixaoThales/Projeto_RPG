using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que modela o player
/// </summary>
public class Player : Caractere {
	public Inventario inventarioPrefab;   // Prefab criado do Inventario
	Inventario inventario; // Inventario do player
	public HealthBar healthBarPrefab;     // Prefab criado da barra de vida
	HealthBar healthBar; // Barra de vida do player

	public PontosDano pontosDano; // Quantidade de vida do player

	bool estaProximoDoNPC; // Variavel de controle de proximidade do player
	bool novaArmaAdquirida; // Variavel de controle do novo player

	/*
		Método que cria o player (inicializa)
	*/
	private void Start() {
		inventario = Instantiate(inventarioPrefab, transform);
		pontosDano.valor = inicioPontosDano;
		healthBar = Instantiate(healthBarPrefab, transform);
		healthBar.caractere = this;
		estaProximoDoNPC = false;
		novaArmaAdquirida = false;
	}

	/*
		Método que avalia o dano recebido pelos inimigos
	*/
	public override IEnumerator DanoCaractere(int dano, float intervalo) {
		while (true) {
			// Inicia a corrotina de flicker do player (mudança de cor vermelho-branco da sprite)
			StartCoroutine(FlickerCaractere());
			pontosDano.valor -= dano;
			// Mata o player destruindo a sprite de da cena
			if (pontosDano.valor <= float.Epsilon) {
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
		Método que instancia os atributos do player (inicializa)
	*/
	public override void ResetCaractere() {
		inventario = Instantiate(inventarioPrefab);
		healthBar = Instantiate(healthBarPrefab);
		healthBar.caractere = this;
		pontosDano.valor = inicioPontosDano;
	}

	/*	
		Método que destroi o player a as sprites relacionadas da tela 
	*/
	public override void KillCaractere() {
		base.KillCaractere();
		Destroy(healthBar.gameObject);
		Destroy(inventario.gameObject);
		CenaManager.instancia.Finaliza(false);
	}

	/*
		Método trigger para quando chegamos perto do NPC adquirirmos a nova arma
	*/
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("NPC")) {
			if (!novaArmaAdquirida) {
				GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
				estaProximoDoNPC = false;
			}
		}
	}

	/*
		Método trigger para gerenciar colisões
	*/
	private void OnTriggerEnter2D(Collider2D collision) {

		// Se a colisão foi com um trigger de cena trocamos a cena para próxima fase
		if (collision.gameObject.CompareTag("Trigger")) {
			CenaManager.instancia.MudaCena("Fase" + (CenaManager.instancia.faseAtual + 1));
		}

		// Se colidirmos com o npc verificamos se ja pegamos a arma para ver se precisamos avisar o player sobre o novo item
		else if (collision.gameObject.CompareTag("NPC")) {
			if (!novaArmaAdquirida) {
				GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250);
				estaProximoDoNPC = true;
			}
		}

		// Se colidirmos com um coletavel avaliamos de qual tipo ele é algum dos sprite coletaveis
		else if (collision.gameObject.CompareTag("Coletavel")) {
			Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
			if (DanoObjeto != null) {
				bool DeveDesaparecer = false;
				switch (DanoObjeto.tipoItem) {
					case Item.TipoItem.MOEDA:
					case Item.TipoItem.RUBI:
					case Item.TipoItem.ESMERALDA:
					case Item.TipoItem.SAFIRA:
						DeveDesaparecer = inventario.AddItem(DanoObjeto);
						break;
					// Retira o diamente da tela e finaliza para cena final
					case Item.TipoItem.DIAMANTE:
						DeveDesaparecer = inventario.AddItem(DanoObjeto);
						CenaManager.instancia.Finaliza(true);
						break;

					// Retira o coração da tela e ajusta a vida do player
					case Item.TipoItem.HEALTH:
						DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade);
						break;

					default:
						break;
				}
				if (DeveDesaparecer) {
					collision.gameObject.SetActive(false);
				}
			}
		}
	}			

	/*
		Método que atualiza os pontos de dano se não exceder os pontos do player
	*/
	public bool AjustePontosDano(int quantidade) {
		if (pontosDano.valor < MaxPontosDano) {
			pontosDano.valor = pontosDano.valor + quantidade;
			return true;
		}
		return false;
	}

	/*
		Método que faz o upgrade de arma do player e dobra a vida do player
	*/
	void FixedUpdate() {
		if (Input.GetButtonDown("Jump") && estaProximoDoNPC) {
			healthBar.multiplicador = 2;
			GetComponent<Armas>().UpgradeArma();
			GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
			novaArmaAdquirida = true;
		}
	}
}

