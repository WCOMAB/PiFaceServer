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
namespace PiFaceServer.Helper
{
    using System;
    using Source.Common;

    public static class LogHelper
    {
        public static void TryAndLog<T>(this T value, Action<T> action, bool skipIfNull = true)
        {
            try
            {
                if (
                    skipIfNull &&
                    ReferenceEquals(value, null)
                    )
                    return;

                if (
                    skipIfNull &&
                    action == null
                    )
                    return;


                action(value);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogException(this Exception ex)
        {
            Log.Exception(
                ex.ToString()
                );
        }
    }
}