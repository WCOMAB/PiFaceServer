// ----------------------------------------------------------------------------------------------
// Copyright (c) WCOM AB.
// ----------------------------------------------------------------------------------------------
// This source code is subject to terms and conditions of the Microsoft Public License. A 
// copy of the license can be found in the LICENSE.md file at the root of this distribution. 
// If you cannot locate the  Microsoft Public License, please send an email to 
// dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
//  by the terms of the Microsoft Public License.
// ----------------------------------------------------------------------------------------------
// You must not remove this notice, or any other, from this software.
// ----------------------------------------------------------------------------------------------
namespace PiFaceServer.Command
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Command
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(Command));

        [MarshalAs(UnmanagedType.U1)]
        public Operation Operation;
        public long Id;
        public byte Target;
        public int Pin;
        public int Value;


        public override string ToString()
        {
            return string.Format(
                "{0,-20} Id: {1} Target: {2} Pin:{3} Value:{4}",
                Operation,
                Id,
                Target,
                Pin,
                Value
                );
        }

        public byte[] ToArray()
        {
            var result = new byte[SizeOf];
            var ptr = Marshal.AllocHGlobal(SizeOf);
            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, result, 0, result.Length);
            Marshal.FreeHGlobal(ptr);
            return result;
        }

        public static Command FromArray(byte[] data)
        {
            var ptr = Marshal.AllocHGlobal(SizeOf);
            Marshal.Copy(data, 0, ptr, SizeOf);
            var result = (Command) Marshal.PtrToStructure(ptr, typeof (Command));
            Marshal.FreeHGlobal(ptr);
            return result;
        }
    }
}