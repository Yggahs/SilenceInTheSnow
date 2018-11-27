using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnimation : MonoBehaviour {

    public void attack()
    {
        GetComponent<Animation>().Play();
    }
}
