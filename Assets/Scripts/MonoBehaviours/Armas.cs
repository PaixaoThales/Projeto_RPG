using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia os estados e propriedades de armas e pool de munição
/// </summary>
[RequireComponent(typeof(Animator))]
public class Armas : MonoBehaviour
{
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

    enum Quadrante
    {
        Sul,
        Leste,
        Oeste,
        Norte
    }	

    /*
		Método que inicia e calcula os coeficientes angulares utilizando os quadrantes mais extremos
		para o slope negativo e positivo
	*/
    private void Start()
    {
        animator = GetComponent<Animator>();
        atirando = false;
        Vector2 abaixoEsquerda = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 acimaDireita = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 acimaEsquerda = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 abaixoDireita = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        slopePositivo = PegaSlope(abaixoEsquerda, acimaDireita);
        slopeNegativo = PegaSlope(acimaEsquerda, abaixoDireita);
    }

    /*
		Método que avalia se o player e do click do mouse
		estão acima ou abaixo da reta de slope positivo
	*/
    bool AcimaSlopePositivo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - slopePositivo * posicaoPlayer.x;
        float entradaInterseccao = posicaoMouse.y - slopePositivo * posicaoMouse.x;
        return entradaInterseccao > interseccaoY;
    }

    /*
		Método que avalia se o player e do click do mouse
		estão acima ou abaixo da reta de slope negativo
	*/
    bool AcimaSlopeNegativo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - slopeNegativo * posicaoPlayer.x;
        float entradaInterseccao = posicaoMouse.y - slopeNegativo * posicaoMouse.x;
        return entradaInterseccao > interseccaoY;
    }

    /*
		Método que encontra o quadrante em que efetuamos o click baseado nas posição relativas a retas
		de coeficiente angular slopePositivo e slopeNegativo
	*/
    Quadrante PegaQuadrante()
    {
        Vector2 poiscaoMouse = Input.mousePosition;
        Vector2 posicaoPlayer = transform.position;
        bool acimaSlopePositivo = AcimaSlopePositivo(Input.mousePosition);
        bool acimaSlopeNegativo = AcimaSlopeNegativo(Input.mousePosition);
        return (Quadrante)((acimaSlopePositivo ? 2 : 0) + (acimaSlopeNegativo ? 1 : 0));
    }

    /*
		Método que atualiza o estado da arma quando ela está atira ou não,
		definindo em qual quadrante o projetil está e, por fim, setando ele
	*/
    void UpdateEstado()
    {
        if (atirando)
        {
            Vector2 vetorQuadrante;
            Quadrante quadranteEnum = PegaQuadrante();
            switch (quadranteEnum)
            {
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
        else
        {
            animator.SetBool("Atirando", false);
        }
    }

    /*
		Método que cria uma lista de munição caso ela seja null do tamanho do atributo tamanhoPiscina
		e para cada munição criada, é copiada dos prefabs e desativada
	*/
    public void Awake()
    {
        if (municaoPiscina == null)
        {
            municaoPiscina = new List<GameObject>();
        }
        for (int i = 0; i < tamanhoPiscina; i++)
        {
            GameObject municaoO = Instantiate(municaoPrefab);
            municaoO.SetActive(false);
            municaoPiscina.Add(municaoO);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            atirando = true;
            DisparaMunicao();
        }
        UpdateEstado();
    }

    /*
		Método que pega o coeficiente angular de dois pontos
	*/
    float PegaSlope(Vector2 ponto1, Vector2 ponto2)
    {
        return (ponto2.y - ponto1.y) / (ponto2.x - ponto1.x);
    }

    /*
		Método que cria uma lista de munição do tamanho do atributo tamanhoPiscina
		e para cada munição criada, é copiada dos prefabs e desativada
	*/
    void ResetPiscina()
    {
        municaoPiscina = new List<GameObject>();
        for (int i = 0; i < tamanhoPiscina; i++)
        {
            GameObject municaoO = Instantiate(municaoPrefab);
            municaoO.SetActive(false);
            municaoPiscina.Add(municaoO);
        }
    }

    /*
		Percorre o estoque de munição e coloca um projétil na tela na posição fornecida
	*/
    GameObject SpawnMunicao(Vector3 posicao)
    {
        foreach (GameObject municao in municaoPiscina)
        {
            if (municao == null)
            {
                ResetPiscina();
                break;
            }
        }
        foreach (GameObject municao in municaoPiscina)
        {
            if (municao.activeSelf == false)
            {
                municao.SetActive(true);
                municao.transform.position = posicao;
                return municao;
            }
        }
        return null;
    }


    /*
		Método que pega a munição do estoque de munições e passa para o script do arco construir a trajetória
    */
    void DisparaMunicao()
    {
		// Pega a posição do click do mouse na tela
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Pega a munição e da spwan dela na posição do player
        GameObject municao = SpawnMunicao(transform.position);
        if (municao != null)
        {
            Arco arcoScript = municao.GetComponent<Arco>();
            float duracaoTrajetoria = 1.0f / velocidadeArma;
			// Constroi a trajetória em uma corrotina que recebe os pontos da trajetória
            StartCoroutine(arcoScript.arcoTrajetoria(posicaoMouse, duracaoTrajetoria));
        }
    }

	/*
		Ao distruir o objeto armas setta o estoque de munição para null
	*/
    private void OnDestroy()
    {
        municaoPiscina = null;
    }

	/*
		Altera a munição do player e reseta o estoque de munição do player para os novos projetéis
	*/
    public void UpgradeArma()
    {
        municaoPrefab = novaMunicaoPrefab;
        ResetPiscina();
    }
}
