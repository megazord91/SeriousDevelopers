using UnityEngine;
using System.Collections;

public class StatsInfo : MonoBehaviour {

	private int[] bonuses = {0, 0, 0, -3, -2, -2, -1, -1, -1, 0, 0, 0, 0, +1, +1, +1, +2, +2, +3 }; 

	public bool actve = true;

	[Header("Statistics")]
	[Range(3, 18)] public int STR = 9;
	[Range(3, 18)] public int CON = 9;
	[Range(3, 18)] public int DEX = 9;
	[Range(3, 18)] public int INT = 9;
	[Range(3, 18)] public int WIS = 9;
	[Range(3, 18)] public int CHR = 9;

	[Header("Hit Points")]
	public int HD = 3;
	public int HDBonus = 0;
	public int HP = 0;

	[Header("Combat Information")]
	public int AC = 10;
	public int THAC0 = 10;
	public int Damage = 8;
	public int Mutiplier = 1;
	public int Bonus = 0;

	[Space(10)]
	public bool showCallout = true;

	private string baseTxt = "";
	private string currentText = "";
	private GUIStyle fg, bg;

	void Start() {
		if (HP == 0) {
			int r;
			for (int i = 0; i < HD; i += 1) {
				r = Random.Range (1, 8) + bonuses [CON];
				HP += (r <= 0) ? 1 : r;
			}
		}

		fg = new GUIStyle ();
		fg.font = Resources.Load<Font>("Fonts/courier");
		fg.normal.textColor = Color.black;
		fg.normal.background = Texture2D.whiteTexture;
		fg.alignment = TextAnchor.MiddleLeft;
		fg.wordWrap = false;

		bg = new GUIStyle ();
		bg.normal.textColor = Color.black;
		bg.normal.background = Texture2D.whiteTexture;
		bg.alignment = TextAnchor.MiddleLeft;
		bg.wordWrap = false;

		baseTxt = "STR " + (STR < 10 ? " " : "") + STR + "\n";
		baseTxt += "CON " + (CON < 10 ? " " : "") + CON + "\n";
		baseTxt += "DEX " + (DEX < 10 ? " " : "") + DEX + "\n";
		baseTxt += "INT " + (INT < 10 ? " " : "") + INT + "\n";
		baseTxt += "WIS " + (WIS < 10 ? " " : "") + WIS + "\n";
		baseTxt += "CHR " + (CHR < 10 ? " " : "") + CHR + "\n";
	}

	void OnMouseEnter() {
		if (showCallout) currentText = baseTxt + "\nHP  " + (HP < 10 ? " " : "") + HP;
	}

	void OnMouseExit() {
		currentText = "";
	}

	void OnGUI() {
		if (showCallout && currentText != "") {
			fg.fontSize = (int) (Screen.width * 0.01f);
			bg.fontSize = (int) (Screen.width * 0.01f);
			float x = Event.current.mousePosition.x;
			float y = Event.current.mousePosition.y;
			float dx = fg.fontSize * 4;
			float dy = fg.fontSize * 8;
			float border = 10;
			GUI.Label( new Rect (x - border + 30, y - border, dx + border + border, dy + border + border), "", bg);
			GUI.Label( new Rect (x + 30, y, dx, dy), currentText, fg);
		}
	}
}
