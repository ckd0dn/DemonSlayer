using UnityEngine;

public struct MapData
{
    public SpriteRenderer roomSprite;
    public SpriteRenderer[] sprites;
    public int roomIdx;
    public bool isPlayerVisited;
    public bool isInPlayer;

    public MapData(SpriteRenderer sprite, int index)
    {
        roomSprite = sprite;
        sprites = roomSprite.GetComponentsInChildren<SpriteRenderer>();

        if (sprites.Length == 0)
        {
            sprites = new SpriteRenderer[] { roomSprite };
        }

        roomIdx = index;
        isPlayerVisited = false;
        isInPlayer = false;
    }

    public void SetAlpha(float value)
    {
        //roomSprite.color = new Color(1, 1, 1, value);
        
        foreach(var sprite in sprites)
        {
            sprite.color = new Color(1, 1, 1, value);            
        }        
    }
}

