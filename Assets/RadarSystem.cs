using UnityEngine;
using HWRcomponent;
using System.Collections;

// Adapted from code as shown in: https://www.youtube.com/watch?time_continue=314&v=hIPC7kOV9DE

namespace HWRcomponent
{
	public enum Alignment
	{
		None,
		LeftTop,
		RightTop,
		LeftBot,
		RightBot,
		MiddleTop,
		MiddleBot
	}

}
public class RadarSystem : MonoBehaviour
{

	private Vector2 inposition; // position of radar in screen
	public float Size = 400; //  minimap size
	public float Distance = 100;// maximum distance of game objects
	public float Transparency = 0.5f;// Texture Transparency
	public Texture2D[] navigationTextures;// textutes list
	//public string[] EnemyTag;// object tags list


    public GameObject[] targets; // actual targets in radar

	public Texture2D NavCompass;// compass texture
	public Texture2D CompassBackground;// background texture
	public Vector2 PositionOffset = new Vector2 (0, 0);// minimap position offset
	public float Scale = 1;// mini map scale ( Scale < 1 = zoom in , Scale > 1 = zoom out)
	public Alignment PositionAlignment = Alignment.None;// position alignment
	public bool MapRotation;
	public GameObject Player;
	public bool Show = true;
	public Color ColorMult = Color.white;

    private GUIStyle textSyle = new GUIStyle();
	
	void Start ()
	{
        // set text style
        textSyle.fontSize = 7;
        textSyle.normal.textColor = ColorMult;
        textSyle.hover.textColor = ColorMult;

    }

    void Update ()
	{
		if (Player == null) {
			Player = this.gameObject;
		}
		
		if (Scale <= 0) {
			Scale = 100;
		}
		// define the position
		switch (PositionAlignment) {
		case Alignment.None:
			inposition = PositionOffset;
			break;
		case Alignment.LeftTop:
			inposition = Vector2.zero + PositionOffset;
			break;
		case Alignment.RightTop:
			inposition = new Vector2 (Screen.width - Size, 0) + PositionOffset;
			break;
		case Alignment.LeftBot:
			inposition = new Vector2 (0, Screen.height - Size) + PositionOffset;
			break;
		case Alignment.RightBot:
			inposition = new Vector2 (Screen.width - Size, Screen.height - Size) + PositionOffset;
			break;
		case Alignment.MiddleTop:
			inposition = new Vector2 ((Screen.width / 2) - (Size / 2), Size) + PositionOffset;
			break;
		case Alignment.MiddleBot:
			inposition = new Vector2 ((Screen.width / 2) - (Size / 2), Screen.height - Size) + PositionOffset;
			break;
		}
		
	}
	// convert 3D position to 2D position
	Vector2 ConvertToNavPosition (Vector3 pos)
	{
		Vector2 res = Vector2.zero;
		if (Player) {
			res.x = inposition.x + (((pos.x - Player.transform.position.x) + (Size * Scale) / 2f) / Scale);
			res.y = inposition.y + ((-(pos.z - Player.transform.position.z) + (Size * Scale) / 2f) / Scale);
		}
		return res;
	}

    /*
	void DrawNav (GameObject[] enemylists, Texture2D navtexture)
	{
        
		if (Player != null) {
			for (int i=0; i<enemylists.Length; i++) {
                float player3dDistance = Vector3.Distance(Player.transform.position, enemylists[i].transform.position);

                if (player3dDistance <= (Distance * Scale)) {
					Vector2 pos = ConvertToNavPosition (enemylists [i].transform.position);
				
					if (Vector2.Distance (pos, (inposition + new Vector2 (Size / 2f, Size / 2f))) + (navtexture.width / 2) < (Size / 2f)) {
						float navscale = Scale;
						if (navscale < 1) {
							navscale = 1;
						}
                        Rect rectPosition = 
                            new Rect(pos.x - (navtexture.width / navscale) / 2, pos.y - (navtexture.height / navscale) / 2, navtexture.width / navscale, navtexture.height / navscale);

                        GUI.DrawTexture (rectPosition, navtexture);
                        
                        GUI.Label(new Rect(rectPosition.x, rectPosition.y, rectPosition.width, rectPosition.height), string.Format("{0:0.0}", player3dDistance), textSyle);
                    }
				}
			}
		}
	}*/

    void drawTargets(GameObject[] targets, Texture2D targetTexture)
    {
        if (Player == null)
        {
            return;
        }

        foreach(GameObject target in targets)
        {
            float player3dDistance = Vector3.Distance(Player.transform.position, target.transform.position);
            if (player3dDistance <= (Distance * Scale)) // is in radar
            {
                Vector2 targetPosInRadar = ConvertToNavPosition(target.transform.position);
                float targetDistance = Mathf.Min(
                    Vector2.Distance(targetPosInRadar, (inposition + new Vector2(Size / 2f, Size / 2f))) + (targetTexture.width / 2));
                if (targetDistance < (Size / 2f)){
                    float navScale = Mathf.Max(1, Scale);

                     Rect rectPosition =
                            new Rect(targetPosInRadar.x - (targetTexture.width / navScale) / 2, targetPosInRadar.y - (targetTexture.height / navScale) / 2, targetTexture.width / navScale, targetTexture.height / navScale);

                    // draw target on screen
                    GUI.DrawTexture(rectPosition, targetTexture);
                    // draw distance
                    GUI.Label(new Rect(rectPosition.x, rectPosition.y + targetTexture.height / 2, rectPosition.width, rectPosition.height), string.Format("{0:0.0}", targetDistance), textSyle);
                }
            }
           
        }
    }

	void OnGUI ()
	{
		if (!Show)
			return;
		
		GUI.color = new Color (ColorMult.r, ColorMult.g, ColorMult.b, Transparency);

		if (MapRotation) {
			GUIUtility.RotateAroundPivot (-(this.transform.eulerAngles.y), inposition + new Vector2 (Size / 2f, Size / 2f)); 
		}

        // draw targets
        /*
        for (int i=0; i<EnemyTag.Length; i++) {
			DrawNav (GameObject.FindGameObjectsWithTag (EnemyTag [i]), navigationTextures [i]);
		}*/

        drawTargets(targets, navigationTextures[0]);

        if (CompassBackground != null) { 
			GUI.DrawTexture (new Rect (inposition.x, inposition.y, Size, Size), CompassBackground);
        }

        GUIUtility.RotateAroundPivot ((this.transform.eulerAngles.y), inposition + new Vector2 (Size / 2f, Size / 2f));

        if (NavCompass != null)
        {
            GUI.DrawTexture(new Rect(inposition.x + (Size / 2f) - (NavCompass.width / 2f), inposition.y + (Size / 2f) - (NavCompass.height / 2f), NavCompass.width, NavCompass.height), NavCompass);
        }
	}
}




