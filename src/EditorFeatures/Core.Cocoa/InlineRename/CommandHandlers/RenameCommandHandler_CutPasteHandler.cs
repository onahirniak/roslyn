﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace Microsoft.CodeAnalysis.Editor.Implementation.InlineRename
{
    internal partial class RenameCommandHandler :
        IChainedCommandHandler<CutCommandArgs>, IChainedCommandHandler<PasteCommandArgs>
    {
        public VSCommanding.CommandState GetCommandState(CutCommandArgs args, Func<VSCommanding.CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(CutCommandArgs args, Action nextHandler, CommandExecutionContext context)
        {
            HandlePossibleTypingCommand(args, nextHandler, span =>
            {
                nextHandler();
            });
        }

        public VSCommanding.CommandState GetCommandState(PasteCommandArgs args, Func<VSCommanding.CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(PasteCommandArgs args, Action nextHandler, CommandExecutionContext context)
        {
            HandlePossibleTypingCommand(args, nextHandler, span =>
            {
                nextHandler();
            });
        }
    }
}
