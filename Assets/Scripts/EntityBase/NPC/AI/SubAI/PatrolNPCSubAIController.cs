using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace EntityBase.NPC.AI.SubControllers
{
    public class PatrolNPCSubAIController : NPCSubAIController
    {
        private List<Vector3> _patrolPoints = new List<Vector3>();
        private int _currentPatrolIndex = 0;
        private float _waitTime = 2f;
        private float _waitTimer = 0f;
        private bool _isWaiting = false;
        private Vector3 _spawnPosition;

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _waitTime = config.patrolWaitTime;
        }

        /// <summary>
        /// Sets custom patrol points for this NPC. If not called, will fall back to generating random points.
        /// </summary>
        /// <param name="patrolPoints">List of world positions to patrol between</param>
        public void SetPatrolPoints(List<Vector3> patrolPoints)
        {
            if (patrolPoints != null && patrolPoints.Count > 0)
            {
                _patrolPoints.Clear();
                _patrolPoints.AddRange(patrolPoints);
                _currentPatrolIndex = 0;
            }
            else
            {
                Debug.LogWarning("Attempted to set empty or null patrol points list");
            }
            for (int i = 0; i < _patrolPoints.Count; i++)
            {
                Debug.LogWarning($"Patrol Point {i}: {_patrolPoints[i]}");
            }
        }
        

        /// <summary>
        /// Gets a copy of the current patrol points
        /// </summary>

        /// <summary>
        /// Clears any custom patrol points and forces regeneration on next OnEnter
        /// </summary>
        public void ClearCustomPatrolPoints()
        {
            _patrolPoints.Clear();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _spawnPosition = npc.View.transform.position;
            

            _currentPatrolIndex = 0;
            _isWaiting = false;
            _waitTimer = 0f;
            if (_patrolPoints.Count > 0)
            {
                MoveToCurrentPatrolPoint();
            }
            else
            {
                Debug.LogWarning("No patrol points available to move to");
            }
        }

        // private void GeneratePatrolPoints()
        // {
        //     _patrolPoints.Clear();
        //     int pointCount = Random.Range(3, 6);
        //
        //     for (int i = 0; i < pointCount; i++)
        //     {
        //         float angle = i * (360f / pointCount) + Random.Range(-30f, 30f);
        //         float rad = angle * Mathf.Deg2Rad;
        //         float distance = _patrolRadius * Random.Range(0.6f, 1f);
        //
        //         Vector3 point = _spawnPosition + new Vector3(
        //             Mathf.Cos(rad) * distance,
        //             Mathf.Sin(rad) * distance,
        //             0
        //         );
        //
        //         _patrolPoints.Add(point);
        //     }
        // }

        private void MoveToCurrentPatrolPoint()
        {
            if (_patrolPoints.Count > 0)
            {
                Debug.LogWarning("Moving to patrol point: " + _patrolPoints[_currentPatrolIndex]);
                MoveToPosition(_patrolPoints[_currentPatrolIndex]);
            }
        }

        public override void UpdateLogic()
        {
            // If we have a target, request switch to combat
            if (HasTarget())
            {
                RequestControllerChange(HelperNPCSubAIControllerName.Combat, "Target spotted during patrol");
                return;
            }

            // Handle patrol logic
            string currentStateId = parent.GetCurrentStateId();

            if (currentStateId == HelperNPCStateName.Idle)
            {
                if (_isWaiting)
                {
                    _waitTimer += Time.deltaTime;
                    if (_waitTimer >= _waitTime)
                    {
                        _isWaiting = false;
                        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
                        MoveToCurrentPatrolPoint();
                    }
                }
                else
                {
                    _isWaiting = true;
                    _waitTimer = 0f;
                }
            }
        }

        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Switch to combat if we have a target
            return false;
        }
    }
}