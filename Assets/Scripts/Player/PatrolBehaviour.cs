using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PatrolBehaviour : MonoBehaviour
    {
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _patrolDelay = 1;

        private Queue<Transform> _patrolPointsQueue;
        private Transform _currentPatrolPoint;
        private bool _isWaiting;

        private void Awake()
        {
            _patrolPointsQueue = new Queue<Transform>(_patrolPoints);
            SetNextPatrolPoint();
        }

        private void Update()
        {
            if (!_isWaiting)
            {
                MoveToNextPatrolPoint();
            }

            RotateTowardsCurrentPatrolPoint();
        }

        private void MoveToNextPatrolPoint()
        {
            var distance = _speed * Time.deltaTime;
            var newPosition = Vector3.MoveTowards(transform.position, _currentPatrolPoint.position, distance);

            if (transform.position == _currentPatrolPoint.position)
            {
                _isWaiting = true;
                Invoke(nameof(SetNextPatrolPoint), _patrolDelay);
            }

            transform.position = newPosition;
        }

        private void RotateTowardsCurrentPatrolPoint()
        {
            if (_currentPatrolPoint != null)
            {
                transform.LookAt(_currentPatrolPoint);
            }
        }

        private void SetNextPatrolPoint()
        {
            _currentPatrolPoint = _patrolPointsQueue.Dequeue();
            _patrolPointsQueue.Enqueue(_currentPatrolPoint);
            _isWaiting = false;
        }
    }
}