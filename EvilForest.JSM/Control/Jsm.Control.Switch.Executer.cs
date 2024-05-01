using System;
using System.Collections.Generic;
using FF8.Core;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Control
        {
            public sealed partial class Switch
            {
                private sealed class Executer : IScriptExecuter
                {
                    private readonly SwitchSegment _seg;

                    public Executer(SwitchSegment seg)
                    {
                        _seg = seg;
                    }

                    public IEnumerable<IAwaitable> Execute(IServices services)
                    {
                        throw new NotSupportedException();
                        // while (CanExecute(_seg.JmpIf, services))
                        // {
                        //     IEnumerable<IJsmInstruction> executable = _seg.GetBodyInstructions();
                        //     IScriptExecuter executer = ExecutableSegment.GetExecuter(executable);
                        //     foreach (IAwaitable result in executer.Execute(services))
                        //         yield return result;
                        //
                        //     // Skip one iteration to give control to other operations.
                        //     yield return SpinAwaitable.Instance;
                        // }
                    }

                    // private Boolean CanExecute(JMP_IF jmpIf, IServices services)
                    // {
                    //     foreach (var condition in jmpIf.Conditions)
                    //     {
                    //         if (!condition.IsTrue(services))
                    //             return false;
                    //     }
                    //
                    //     return true;
                    // }
                }
            }
        }
    }
}