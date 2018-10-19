using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CollisionWithBlood : MonoBehaviour
{
    Vector4 channelMask = new Vector4(0, 1, 0, 0);

    int splatsX = 1;
    int splatsY = 1;

    public float splatScale = 1.0f;
    public GameObject Droplet = null;




    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject == Droplet)
        {
            CreateSplat(other);
            Destroy(Droplet);

        }

    }

    private void Update()
    {

        Droplet = GameObject.FindGameObjectWithTag("blood");

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
                newSplat.channelMask = channelMask;

                float splatscaleX = 1.0f / splatsX;
                float splatscaleY = 1.0f / splatsY;
                float splatsBiasX = Mathf.Floor(Random.Range(0, splatsX * 0.99f)) / splatsX;
                float splatsBiasY = Mathf.Floor(Random.Range(0, splatsY * 0.99f)) / splatsY;



                newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);

                SplatManagerSystem.instance.AddSplat(newSplat);
                GameObject.Destroy(newSplatObject);
        
    }
}
 
 


