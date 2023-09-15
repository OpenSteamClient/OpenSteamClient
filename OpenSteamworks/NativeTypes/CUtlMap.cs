using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlMap<KeyType_t, ElemType_t> where KeyType_t : unmanaged where ElemType_t : unmanaged {
    public CUtlRBTree<Node_t, int, KeyType_t, CKeyLess> m_Tree;

    public CUtlMap(int growSize = 0, int initSize = 0, delegate* unmanaged[Cdecl]<KeyType_t*, KeyType_t*, byte> lessFunc = null) {
        this.m_Tree = new CUtlRBTree<Node_t, int, KeyType_t, CKeyLess>(-1, 0, 1, growSize, initSize, lessFunc);
    }

	public Dictionary<KeyType_t, ElemType_t> ToManaged()
    {
        var dict = new Dictionary<KeyType_t, ElemType_t>();
		for (int i = 0; i < this.Count(); i++)
		{
			var node = Node(i);
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