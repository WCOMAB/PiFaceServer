


// ############################################################################
// #                                                                          #
// #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
// #                                                                          #
// # This means that any edits to the .cs file will be lost when its          #
// # regenerated. Changes should instead be applied to the corresponding      #
// # text template file (.tt)                                                 #
// ############################################################################



// ############################################################################
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Command.cs
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Operation.cs
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\Include_T4Include.cs
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PiFaceClient.cs
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PinState.cs
// @@@ SKIPPING (Blacklisted): C:\temp\dev\github\PiFaceServer\src\PiFaceClient\Properties\AssemblyInfo.cs
// @@@ INCLUDING: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PullUpDn.cs
// ############################################################################
// Certains directives such as #define and // Resharper comments has to be 
// moved to top in order to work properly    
// ############################################################################
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Command.cs
namespace ProjectInclude
{
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
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Command.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Operation.cs
namespace ProjectInclude
{
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
        public enum Operation : byte
        {
            None = 0,
            Connect = 1,
            WiringPiSetupPiFace = 2,
            PullUpDnControl = 3,
            DigitalWrite = 4,
            DigitalRead = 5,
            MonitorInput = 6,
            MonitorInputChanged = 7
        }
    }
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Operation.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\Include_T4Include.cs
namespace ProjectInclude
{
    
    // ############################################################################
    // #                                                                          #
    // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
    // #                                                                          #
    // # This means that any edits to the .cs file will be lost when its          #
    // # regenerated. Changes should instead be applied to the corresponding      #
    // # text template file (.tt)                                                 #
    // ############################################################################
    
    
    
    // ############################################################################
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Extensions/BasicExtensions.cs
    // @@@ INCLUDE_FOUND: ../Common/Array.cs
    // @@@ INCLUDE_FOUND: ../Common/Config.cs
    // @@@ INCLUDE_FOUND: ../Common/Log.cs
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs
    // @@@ INCLUDE_FOUND: Config.cs
    // @@@ INCLUDE_FOUND: Generated_Log.cs
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Common/ConsoleLog.cs
    // @@@ INCLUDE_FOUND: Config.cs
    // @@@ INCLUDE_FOUND: Log.cs
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Common/Array.cs
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs
    // @@@ SKIPPING (Already seen): https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs
    // @@@ SKIPPING (Already seen): https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs
    // @@@ INCLUDING: https://raw.github.com/WCOMAB/T4Include/master/Common/Generated_Log.cs
    // @@@ SKIPPING (Already seen): https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs
    // @@@ SKIPPING (Already seen): https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs
    // ############################################################################
    // Certains directives such as #define and // Resharper comments has to be 
    // moved to top in order to work properly    
    // ############################################################################
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Extensions/BasicExtensions.cs
    namespace PiFaceClient
    {
        // ----------------------------------------------------------------------------------------------
        // Copyright (c) M?rten R?nge.
        // ----------------------------------------------------------------------------------------------
        // This source code is subject to terms and conditions of the Microsoft Public License. A 
        // copy of the license can be found in the License.html file at the root of this distribution. 
        // If you cannot locate the  Microsoft Public License, please send an email to 
        // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
        //  by the terms of the Microsoft Public License.
        // ----------------------------------------------------------------------------------------------
        // You must not remove this notice, or any other, from this software.
        // ----------------------------------------------------------------------------------------------
        
        
        
        namespace Source.Extensions
        {
            using System;
            using System.Collections.Generic;
            using System.Globalization;
            using System.IO;
            using System.Reflection;
        
            using Source.Common;
        
            static partial class BasicExtensions
            {
                public static bool IsNullOrWhiteSpace (this string v)
                {
                    return string.IsNullOrWhiteSpace (v);
                }
        
                public static bool IsNullOrEmpty (this string v)
                {
                    return string.IsNullOrEmpty (v);
                }
        
