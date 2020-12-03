// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record.
    /// </summary>
    public partial class GetLatestRecordOp : ReturningOperationBase<StreamRecord>
    {
    }
}
