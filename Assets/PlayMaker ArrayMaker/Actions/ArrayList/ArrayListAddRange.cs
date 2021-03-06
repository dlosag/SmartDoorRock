//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Add several items to a PlayMaker Array List Proxy component")]
	public class ArrayListAddRange : ArrayListActions
	{
		
		[ActionSection("Set up")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;
		
		[ActionSection("Data")]
		
		[RequiredField]
		[Tooltip("The variables to add.")]
		public FsmVar[] variables;

		[Tooltip("Ints can be stored as bytes, useful when serializing over network for efficiency")]
		public bool convertIntsToBytes;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variables = new FsmVar[2];

			convertIntsToBytes = false;
		}
		
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				DoArrayListAddRange();
			
			Finish();
		}

		public void DoArrayListAddRange()
		{
			if (! isProxyValid() ) 
				return;
			
			foreach(FsmVar _var in variables)
			{
				var _value = PlayMakerUtils.GetValueFromFsmVar(Fsm,_var);
				
				if (_var.Type == VariableType.Int && convertIntsToBytes)
				{
					proxy.Add(System.Convert.ToByte(_value),_var.Type.ToString(),true);
				}else{
					proxy.Add(_var,_var.Type.ToString(),true);
				}

			}
			
		}
		
		
	}
}