                public static T FirstOrReturn<T>(this T[] values, T defaultValue)
                {
                    if (values == null)
                    {
                        return defaultValue;
                    }
        
                    if (values.Length == 0)
                    {
                        return defaultValue;
                    }
        
                    return values[0];
                }
        
                public static T FirstOrReturn<T>(this IEnumerable<T> values, T defaultValue)
                {
                    if (values == null)
                    {
                        return defaultValue;
                    }
        
                    foreach (var value in values)
                    {
                        return value;
                    }
        
                    return defaultValue;
                }
        
                public static void Shuffle<T>(this T[] values, Random random)
                {
                    if (values == null)
                    {
                        return;
                    }
        
                    if (random == null)
                    {
                        return;
                    }
        
                    for (var iter = 0; iter < values.Length; ++iter)
                    {
                        var swapWith = random.Next (iter, values.Length);
        
                        var tmp = values[iter];
        
                        values[iter] = values[swapWith];
                        values[swapWith] = tmp;
                    }
        
                }
        
                public static string DefaultTo (this string v, string defaultValue = null)
                {
                    return !v.IsNullOrEmpty () ? v : (defaultValue ?? "");
                }
        
                public static IEnumerable<T> DefaultTo<T>(
                    this IEnumerable<T> values, 
                    IEnumerable<T> defaultValue = null
                    )
                {
                    return values ?? defaultValue ?? Array<T>.Empty;
                }
        
                public static T[] DefaultTo<T>(this T[] values, T[] defaultValue = null)
                {
                    return values ?? defaultValue ?? Array<T>.Empty;
                }
        
                public static T DefaultTo<T>(this T v, T defaultValue = default (T))
                    where T : struct, IEquatable<T>
                {
                    return !v.Equals (default (T)) ? v : defaultValue;
                }
        
                public static string FormatWith (this string format, CultureInfo cultureInfo, params object[] args)
                {
                    return string.Format (cultureInfo, format ?? "", args.DefaultTo ());
                }
        
                public static string FormatWith (this string format, params object[] args)
                {
                    return format.FormatWith (Config.DefaultCulture, args);
                }
        
                public static TValue Lookup<TKey, TValue>(
                    this IDictionary<TKey, TValue> dictionary, 
                    TKey key, 
                    TValue defaultValue = default (TValue))
                {
                    if (dictionary == null)
                    {
                        return defaultValue;
                    }
        
                    TValue value;
                    return dictionary.TryGetValue (key, out value) ? value : defaultValue;
                }
        
                public static TValue GetOrAdd<TKey, TValue>(
                    this IDictionary<TKey, TValue> dictionary, 
                    TKey key, 
                    TValue defaultValue = default (TValue))
                {
                    if (dictionary == null)
                    {
                        return defaultValue;
                    }
        
                    TValue value;
                    if (!dictionary.TryGetValue (key, out value))
                    {
                        value = defaultValue;
                        dictionary[key] = value;
                    }
        
                    return value;
                }
        
                public static TValue GetOrAdd<TKey, TValue>(
                    this IDictionary<TKey, TValue> dictionary,
                    TKey key,
                    Func<TValue> valueCreator
                    )
                {
                    if (dictionary == null)
                    {
                        return valueCreator ();
                    }
        
                    TValue value;
                    if (!dictionary.TryGetValue (key, out value))
                    {
                        value = valueCreator ();
                        dictionary[key] = value;
                    }
        
                    return value;
                }
        
                public static void DisposeNoThrow (this IDisposable disposable)
                {
                    try
                    {
                        if (disposable != null)
                        {
                            disposable.Dispose ();
                        }
                    }
                    catch (Exception exc)
                    {
                        Log.Exception ("DisposeNoThrow: Dispose threw: {0}", exc);
                    }
                }
        
                public static TTo CastTo<TTo> (this object value, TTo defaultValue)
                {
                    return value is TTo ? (TTo) value : defaultValue;
                }
        
