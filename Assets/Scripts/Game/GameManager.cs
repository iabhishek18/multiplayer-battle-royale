using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float matchDuration = 1200f;
    [SerializeField] private float zoneStartDelay = 120f;
    [SerializeField] private int maxPlayers = 100;

    private float matchTimer;
    private int playersAlive;
    private List<string> eliminationFeed = new List<string>();
    private bool matchStarted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playersAlive = PhotonNetwork.CurrentRoom.PlayerCount;
            matchTimer = matchDuration;
            matchStarted = true;
        }
    }

    private void Update()
    {
        if (!matchStarted || !PhotonNetwork.IsMasterClient) return;

        matchTimer -= Time.deltaTime;

        if (matchTimer <= matchDuration - zoneStartDelay)
        {
            ZoneController.Instance?.ShrinkZone();
        }

        if (playersAlive <= 1)
        {
            EndMatch();
        }
    }

    public void PlayerEliminated(string victim, string killer)
    {
        photonView.RPC("RPC_PlayerEliminated", RpcTarget.All, victim, killer);
    }

    [PunRPC]
    private void RPC_PlayerEliminated(string victim, string killer)
    {
        playersAlive--;
        eliminationFeed.Add($"{killer} eliminated {victim}");
        if (eliminationFeed.Count > 5) eliminationFeed.RemoveAt(0);
    }

    private void EndMatch()
    {
        matchStarted = false;
        photonView.RPC("RPC_MatchEnded", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_MatchEnded()
    {
        Debug.Log("Match Over! Winner: " + PhotonNetwork.LocalPlayer.NickName);
    }
}
