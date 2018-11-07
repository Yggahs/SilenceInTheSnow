using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletServer : Photon.MonoBehaviour
{
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPositionBullet = Vector3.zero;
    private Vector3 syncEndPositionBullet = Vector3.zero;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Rigidbody>().position);
            stream.SendNext(GetComponent<Rigidbody>().velocity);
        }
        else
        {
            Vector3 syncBulletPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncBulletVelocity = (Vector3)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;



            syncStartPositionBullet = GetComponent<Rigidbody>().position;
            syncEndPositionBullet = syncBulletPosition + syncBulletVelocity * syncDelay;
        }
    }

    void Awake()
    {
        lastSynchronizationTime = Time.time;
    }

    void Update()
    {
        
        
            SyncedMovement();
        
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPositionBullet, syncEndPositionBullet, syncTime / syncDelay);
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(syncStartPositionBullet, syncEndPositionBullet, syncTime / syncDelay);
    }
}
