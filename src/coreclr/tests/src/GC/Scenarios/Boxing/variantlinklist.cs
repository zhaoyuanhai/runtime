// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/**
 * Description:
 *      Mainly stresses the GC by creating a link list of Variants. A large number
 *      of nodes are created and leaks generated by traversing the link list and changing
 *      the variant objects in the nodes
 */


namespace DefaultNamespace {
    using System;

    internal class Node
    {
// disabling unused variable warning
#pragma warning disable 0414
        internal Object m_Var;
#pragma warning restore 0414

        internal Object [] m_aVar;
        internal Node next;

        public Node()
        {
            m_aVar = new Object[10];
            m_aVar[0] = 10;
            m_aVar[1] = ("ABC");
        }
    }

    internal class VariantLinkList
    {
        internal Node m_Root;

        public static int Main(String [] Args){
        int iRep = 0;
        int iObj = 0;
        Console.WriteLine("Test should return with ExitCode 100 ...");

            if (Args.Length==2)
            {
                if (!Int32.TryParse( Args[0], out iRep ) ||
                    !Int32.TryParse( Args[0], out iObj ))
                {
                    iRep = 40000;
                    iObj = 100;
                }
            }
            else
            {
                iRep = 40000;
                iObj = 100;
            }

            VariantLinkList Mv_Obj = new VariantLinkList();
            if(Mv_Obj.runTest(iRep, iObj))
            {
                Console.WriteLine("Test Passed");
                return 100;
            }
            Console.WriteLine("Test Failed");
            return 1;
        }

        public bool runTest(int iRep, int iObj)
        {
            m_Root = new Node();
            m_Root.m_Var = null;
            m_Root.next = null;
            Node temp = m_Root;
            for(int i = 0; i < iRep; i++)
            {
                temp.next = new Node();
                temp = temp.next;
                temp.m_Var = (i);
                temp.next = null;
                if ((i+1)%10000 == 0)
                {
                    Console.Write("Nodes Created: ");
                    Console.WriteLine(i+1);
                }
            }
            temp = m_Root;
            m_Root = null;

            Console.WriteLine("Done creating");

            for(int i = 0; i < iRep; i++)
            {
                temp = temp.next;
                if ((i+1)%10000 == 0)
                {
                    Console.Write("Nodes Traversed: ");
                    Console.WriteLine(i + 1);
                    GC.Collect();
                }
            }
            GC.Collect();
            return true;
        }

    }
}
