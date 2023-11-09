using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    float dir;
    Rigidbody2D rb;
    private float speed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkDespawn()
    {
        // if (IsOwner)
        // {
        //     Move();
        // }
    }
    
    void Update()
    {
        if (!IsOwner) return;

        dir = Input.GetAxisRaw("Horizontal");

    }

    void FixedUpdate()
    {
        Mover();
    }

    public void Mover()
    {
        rb.velocity = new Vector2(speed * dir, rb.velocity.y);
    }

    // public void Move()
    // {
    //     if (NetworkManager.Singleton.IsServer)
    //     {
    //         var randomPosition = GetRandomPositionOnPlane();
    //         transform.position = randomPosition;
    //         position.Value = randomPosition;
    //     }
    //     else
    //     {
    //         SubmitPositionRequestServerRpc();
    //     }
    // }
    
    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }


    
    
}
