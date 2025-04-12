using System.Collections.Generic;
using System.Linq;
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class Conditional : Object
    {
		public Expression initialCondition { get; private set; }
		public List<ConditionalSingleBranch> branches { get; private set; }
        
        public Conditional (Expression condition, List<ConditionalSingleBranch> branches)
        {
            this.initialCondition = condition;
            if (this.initialCondition) {
                AddContent (condition);
            }

            this.branches = branches;
            if (this.branches != null) {
                AddContent (this.branches.Cast<Object> ().ToList ());
            }

        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            var container = new Container ();

            // Initial condition
            if (this.initialCondition) {
                container.AddContent (initialCondition.runtimeObject);
            }

            // Individual branches
            foreach (var branch in branches) {
                var branchContainer = (Container) branch.runtimeObject;
                container.AddContent (branchContainer);
            }

            // If it's a switch-like conditional, each branch
            // will have a "duplicate" operation for the original
            // switched value. If there's no final else clause
            // and we fall all the way through, we need to clean up.
            // (An else clause doesn't dup but it *does* pop)
            if (this.initialCondition != null && branches [0].ownExpression != null && !branches [branches.Count - 1].isElse) {
                container.AddContent (ControlCommand.PopEvaluatedValue ());
            }

            // Target for branches to rejoin to
            _reJoinTarget = ControlCommand.NoOp ();
            container.AddContent (_reJoinTarget);

            return container;
        }

        public override void ResolveReferences (Story context)
        {
            var pathToReJoin = _reJoinTarget.path;

            foreach (var branch in branches) {
                branch.returnDivert.targetPath = pathToReJoin;
            }

            base.ResolveReferences (context);
        }
            
        ControlCommand _reJoinTarget;
    }
}

