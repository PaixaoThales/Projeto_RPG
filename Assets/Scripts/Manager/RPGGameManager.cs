using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Classe que gerencia o Game, e tem funções que podem ser reutilizadas em outras cenas
///</summary>
public class RPGGameManager : MonoBehaviour {
	public static RPGGameManager instanciaCompartilhada = null;
	public RPGCameraManager cameraManager;

	public PontoSpawn playerPontoSpawn;

	/*
		Método que carrega ou não destroi esse objeto sempre que uma cena nova é carregada
	*/
	private void Awake() {
		if (instanciaCompartilhada != null && instanciaCompartilhada != this) {
			Destroy(gameObject);
		}
		else {
			instanciaCompartilhada = this;
		}
	}

	// Start is called before the first frame update
	void Start() {
		SetupScene();
	}

	/*
		Método que configura a cena chamando o método de spawn do player
	*/
	public void SetupScene() {
		SpawnPlayer();
	}

	/*
		Método que pega o objeto player e insere ele em outra cena no ponto de spaw definido
		Além disso diz para o Unity não destruir o objeto play ao carregar outra cena
	*/
	public void SpawnPlayer() {
		if (playerPontoSpawn != null) {
			GameObject player = CenaManager.instancia.player;
			if (player == null) {
				player = playerPontoSpawn.SpawnO();
				CenaManager.instancia.player = player;
				DontDestroyOnLoad(CenaManager.instancia.player);
			}
			player.transform.position = playerPontoSpawn.transform.position;
			cameraManager.virtualCamera.Follow = player.transform;
		}
	}

	// Update is called once per frame
	void Update() {

	}
}
