using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPlayers : Photon.MonoBehaviour {
    Vector4 channelMask = new Vector4(1, 0, 0, 0);

    int splatsX = 1;
    int splatsY = 1;
    bool dead = false;

    public float splatScale = 1.0f;
    Transform target; //the enemy's target
    int moveSpeed = 3; //move speed
    int playerID = 1;


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
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Death();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //die
    }

    void Death()
    {
        dead = true;
        CreateSplat();
        FindObjectOfType<SpawnEnemies>().enemycount--;
        Destroy(gameObject, 1);

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
            default:
                channelMask = new Vector4(1, 1, 1, 1);
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
        //if (photonView.isMine)
        //{
            photonView.RPC("ChangePostionTo", PhotonTargets.OthersBuffered, myposition);
        //}
    }
}
