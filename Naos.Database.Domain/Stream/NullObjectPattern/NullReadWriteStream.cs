// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IReadWriteStream"/> that does not have an actual identifier.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class NullReadWriteStream : IReadWriteStream,
                                               IStreamManagementProtocolFactory,
                                               IStreamRecordHandlingProtocolFactory,
                                               IModelViaCodeGen
    {
        /// <inheritdoc />
        public string Name => nameof(NullReadWriteStream);

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => new NullStreamRepresentation();

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => new NullResourceLocatorProtocols();

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
        }
    }
}
