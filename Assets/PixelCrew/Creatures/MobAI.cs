﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components;

namespace PixelCrew.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1.0f;
        [SerializeField] private float _missHeroCooldown = 1.0f;
        
        private Coroutine _current;
        private GameObject _target;
        private bool _isDead;
       
        private static readonly int IsDeadKey = Animator.StringToHash("IsDead");

        private SpawnListComponent _particles;
        private Creatures _creature;
        private Animator _animator;
        private Patrol _patrol;
        private CapsuleCollider2D _collider;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creatures>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;

            StartState(AggroToHero());
        }

        private IEnumerator AggroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();   
                }
                yield return null;
            }

            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missHeroCooldown);
            _patrol.DoPatrol();
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            _collider.direction = CapsuleDirection2D.Horizontal;
            _collider.size = new Vector2(0.7f, 0.35f);

            if (_current != null)
                StopCoroutine(_current);
        }
    }
}
