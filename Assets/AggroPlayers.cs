using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPlayers : Photon.MonoBehaviour, IPunObservable {
    Vector4 channelMask = new Vector4(1, 0, 0, 0);

    int splatsX = 1;
    int splatsY = 1;
    bool dead = false;

    public float splatScale = 1.0f;
    Transform target; //the enemy's target
    int moveSpeed = 3; //move speed
    [SerializeField] public int playerIDinEnemy;
    public GameObject droplets;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;

    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private Quaternion syncStartPositionR = Quaternion.Euler(Vector3.zero);
    private Quaternion syncEndPositionR = Quaternion.Euler(Vector3.zero);


    void Awake()
    {
        
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        target = GameObject.FindWithTag("Player").transform;
        
        if (dead == false)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
       
    }

    void OnTriggerEnter(Collider collision)
    {
        //if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsAttacking == true)
        //{
            if (collision.gameObject.tag == "sword")
            {
                playerIDinEnemy = collision.gameObject.transform.parent.gameObject.GetComponent<Player>().playerIDinPlayer;
                Death();
            }
        //}
    }

    void Death()
    {
        //Debug.Log("Killed by player "+playerIDinEnemy);
        dead = true;
        Bleed();
        CreateSplat();
        FindObjectOfType<SpawnEnemies>().enemycount--;
        Destroy(gameObject, 1);

    }

    Vector4 ChooseChannelmask()
    {
        switch (playerIDinEnemy)
        {
            case 4:
                channelMask = new Vector4(1, 0, 0, 0);
                break;
            case 3:
                channelMask = new Vector4(0, 0, 1, 0);
                break;
            case 2:
                channelMask = new Vector4(0, 0, 0, 1);
                break;
            case 1:
                channelMask = new Vector4(0, 1, 0, 0);
                break;
            default:
                channelMask = new Vector4(0, 0, 0, 0);
                break;
        }
        return channelMask;
    }


    void CreateSplat()
    {
        // Get how many splats are in the splat atlas
        splatsX = SplatManagerSystem.instance.splatsX;
        splatsY = SplatManagerSystem.instance.splatsY;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(-Vector3.up), out hit, 100))
        {          
            Vector3 leftVec = Vector3.Cross(hit.normal, Vector3.up);
            float randScale = Random.Range(0.5f, 1.5f);

            GameObject newSplatObject = new GameObject();
            newSplatObject.transform.position = hit.point;
            if (leftVec.magnitude > 0.001f)
            {
                newSplatObject.transform.rotation = Quaternion.LookRotation(leftVec, hit.normal);
            }
            newSplatObject.transform.RotateAround(hit.point, hit.normal, Random.Range(-180, 180));
            newSplatObject.transform.localScale = new Vector3(randScale, randScale * 0.5f, randScale) * splatScale;

            Splat newSplat;
            newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
            newSplat.channelMask = ChooseChannelmask();

            float splatscaleX = 1.0f / splatsX;
            float splatscaleY = 1.0f / splatsY;
            float splatsBiasX = Mathf.Floor(Random.Range(0, splatsX * 0.99f)) / splatsX;
            float splatsBiasY = Mathf.Floor(Random.Range(0, splatsY * 0.99f)) / splatsY;

            newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);

            SplatManagerSystem.instance.AddSplat(newSplat);

            GameObject.Destroy(newSplatObject);
        }
    }

    [PunRPC]
    void ChangePosition(Vector3 myposition)
    {
        GetComponent<Transform>().position = myposition;
        if (photonView.isMine)
        {
            photonView.RPC("ChangePostionTo", PhotonTargets.OthersBuffered, myposition);
        }
    }

    void Bleed()
    {
        if (photonView.isMine)
        {
            GameObject droplet = PhotonNetwork.Instantiate(droplets.name, gameObject.transform.position, Quaternion.identity, 0) as GameObject;
            droplet.GetComponent<BulletServer>().playerIDinDroplet = playerIDinEnemy;
        }
    }
        

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Rigidbody>().position);
            stream.SendNext(GetComponent<Rigidbody>().velocity);
            stream.SendNext(GetComponent<Rigidbody>().rotation); //added for rotation
            // We own this player: send the others our data
            stream.SendNext(playerIDinEnemy);
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

            // Network player, receive data
            this.playerIDinEnemy = (int)stream.ReceiveNext();
        }
    }
}
