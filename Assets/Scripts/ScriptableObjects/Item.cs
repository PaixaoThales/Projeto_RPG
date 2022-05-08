using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
/// <summary>
/// Classe que modela os coletaveis e consumiveis do game
/// </summary>
public class Item : ScriptableObject {
	public string NomeObjeto; // Nome do item
	public Sprite sprite; // Sprite do item
	public int quantidade; // Quantidade do item
	public bool empilhavel; // Posibilidade de acumular

	public enum TipoItem {
		MOEDA,
		RUBI,
		ESMERALDA,
		SAFIRA,
		DIAMANTE,
		HEALTH
	}

	public TipoItem tipoItem;

	// Dicionário para mapear o tipo de item com o som que ele toca quando pega
	private Dictionary<TipoItem, string> tipoItemParaSomAoPegar = new Dictionary<TipoItem, string>()
	{
		{ TipoItem.MOEDA, "coin_pickup" },
		{ TipoItem.RUBI, "coin_pickup" },
		{ TipoItem.ESMERALDA, "coin_pickup" },
		{ TipoItem.SAFIRA, "coin_pickup" },
		{ TipoItem.HEALTH, "health_pickup" }
	};

	/*
		Método chamado quando o player pega um item
	*/
	public void AoPegar()
	{
		bool temSomAoPegar = tipoItemParaSomAoPegar.TryGetValue(this.tipoItem, out string somAoPegar);
		if (!temSomAoPegar) return;

		var som = Resources.Load<AudioClip>("Sons/" + somAoPegar);
		
		var audioSource = GameObject.Find("CenaManager").GetComponent<CenaManager>().sonsSource;
		audioSource.PlayOneShot(som);
	}
}
