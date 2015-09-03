// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShareDatabaseNameMessageHandlerTest.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Test
{
    using Naos.Database.MessageBus.Contract;
    using Naos.Database.MessageBus.Handler;

    using Xunit;

    public class ShareDatabaseNameMessageHandlerTest
    {
        [Fact]
        public void Handle_MessageWithName_AssignedToShare()
        {
            // arrange
            var testName = "Monkey";
            var message = new ShareDatabaseNameMessage { DatabaseNameToShare = testName };
            var handler = new ShareDatabaseNameMessageHandler();

            // act
            handler.HandleAsync(message).Wait();

            // assert
            Assert.Equal(testName, message.DatabaseNameToShare);
            Assert.Equal(testName, handler.DatabaseName);
        }
    }
}
