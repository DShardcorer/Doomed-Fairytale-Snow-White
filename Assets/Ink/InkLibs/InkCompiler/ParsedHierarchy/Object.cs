using System.Collections.Generic;
using System.Text;
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
	public abstract class Object
	{
        public DebugMetadata debugMetadata {
            get {
                if (_debugMetadata == null) {
                    if (parent) {
                        return parent.debugMetadata;
                    }
                }

                return _debugMetadata;
            }

            set {
                _debugMetadata = value;
            }
        }
        private DebugMetadata _debugMetadata;

        public bool hasOwnDebugMetadata {
            get {
                return _debugMetadata != null;
            }
        }

        public virtual string typeName {
            get {
                return GetType().Name;
            }
        }

		public Object parent { get; set; }
        public List<Object> content { get; protected set; }

        public Story story {
            get {
                Object ancestor = this;
                while (ancestor.parent) {
                    ancestor = ancestor.parent;
                }
                return ancestor as Story;
            }
        }

		private InkRuntime.Object _runtimeObject;
		public InkRuntime.Object runtimeObject
		{
			get {
				if (_runtimeObject == null) {
					_runtimeObject = GenerateRuntimeObject ();
                    if( _runtimeObject )
                        _runtimeObject.debugMetadata = debugMetadata;
				}
				return _runtimeObject;
			}

			set {
				_runtimeObject = value;
			}
		}

        // virtual so that certian object types can return a different
        // path than just the path to the main runtimeObject.
        // e.g. a Choice returns a path to its content rather than
        // its outer container.
        public virtual InkRuntime.Path runtimePath
        {
            get {
                return runtimeObject.path;
            }
        }

        // When counting visits and turns since, different object
        // types may have different containers that needs to be counted.
        // For most it'll just be the object's main runtime object,
        // but for e.g. choices, it'll be the target container.
        public virtual Container containerForCounting
        {
            get {
                return this.runtimeObject as Container;
            }
        }

        public Path PathRelativeTo(Object otherObj)
        {
            var ownAncestry = ancestry;
            var otherAncestry = otherObj.ancestry;

            Object highestCommonAncestor = null;
            int minLength = System.Math.Min (ownAncestry.Count, otherAncestry.Count);
            for (int i = 0; i < minLength; ++i) {
                var a1 = ancestry [i];
                var a2 = otherAncestry [i];
                if (a1 == a2)
                    highestCommonAncestor = a1;
                else
                    break;
            }

            FlowBase commonFlowAncestor = highestCommonAncestor as FlowBase;
            if (commonFlowAncestor == null)
                commonFlowAncestor = highestCommonAncestor.ClosestFlowBase ();


            var pathComponents = new List<Identifier> ();
            bool hasWeavePoint = false;
            FlowLevel baseFlow = FlowLevel.WeavePoint;

            var ancestor = this;
            while(ancestor && (ancestor != commonFlowAncestor) && !(ancestor is Story)) {

                if (ancestor == commonFlowAncestor)
                    break;

                if (!hasWeavePoint) {
                    var weavePointAncestor = ancestor as IWeavePoint;
                    if (weavePointAncestor != null && weavePointAncestor.identifier != null) {
                        pathComponents.Add (weavePointAncestor.identifier);
                        hasWeavePoint = true;
                        continue;
                    }
                }

                var flowAncestor = ancestor as FlowBase;
                if (flowAncestor) {
                    pathComponents.Add (flowAncestor.identifier);
                    baseFlow = flowAncestor.flowLevel;
                }

                ancestor = ancestor.parent;
            }

            pathComponents.Reverse ();

            if (pathComponents.Count > 0) {
                return new Path (baseFlow, pathComponents);
            }

            return null;
        }

        public List<Object> ancestry
        {
            get {
                var result = new List<Object> ();

                var ancestor = this.parent;
                while(ancestor) {
                    result.Add (ancestor);
                    ancestor = ancestor.parent;
                }

                result.Reverse ();

                return result;
            }
        }

        public string descriptionOfScope
        {
            get {
                var locationNames = new List<string> ();

                Object ancestor = this;
                while (ancestor) {
                    var ancestorFlow = ancestor as FlowBase;
                    if (ancestorFlow && ancestorFlow.identifier != null) {
                        locationNames.Add ("'" + ancestorFlow.identifier + "'");
                    }
                    ancestor = ancestor.parent;
                }

                var scopeSB = new StringBuilder ();
                if (locationNames.Count > 0) {
                    var locationsListStr = string.Join (", ", locationNames.ToArray());
                    scopeSB.Append (locationsListStr);
                    scopeSB.Append (" and ");
                }

                scopeSB.Append ("at top scope");

                return scopeSB.ToString ();
            }
        }

        // Return the object so that method can be chained easily
        public T AddContent<T>(T subContent) where T : Object
        {
            if (content == null) {
                content = new List<Object> ();
            }

            // Make resilient to content not existing, which can happen
            // in the case of parse errors where we've already reported
            // an error but still want a valid structure so we can
            // carry on parsing.
            if( subContent ) {
                subContent.parent = this;
                content.Add(subContent);
            }

            return subContent;
        }

        public void AddContent<T>(List<T> listContent) where T : Object
        {
            foreach (var obj in listContent) {
                AddContent (obj);
            }
        }

        public T InsertContent<T>(int index, T subContent) where T : Object
        {
            if (content == null) {
                content = new List<Object> ();
            }

            subContent.parent = this;
            content.Insert (index, subContent);

            return subContent;
        }

        public delegate bool FindQueryFunc<T>(T obj);
        public T Find<T>(FindQueryFunc<T> queryFunc = null) where T : class
        {
            var tObj = this as T;
            if (tObj != null && (queryFunc == null || queryFunc (tObj) == true)) {
                return tObj;
            }

            if (content == null)
                return null;

            foreach (var obj in content) {
                var nestedResult = obj.Find (queryFunc);
                if (nestedResult != null)
                    return nestedResult;
            }

            return null;
        }


        public List<T> FindAll<T>(FindQueryFunc<T> queryFunc = null) where T : class
        {
            var found = new List<T> ();

            FindAll (queryFunc, found);

            return found;
        }

        void FindAll<T>(FindQueryFunc<T> queryFunc, List<T> foundSoFar) where T : class
        {
            var tObj = this as T;
            if (tObj != null && (queryFunc == null || queryFunc (tObj) == true)) {
                foundSoFar.Add (tObj);
            }

            if (content == null)
                return;

            foreach (var obj in content) {
                obj.FindAll (queryFunc, foundSoFar);
            }
        }

		public abstract InkRuntime.Object GenerateRuntimeObject ();

        public virtual void ResolveReferences(Story context)
		{
            if (content != null) {
                foreach(var obj in content) {
                    obj.ResolveReferences (context);
                }
            }
		}

        public FlowBase ClosestFlowBase()
        {
            var ancestor = this.parent;
            while (ancestor) {
                if (ancestor is FlowBase) {
                    return (FlowBase)ancestor;
                }
                ancestor = ancestor.parent;
            }

            return null;
        }

        public virtual void Error(string message, Object source = null, bool isWarning = false)
		{
			if (source == null) {
				source = this;
			}

            // Only allow a single parsed object to have a single error *directly* associated with it
            if (source._alreadyHadError && !isWarning) {
                return;
            }
            if (source._alreadyHadWarning && isWarning) {
                return;
            }

            if (this.parent) {
                this.parent.Error (message, source, isWarning);
            } else {
                throw new System.Exception ("No parent object to send error to: "+message);
            }

            if (isWarning) {
                source._alreadyHadWarning = true;
            } else {
                source._alreadyHadError = true;
            }

		}

        public void Warning(string message, Object source = null)
        {
            Error (message, source, isWarning: true);
        }

        // Allow implicit conversion to bool so you don't have to do:
        // if( myObj != null ) ...
        public static implicit operator bool (Object obj)
        {
            var isNull = object.ReferenceEquals (obj, null);
            return !isNull;
        }

        public static bool operator ==(Object a, Object b)
        {
            return object.ReferenceEquals (a, b);
        }

        public static bool operator !=(Object a, Object b)
        {
            return !(a == b);
        }

        public override bool Equals (object obj)
        {
            return object.ReferenceEquals (obj, this);
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        bool _alreadyHadError;
        bool _alreadyHadWarning;
	}
}

