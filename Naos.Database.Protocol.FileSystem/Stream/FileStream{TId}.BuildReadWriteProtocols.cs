// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStream{TId}.BuildReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    public partial class FileStream<TId>
    {
        /// <inheritdoc />
        public override ISyncAndAsyncReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject> BuildGetLatestByIdAndTypeProtocol<TObject>()
        {
            return new FileStreamObjectOperationsProtocol<TId, TObject>(this);
        }

        /// <inheritdoc />
        public override ISyncAndAsyncVoidProtocol<PutOp<TObject>> BuildPutProtocol<TObject>()
        {
            return new FileStreamObjectOperationsProtocol<TId, TObject>(this);
        }
    }
}