                public static string Concatenate (this IEnumerable<string> values, string delimiter = null, int capacity = 16)
                {
                    values = values ?? Array<string>.Empty;
                    delimiter = delimiter ?? ", ";
        
                    return string.Join (delimiter, values);
                }
        
                public static string GetResourceString (this Assembly assembly, string name, string defaultValue = null)
                {
                    defaultValue = defaultValue ?? "";
        
                    if (assembly == null)
                    {
                        return defaultValue;
                    }
        
                    var stream = assembly.GetManifestResourceStream (name ?? "");
                    if (stream == null)
                    {
                        return defaultValue;
                    }
        
                    using (stream)
                    using (var streamReader = new StreamReader (stream))
                    {
                        return streamReader.ReadToEnd ();
                    }
                }
        
                public static IEnumerable<string> ReadLines (this TextReader textReader)
                {
                    if (textReader == null)
                    {
                        yield break;
                    }
        
                    string line;
        
                    while ((line = textReader.ReadLine ()) != null)
                    {
                        yield return line;
                    }
                }
        
        #if !NETFX_CORE
                public static IEnumerable<Type> GetInheritanceChain (this Type type)
                {
                    while (type != null)
                    {
                        yield return type;
                        type = type.BaseType;
                    }
                }
        #endif
            }
        }
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Extensions/BasicExtensions.cs
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs
    namespace PiFaceClient
    {
        // ----------------------------------------------------------------------------------------------
        // Copyright (c) M?rten R?nge.
        // ----------------------------------------------------------------------------------------------
        // This source code is subject to terms and conditions of the Microsoft Public License. A 
        // copy of the license can be found in the License.html file at the root of this distribution. 
        // If you cannot locate the  Microsoft Public License, please send an email to 
        // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
        //  by the terms of the Microsoft Public License.
        // ----------------------------------------------------------------------------------------------
        // You must not remove this notice, or any other, from this software.
        // ----------------------------------------------------------------------------------------------
        
        
        
        namespace Source.Common
        {
            using System;
            using System.Globalization;
        
            static partial class Log
            {
                static partial void Partial_LogLevel (Level level);
                static partial void Partial_LogMessage (Level level, string message);
                static partial void Partial_ExceptionOnLog (Level level, string format, object[] args, Exception exc);
        
                public static void LogMessage (Level level, string format, params object[] args)
                {
                    try
                    {
                        Partial_LogLevel (level);
                        Partial_LogMessage (level, GetMessage (format, args));
                    }
                    catch (Exception exc)
                    {
                        Partial_ExceptionOnLog (level, format, args, exc);
                    }
                    
                }
        
                static string GetMessage (string format, object[] args)
                {
                    format = format ?? "";
                    try
                    {
                        return (args == null || args.Length == 0)
                                   ? format
                                   : string.Format (Config.DefaultCulture, format, args)
                            ;
                    }
                    catch (FormatException)
                    {
        
                        return format;
                    }
                }
            }
        }
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/ConsoleLog.cs
    namespace PiFaceClient
    {
        // ----------------------------------------------------------------------------------------------
        // Copyright (c) M?rten R?nge.
        // ----------------------------------------------------------------------------------------------
        // This source code is subject to terms and conditions of the Microsoft Public License. A 
        // copy of the license can be found in the License.html file at the root of this distribution. 
        // If you cannot locate the  Microsoft Public License, please send an email to 
        // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
        //  by the terms of the Microsoft Public License.
        // ----------------------------------------------------------------------------------------------
        // You must not remove this notice, or any other, from this software.
        // ----------------------------------------------------------------------------------------------
        
        
        
        namespace Source.Common
        {
            using System;
            using System.Globalization;
        
