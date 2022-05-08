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
}
