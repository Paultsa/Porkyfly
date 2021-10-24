
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Shop Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite[] sprites;
    public int[] costs;
    public Sprite[] stars;
}
