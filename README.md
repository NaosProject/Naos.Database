[![Build status](https://ci.appveyor.com/api/projects/status/36b5onyj9ddkq6gr?svg=true)](https://ci.appveyor.com/project/Naos-Project/naos-database)

Naos.Database
===============

Overview
------------
**Naos.Database** provides a grammar for the persitence and management of data along with in-memory and file system implementations.  Store-specific implementations exist in other projects (e.g. `Naos.SqlServer.Protocol.Client` for Microsoft Sql Server).

This grammar supports a variety of data, persistence, and information exchange paradigms such as Event Sourcing, Streams & Stream Processing, Message Bus, Producer/Consumer Queues, Document Databases, and Timeseries Databases.  These paradigms lend themselves well to different problem spaces.  The grammar does not force a choice nor is it opinionated on the exclusive use of a specific paradigm allowing them to be blended as needed.  This empowers developers to optimally adapt the technology to the problem at hand and not vice-versa.  Developers can choose different paradigms for different systems without compromising interoperability.  Further, the chosen paradigm can be changed as the needs change, without incurring a significant refactoring cost.

The central concept in the grammar is a **Stream**.  A Stream is place to put and retrieve data in the form of objects.  The data paradigm drives the design of the objects being recorded and the specific operations executed against a Stream.  A Stream can be accessed read-only, read-write, or write-only, depending on the producers and consumers of the objects in the stream.  Each object, along with some additional context, is internally persisted as a **Record**.  Records will always be persisted with an auto-generated timestamp and auto-generated sequential unique identifier.

Objects can be put into a Stream with specified identifier or not.  When an identifier is specified, uniqueness is not enforced.
- A queueing system may have no need for an identifier nor an timestamps.  Objects are put into the stream and dequeued and processed.
- A unique identifier would be used in a Document Database paradigm.
- A non unique identifier would be used in an Event Sourced paradigm where the identifier is the Aggregate identifier and it is expected that more than one kind of event will be put into the stream for the same Aggregate.

Objects can be tagged when put into a Stream to facilitate querying.  Tags are supported as strings, or key-value pairs of strings (e.g. "failure" or  { "work-result", "failed" } ).


Adding External Operations
--------------------------
- Add the operation to the appropropriate Protocol Interface (e.g. `IStreamReadProtocols`)
- If the operation is a Read or Write operation, then add the appropriate extension methods to `ReadOnlyStreamExtensions` or `WriteOnlyStreamExtensions`
- Add a standardizing method in `RecordStandardizeExtensions`