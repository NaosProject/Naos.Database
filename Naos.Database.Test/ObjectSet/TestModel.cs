// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModel.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;

    public class TestModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This is what I want.")]
        public TestModel(string name, Guid id = default(Guid))
        {
            Name = name;
            Id = (id == default(Guid)) ? Guid.NewGuid() : id;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This is what I want.")]
        public static IReadOnlyList<TestModel> CreateMany(string namePrefix, int count = 3)
        {
            var testModels = new List<TestModel>(count);

            for (var i = 0; i < count; i++)
            {
                var model = new TestModel(namePrefix + i);

                testModels.Add(model);
            }

            return testModels;
        }
    }
}
