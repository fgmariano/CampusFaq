using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;

namespace CampusFaq.Chatbot.Dialogs
{
    [LuisModel("0f2e72b6-65dd-4217-9ee3-11a584e6be29", "7405ef826602457f8b015b619203e1ec", domain: "https://westus.api.cognitive.microsoft.com")]
    [Serializable]
    public class CognitiveDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Não entendi o seu pedido. pode repetir?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("atendimentoEstagio")]
        public async Task AtendimentoEstagio(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Os horários de atendimento do estágio são topper.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("atendimentoSecretaria")]
        public async Task AtendimentoSecretaria(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Os horários de atendimento da secretaria estão no site.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("gradeHoraria")]
        public async Task GradeHoraria(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("a grade horária para o curso de adsfasdfasd.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("reservaBiblioteca")]
        public async Task ReservaBiblioteca(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("É possível reservar um livro na biblioteca por aqui.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("documentosEstagio")]
        public async Task DocumentosEstagio(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Selecione um documento para baixar:");
            context.Wait(MessageReceived);
        }

    }
}