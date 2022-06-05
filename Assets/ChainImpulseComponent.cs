using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainImpulseComponent : MonoBehaviour
{
    [SerializeField] private float _impulse = 5f;
    
    void Start()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Vector2.right * _impulse, ForceMode2D.Impulse);
    }

}
