using UnityEngine;
using System.Collections;

namespace SurvialShoooter.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public static GameObject playerGO;
        public static PlayerHealth playerStatus;
		public static PlayerMana playerMana;
		public static Texture2D cursorTexture;

		public static PlayerShooting playerShooting;



        // Use this for initialization
        void Awake()
        {
			playerGO = GameObject.FindGameObjectWithTag("Player");
            playerStatus = playerGO.GetComponent<PlayerHealth>();
			playerMana = playerGO.GetComponent<PlayerMana>();
			playerShooting = playerGO.transform.Find("GunEnd").GetComponent<PlayerShooting> ();

            //SetMouseCursor();
        }

		public static void SetMouseCursor(string iconName)
        {
			string mouseIconPath = "MouseIcon/";
			mouseIconPath += iconName;
			Texture2D cursorTexture = Instantiate(Resources.Load(mouseIconPath,typeof(Texture2D))) as Texture2D;
			//cursorTexture.Resize (64,64);
			//Texture2D cursorTexture = new Texture2D(128, 128);

			//cursorTexture = Instantiate (mouseIconTexture);
			//Debug.Log(cursorTexture);
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

        }

        public static void ResetMouseCursor()
        {
            cursorTexture = null;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }

		public static AnimatorStateInfo GetCurrentAnimatorStateInfo()
		{
			return playerStatus.GetAnimtor ().GetCurrentAnimatorStateInfo (playerStatus.GetAnimtor ().GetLayerIndex ("Base Layer"));
		}
    }
}
