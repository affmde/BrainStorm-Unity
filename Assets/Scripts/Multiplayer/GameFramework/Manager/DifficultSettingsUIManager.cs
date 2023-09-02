using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class DifficultSettingsUIManager : NetworkBehaviour
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

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
		NetworkManager.OnServerStopped += NetworkManager_OnServerStopped;
	}

	private void NetworkManager_OnServerStarted()
	{
		if (!IsServer) return;
		DiffcultyOptionsManager.onUpdateDifficultySettings += UpdateDifficultySettings;
		DiffcultyOptionsManager.SetShowSettingsPanel += SetShowSettingsPanel;
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		DiffcultyOptionsManager.SetShowSettingsPanel -= SetShowSettingsPanel;
		DiffcultyOptionsManager.onUpdateDifficultySettings -= UpdateDifficultySettings;
	}

	private void Start()
	{
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

	public override void OnDestroy()
	{
		if (NetworkManager)
		{
			NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
			NetworkManager.OnServerStopped -= NetworkManager_OnServerStopped;
		}
		cancelButton.onClick.RemoveListener(Cancel);
		confirmButton.onClick.RemoveListener(Confirm);

		base.OnDestroy();
	}

	private void Cancel()
	{
		DiffcultyOptionsManager.SetShowSettingsPanel?.Invoke(false);
	}

	private void Confirm()
	{
		if (!IsServer) return;
		ConfirmClientRpc(maxPointsToWin, maxTimeToAnswer);
	}
	[ClientRpc]
	private void ConfirmClientRpc(int maxPoints, float timeAnswer)
	{
		DiffcultyOptionsManager.instance.MaxPointsToWin = maxPoints;
		DiffcultyOptionsManager.instance.MaxTimeToAnswer = timeAnswer;
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
