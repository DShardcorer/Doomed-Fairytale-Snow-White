using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class TunnelOnwards : Object
    {
        public Divert divertAfter {
            get {
                return _divertAfter;
            }
            set {
                _divertAfter = value;
                if (_divertAfter) AddContent (_divertAfter);
            }
        }
        Divert _divertAfter;

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            var container = new Container ();

            // Set override path for tunnel onwards (or nothing)
            container.AddContent (ControlCommand.EvalStart ());

            if (divertAfter) {

                // Generate runtime object's generated code and steal the arguments runtime code
                var returnRuntimeObj = divertAfter.GenerateRuntimeObject ();
                var returnRuntimeContainer = returnRuntimeObj as Container;
                if (returnRuntimeContainer) {

                    // Steal all code for generating arguments from the divert
                    var args = divertAfter.arguments;
                    if (args != null && args.Count > 0) {

                        // Steal everything betwen eval start and eval end
                        int evalStart = -1;
                        int evalEnd = -1;
                        for (int i = 0; i < returnRuntimeContainer.content.Count; i++) {
                            var cmd = returnRuntimeContainer.content [i] as ControlCommand;
                            if (cmd) {
                                if (evalStart == -1 && cmd.commandType == ControlCommand.CommandType.EvalStart)
                                    evalStart = i;
                                else if (cmd.commandType == ControlCommand.CommandType.EvalEnd)
                                    evalEnd = i;
                            }
                        }

                        for (int i = evalStart + 1; i < evalEnd; i++) {
                            var obj = returnRuntimeContainer.content [i];
                            obj.parent = null; // prevent error of being moved between owners
                            container.AddContent (returnRuntimeContainer.content [i]);
                        }
                    }
                }
                
                // Supply the divert target for the tunnel onwards target, either variable or more commonly, the explicit name
                var returnDivertObj = returnRuntimeObj as InkRuntime.Divert;
                if( returnDivertObj != null && returnDivertObj.hasVariableTarget ) {
                    var runtimeVarRef = new InkRuntime.VariableReference (returnDivertObj.variableDivertName);
                    container.AddContent(runtimeVarRef);
                } else {
                    _overrideDivertTarget = new DivertTargetValue ();
                    container.AddContent (_overrideDivertTarget);
                }

            } 

            // No divert after tunnel onwards
            else {
                container.AddContent (new Void ());
            }

            container.AddContent (ControlCommand.EvalEnd ());

            container.AddContent (ControlCommand.PopTunnel ());

            return container;
        }

        public override void ResolveReferences (Story context)
        {
            base.ResolveReferences (context);

            if (divertAfter && divertAfter.targetContent)
                _overrideDivertTarget.targetPath = divertAfter.targetContent.runtimePath;
        }

        DivertTargetValue _overrideDivertTarget;
    }
}

