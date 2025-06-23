using UnityEngine;
using Helpers;

namespace EntityBase.NPC.AI.SubControllers
{
    public class KeepPositionSubAIController : NPCSubAIController
    {
        private Vector3 _originalPosition;
        private float _returnRadius = 1.5f;
        private bool _needsToReturn = false;
        private string _subcontrollerIdToChangeToWhenTargetSpotted;

        public KeepPositionSubAIController(
            string subcontrollerIdToChangeToWhenTargetSpotted = HelperNPCSubAIControllerName.Flee)
        {
            _subcontrollerIdToChangeToWhenTargetSpotted = subcontrollerIdToChangeToWhenTargetSpotted;
        }

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            // Store the original position when first initialized
            _originalPosition = npc.View.transform.position;
            _returnRadius = 2f;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Check if we need to return to original position
            float distanceFromOrigin = Vector3.Distance(npc.View.transform.position, _originalPosition);
            if (distanceFromOrigin > _returnRadius)
            {
                _needsToReturn = true;
                MoveToPosition(_originalPosition);
            }
            else
            {
                _needsToReturn = false;
                ChangeToState(HelperNPCStateName.Idle);
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            // If we spot a target, flee
            if (HasTarget())
            {
                RequestControllerChange(_subcontrollerIdToChangeToWhenTargetSpotted, "Target spotted while idling");
                return;
            }

            // Check if we're returning to position
            if (_needsToReturn)
            {
                string currentStateId = parent.GetCurrentStateId();

                if (currentStateId == HelperNPCStateName.Idle)
                {
                    // We've reached our destination or path failed
                    float currentDistance = Vector3.Distance(npc.View.transform.position, _originalPosition);

                    // If we're still too far, try again
                    if (currentDistance > _returnRadius * 1.5f)
                    {
                        MoveToPosition(_originalPosition);
                    }
                    else
                    {
                        _needsToReturn = false;
                    }
                }
                else if (currentStateId == HelperNPCStateName.Move)
                {
                    // Check if we're close enough to stop
                    float currentDistance = Vector3.Distance(npc.View.transform.position, _originalPosition);
                    if (currentDistance <= _returnRadius)
                    {
                        _needsToReturn = false;
                        ChangeToState(HelperNPCStateName.Idle);
                    }
                }
            }
        }

        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Let global conditions control this controller
            return false;
        }
    }
}