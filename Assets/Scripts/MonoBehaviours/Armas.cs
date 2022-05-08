using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Armas : MonoBehaviour {
  public GameObject municaoPrefab;             // armazena o prefab da Munição
  static List<GameObject> municaoPiscina;      // Piscina de Munição
  public int tamanhoPiscina;                   // Tamanho da Piscina
	public float velocidadeArma;                 // velocidade da Munição
	public GameObject novaMunicaoPrefab;

	bool atirando;
	[HideInInspector]
	public Animator animator;

	float slopePositivo;
	float slopeNegativo;

	enum Quadrante {
		Sul,
		Leste,
		Oeste,
		Norte
	}

	private void Start() {
		animator = GetComponent<Animator>();
		atirando = false;
		Vector2 abaixoEsquerda = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
		Vector2 acimaDireita = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		Vector2 acimaEsquerda = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));
		Vector2 abaixoDireita = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
		slopePositivo = PegaSlope(abaixoEsquerda, acimaDireita);
		slopeNegativo = PegaSlope(acimaEsquerda, abaixoDireita);
	}

	bool AcimaSlopePositivo(Vector2 posicaoEntrada) {
		Vector2 posicaoPlayer = gameObject.transform.position;
		Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(posicaoEntrada);
		float interseccaoY = posicaoPlayer.y - slopePositivo * posicaoPlayer.x;
		float entradaInterseccao = posicaoMouse.y - slopePositivo * posicaoMouse.x;
		return entradaInterseccao > interseccaoY;
	}

	bool AcimaSlopeNegativo(Vector2 posicaoEntrada) {
		Vector2 posicaoPlayer = gameObject.transform.position;
		Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(posicaoEntrada);
		float interseccaoY = posicaoPlayer.y - slopeNegativo * posicaoPlayer.x;
		float entradaInterseccao = posicaoMouse.y - slopeNegativo * posicaoMouse.x;
		return entradaInterseccao > interseccaoY;
	}

	Quadrante PegaQuadrante() {
		Vector2 poiscaoMouse = Input.mousePosition;
		Vector2 posicaoPlayer = transform.position;
		bool acimaSlopePositivo = AcimaSlopePositivo(Input.mousePosition);
		bool acimaSlopeNegativo = AcimaSlopeNegativo(Input.mousePosition);
		return (Quadrante)((acimaSlopePositivo ? 2 : 0) + (acimaSlopeNegativo ? 1 : 0));
	}

	void UpdateEstado() {
		if (atirando) {
			Vector2 vetorQuadrante;
			Quadrante quadranteEnum = PegaQuadrante();
			switch (quadranteEnum) {
				case Quadrante.Leste:
					vetorQuadrante = new Vector2(1.0f, 0.0f);
					break;
				case Quadrante.Sul:
					vetorQuadrante = new Vector2(0.0f, -1.0f);
					break;
				case Quadrante.Oeste:
					vetorQuadrante = new Vector2(-1.0f, 0.0f);
					break;
				case Quadrante.Norte:
					vetorQuadrante = new Vector2(0.0f, 1.0f);
					break;
				default:
					vetorQuadrante = Vector2.zero;
					break;
			}
			animator.SetBool("Atirando", true);
			animator.SetFloat("AtiraX", vetorQuadrante.x);
			animator.SetFloat("AtiraY", vetorQuadrante.y);
			atirando = false;
		}
		else {
			animator.SetBool("Atirando", false);
		}
	}

  public void Awake() {
    if (municaoPiscina == null) {
      municaoPiscina = new List<GameObject>();
    }
    for (int i = 0;i < tamanhoPiscina;i++) {
      GameObject municaoO = Instantiate(municaoPrefab);
      municaoO.SetActive(false);
      municaoPiscina.Add(municaoO);
    }
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetMouseButtonDown(0)) {
			atirando = true;
      DisparaMunicao();
    }
		UpdateEstado();
  }

	float PegaSlope(Vector2 ponto1, Vector2 ponto2) {
		return (ponto2.y - ponto1.y) / (ponto2.x - ponto1.x);
	}

	void ResetPiscina() {
		municaoPiscina = new List<GameObject>();
		for (int i = 0;i < tamanhoPiscina;i++) {
			GameObject municaoO = Instantiate(municaoPrefab);
			municaoO.SetActive(false);
			municaoPiscina.Add(municaoO);
		}
	}

  GameObject SpawnMunicao(Vector3 posicao) {
    foreach (GameObject municao in municaoPiscina) {
			if (municao == null) {
				ResetPiscina();
				break;
			}
		}
    foreach (GameObject municao in municaoPiscina) {
      if (municao.activeSelf == false) {
        municao.SetActive(true);
        municao.transform.position = posicao;
				return municao;
      }
    }
		return null;
  }

  void DisparaMunicao() {
		Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject municao = SpawnMunicao(transform.position);
		if (municao != null) {
			Arco arcoScript = municao.GetComponent<Arco>();
			float duracaoTrajetoria = 1.0f / velocidadeArma;
			StartCoroutine(arcoScript.arcoTrajetoria(posicaoMouse, duracaoTrajetoria));
		}
  }

  private void OnDestroy() {
		municaoPiscina = null;
	}

	public void UpgradeArma() {
		municaoPrefab = novaMunicaoPrefab;
		ResetPiscina();
	}
}
