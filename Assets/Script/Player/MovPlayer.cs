using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovPlayer : NetworkBehaviour
{
    NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    public Vector2 Dir { get; private set; }
    Rigidbody2D rb;
    private float speed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    void Update()
    {
        // if (!IsOwner) return;

        Dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        

    }

    void FixedUpdate()
    {
        Mover();
    }

    public void Mover()
    {
        rb.MovePosition(rb.position + Dir * (speed * Time.deltaTime));
    }

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
