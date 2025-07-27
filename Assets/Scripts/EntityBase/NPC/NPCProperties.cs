using System.Collections.Generic;
using EntityBase.Faction;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC
{
    public class NPCProperties : EntityProperties
    {
        private Vector3 _targetPosition;
        private string _returnState = HelperNPCStateName.Idle;

        public Vector3 TargetPosition => _targetPosition;
        public string ReturnState => _returnState;

        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position;
        }

        public void SetReturnState(string state)
        {
            _returnState = state;
        }
        
        private NPCObjective _currentObjective;
        public NPCObjective CurrentObjective => _currentObjective;
        
        
        public NPCProperties(EntityFaction entityFaction, float moveSpeed) : base(entityFaction, moveSpeed)
        {


        }

    }
}

