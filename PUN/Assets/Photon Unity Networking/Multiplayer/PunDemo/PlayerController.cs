using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : Photon.MonoBehaviour {
	public float speed;
	public Animation _anim;
	private PhotonView myPhotonView;
	
	private string userIdInput = "";
	
	private string inputLine = "";
	
	public Text _ChatBox; 
	
	public Text _PlayerName;
	private string _temp_name;
	//	public InputField _Message;
	private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	public bool _k1 ;
	public enum EPlayerStatusEnum
	{
		Idle,
		Walk
		
	}
	public EPlayerStatusEnum ePlayerStatus;
	bool stream1 ;
	public string _currentAni ="Idle";
	
	public string _receiveString ;
	public string _ReceiveName;
	public string _ReceiveMessage;


	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animation> ();
		ePlayerStatus = EPlayerStatusEnum.Idle;
		_anim.Play ("Idle");
		
		myPhotonView = this.gameObject.GetComponent<PhotonView>();
		
		if (myPhotonView.isMine) {
			
			_temp_name = PlayerPrefs.GetString ("Player_Name");
			_PlayerName.text = _temp_name;
			_k1 = myPhotonView.isMine ;


			
		} else 
		{
			if(_ReceiveName == ""){
				_ReceiveName="No Name "+PhotonNetwork.player.ID;
			}else{
			_PlayerName.text = _ReceiveName;
				_ChatBox.text = _ReceiveMessage;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		//_PlayerName.text = EventManager._instances._userName ;
		if (myPhotonView.isMine) {
			Rotate ();
			MovePlayer ();
			PlayAnimations ();
		} else
		{
		//	_PlayerName.text = EventManager._instances._userName ; //Rohan

			transform.position = correctPlayerPos;
			transform.rotation = correctPlayerRot;	
			PlayAnimations1 ();
			_PlayerName.text = _ReceiveName;
			_ChatBox.text = _ReceiveMessage;
		}
	}
	
	
	[PunRPC]
	void SendClicked(){
		
		print ("Message in player controller ");
	}
	
	private void PlayAnimations(){
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			_anim.Play ("Walk");
			_currentAni = "Walk";
		} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			_anim.Play ("Walk");
			_currentAni = "Walk";
		} else if(Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow)|| Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow )){
			_anim.Play ("Idle");
			_currentAni = "Idle";
		}
		
	}
	private void PlayAnimations1()
	{
		if(_receiveString=="Walk"||_receiveString=="Idle")
		{
			_anim.Play (_receiveString);
		}
		
		
	}
	
	
	private void Rotate(){
		if(Input.GetAxis("Horizontal")<0){
			this.transform.Rotate(Vector3.up *-1* speed * Time.deltaTime);
		}else if(Input.GetAxis("Horizontal")>0){
			this.transform.Rotate(Vector3.up * speed * Time.deltaTime);
		}
	}
	
	private void MovePlayer()
	{
		if(Input.GetAxis("Vertical")<0){
			//print("forward");
			transform.Translate ( Vector3.forward *-1*.5f *Time.deltaTime);
		}else if(Input.GetAxis("Vertical")>0){
			//print("back");
			transform.Translate (Vector3.forward *.5f *Time.deltaTime);
		}
	}

	public void ShowMessageTop(string _message){
		print ("Message Received "+_message);

		_ChatBox.text = "" + _message;


	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream1 = stream.isWriting ;
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(_currentAni);
			stream.SendNext(_temp_name);
			stream.SendNext(_ChatBox.text);

		}
		else
		{
			// Network player, receive data
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			_receiveString = (string)stream.ReceiveNext();
			_ReceiveName = (string) stream.ReceiveNext();
			_ReceiveMessage = (string) stream.ReceiveNext();
		}
	}
	
	
	public void OnGUI()
	{
		
		GUI.Label(new Rect(20, Screen.height/2+110, 350, 200),""+stream1);
	}

}
