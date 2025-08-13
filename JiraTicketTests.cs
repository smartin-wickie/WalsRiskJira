using NUnit.Framework;
using System.IO;
using System.Text.Json;

namespace NetConsoleWalsRiskJira.Tests
{
    public class JiraTicketTests
    {
        [Test]
        public void CanDeserializeMockJiraTicket()
        {
            var json = File.ReadAllText("mock-jira-ticket.json");
            var ticket = JsonSerializer.Deserialize<JiraTicket>(json);
            Assert.IsNotNull(ticket);
            Assert.AreEqual("Implement OAuth2 Authentication", ticket.Title);
            Assert.AreEqual("Identity", ticket.WorkTeam);
            Assert.AreEqual("Authentication", ticket.FeatureArea);
            Assert.IsTrue(ticket.AdditionalFields.ContainsKey("Priority"));
        }
    }
}
