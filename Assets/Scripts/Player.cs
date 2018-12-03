using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour, IPunObservable

{

    public float speed = 1f; // 1 is very fast
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    public AttackAnimation _attackAnimation;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private Quaternion syncStartPositionR = Quaternion.Euler(Vector3.zero);
    private Quaternion syncEndPositionR = Quaternion.Euler(Vector3.zero);
    //public bool IsAttacking = false;
    //relative movement
    public Transform cam;
    public Transform camPivot;
    float heading = 0;

    Vector2 input;
    




    [SerializeField] public int playerIDinPlayer = 0;
    // Use this for initialization
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //show player movement over network, serialize player id and sword's mesh collider status (enabled/disabled)
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Rigidbody>().position);
            stream.SendNext(GetComponent<Rigidbody>().velocity);
            stream.SendNext(GetComponent<Rigidbody>().rotation); //added for rotation
            stream.SendNext(playerIDinPlayer);
            stream.SendNext(gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled);
            

        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();
            Quaternion syncRotation = (Quaternion)stream.ReceiveNext(); //sync object's rotation

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncEndPosition = syncPosition + syncVelocity * syncDelay;

            syncEndPositionR = syncRotation * Quaternion.Euler(syncVelocity * syncDelay); //object start position for rotation
            syncStartPosition = GetComponent<Rigidbody>().position;
            syncStartPositionR = GetComponent<Rigidbody>().rotation; //object start position for rotation

            this.playerIDinPlayer = (int)stream.ReceiveNext();
            this.gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = (bool)stream.ReceiveNext();
        }
	}

    
    void Awake()
    {
        
        playerIDinPlayer = PhotonNetwork.player.ID;
        cam = GetComponent<Transform>();
        camPivot = GetComponent<Transform>();
        lastSynchronizationTime = Time.time;
        gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
        //switch player color based on id
        switch (playerIDinPlayer)
        {
            case 4:
                ChangeColorTo(new Vector3(255, 165, 0));
                break;
            case 3:
                ChangeColorTo(new Vector3(0, 255, 0));
                break;
            case 2:
                ChangeColorTo(new Vector3(0, 0, 255));
                break;
            case 1:
                ChangeColorTo(new Vector3(255, 0, 0));
                break;
            default:
                ChangeColorTo(new Vector3(0, 0, 0));
                break;
        }

    }

    private void Start()
    {
        //bind camera to player
        if (photonView.isMine)
        {
            Camera.main.transform.position = this.transform.position - this.transform.forward * 10 + this.transform.up * 3;
            Camera.main.transform.LookAt(this.transform.position);
            Camera.main.transform.parent = this.transform;

            switch(PhotonNetwork.player.ID)
            {
                case 4:
                    GameObject.FindGameObjectWithTag("nick4").GetComponent<Text>().text = PhotonNetwork.player.NickName;
                    break;
                case 3:
                    GameObject.FindGameObjectWithTag("nick3").GetComponent<Text>().text = PhotonNetwork.player.NickName;
                    break;
                case 2:
                    GameObject.FindGameObjectWithTag("nick2").GetComponent<Text>().text = PhotonNetwork.player.NickName;
                    break;
                case 1:
                    GameObject.FindGameObjectWithTag("nick1").GetComponent<Text>().text = PhotonNetwork.player.NickName;
                    print("kek");
                    break;

            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        
        if (photonView.isMine)
        {
            
            InputMovement();

            //InputColorChange();
        }
        else
        {
            SyncedMovement();
        }
    }

    void InputMovement() //camera relative movement
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 100;
        camPivot.rotation = Quaternion.Euler(0, heading, 0);

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        transform.position += ((camF * input.y + camR * input.x)*speed); //speed of player is determined by number that multiplies the first equation

        //if (Input.GetKeyDown(KeyCode.Space))
        //    GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Vector3.up * 2);

        if (Input.GetMouseButtonDown(0))
        {
            _attackAnimation.Attack();
            gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
            Invoke("DeactivateSword", 0.417f);
            //Debug.Log(gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled);
        }
    }
    //deactivates mesh collider for the player's sword
    void DeactivateSword()
    {
        gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
    }
    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        GetComponent<Rigidbody>().rotation = Quaternion.Lerp(syncStartPositionR, syncEndPositionR, syncTime / syncDelay); //apply synced rotation
    }

    //void InputColorChange()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    //    }
    //}

    //change color of player and show over network
    [PunRPC]
    void ChangeColorTo(Vector3 color)
    {
        GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z, 1f);
        if (photonView.isMine)
            photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, color);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GameObject.FindGameObjectWithTag("nick1").GetComponent<Text>().text);
        }
        else GameObject.FindGameObjectWithTag("nick1").GetComponent<Text>().text = (string) stream.ReceiveNext();
        
    }
}
