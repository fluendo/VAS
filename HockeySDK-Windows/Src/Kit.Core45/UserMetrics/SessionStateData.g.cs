
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 3.02
//   File : SessionStateData_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------


#region ReSharper warnings
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameter.Local
// ReSharper disable RedundantUsingDirective
#endregion

namespace Microsoft.HockeyApp.Extensibility.Implementation.External
{
    using System.Collections.Generic;

    
    [System.CodeDom.Compiler.GeneratedCode("gbc", "3.02")]
    internal enum SessionState
    {
        Start,
        End,
    }

    
    [System.CodeDom.Compiler.GeneratedCode("gbc", "3.02")]
    internal partial class SessionStateData
        
    {
        
        public int ver { get; set; }

        
        public SessionState state { get; set; }
        
        public SessionStateData()
            : this("AI.SessionStateData", "SessionStateData")
        {}

        protected SessionStateData(string fullName, string name)
        {
            ver = 2;
            state = SessionState.Start;
        }
    }
} // AI