            partial class Log
            {
                static readonly object s_colorLock = new object ();
                static partial void Partial_LogMessage (Level level, string message)
                {
                    var now = DateTime.Now;
                    var finalMessage = string.Format (
                        Config.DefaultCulture,
                        "{0:HHmmss} {1} : {2}",
                        now,
                        GetLevelMessage (level),
                        message
                        );
                    lock (s_colorLock)
                    {
                        var oldColor = Console.ForegroundColor;
                        Console.ForegroundColor = GetLevelColor (level);
                        try
                        {
                            Console.WriteLine (finalMessage);
                        }
                        finally
                        {
                            Console.ForegroundColor = oldColor;
                        }
        
                    }
                }
            }
        }
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/ConsoleLog.cs
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Array.cs
    namespace PiFaceClient
    {
        // ----------------------------------------------------------------------------------------------
        // Copyright (c) M?rten R?nge.
        // ----------------------------------------------------------------------------------------------
        // This source code is subject to terms and conditions of the Microsoft Public License. A 
        // copy of the license can be found in the License.html file at the root of this distribution. 
        // If you cannot locate the  Microsoft Public License, please send an email to 
        // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
        //  by the terms of the Microsoft Public License.
        // ----------------------------------------------------------------------------------------------
        // You must not remove this notice, or any other, from this software.
        // ----------------------------------------------------------------------------------------------
        
        namespace Source.Common
        {
            static class Array<T>
            {
                public static readonly T[] Empty = new T[0];
            }
        }
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Array.cs
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs
    namespace PiFaceClient
    {
        // ----------------------------------------------------------------------------------------------
        // Copyright (c) M?rten R?nge.
        // ----------------------------------------------------------------------------------------------
        // This source code is subject to terms and conditions of the Microsoft Public License. A 
        // copy of the license can be found in the License.html file at the root of this distribution. 
        // If you cannot locate the  Microsoft Public License, please send an email to 
        // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
        //  by the terms of the Microsoft Public License.
        // ----------------------------------------------------------------------------------------------
        // You must not remove this notice, or any other, from this software.
        // ----------------------------------------------------------------------------------------------
        
        
        namespace Source.Common
        {
            using System.Globalization;
        
            sealed partial class InitConfig
            {
                public CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
            }
        
            static partial class Config
            {
                static partial void Partial_Constructed(ref InitConfig initConfig);
        
                public readonly static CultureInfo DefaultCulture;
        
                static Config ()
                {
                    var initConfig = new InitConfig();
        
                    Partial_Constructed (ref initConfig);
        
                    initConfig = initConfig ?? new InitConfig();
        
                    DefaultCulture = initConfig.DefaultCulture;
                }
            }
        }
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs
    // ############################################################################
    
    // ############################################################################
    // @@@ BEGIN_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Generated_Log.cs
    namespace PiFaceClient
    {
        // ############################################################################
        // #                                                                          #
        // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
        // #                                                                          #
        // # This means that any edits to the .cs file will be lost when its          #
        // # regenerated. Changes should instead be applied to the corresponding      #
        // # template file (.tt)                                                      #
        // ############################################################################
        
        
        
        
        
        
        namespace Source.Common
        {
            using System;
        
            partial class Log
            {
                public enum Level
                {
                    Success = 1000,
                    HighLight = 2000,
                    Info = 3000,
                    Warning = 10000,
                    Error = 20000,
                    Exception = 21000,
                }
        
