using UnityEngine;
public class GridSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gridSprite;

    public Vector2 GetSizeGrid()
    {
        return gridSprite.sprite.bounds.size;
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
