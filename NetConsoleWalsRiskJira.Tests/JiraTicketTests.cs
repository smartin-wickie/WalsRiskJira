using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using NetConsoleWalsRiskJira.Shared;

namespace NetConsoleWalsRiskJira.Tests
{
    public class JiraTicketTests
    {
        [Test]
        public async Task CanDeserializeMockJiraTicket()
        {
            var json = await File.ReadAllTextAsync("mock-jira-ticket.json");
            var ticket = JsonSerializer.Deserialize<JiraTicket>(json);
            Assert.IsNotNull(ticket);
            Assert.AreEqual("Implement OAuth2 Authentication", ticket.Title);
            Assert.AreEqual("Identity", ticket.WorkTeam);
            Assert.AreEqual("Authentication", ticket.FeatureArea);
            Assert.IsTrue(ticket.AdditionalFields.ContainsKey("Priority"));
        }
    }
}