                public static void Success (string format, params object[] args)
                {
                    LogMessage (Level.Success, format, args);
                }
                public static void HighLight (string format, params object[] args)
                {
                    LogMessage (Level.HighLight, format, args);
                }
                public static void Info (string format, params object[] args)
                {
                    LogMessage (Level.Info, format, args);
                }
                public static void Warning (string format, params object[] args)
                {
                    LogMessage (Level.Warning, format, args);
                }
                public static void Error (string format, params object[] args)
                {
                    LogMessage (Level.Error, format, args);
                }
                public static void Exception (string format, params object[] args)
                {
                    LogMessage (Level.Exception, format, args);
                }
        #if !NETFX_CORE && !SILVERLIGHT && !WINDOWS_PHONE
                static ConsoleColor GetLevelColor (Level level)
                {
                    switch (level)
                    {
                        case Level.Success:
                            return ConsoleColor.Green;
                        case Level.HighLight:
                            return ConsoleColor.White;
                        case Level.Info:
                            return ConsoleColor.Gray;
                        case Level.Warning:
                            return ConsoleColor.Yellow;
                        case Level.Error:
                            return ConsoleColor.Red;
                        case Level.Exception:
                            return ConsoleColor.Red;
                        default:
                            return ConsoleColor.Magenta;
                    }
                }
        #endif
                static string GetLevelMessage (Level level)
                {
                    switch (level)
                    {
                        case Level.Success:
                            return "SUCCESS  ";
                        case Level.HighLight:
                            return "HIGHLIGHT";
                        case Level.Info:
                            return "INFO     ";
                        case Level.Warning:
                            return "WARNING  ";
                        case Level.Error:
                            return "ERROR    ";
                        case Level.Exception:
                            return "EXCEPTION";
                        default:
                            return "UNKNOWN  ";
                    }
                }
        
            }
        }
        
    }
    // @@@ END_INCLUDE: https://raw.github.com/WCOMAB/T4Include/master/Common/Generated_Log.cs
    // ############################################################################
    // ############################################################################
    // Certains directives such as #define and // Resharper comments has to be 
    // moved to bottom in order to work properly    
    // ############################################################################
    // ############################################################################
    namespace PiFaceClient.Include
    {
        static partial class MetaData
        {
            public const string RootPath        = @"https://raw.github.com/";
            public const string IncludeDate     = @"2014-12-16T20:41:26";
    
            public const string Include_0       = @"https://raw.github.com/WCOMAB/T4Include/master/Extensions/BasicExtensions.cs";
            public const string Include_1       = @"https://raw.github.com/WCOMAB/T4Include/master/Common/Log.cs";
            public const string Include_2       = @"https://raw.github.com/WCOMAB/T4Include/master/Common/ConsoleLog.cs";
            public const string Include_3       = @"https://raw.github.com/WCOMAB/T4Include/master/Common/Array.cs";
            public const string Include_4       = @"https://raw.github.com/WCOMAB/T4Include/master/Common/Config.cs";
            public const string Include_5       = @"https://raw.github.com/WCOMAB/T4Include/master/Common/Generated_Log.cs";
        }
    }
    // ############################################################################
    
    
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\Include_T4Include.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PiFaceClient.cs
namespace ProjectInclude
{
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
    namespace PiFaceClient
    {
        using System;
        using System.Collections.Concurrent;
        using System.Linq;
        using System.Net;
        using System.Net.Sockets;
        using System.Threading;
        using System.Threading.Tasks;
        using Source.Common;
        using PiFaceServer.Command;
    
        public class PiFaceClient : IDisposable
        {
            private readonly ConcurrentDictionary<long, Action<Command>> _callBackCommands = new ConcurrentDictionary<long, Action<Command>>();
            private readonly IPEndPoint _remoteEndPoint;
            private readonly Socket _ackSocket;
            private readonly Socket _cmdSocket;
    
            public IPEndPoint RemoteEndpoint
            {
                get { return _remoteEndPoint; }
            }
    
            public bool IsDisposed { get; private set; }
    
            public Action<Command> AckRecieved { get; set; }
    
            public void Connect()
            {
                _ackSocket.Connect(RemoteEndpoint);
                SendCommand(
                    _ackSocket,
                    new Command
                        {
                            Operation = Operation.Connect
                        }
                    );
                Command initAckCommand;
                ReceiveCommand(_ackSocket, out initAckCommand);
    
                new Thread(
                    () =>
                    {
                        while (!IsDisposed && IsConnected(_ackSocket))
                        {
                            Command ackCommand;
                            ReceiveCommand(_ackSocket, out ackCommand);
                        }
                    }
                    ).Start();
                _cmdSocket.Connect(RemoteEndpoint);
            }
    
