﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : Photon.MonoBehaviour, IPunObservable
{
    public AudioClip splat;

    Vector4 channelMask = new Vector4(1, 0, 0, 0);

    int splatsX = 1;
    int splatsY = 1;

    public float splatScale = 1.0f;
    public GameObject Plane = null;
    public GameObject Player = null;
    [SerializeField] public int playerIDinDroplet;

    //find the plane the will destroy the droplet, have droplet fly out of the enemy's back
    private void Awake()
    {
        Plane = GameObject.Find("Plane");

        if (photonView.isMine)
        {
            GetComponent<Rigidbody>().velocity = GameObject.Find("Enemy(Clone)").transform.forward * -5;
        }
    }

    //if it hits the plane, create a splat at contact position and destroy the droplet
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == Plane)
        {
            CreateSplat(other);
            Destroy(gameObject);
        }
    }
    //choose color based on the player who killed the enemy's id
    Vector4 ChooseChannelmask()
    {
        switch (playerIDinDroplet)
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

    //create splatter on the ground
    void CreateSplat(Collision other)
    {
        AudioSource.PlayClipAtPoint(splat, this.transform.position);
        

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
    //sync position of the droplet over the network
    [PunRPC]
    void ChangePosition(Vector3 myposition)
    {
        GetComponent<Transform>().position = myposition;
        if (photonView.isMine)
        {
            photonView.RPC("ChangePostionTo", PhotonTargets.OthersBuffered, myposition);
        }
    }
    //serialize the player who killed the enemy's id
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(playerIDinDroplet);
        }
        else
        {
            // Network player, receive data
            this.playerIDinDroplet = (int)stream.ReceiveNext();
        }
    }
}
