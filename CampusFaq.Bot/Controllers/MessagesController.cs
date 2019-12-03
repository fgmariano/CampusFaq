using CampusFaq.Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CampusFaq.Bot.Controllers
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
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
                catch (Exception e)
                {
                    var client = new ConnectorClient(new Uri(activity.ServiceUrl));
                    Activity reply = activity.CreateReply(e.Message + "::" + e.StackTrace);
                    await client.Conversations.ReplyToActivityAsync(reply);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
