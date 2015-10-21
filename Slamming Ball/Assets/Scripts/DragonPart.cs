using UnityEngine;
using System.Collections;

public class DragonPart : MonoBehaviour {

   public Sprite regularSprite;
   public Sprite fastSprite;

   public SpriteRenderer renderer;

	public void UseSprite(string matType)
   {
      switch (matType)
      {
         case "regular":
            renderer.sprite = regularSprite;
            break;
         case "fast":
            renderer.sprite = fastSprite;
            break;
      }
   }
}
