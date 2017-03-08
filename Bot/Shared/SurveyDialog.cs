using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bot.Models;
using HtmlAgilityPack;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Shared
{
    [Serializable]
    public partial class SurveyDialog : IDialog<object>
    {
        /// <summary>
        /// Here you need your link to the Nfield survey you created in your domain/ Link here currently is to a demo domain
        /// this can be deleted at any point and then link will not work anymore
        /// </summary>
        private const string LiveLink =
            "https://blue-interviewing.niposoftware-dev.com/Interviews/UUqdx/fsGWZybyej3iz7Lku6ax";

        public async Task StartAsync(IDialogContext context)
        {
            var client = new HttpClient();
            //response from the Nfield interviewing system
            var response = await client.GetAsync(LiveLink);

            var capturedScreen = await CaptureScreen(response);

            context.UserData.SetValue("screen", capturedScreen);

            var replyMessage = context.MakeMessage();
            replyMessage.AddHeroCard(capturedScreen.QuestionText, capturedScreen.Options);

            await context.PostAsync(replyMessage);
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            CapturedScreen capturedScreen;
            context.UserData.TryGetValue("screen", out capturedScreen);

            var message = await argument;
            var answer = message.Text.Trim();

            var foundAnswer = capturedScreen.Options.SingleOrDefault(o => o.Equals(answer));
            if (!string.IsNullOrWhiteSpace(foundAnswer))
            {
                var foundAnswerIndex = capturedScreen.Options.IndexOf(foundAnswer);
                var answeredCategory = capturedScreen.Categories[foundAnswerIndex];

                var parameters = new Dictionary<string, string>
                {
                    {"screenId", capturedScreen.ScreenId},
                    {"historyOrder", capturedScreen.HistoryOrder},
                    {$"{answeredCategory.Name}-m", foundAnswerIndex.ToString()},
                    {answeredCategory.Name, answeredCategory.Value},
                    {"button-next", "Next"}
                };
                var formContent = new FormUrlEncodedContent(parameters);

                var client = new HttpClient();
                var response = await client.PostAsync(capturedScreen.RequestUri, formContent);
                capturedScreen = await CaptureScreen(response);
                if (capturedScreen == null)
                {
                    await context.PostAsync("Thank you");
                    context.Done("we're done");
                    return;
                }
                context.UserData.SetValue("screen", capturedScreen);

                var replyMessage = context.MakeMessage();
                replyMessage.AddHeroCard(capturedScreen.QuestionText, capturedScreen.Options);

                await context.PostAsync(replyMessage);
            }
            context.Wait(MessageReceivedAsync);
        }

        /// <summary>
        /// Rendered question screen
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<CapturedScreen> CaptureScreen(HttpResponseMessage response)
        {
            var screen = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(screen);
            var screenId = htmlDocument.GetElementbyId("screenId")?.GetAttributeValue("value", "x");
            if (string.IsNullOrWhiteSpace(screenId))
            {
                return null;
            }
            var historyOrder = htmlDocument.GetElementbyId("historyOrder").GetAttributeValue("value", "x");
            var questionText =
                htmlDocument.DocumentNode.Descendants()
                    .SingleOrDefault(
                        n =>
                            n.Name.Equals("div") && n.Attributes["class"] != null &&
                            n.Attributes["class"].Value.Contains("question"))
                    ?.InnerText
                    .Trim();
            var options = htmlDocument.DocumentNode.Descendants()
                .Where(
                    n =>
                        n.Name.Equals("div") && n.Attributes["class"] != null &&
                        n.Attributes["class"].Value.Contains("category-label"))
                .Select(n => n.InnerText.Trim());

            var categories = htmlDocument.DocumentNode.Descendants()
                .Where(
                    n =>
                        n.Name.Equals("input") && n.Attributes["class"] != null &&
                        n.Attributes["class"].Value.Equals("category"))
                .Select(n => new Category {Name = n.Attributes["name"].Value, Value = n.Attributes["value"].Value});

            return new CapturedScreen
            {
                ScreenId = screenId,
                QuestionText = questionText,
                Options = options.ToList(),
                Categories = categories.ToList(),
                RequestUri = response.RequestMessage.RequestUri,
                HistoryOrder = historyOrder
            };
        }
    }
}