            public void Dispose()
            {
                if (IsDisposed)
                    return;
                IsDisposed = true;
                try
                {
                    _ackSocket.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex.ToString());
                }
                try
                {
                    _cmdSocket.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex.ToString());
                }
            }
    
            private static long _currId = DateTime.UtcNow.Ticks;
            private static long GetNextId()
            {
                return Interlocked.Increment(ref _currId);
            }
    
    
    
            private static void SendCommand(Socket socket, params Command[] commands)
            {
                Array.ForEach(
                    commands,
                    command =>
                    {
                        var commandBytes = command.ToArray();
    
                        Log.Info(
                            "Sending command {0} ({1})",
                            command,
                            commandBytes.Length
                            );
    
    
                        var bytesSent = socket.Send(
                            commandBytes
                            );
                        Log.Success("Sent {0} bytes", bytesSent);
                    }
                    );
            }
    
            private void ReceiveCommand(Socket ackSocket, out Command ackCommand)
            {
                var ackBytes = new byte[Command.SizeOf];
                Log.Info("Recieving bytes");
                SocketError errorCode;
                var bytesRecieved = ackSocket.Receive(
                    ackBytes,
                    0,
                    Command.SizeOf,
                    0,
                    out errorCode
                    );
                if (errorCode != SocketError.Success)
                {
                    Log.Error("Recieve error {0}", errorCode);
                    ackCommand = default(Command);
                    return;
                }
    
                Log.Success("Recieved {0} bytes", bytesRecieved);
                Log.Info(
                    "Command from array of {0} bytes ({1})",
                    ackBytes.Length,
                    string.Concat(
                        ackBytes.Select(x => x.ToString("x2")
                            )
                        )
                    );
    
                ackCommand = Command.FromArray(ackBytes);
                Log.Success("Recived command {0}", ackCommand);
    
                try
                {
                    if (AckRecieved != null)
                        AckRecieved(ackCommand);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex.ToString());
                }
    
                try
                {
                    Action<Command> callback;
                    if (
                        (
                        (ackCommand.Operation==Operation.MonitorInputChanged || ackCommand.Operation==Operation.MonitorInput)
                        ? _callBackCommands.TryGetValue(ackCommand.Id, out callback)
                        : _callBackCommands.TryRemove(ackCommand.Id, out callback)
                            )&&
                        callback != null
                        )
                    {
                        callback(ackCommand);
                    }
    
                }
                catch (Exception ex)
                {
                    Log.Exception(ex.ToString());
                }
            }
    
            private static bool IsConnected(Socket socket)
            {
                return socket.IsBound &&
                       socket.Connected &&
                       !(
                            socket.Poll(1, SelectMode.SelectRead)
                            && socket.Available == 0
                        );
            }
    
    
    
            private static Action<Command> GetCallbackTask(out Task<Command> task)
            {
                var result = default(Command);
                var done = new ManualResetEventSlim();
                Action<Command> callback = command =>
                {
                    result = command;
                    done.Set();
                };
                task = Task.Run(
                    () =>
                        {
                            using (done)
                            {
                                done.Wait(100000);
                                return result;
                            }
                        }
                    );
                return callback;
            }
    
            public PiFaceClient(IPEndPoint remoteEndPoint)
            {
                _remoteEndPoint = remoteEndPoint;
                _ackSocket = new Socket(
                    RemoteEndpoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );
                _cmdSocket = new Socket(
                    RemoteEndpoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );
            }
    
            public Task<Command> WiringPiSetupPiFaceAsync(byte target = 0)
            {
                Task<Command> task;
                BeginWiringPiSetupPiFace(target, GetCallbackTask(out task));
                return task;
            }
    
