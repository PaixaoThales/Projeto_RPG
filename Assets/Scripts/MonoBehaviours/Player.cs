using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere {
	public Inventario inventarioPrefab;   // referencia ao objeto prefab criado do Inventario
	Inventario inventario;
	public HealthBar healthBarPrefab;     // referencia ao objeto prefab criado da HealthBar
	HealthBar healthBar;

	public PontosDano pontosDano;            // tem o valor da "saúde" do objeto

	bool estaProximoDoNPC;
	bool novaArmaAdquirida;

	private void Start() {
		inventario = Instantiate(inventarioPrefab, transform);
		pontosDano.valor = inicioPontosDano;
		healthBar = Instantiate(healthBarPrefab, transform);
		healthBar.caractere = this;
		estaProximoDoNPC = false;
		novaArmaAdquirida = false;
	}

	public override IEnumerator DanoCaractere(int dano, float intervalo) {
		while (true) {
			StartCoroutine(FlickerCaractere());
			pontosDano.valor -= dano;
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

	public override void ResetCaractere() {
		inventario = Instantiate(inventarioPrefab);
		healthBar = Instantiate(healthBarPrefab);
		healthBar.caractere = this;
		pontosDano.valor = inicioPontosDano;
	}

	public override void KillCaractere() {
		base.KillCaractere();
		Destroy(healthBar.gameObject);
		Destroy(inventario.gameObject);
		CenaManager.instancia.Finaliza(false);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("NPC")) {
			if (!novaArmaAdquirida) {
				GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
				estaProximoDoNPC = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Trigger")) {
			CenaManager.instancia.MudaCena("Fase" + (CenaManager.instancia.faseAtual + 1));
		}
		else if (collision.gameObject.CompareTag("NPC")) {
			if (!novaArmaAdquirida) {
				GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250);
				estaProximoDoNPC = true;
			}
		}
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

					case Item.TipoItem.DIAMANTE:
						DeveDesaparecer = inventario.AddItem(DanoObjeto);
						CenaManager.instancia.Finaliza(true);
						break;

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

	public bool AjustePontosDano(int quantidade) {
		if (pontosDano.valor < MaxPontosDano) {
			pontosDano.valor = pontosDano.valor + quantidade;
			print("Ajustando PD por: " + quantidade + ". Novo Valor = " + pontosDano.valor);
			return true;
		}
		return false;
	}

	void FixedUpdate() {
		if (Input.GetButtonDown("Jump") && estaProximoDoNPC) {
			GetComponent<Armas>().UpgradeArma();
			GameObject.Find("Aviso").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
			novaArmaAdquirida = true;
		}
	}
}

