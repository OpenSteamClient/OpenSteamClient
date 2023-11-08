using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

/// <summary>
/// Creates a LessFunc for an IComparisonOperators value.
/// </summary>
//  Using this would have been much simpler, but generics are unsupported in UnmanagedCallersOnly functions...
// 	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
//	public static unsafe byte LessFunc(KeyType_t* firstPtr, KeyType_t* secondPtr) {
//		return Convert.ToByte(*firstPtr < *secondPtr);
//	}
public class LessFuncFactory {
	private static Dictionary<IntPtr, GCHandle> usedFuncs = new();
	delegate byte LessFunc(IntPtr firstPtr, IntPtr secondPtr);
	public static unsafe IntPtr CreateLessFunc(Type type) {
		if (!type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IComparisonOperators<,,>))) {
			throw new ArgumentException("Type " + type.Name + " does not support IComparisonOperators");
		}

		var icomparisonoperators = typeof(IComparisonOperators<,,>);
		Type[] icomparisonoperatorsTypeArgs = {type, type, typeof(bool)};
		icomparisonoperators = icomparisonoperators.MakeGenericType(icomparisonoperatorsTypeArgs);
		var lessThanFunc = icomparisonoperators.GetMethod("op_LessThan");
		if (lessThanFunc == null) {
			throw new ArgumentException("Type does not have <");
		}

		var func = new LessFunc(new Func<IntPtr, IntPtr, byte>((IntPtr firstPtr, IntPtr secondPtr) => {
			var first = Marshal.PtrToStructure(firstPtr, type);
			var second = Marshal.PtrToStructure(secondPtr, type);
			
			return Convert.ToByte(lessThanFunc.Invoke(first, new object?[] { second }));
		}));

		GCHandle handle = GCHandle.Alloc(func);
		IntPtr ptr = Marshal.GetFunctionPointerForDelegate<LessFunc>(func);
		SteamClient.CUtlLogger.Debug("Allocated LessFunc " + ptr);
		usedFuncs.Add(ptr, handle);
		return ptr;
	}
	public static unsafe void Free(IntPtr func) {
		if (!usedFuncs.ContainsKey(func)) {
			throw new ArgumentException("Func not found in usedFuncs");
		}

		SteamClient.CUtlLogger.Debug("Freeing LessFunc " + func);

		usedFuncs[func].Free();
		usedFuncs.Remove(func);
	}
}
[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlMap<KeyType_t, ElemType_t> where KeyType_t : unmanaged, IComparisonOperators<KeyType_t, KeyType_t, bool> where ElemType_t : unmanaged {
    public CUtlRBTree<Node_t, int, KeyType_t, CKeyLess> m_Tree;

	/// <summary>
	/// Creates a new CUtlMap
	/// </summary>
	/// <param name="growSize"></param>
	/// <param name="initSize"></param>
    public CUtlMap(int growSize = 0, int initSize = 0) {
        this.m_Tree = new CUtlRBTree<Node_t, int, KeyType_t, CKeyLess>(-1, 0, 1, growSize, initSize, (delegate* unmanaged[Cdecl]<KeyType_t*, KeyType_t*, byte>)LessFuncFactory.CreateLessFunc(typeof(KeyType_t)));
    }

	public Dictionary<KeyType_t, ElemType_t> ToManaged()
    {
        var dict = new Dictionary<KeyType_t, ElemType_t>();
		for (int i = 0; i < this.Count(); i++)
		{
			var node = Node(i);
			// Dictionaries are meant to be unique. Maps are meant to be unique. Are CUtlMaps? They can sometimes contain two of the same element though...
			if (dict.ContainsKey(node.key)) {
                SteamClient.CUtlLogger.Warning("Skipping duplicate key " + node.key + " in CUtlMap.ToManaged. Incorrect datatype lengths?");
                continue;
            }
			dict.Add(node.key, node.elem);
		}
		
		return dict;
    }

    public Dictionary<KeyType_t, ElemType_t> ToManagedAndFree() {
        var dict = this.ToManaged();
        this.Free();
        return dict;
    }

	public void Free() {
		LessFuncFactory.Free((nint)this.m_Tree.m_LessFunc);
		this.m_Tree.Free();
	}

	public ElemType_t Element( int i ) { 
		return m_Tree.Element( i ).elem; 
	}

	public Node_t Node( int i ) { 
		return m_Tree.Element( i ); 
	}

	public int Count() {
        return m_Tree.Count();
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Node_t
	{
		public Node_t()
		{
		}

		public Node_t( Node_t from )
		{
            this.key = from.key;
            this.elem = from.elem;
		}

		public KeyType_t key;
		public ElemType_t elem;
	};

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CKeyLess
	{
		public CKeyLess( delegate* unmanaged[Cdecl]<KeyType_t*, KeyType_t*, byte> lessFunc ) {
            this.m_LessFunc = lessFunc;
        }
		public delegate* unmanaged[Cdecl]<KeyType_t*, KeyType_t*, byte> m_LessFunc;
	};
}