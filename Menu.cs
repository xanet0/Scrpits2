using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class Menu : MonoBehaviourPunCallbacks
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("lol");
        }
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("2322");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("2322");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void CreateGame()
    {
        SceneManager.LoadScene("Game");
    }
}
