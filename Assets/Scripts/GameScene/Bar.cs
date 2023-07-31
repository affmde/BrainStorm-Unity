using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
	[SerializeField] Image fillBar;
	[SerializeField] GameObject manager;
	private GameManager gm;
	private float fillAmount;
	private void Update()
	{
		fillAmount = Mathf.Clamp01(gm.GetTotalCorrect() / gm.GetTotal());
		fillBar.fillAmount = fillAmount;
	}
}
