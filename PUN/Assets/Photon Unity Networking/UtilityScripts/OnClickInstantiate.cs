using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClickInstantiate : MonoBehaviour
{
    public GameObject Prefab;

	public GameObject _Panel;
	public GameObject _MessagePanel;
	public InputField _inputField;

    public int InstantiateType;
    private string[] InstantiateTypeNames = {"Mine", "Scene"};

    public bool showGui, _instDone;

	public GameObject _NameText;

	public GameObject _MyPlayer;

	public InputField _Message;
	//private PhotonView _PlayerPhoton;

	void Start(){
		_MessagePanel.SetActive (false);

		_instDone = false;
	}


    void OnClick()
    {

    }


	public void ClickAndSpawn(){
		if (PhotonNetwork.connectionStateDetailed != PeerState.Joined )
		{
			// only use PhotonNetwork.Instantiate while in a room.
			return;
		}

		_MyPlayer = PhotonNetwork.Instantiate(Prefab.name, new Vector3(0, 0.5f, 0), Quaternion.identity, 0) as GameObject;
		//_PlayerPhoton = _MyPlayer.GetComponent<PhotonView> ();
		_Panel.SetActive (false);


		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetString("Player_Name", _inputField.text);
		_MessagePanel.SetActive (true);

	}
	void SetName(){
		_NameText.GetComponent<Text> ().text = _inputField.text;
		_inputField.text = "";
	
	}

	public void PassMessage(){
		// Pass message here

			
	
			_MyPlayer. SendMessage ("ShowMessageTop", _Message.text);
//			_PlayerPhoton.RPC("SendClicked",PhotonTargets.All, _Message.text);
		_Message.text = "";

	}

    void OnGUI()
    {
        if (showGui)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 180, 0, 180, 50));
            InstantiateType = GUILayout.Toolbar(InstantiateType, InstantiateTypeNames);
            GUILayout.EndArea();
        }
    }


}
