using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Slack.Webhooks;
using System.Collections.Generic;

namespace test
{
    public class Alert
    {
        public event EventHandler<EventArgs> SendMessage;

        public void Execute()
        {
            SendMessage(this, new EventArgs());
        }
    }

    public class Subscriber
    {
        Alert alert = new Alert();

        public void Subscribe()
        {
            alert.SendMessage += (sender, e) => { Console.WriteLine("First"); };
            alert.SendMessage += (sender, e) => { Console.WriteLine("Second"); };
            alert.SendMessage += (sender, e) => { Console.WriteLine("Third"); };
            alert.SendMessage += (sender, e) => { Console.WriteLine("Third"); };
        }

        public void Execute()
        {
            alert.Execute();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var slackClient = new SlackClient("https://hooks.slack.com/services/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");

            var slackMessage = new SlackMessage
            {
                Text = "Your message",
                IconEmoji = Emoji.Ghost,
                //Username = "yang"
            };

            var slackAttachment = new SlackAttachment
            {
                Fallback = "New open task [Urgent]: <http://url_to_task|Test out Slack message attachments>",
                Text = "New open task *[Urgent]*: <http://url_to_task|Test out Slack message attachments>",
                Color = "#D00000",
                Fields =
            new List<SlackField>
                {
                    new SlackField
                        {
                            Title = "Notes",
                            Value = "This is much *easier* than I thought it would be."
                        }
                }
            };
            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };

            for (int i = 0; i < 5; i++)
            {
                slackClient.Post(slackMessage);

            }

        }
    }
}
