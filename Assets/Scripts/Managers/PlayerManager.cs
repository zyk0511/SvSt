using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalShooter.Manager
{
    public class PlayerManager : MonoBehaviour
    {
		static PlayerManager instance;
		public static GameObject playerGO;
		public static GameObject HUDCanvasGO;
		public static GameObject skillPanelGO;
		public static GameObject itemPanelGO;
		public static GameObject mainCameraGO;
        public static PlayerHealth playerStatus;
		public static PlayerMana playerMana;
		public static Texture2D cursorTexture;

		public static PlayerShooting playerShooting;

		static GameObject messagePanelGO;
		static GameObject messageTextGO;

		RectTransform messagePanelRectTrans;

		public static PlayerManager GetInstance()
		{
			if (instance == null)
				instance = new PlayerManager();

			return instance;
		}

        // Use this for initialization
        void Awake()
        {
			//游戏开始时设定默认样式的鼠标指针
			ResetMouseCursor ();

			playerGO = GameObject.FindGameObjectWithTag("Player");
			HUDCanvasGO = GameObject.FindGameObjectWithTag ("HUDCanvas");
			skillPanelGO = GameObject.FindGameObjectWithTag ("SkillPanel");
			mainCameraGO = GameObject.FindGameObjectWithTag("MainCamera");
			itemPanelGO = HUDCanvasGO.transform.Find ("ItemPanel").gameObject;
			playerStatus = playerGO.GetComponent<PlayerHealth>();
			playerMana = playerGO.GetComponent<PlayerMana>();
			playerShooting = playerGO.transform.Find("GunEnd").GetComponent<PlayerShooting> ();

			//SetMouseCursor();

			messagePanelGO = GameObject.FindGameObjectWithTag ("MessagePanel");
			messageTextGO = messagePanelGO.transform.Find ("MessageText").gameObject;

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
			//string mouseIconPath = "MouseIcon/icons8-cursor-16";
			//Texture2D cursorTexture = Instantiate(Resources.Load(mouseIconPath,typeof(Texture2D))) as Texture2D;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }

		public static AnimatorStateInfo GetCurrentAnimatorStateInfo()
		{
			return playerStatus.GetAnimtor ().GetCurrentAnimatorStateInfo (playerStatus.GetAnimtor ().GetLayerIndex ("Base Layer"));
		}

		public void ShowMessageCanvas(bool isSkillMessage,string Infotext)
		{
			messagePanelRectTrans = messagePanelGO.GetComponent<RectTransform> (); 

			//Camera camera = mainCameraGO.GetComponent<Camera> ();
			//Vector2 localPoint;

			if (isSkillMessage) {
				//Vector2 skillInfoPosition = new Vector2 (-260f, 60f);
				//messagePanelRectTrans.localPosition = this.skillInfoPosition;
				//messagePanelRectTrans.transform.TransformPoint(skillInfoPosition);
				//RectTransformUtility.ScreenPointToLocalPointInRectangle (messagePanelRectTrans, skillInfoPosition, null, out localPoint);

				//anchoredPosition表示RectTransform中心点的位置
				messagePanelRectTrans.anchoredPosition = -260 * Vector2.right + 60 * Vector2.up;
			} else {
				//Vector2 itemInfoPosition = new Vector2 (-535f, 60f);
				//messagePanelRectTrans.localPosition = this.itemInfoPosition;
				//messagePanelRectTrans.transform.TransformPoint(itemInfoPosition);
				//RectTransformUtility.ScreenPointToLocalPointInRectangle (messagePanelRectTrans, itemInfoPosition, null, out localPoint);
				messagePanelRectTrans.anchoredPosition =  -535 * Vector2.right + 60 * Vector2.up;
			}

			messageTextGO.transform.GetComponent<Text> ().text = Infotext;

			int height = Infotext.Length / 20 * 20;
			height = height <= 70 ? 70 : height;

			//修改sizeDelta的值可动态调整RectTransform的大小
			messagePanelRectTrans.sizeDelta = 250 * Vector2.right + height * Vector2.up;

			messagePanelRectTrans.localScale = Vector3.one;
		}

		public void hideMessageCanvas()
		{
			messagePanelRectTrans = messagePanelGO.GetComponent<RectTransform> (); 
			messagePanelRectTrans.localScale = Vector3.zero;
			messageTextGO.transform.GetComponent<Text> ().text.Remove (0);
		}
    }
}
