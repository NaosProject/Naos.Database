// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdReaderTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Xunit;

    public class IdReaderTest
    {
        // Not going to go combinatorial on these tests so alternating several axis per test: name,
        // visibility, and type
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_Id_property()
        {
            var testModel = new TestModel("test", Guid.NewGuid());

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(testModel.Id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_Id_property()
        {
            var type = IdReader.ReadType<TestModel>();
            type.Should().Be(typeof(Guid));

            var type2 = IdReader.ReadType(typeof(TestModel));
            type2.Should().Be(typeof(Guid));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_Id_property()
        {
            var name = IdReader.ReadName<TestModel>();
            name.Should().Be("Id");
        }

        private class TestModel2
        {
            public int Id;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_Id_field()
        {
            var testModel = new TestModel2
            {
                Id = 5
            };

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(testModel.Id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_Id_field()
        {
            var type = IdReader.ReadType<TestModel2>();
            type.Should().Be(typeof(int));

            var type2 = IdReader.ReadType(typeof(TestModel2));
            type2.Should().Be(typeof(int));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_Id_field()
        {
            var name = IdReader.ReadName<TestModel2>();
            name.Should().Be("Id");
        }

        private class TestModel3
        {
            internal string id { get; set; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_lowercase_id_property()
        {
            var testModel = new TestModel3
            {
                id = "hello"
            };

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(testModel.id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_lowercase_id_property()
        {
            var type = IdReader.ReadType<TestModel3>();
            type.Should().Be(typeof(string));

            var type2 = IdReader.ReadType(typeof(TestModel3));
            type2.Should().Be(typeof(string));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_lowercase_id_property()
        {
            var name = IdReader.ReadName<TestModel3>();
            name.Should().Be("id");
        }

        private class TestModel4
        {
            internal long id;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_lowercase_id_field()
        {
            var testModel = new TestModel4
            {
                id = 1000
            };

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(testModel.id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_lowercase_id_field()
        {
            var type = IdReader.ReadType<TestModel4>();
            type.Should().Be(typeof(long));

            var type2 = IdReader.ReadType(typeof(TestModel4));
            type2.Should().Be(typeof(long));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_lowercase_id_field()
        {
            var name = IdReader.ReadName<TestModel4>();
            name.Should().Be("id");
        }

        private class TestModel5
        {
            private float _id { get; set; }

            public TestModel5(float id)
            {
                _id = id;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_underscore_id_property()
        {
            var _id = 1000.0f;
            var testModel = new TestModel5(_id);

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(_id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_underscore_id_property()
        {
            var type = IdReader.ReadType<TestModel5>();
            type.Should().Be(typeof(float));

            var type2 = IdReader.ReadType(typeof(TestModel5));
            type2.Should().Be(typeof(float));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_underscore_id_property()
        {
            var name = IdReader.ReadName<TestModel5>();
            name.Should().Be("_id");
        }

        private class TestModel6
        {
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Value is used via reflection.")]
            private double _id;

            public TestModel6(double id)
            {
                _id = id;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_underscore_id_field()
        {
            var _id = 1000.0;
            var testModel = new TestModel6(_id);

            var id = IdReader.ReadValue(testModel);
            id.Should().Be(_id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_underscore_id_field()
        {
            var type = IdReader.ReadType<TestModel6>();
            type.Should().Be(typeof(double));

            var type2 = IdReader.ReadType(typeof(TestModel6));
            type2.Should().Be(typeof(double));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_underscore_id_field()
        {
            var name = IdReader.ReadName<TestModel6>();
            name.Should().Be("_id");
        }

        private class TestModel7Base
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Value is used via reflection.")]
            protected uint MyId { get; set; }
        }

        private class TestModel7 : TestModel7Base
        {
            public TestModel7(uint id)
            {
                MyId = id;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_user_defined_id_property()
        {
            var _id = 3u;
            var testModel = new TestModel7(_id);

            IdReader.SetIdMember<TestModel7>("MyId");
            var id = IdReader.ReadValue(testModel);
            id.Should().Be(_id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_user_defined_id_property()
        {
            IdReader.SetIdMember<TestModel7>("MyId");
            var type = IdReader.ReadType<TestModel7>();
            type.Should().Be(typeof(uint));

            var type2 = IdReader.ReadType(typeof(TestModel7));
            type2.Should().Be(typeof(uint));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_user_defined_id_property()
        {
            IdReader.SetIdMember<TestModel7>("MyId");
            var name = IdReader.ReadName<TestModel7>();
            name.Should().Be("MyId");
        }

        private class TestModel8Base
        {
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Value is used via reflection.")]
            protected ulong entityId;
        }

        private class TestModel8 : TestModel8Base
        {
            public TestModel8(ulong id)
            {
                entityId = id;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_reads_user_defined_id_field()
        {
            var _id = 3ul;
            var testModel = new TestModel8(_id);

            IdReader.SetIdMember<TestModel8>("entityId");
            var id = IdReader.ReadValue(testModel);
            id.Should().Be(_id);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_reads_user_defined_id_field()
        {
            IdReader.SetIdMember<TestModel8>("entityId");
            var type = IdReader.ReadType<TestModel8>();
            type.Should().Be(typeof(ulong));

            var type2 = IdReader.ReadType(typeof(TestModel8));
            type2.Should().Be(typeof(ulong));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_reads_user_defined_id_field()
        {
            IdReader.SetIdMember<TestModel8>("entityId");
            var name = IdReader.ReadName<TestModel8>();
            name.Should().Be("entityId");
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_throws_when_no_id_field_present()
        {
            var testModel = new TestMetadata("test");


            var ex = Record.Exception(() => IdReader.ReadValue(testModel));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_throws_when_no_id_field_present()
        {
            var ex = Record.Exception(() => IdReader.ReadType<TestMetadata>());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadName_throws_when_no_id_field_present()
        {
            var ex = Record.Exception(() => IdReader.ReadName<TestMetadata>());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadValue_throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => IdReader.ReadValue<IdReaderTest>(null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void ReadType_throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => IdReader.ReadType(null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Fact]
        public void SetIdMember_throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => IdReader.SetIdMember<IdReaderTest>(" "));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }
    }
}
