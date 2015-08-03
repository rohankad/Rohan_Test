using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EventManager : Photon.MonoBehaviour {

	public static EventManager _instances ;


	public Text _PlayerName;
	
	public string _userName ;

	public string _t1 ;
	private PhotonView myPhotonView;
	public bool _o ;

	public string _receiveString ;
	void Awake()
	{
		_instances = this;
	}
	// Use this for initialization
	void Start () {
		//myPhotonView = this.gameObject.GetComponent<PhotonView>();
		//myPhotonView = OnClickInstantiate._MyPlayer.GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (myPhotonView.isMine) 
		{

		} else
		{
			_userName = _receiveString;
		}
		//_userName = _receiveString;
	}


	[PunRPC]
	void SendClicked(){
		
		print ("Message in player controller ");

	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(_PlayerName.text);
			print("Writing");
		}
		else
		{
			// Network player, receive data

			print("Receiving");
				this._receiveString =(string)stream.ReceiveNext();
		}
	}
	public void DisableScript()
	{
		this.enabled = false;
	}
	
	public void OnGUI()
	{
		
		GUI.Label(new Rect(200 , Screen.height/2+110, 350, 200),"Player Name : "+_userName);
	}
}
