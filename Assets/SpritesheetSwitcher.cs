using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesheetSwitcher : MonoBehaviour
{
    public List<Sprite> spriteSheet;
    public string currentSprite;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (_spriteRenderer == null) {return;}
        if (spriteSheet == null) {return;}

        currentSprite = _spriteRenderer.sprite.name;
        var newSprite = spriteSheet.Find(x => x.name == currentSprite);

        if (newSprite)
        {
            _spriteRenderer.sprite = newSprite;
        }
    }
}
