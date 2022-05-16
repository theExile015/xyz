using PixelCrew.Creatures.Mobs.Patrlolling;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Tentacles
{
    public class TentacleAI : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Patrol _patrol;

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(_patrol.DoPatrol());
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }


        // Update is called once per frame
        private void Update()
        {
            if (_direction.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if(_direction.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
}