using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class IpManager : MonoBehaviour
{
	public static IpManager instance;

	[Header(" elements ")]
	[SerializeField] private TextMeshProUGUI ipText;
	[SerializeField] private TextMeshProUGUI ipInputText;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		string ipAdress = GetLocalIpv4();
		ipText.text = "IP:\n" + ipAdress;

		UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
		utp.SetConnectionData(GetLocalIpv4(), 7777);
	}

	public string GetInputIp() { return ipInputText.text; }
	private string GetLocalIpv4()
	{
		return Dns.GetHostEntry(Dns.GetHostName())
					.AddressList.First(
						f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
					.ToString();
	}
}
