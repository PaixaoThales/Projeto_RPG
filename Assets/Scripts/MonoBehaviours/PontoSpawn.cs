using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que controla o spwan dos inimigos
///</summary>
public class PontoSpawn : MonoBehaviour {
	public GameObject prefabParaSpawn;

	public float intervaloRepeticao;

	// Start is called before the first frame update
	void Start() {
		if (intervaloRepeticao > 0) {
			// invoca um inimigo
			InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);
		}
	}

	/*
		Método que se o inimigo existir no prefab, é retornado uma instância desse prefab
	*/
	public GameObject SpawnO() {
		if (prefabParaSpawn != null) {
			return Instantiate(prefabParaSpawn, transform.position, Quaternion.identity);
		}
		return null;
	}

	// Update is called once per frame
	void Update() {

	}
}
