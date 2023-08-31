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
		DiffcultyOptionsManager.onUpdateDifficultySettings += UpdateDifficultySettings;
		DiffcultyOptionsManager.SetShowSettingsPanel += SetShowSettingsPanel;

		maxPointsSlider.onValueChanged.AddListener(v => {
			maxPointsToWin = (int)v;
			maxPointsToWinText.text = "" + maxPointsToWin;
		});
		timeToAnswerSlider.onValueChanged.AddListener(v => {
			maxTimeToAnswer = v;
			maxTimeToAnswerText.text = maxTimeToAnswer.ToString("F1") + " sec";
		});

		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
	}

	private void OnDestroy()
	{
		cancelButton.onClick.RemoveListener(Cancel);
		confirmButton.onClick.RemoveListener(Confirm);
		DiffcultyOptionsManager.SetShowSettingsPanel -= SetShowSettingsPanel;
		DiffcultyOptionsManager.onUpdateDifficultySettings -= UpdateDifficultySettings;
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
		if (val)
			DiffcultyOptionsManager.onUpdateDifficultySettings?.Invoke();
		settingsPanel.SetActive(val);
	}

	private void UpdateDifficultySettings()
	{
		maxPointsToWin = DiffcultyOptionsManager.instance.MaxPointsToWin;
		maxPointsSlider.value = maxPointsToWin;
		maxPointsToWinText.text = "" + maxPointsToWin;
		maxTimeToAnswer = DiffcultyOptionsManager.instance.MaxTimeToAnswer;
		timeToAnswerSlider.value = maxTimeToAnswer;
		maxTimeToAnswerText.text = maxTimeToAnswer.ToString("F1") + " sec";
	}
}
