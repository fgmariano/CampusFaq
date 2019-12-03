using CampusFaq.Chatbot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CampusFaq.Chatbot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                try
                {
                    await Conversation.SendAsync(activity, () => new CognitiveDialog());
                }
                catch (Exception e)
                {
                    var client = new ConnectorClient(new Uri(activity.ServiceUrl));
                    Activity reply = activity.CreateReply(e.Message + "::" + e.StackTrace);
                    await client.Conversations.ReplyToActivityAsync(reply);
                }
            }
            //else if (activity != null && activity.GetActivityType() == ActivityTypes.ConversationUpdate)
            //{
            //    var message = activity.AsConversationUpdateActivity();
            //    foreach (var item in message.MembersAdded)
            //    {
            //        if (!string.IsNullOrEmpty(item.Name) && item.Name.ToLower().Contains("bot"))
            //        {
            //            try
            //            {
            //                await Conversation.SendAsync(activity, () => new CognitiveDialog());
            //            }
            //            catch (Exception e)
            //            {
            //                var client = new ConnectorClient(new Uri(activity.ServiceUrl));
            //                Activity reply = activity.CreateReply(e.Message + ":::" + e.StackTrace);
            //                await client.Conversations.ReplyToActivityAsync(reply);
            //            }
            //        }
            //    }
            //}

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
