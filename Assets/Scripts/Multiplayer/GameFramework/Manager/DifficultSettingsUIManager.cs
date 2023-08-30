using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultSettingsUIManager : MonoBehaviour
{
	[SerializeField] private Button cancelButton;
	[SerializeField] private Button confirmButton;
	[SerializeField] private Slider maxPointsSlider;
	[SerializeField] private Slider timeToAnswerSlider;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private int maxPointsToWin;
	[SerializeField] private float maxTimeToAnswer;
	[SerializeField] private TextMeshProUGUI maxPointsToWinText;
	[SerializeField] private TextMeshProUGUI maxTimeToAnswerText;
	private void Start()
	{

		maxPointsSlider.onValueChanged.AddListener(v => {
			maxPointsToWin = (int)v;
			maxPointsToWinText.text = "" + maxPointsToWin;
		});
		maxPointsToWin = DiffcultyOptionsManager.instance.MaxPointsToWin;
		maxPointsSlider.value = (float)maxTimeToAnswer;

		timeToAnswerSlider.onValueChanged.AddListener(v => {
			maxTimeToAnswer = v;
			maxTimeToAnswerText.text = maxTimeToAnswer.ToString("F1") + " sec";
		});
		maxTimeToAnswer = DiffcultyOptionsManager.instance.MaxTimeToAnswer;
		timeToAnswerSlider.value = maxTimeToAnswer;

		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
		DiffcultyOptionsManager.SetShowSettingsPanel += SetShowSettingsPanel;
	}

	private void OnDestroy()
	{
		cancelButton.onClick.RemoveListener(Cancel);
		confirmButton.onClick.RemoveListener(Confirm);
		DiffcultyOptionsManager.SetShowSettingsPanel -= SetShowSettingsPanel;
	}

	private void Cancel()
	{
		DiffcultyOptionsManager.SetShowSettingsPanel?.Invoke(false);
	}

	private void Confirm()
	{
		DiffcultyOptionsManager.instance.MaxPointsToWin = maxPointsToWin;
		DiffcultyOptionsManager.instance.MaxTimeToAnswer = maxTimeToAnswer;
		DiffcultyOptionsManager.SetShowSettingsPanel?.Invoke(false);
	}

	private void SetShowSettingsPanel(bool val)
	{
		settingsPanel.SetActive(val);
	}
}
