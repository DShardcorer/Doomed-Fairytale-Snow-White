using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class PatrolNPCSubAIController : NPCSubAIController
    {
        private List<Vector3> _patrolPoints = new List<Vector3>();
        private int _currentPatrolIndex = 0;
        private float _patrolRadius = 5f;
        private float _waitTime = 2f;
        private float _waitTimer = 0f;
        private bool _isWaiting = false;
        private Vector3 _spawnPosition;
        
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _patrolRadius = config.patrolRadius;
            _waitTime = config.patrolWaitTime;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _spawnPosition = npc.View.transform.position;
            GeneratePatrolPoints();
            _currentPatrolIndex = 0;
            _isWaiting = false;
            _waitTimer = 0f;
            
            // Start moving to first patrol point
            if (_patrolPoints.Count > 0)
            {
                MoveToCurrentPatrolPoint();
            }
            else
            {
                ChangeToState(HelperNPCStateName.Idle);
            }
        }
        
        private void GeneratePatrolPoints()
        {
            _patrolPoints.Clear();
            int pointCount = Random.Range(3, 6);
            
            for (int i = 0; i < pointCount; i++)
            {
                float angle = i * (360f / pointCount) + Random.Range(-30f, 30f);
                float rad = angle * Mathf.Deg2Rad;
                float distance = _patrolRadius * Random.Range(0.6f, 1f);
                
                Vector3 point = _spawnPosition + new Vector3(
                    Mathf.Cos(rad) * distance,
                    Mathf.Sin(rad) * distance,
                    0
                );
                
                _patrolPoints.Add(point);
            }
        }
        
        private void MoveToCurrentPatrolPoint()
        {
            if (_patrolPoints.Count > 0)
            {
                MoveToPosition(_patrolPoints[_currentPatrolIndex], "patrol_wait");
            }
        }
        
        public override void UpdateLogic()
        {
            // Check if we should switch to combat mode
            if (HasTarget())
            {
                parent.ChangeNPCSubAIController("combat");
                return;
            }
            
            // Handle patrol logic based on current state
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            
            if (currentStateName == "NPCIdleState")
            {
                // We're idling, check if we should wait or move
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
                    // Start waiting
                    _isWaiting = true;
                    _waitTimer = 0f;
                }
            }
        }
        
        protected override string GetIdleStateAfterMove()
        {
            return HelperNPCStateName.Idle; // Will trigger waiting logic
        }
    }
}