            public void BeginWiringPiSetupPiFace(byte target = 0, Action<Command> callback = null)
            {
                var command = GetNewCommand(Operation.WiringPiSetupPiFace, 0, 0, target, callback);
                SendCommand(_cmdSocket, command);
            }
    
            public Task<Command> PullUpDnControlAsync(int pin, PullUpDn pud, byte target = 0)
            {
                Task<Command> task;
                BeginPullUpDnControl(pin, pud, target, GetCallbackTask(out task));
                return task;
            }
    
            public void BeginPullUpDnControl(int pin, PullUpDn pud, byte target = 0, Action<Command> callback = null)
            {
                var command = GetNewCommand(Operation.PullUpDnControl, pin, (int)pud, target, callback);
                SendCommand(_cmdSocket, command);
            }
    
            public Task<Command> DigitalWriteAsync(int pin, PinState pinState, byte target = 0)
            {
                Task<Command> task;
                BeginDigitalWrite(pin, pinState, target, GetCallbackTask(out task));
                return task;
            }
    
            public void BeginDigitalWrite(int pin, PinState pinState, byte target = 0, Action<Command> callback = null)
            {
                var command = GetNewCommand(Operation.DigitalWrite, pin, (int)pinState, target, callback);
                SendCommand(_cmdSocket, command);
            }
    
            public Task<Command> DigitalReadAsync(int pin, byte target = 0)
            {
                Task<Command> task;
                BeginDigitalRead(pin, target, GetCallbackTask(out task));
                return task;
            }
    
            public void BeginDigitalRead(int pin, byte target = 0, Action<Command> callback = null)
            {
                var command = GetNewCommand(Operation.DigitalRead, pin, 0, target, callback);
                SendCommand(_cmdSocket, command);
            }
    
            public void BeginMonitorInput(int pin, bool? monitor = null, byte target = 0, Action<Command> callback = null)
            {
                var command = GetNewCommand(
                    Operation.MonitorInput,
                    pin,
                    (monitor ?? true) ? 1 : 0,
                    target,
                    callback
                    );
                SendCommand(_cmdSocket, command);
            }
    
            private Command GetNewCommand(Operation operation, int pin, int value, byte target, Action<Command> callback = null)
            {
                var id = GetNextId();
                if (callback != null)
                    _callBackCommands.TryAdd(
                        id,
                        callback
                        );
                return new Command
                                   {
                                       Id = id,
                                       Operation = operation,
                                       Target = target,
                                       Pin = pin,
                                       Value = value
                                   };
            }
        }
    }
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PiFaceClient.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PinState.cs
namespace ProjectInclude
{
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
    namespace PiFaceClient
    {
        public enum PinState
        {
            Error = -1,
            Low = 0,
            High = 1
        }
    }
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PinState.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PullUpDn.cs
namespace ProjectInclude
{
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
    namespace PiFaceClient
    {
        public enum PullUpDn
        {
            Error = -1,
            Off = 0,
            Down = 1,
            Up = 2
        }
    }
}
// @@@ END_INCLUDE: C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PullUpDn.cs
// ############################################################################
// ############################################################################
// Certains directives such as #define and // Resharper comments has to be 
// moved to bottom in order to work properly    
// ############################################################################
// ############################################################################
namespace ProjectInclude.Include
{
    static partial class MetaData
    {
        public const string RootPath        = @"C:\temp\dev\github\PiFaceServer\src\PiFaceClientTest\..\PiFaceClient";
        public const string IncludeDate     = @"2014-12-16T21:33:38";

        public const string Include_0       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Command.cs";
        public const string Include_1       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceServer\Command\Operation.cs";
        public const string Include_2       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceClient\Include_T4Include.cs";
        public const string Include_3       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PiFaceClient.cs";
        public const string Include_4       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PinState.cs";
        public const string Include_5       = @"C:\temp\dev\github\PiFaceServer\src\PiFaceClient\PullUpDn.cs";
    }
}
// ############################################################################




