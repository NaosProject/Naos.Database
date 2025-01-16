1. Add interface to IStreamRead[WithId]Protocols[TObject|TId|TId,TObject]

2. Add operation
    Here are the parameters available in the order they should appear in the Op's constructor.
    NOT ALL are applicable, so trim out the ones that aren't.

                TypeRepresentation identifierType = null,
                TypeRepresentation objectType = null,
                VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
                IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
                TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
                RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
                OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending,
                IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
                TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType

3. Add extension methods in ReadOnlyStreamExtensions...

4. Add lambda protocols

5. Add standardization extension if applicable

6. Add code to StandardStreamReadWrite...Protocols, RecordingStandardStreamReadWrite...Protocols, and NullStandardStreamReadWrite...Protocols

7. Code Gen, add unit tests
