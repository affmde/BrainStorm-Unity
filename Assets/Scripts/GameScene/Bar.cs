using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
	[SerializeField] Image fillBar;
	[SerializeField] GameObject manager;
	private GameManager gm;
	private float fillAmount;

	private void Awake()
	{
		gm = manager.GetComponent<GameManager>();
	}
	private void Update()
	{
		fillAmount = (float)gm.GetTotalCorrect() / (float)gm.GetTotal();
		fillBar.fillAmount = fillAmount;
	}
}
