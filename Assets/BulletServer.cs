using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletServer : Photon.MonoBehaviour
{
    Vector4 channelMask = new Vector4(1, 0, 0, 0);

    int splatsX = 1;
    int splatsY = 1;

    public float splatScale = 1.0f;
    public GameObject Plane = null;
    public GameObject Player = null;
    public int playerID;

    private void Awake()
    {
        Plane = GameObject.Find("Plane");
        //Player = GameObject.Find("PlayerX(Clone)");
        //playerID = Player.GetComponent<Player>().playerID;

        if (photonView.isMine)
        {
            GetComponent<Rigidbody>().velocity = GameObject.Find("PlayerX(Clone)").transform.forward * 6;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == Plane)
        {
            CreateSplat(other);
            Debug.Log(playerID);
            Destroy(gameObject);
        }
    }

    Vector4 ChooseChannelmask()
    {
        switch (playerID)
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
            //default:
            //    channelMask = new Vector4(1, 1, 1, 1);
            //    break;
        }
        return channelMask;
    }

    
    void CreateSplat(Collision other)
    {
        // Get how many splats are in the splat atlas
        splatsX = SplatManagerSystem.instance.splatsX;
        splatsY = SplatManagerSystem.instance.splatsY;

        Vector3 leftVec = Vector3.Cross(other.contacts[0].normal, Vector3.up);
        float randScale = Random.Range(0.5f, 1.5f);

        GameObject newSplatObject = new GameObject();
        newSplatObject.transform.position = other.contacts[0].point;
        if (leftVec.magnitude > 0.001f)
        {
            newSplatObject.transform.rotation = Quaternion.LookRotation(leftVec, other.contacts[0].normal);
        }
        newSplatObject.transform.RotateAround(other.contacts[0].point, other.contacts[0].normal, Random.Range(-180, 180));
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

    [PunRPC]
    void ChangePosition(Vector3 myposition)
    {
        GetComponent<Transform>().position = myposition;
        if (photonView.isMine)
        {
            photonView.RPC("ChangePostionTo", PhotonTargets.OthersBuffered, myposition);
        }
    }

}
