// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMetadata.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    public class TestMetadata
    {
        public TestMetadata(string name)
        {
            FirstName = "First " + name;
            LastName = "Last " + name;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
